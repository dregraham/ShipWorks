using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using log4net;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Utility class for working with the www
    /// </summary>
    public static class WebHelper
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(WebHelper));

        // Reflected method for preserving StackTrace when doing a rethrow
        private static MethodInfo exceptionInternalPreserverStackTrace = null;

        /// <summary>
        /// Static Constructor
        /// </summary>
        static WebHelper()
        {
            exceptionInternalPreserverStackTrace = typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);
            if (exceptionInternalPreserverStackTrace == null)
            {
                throw new InvalidOperationException("Could not reflect Exception.InternalPreserveStackTrace");
            }
        }

        /// <summary>
        /// Open the given URL in the users default browser.
        /// </summary>
        public static void OpenUrl(string url, IWin32Window errorOwner)
        {
            OpenUrl(new Uri(url), errorOwner);
        }

        /// <summary>
        /// Open the given URL in the users default browser.
        /// </summary>
        public static void OpenUrl(Uri uri, IWin32Window errorOwner)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            try
            {
                Process.Start(uri.OriginalString);
            }
            catch (Win32Exception ex)
            {
                log.Error(string.Format("Failed to open URL '{0}'", uri), ex);

                MessageHelper.ShowError(errorOwner, string.Format("ShipWorks could not open the URL '{0}'.\n\n({1})", uri, ex.Message));
            }
        }

        /// <summary>
        /// Open the computer's default mail composer with the given email address
        /// </summary>
        public static void OpenMailTo(string emailAddress, IWin32Window errorOwner)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                throw new ArgumentException("The email address cannot be blank.", "emailAddress");
            }

            if (!emailAddress.StartsWith("mailto:"))
            {
                emailAddress = "mailto:" + emailAddress;
            }

            try
            {
                Process.Start(emailAddress);
            }
            catch (Win32Exception ex)
            {
                log.Error(string.Format("Failed to email client '{0}'", emailAddress), ex);

                MessageHelper.ShowError(errorOwner, string.Format("ShipWorks could not open your default email application: \n\n{0}", ex.Message));
            }
        }

        /// <summary>
        /// If the given exception is an exception that is known to come from WebRequest based calls, or Soap based calls, it is wrapped in specified type and returned.  Otherwise
        /// the original exception is returned, but in such a way that when it is rethrown the StackTrace is not lost.
        /// </summary>
        public static Exception TranslateWebException(Exception ex, Type rethrowType)
        {
            if (IsWebException(ex))
            {
                Exception newEx = Activator.CreateInstance(rethrowType, ex.Message, ex) as Exception;
                if (newEx == null)
                {
                    throw new InvalidOperationException("rethrowType must be derived from Exception.");
                }

                return newEx;
            }
            else
            {
                // Preserver the StackTrace so when the caller rethrows its preserved as if it was never caught
                exceptionInternalPreserverStackTrace.Invoke(ex, null);
                return ex;
            }
        }

        /// <summary>
        /// Indicates if the given Exception is a type that can been thrown from WebRequest or Soap calls.
        /// </summary>
        public static bool IsWebException(Exception ex)
        {
            return
                ex is WebException ||
                ex is SoapException ||
                ex is TimeoutException ||
                ex is ProtocolViolationException ||
                ex is IOException ||
                ex is InvalidSoapException ||
                ex is SocketException;
        }

        /// <summary>
        /// Certificate policy that will validate all certificates, including expired ones.
        /// </summary>
        public static bool TrustAllCertificatePolicy(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
