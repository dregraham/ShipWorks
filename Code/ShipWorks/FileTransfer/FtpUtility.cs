﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using Rebex.Net;
using log4net;
using Interapptive.Shared.Utility;

namespace ShipWorks.FileTransfer
{
    /// <summary>
    /// FTP utility functions
    /// </summary>
    public static class FtpUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FtpUtility));

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

            // Excplicit
            {
                log.InfoFormat("Testing Explicit FTP security...");

                // if not overridden, use port 21
                if (portOverride == 0)
                {
                    account.Port = 21;
                }
                account.SecurityType = (int) FtpSecurityType.Explicit;

                if (TestPassiveActiveModes(account))
                {
                    return account;
                }
            }

            // Implicit
            {
                log.InfoFormat("Testing Implicit FTP security...");

                // if not overridden, use port 990
                if (portOverride == 0)
                {
                    account.Port = 990;
                }
                account.SecurityType = (int) FtpSecurityType.Implicit;
                                                
                if (TestPassiveActiveModes(account))
                {
                    return account;
                }
            }

            // Unsecure
            {
                log.InfoFormat("Testing unsecure FTP security...");

                // if not overridden, use port 21
                if (portOverride == 0)
                {
                    account.Port = 21;
                }

                account.SecurityType = (int) FtpSecurityType.Unsecure;
                    
                if (TestPassiveActiveModes(account))
                {
                    return account;
                }
            }

            // We've tried everything we can, but couldn't connect.  Throw
            throw new FileTransferException("ShipWorks was unable to connect to the FTP site with the information provided.");
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

            using (Ftp ftp = LogonToFtp(account))
            {
                try
                {
                    ftp.GetList();
                }
                catch (FtpException ex)
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
        /// Logon to the already connected FTP connection, with the credentials in teh given acount.
        /// </summary>
        private static void InternalLogon(Ftp ftp, FtpAccountEntity account)
        {
            try
            {
                ftp.Login(account.Username, SecureText.Decrypt(account.Password, account.Username));
                
                // Apply settings
                ftp.Passive = account.Passive;
            }
            catch (FtpException ex)
            {
                throw new FileTransferException("ShipWorks could not login with the given username and password.\n\nDetail: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Attempt to open the given FTP connection, throws FileTransferException if not succesful
        /// </summary>
        private static Ftp OpenConnection(FtpAccountEntity account)
        {
            TlsParameters tls = new TlsParameters();
            tls.CertificateVerifier = CertificateVerifier.AcceptAll;

            try
            {
                Ftp ftp = new Ftp();
                ftp.Timeout = (int) TimeSpan.FromSeconds(10).TotalMilliseconds;
                ftp.Connect(account.Host, account.Port, tls, (FtpSecurity) account.SecurityType);

                return ftp;
            }
            catch (FtpException ex)
            {
                throw new FileTransferException("ShipWorks could not connect to the FTP host specified.\n\nDetail: " + ex.Message, ex);
            }
            catch (TlsException ex)
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
        /// Connect an login to the given FTP account
        /// </summary>
        public static Ftp LogonToFtp(FtpAccountEntity account)
        {
            Ftp ftp = OpenConnection(account);

            InternalLogon(ftp, account);

            return ftp;
        }
    }
}
