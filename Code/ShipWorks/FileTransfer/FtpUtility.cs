﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Security;
using log4net;
using Rebex.Net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.FileTransfer
{
    /// <summary>
    /// FTP utility functions
    /// </summary>
    public static class FtpUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FtpUtility));

        private static readonly Dictionary<FtpSecurityType, int> defaultPorts =
            new Dictionary<FtpSecurityType, int>
            {
                {FtpSecurityType.Unsecure, 21},
                {FtpSecurityType.Implicit, 990},
                {FtpSecurityType.Explicit, 21},
                {FtpSecurityType.Sftp, 22},
            };

        /// <summary>
        /// Gets the default port for the given security type
        /// </summary>
        public static int GetDefaultPort(FtpSecurityType securityType)
        {
            return defaultPorts[securityType];
        }

        /// <summary>
        /// Creates a new FTP account initialized with default settings
        /// </summary>
        public static FtpAccountEntity CreateDefaultAccount(string host, string username, string password)
        {
            FtpAccountEntity account = new FtpAccountEntity();
            account.Host = host;
            account.Username = username;
            account.Password = SecureText.Encrypt(password, username);

            account.Port = 21;
            account.SecurityType = (int) FtpSecurityType.Unsecure;

            account.Passive = true;

            account.InternalOwnerID = null;
            account.ReuseControlConnectionSession = true;

            return account;
        }

        /// <summary>
        /// Create the most secure connection possible using the given settings.  Throws FileTransferException if it can't connect
        /// </summary>
        public static FtpAccountEntity CreateMostSecureAccount(string host, string username, string password)
        {
            FtpAccountEntity account = CreateDefaultAccount(host, username, password);

            // if the user specifies a port, use it instead of the defaults
            int portOverride = 0;
            if (host.IndexOf(":", StringComparison.OrdinalIgnoreCase) > -1)
            {
                string[] parts = host.Split(':');
                if (parts.Length == 2)
                {
                    if (int.TryParse(parts[1], out portOverride))
                    {
                        account.Port = portOverride;
                        account.Host = parts[0];
                    }
                }
            }

            // Explicit with ReuseControlConnectionSession true
            if (CheckFtpSecurityExplicit(portOverride, account, true))
            {
                return account;
            }

            // Explicit with ReuseControlConnectionSession false
            if (CheckFtpSecurityExplicit(portOverride, account, false))
            {
                return account;
            }

            // Implicit
            if (CheckFtpSecurityImplicit(portOverride, account))
            {
                return account;
            }

            // Unsecure
            if (CheckFtpSecurityUnsecure(portOverride, account))
            {
                return account;
            }

            // Sftp
            log.Info("Testing SFTP...");

            if (portOverride == 0)
            {
                account.Port = GetDefaultPort(FtpSecurityType.Sftp);
            }

            account.SecurityType = (int) FtpSecurityType.Sftp;

            try
            {
                TestDataTransfer(account);
                return account;
            }
            catch (FileTransferException ex)
            {
                log.Warn(ex.Message);
            }

            // We've tried everything we can, but couldn't connect.  Throw
            throw new FileTransferException("ShipWorks was unable to connect to the FTP site with the information provided.");
        }

        /// <summary>
        /// Check ftp security as unsecure
        /// </summary>
        private static bool CheckFtpSecurityUnsecure(int portOverride, FtpAccountEntity account)
        {
            log.InfoFormat("Testing unsecure FTP security...");

            // if not overridden, use port 21
            if (portOverride == 0)
            {
                account.Port = GetDefaultPort(FtpSecurityType.Unsecure);
            }

            account.SecurityType = (int) FtpSecurityType.Unsecure;

            return TestPassiveActiveModes(account);
        }

        /// <summary>
        /// Check ftp security as implicit
        /// </summary>
        private static bool CheckFtpSecurityImplicit(int portOverride, FtpAccountEntity account)
        {
            log.InfoFormat("Testing Implicit FTP security...");

            // if not overridden, use port 990
            if (portOverride == 0)
            {
                account.Port = GetDefaultPort(FtpSecurityType.Implicit);
            }
            account.SecurityType = (int) FtpSecurityType.Implicit;

            return TestPassiveActiveModes(account);
        }

        /// <summary>
        /// Check ftp security as explicit
        /// </summary>
        private static bool CheckFtpSecurityExplicit(int portOverride, FtpAccountEntity account, bool reuseControlConnectionSession)
        {
            log.InfoFormat("Testing Explicit FTP security...");

            // if not overridden, use port 21
            if (portOverride == 0)
            {
                account.Port = GetDefaultPort(FtpSecurityType.Explicit);
            }
            account.SecurityType = (int) FtpSecurityType.Explicit;
            account.ReuseControlConnectionSession = reuseControlConnectionSession;

            return TestPassiveActiveModes(account);
        }

        /// <summary>
        /// Test data transfer for passive then active modes.  If passive succeeds, return true.  If passive fails, try active, and return true if it succeeds.
        /// If both fail, return false.
        /// </summary>
        /// <param name="account">The ftp account entity with current ftp settings</param>
        /// <returns>True if either active or passive succeeds, false if both fail.  Sets account.Passive to the attempted mode.</returns>
        private static bool TestPassiveActiveModes(FtpAccountEntity account)
        {
            // Try passive mode
            try
            {
                account.Passive = true;
                TestDataTransfer(account);

                // No exception, return true
                return true;
            }
            catch (FileTransferException ex)
            {
                log.Warn(ex.Message);
            }

            // Try active
            try
            {
                account.Passive = false;
                TestDataTransfer(account);

                // No exception, return true
                return true;
            }
            catch (FileTransferException ex)
            {
                log.Warn(ex.Message);
            }

            // Neither succeded, return false.
            return false;
        }

        /// <summary>
        /// Test data transfer on the given account
        /// </summary>
        public static void TestDataTransfer(FtpAccountEntity account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            using (IFtp ftp = LogonToFtp(account))
            {
                try
                {
                    ftp.GetList();
                }
                catch (NetworkSessionException ex)
                {
                    log.Warn("Failed in call to GetList", ex);

                    throw new FileTransferException(string.Format("Could not establish data transfer in '{0}' mode.", account.Passive ? "Passive" : "Active"), ex);
                }
                finally
                {
                    if (ftp != null)
                    {
                        ftp.Disconnect();
                    }
                }
            }
        }

        /// <summary>
        /// Logon to the already connected FTP connection, with the credentials in the given account.
        /// </summary>
        private static void InternalLogon(IFtp ftp, FtpAccountEntity account)
        {
            try
            {
                ftp.Login(account.Username, SecureText.Decrypt(account.Password, account.Username));

                // Apply settings
                Ftp typedFtp = ftp as Ftp;
                if (typedFtp != null)
                {
                    typedFtp.Passive = account.Passive;
                }
            }
            catch (NetworkSessionException ex)
            {
                throw new FileTransferException("ShipWorks could not login with the given username and password.\n\nDetail: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Logon to the already connected FTP connection, with the credentials in the given account.
        /// </summary>
        private static async Task InternalLogonAsync(IFtp ftp, FtpAccountEntity account)
        {
            try
            {
                await ftp.LoginAsync(account.Username, SecureText.Decrypt(account.Password, account.Username)).ConfigureAwait(false);

                // Apply settings
                Ftp typedFtp = ftp as Ftp;
                if (typedFtp != null)
                {
                    typedFtp.Passive = account.Passive;
                }
            }
            catch (NetworkSessionException ex)
            {
                throw new FileTransferException("ShipWorks could not login with the given username and password.\n\nDetail: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Attempt to open the given FTP connection, throws FileTransferException if not successful
        /// </summary>
        private static IFtp OpenConnection(FtpAccountEntity account)
        {
            try
            {
                return account.SecurityType == (int) FtpSecurityType.Sftp ?
                    OpenSftpConnection(account) :
                    OpenFtpConnection(account);
            }
            catch (NetworkSessionException ex)
            {
                throw new FileTransferException("ShipWorks could not connect to the FTP host specified.\n\nDetail: " + ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                // Can happen when the host name is invalid
                throw new FileTransferException("ShipWorks could not connect to the FTP host specified.\n\nDetail: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Attempt to open the given FTP connection, throws FileTransferException if not successful
        /// </summary>
        private static async Task<IFtp> OpenConnectionAsync(FtpAccountEntity account)
        {
            try
            {
                return account.SecurityType == (int) FtpSecurityType.Sftp ?
                    await OpenSftpConnectionAsync(account).ConfigureAwait(false) :
                    await OpenFtpConnectionAsync(account).ConfigureAwait(false);
            }
            catch (NetworkSessionException ex)
            {
                throw new FileTransferException("ShipWorks could not connect to the FTP host specified.\n\nDetail: " + ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                // Can happen when the host name is invalid
                throw new FileTransferException("ShipWorks could not connect to the FTP host specified.\n\nDetail: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Attempt to open the given SFTP connection, throws FileTransferException if not successful
        /// </summary>
        private static IFtp OpenSftpConnection(FtpAccountEntity account)
        {
            Sftp ftp = new Sftp();
            ftp.Timeout = (int) TimeSpan.FromSeconds(10).TotalMilliseconds;
            ftp.Connect(account.Host, account.Port);

            return ftp;
        }

        /// <summary>
        /// Attempt to open the given SFTP connection, throws FileTransferException if not successful
        /// </summary>
        private static async Task<IFtp> OpenSftpConnectionAsync(FtpAccountEntity account)
        {
            Sftp ftp = new Sftp();
            ftp.Timeout = (int) TimeSpan.FromSeconds(10).TotalMilliseconds;
            await ftp.ConnectAsync(account.Host, account.Port).ConfigureAwait(false);

            return ftp;
        }

        /// <summary>
        /// Attempt to open the given FTP connection, throws FileTransferException if not successful
        /// </summary>
        private static IFtp OpenFtpConnection(FtpAccountEntity account)
        {
            Ftp ftp = new Ftp();
            ftp.Settings.SslAllowedVersions = TlsVersion.TLS12;
            
            ftp.Timeout = (int) TimeSpan.FromSeconds(10).TotalMilliseconds;
            ftp.Settings.ReuseControlConnectionSession = (bool) account.ReuseControlConnectionSession;

            ftp.Connect(account.Host, account.Port, GetSslMode(account));

            return ftp;
        }

        /// <summary>
        /// Attempt to open the given FTP connection, throws FileTransferException if not successful
        /// </summary>
        private static async Task<IFtp> OpenFtpConnectionAsync(FtpAccountEntity account)
        {
            Ftp ftp = new Ftp();
            ftp.Settings.SslAllowedVersions = TlsVersion.TLS12;
            ftp.Timeout = (int) TimeSpan.FromSeconds(10).TotalMilliseconds;
            ftp.Settings.ReuseControlConnectionSession = (bool) account.ReuseControlConnectionSession;
            ftp.Settings.SslAcceptAllCertificates = true;

            await ftp.ConnectAsync(account.Host, account.Port, GetSslMode(account)).ConfigureAwait(false);

            return ftp;
        }

        /// <summary>
        /// Get the SslMode for the account
        /// </summary>
        /// <remarks>Convert from our FtpSecurityType to Rebex SslMode</remarks>
        private static SslMode GetSslMode(FtpAccountEntity account) =>
            (FtpSecurityType) account.SecurityType == FtpSecurityType.Sftp ? SslMode.Explicit : (SslMode) account.SecurityType;

        /// <summary>
        /// Connect an login to the given FTP account
        /// </summary>
        public static IFtp LogonToFtp(FtpAccountEntity account)
        {
            IFtp ftp = OpenConnection(account);

            InternalLogon(ftp, account);

            return ftp;
        }

        /// <summary>
        /// Connect an login to the given FTP account
        /// </summary>
        public static async Task<IFtp> LogonToFtpAsync(FtpAccountEntity account)
        {
            IFtp ftp = await OpenConnectionAsync(account).ConfigureAwait(false);

            await InternalLogonAsync(ftp, account).ConfigureAwait(false);

            return ftp;
        }
    }
}
