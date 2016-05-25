using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Data;
using Interapptive.Shared.Net;
using System.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common.Threading;
using Interapptive.Shared;
using System.Collections.Generic;
using ShipWorks.Data.Connection;
using Interapptive.Shared.IO.Zip;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Crashes 
{
    /// <summary>
    /// Used to submit crash reports to interapptive
    /// </summary>
    public static class CrashSubmitter
    {
        // Use this for trying to submit crashes internally; there have been
        // problems tyring to connect to the other URL while inside the network
        //const string url = "http://intapp01/shipworks/crash.ashx";
        const string url = "http://springfield.interapptive.com/shipworks/crash.ashx";

        // Properties we dont want to display for exception output
        static Regex reUnwantedProperties = new Regex(@"^(StackTrace|Source|TargetSite|InnerException|Data)$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Submits a crash report to interapptive.  The response to show the user is returned.  If logFileName is null no logs are submitted, otherwise it must be
        /// a full path a log content file.
        /// </summary>
        public static CrashResponse Submit(Exception ex, string email, string comments, string logFileName)
        {
            HttpFilePostRequestSubmitter postRequest = new HttpFilePostRequestSubmitter();
            postRequest.Uri = new Uri(url);

            postRequest.Variables.Add("identifier", GetIdentifier(ex));
            postRequest.Variables.Add("version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            postRequest.Variables.Add("email", email);
            postRequest.Variables.Add("comments", comments);
            postRequest.Variables.Add("exceptionTitle", GetExceptionTitle(ex));
            postRequest.Variables.Add("exceptionSummary", GetExceptionSummary(ex));
            postRequest.Variables.Add("exception", GetExceptionDetail(ex));
            postRequest.Variables.Add("environment", GetEnvironmentInfo());
            postRequest.Variables.Add("assemblies", GetLoadedAssemblyList());

            if (!string.IsNullOrWhiteSpace(logFileName))
            {
                // Post the file
                postRequest.Files.Add(new HttpFile("crashlog", logFileName));
            }

            // Download and parse request
            using (IHttpResponseReader response = postRequest.GetResponse())
            {
                string responseText = response.ReadResult();

                return CrashResponse.Read(responseText);
            }
        }

        /// <summary>
        /// Formats the description of the exception into a unique identifiable string. The reason for 
        /// this method and not just a simpler way of producing the description is that this
        /// string will be used to find existing bugs in the database to add occurances to, instead of adding
        /// new bugs for each occurance.
        /// </summary>
        public static string GetIdentifier(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            // If its really a background exception, use the information from the actual exception to build the description
            BackgroundException backgroundEx = ex as BackgroundException;
            if (backgroundEx != null)
            {
                ex = backgroundEx.ActualException;
            }

            string versionFormat = "{0}.{1}.{2}.{3}";

            StringBuilder desc = new StringBuilder();

            // Add the version number
            Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            string version = String.Format(versionFormat,
                assemblyVersion.Major,
                assemblyVersion.Minor,
                assemblyVersion.Build,
                assemblyVersion.Revision);
            desc.AppendFormat("V{0} ", version);

            // Get the class name of the exception that occured
            desc.Append(ex.GetType().Name);

            // Now add exception message and inner exception message (if there is one)
            desc.AppendFormat("{0},{1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "None");

            // Now add the stack trace for exception location uniqueness
            desc.AppendFormat("{0}", ex.StackTrace);

            // Return result
            return desc.ToString();
        }

        /// <summary>
        /// Get the first method out of the given stack trace that is within our code
        /// </summary>
        private static string GetFirstShipWorksMethod(string stackTrace)
        {
            if (!string.IsNullOrWhiteSpace(stackTrace))
            {
                Regex reMethodReference = new Regex("at\\s+(?<methodname>[^(]+)\\(.*\\)", RegexOptions.IgnoreCase);

                foreach (string line in stackTrace.Split('\n', '\r'))
                {
                    Match ma = reMethodReference.Match(line);
                    if (ma.Success)
                    {
                        string method = ma.Groups["methodname"].Value;

                        List<string> ignoreNamespaces = new List<string> 
                            {
                                "System",
                                "Microsoft",
                                "SD.LLBLGen",
                                "Rebex",
                                "ActiproSoftware",
                                "ComponentFactory",
                                "Divelements",
                                "SandDock"
                            };

                        List<Type> ignoreTypes = new List<Type>
                            {
                                typeof(SqlAdapter)
                            };

                        if (ignoreNamespaces.Any(n => method.StartsWith(n, StringComparison.OrdinalIgnoreCase)))
                        {
                            continue;
                        }

                        if (ignoreTypes.Any(t => method.StartsWith(t.FullName, StringComparison.OrdinalIgnoreCase)))
                        {
                            continue;
                        }

                        return method;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get the content of the crash report
        /// </summary>
        public static string GetContent(Exception ex, string comments)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Title: " + GetIdentifier(ex));
            sb.AppendLine();

            // User Comments
            sb.AppendLine("User Comments:");
            if (string.IsNullOrEmpty(comments)) 
            {
                sb.AppendLine("   [None]");
            }
            else
            {
                foreach (string line in comments.Split('\n'))
                {
                    sb.AppendLine("   " + line.TrimEnd());
                }
            }
            sb.AppendLine();

            AppendExceptionDetail(ex, sb, "");

            AppendRunningSqlCommands(sb);

            // Other info
            sb.AppendLine("Environment:");
            sb.Append(GetEnvironmentInfo());
            sb.AppendLine();

            // Assemblies
            sb.AppendLine("Assemblies:");
            sb.Append(GetLoadedAssemblyList());

            return sb.ToString();
        }

        /// <summary>
        /// Appends any running sql queries, if SqlSession is configured.
        /// </summary>
        private static void AppendRunningSqlCommands(StringBuilder sb)
        {
            if (SqlSession.IsConfigured)
            {
                sb.AppendLine();
                sb.AppendLine(SqlUtility.GetRunningSqlCommands(SqlSession.Current.Configuration.GetConnectionString()));
                sb.AppendLine();
            }
        }

        /// <summary>
        /// Create a zip file of the logs to send, and return the filename
        /// </summary>
        public static string CreateCrashLogZip()
        {
            ZipWriter writer = new ZipWriter();

            DirectoryInfo logRoot = new DirectoryInfo(DataPath.LogRoot);

            // Zip up every file in the root (probably just crash.txt and shipworks.log - but could be rolled over logs)
            foreach (string logFile in Directory.GetFiles(LogSession.LogFolder))
            {
                writer.Items.Add(new ZipWriterFileItem(logFile, logRoot));
            }

            // API calls get logged to subfolders.  But we don't need all the logs - that would be a ton of data.  For shipping services
            // if you did lots of volume that basically includes every label in your log submission.  We'll just take at most the last Y calls from each,
            // which should more than cover it.
            foreach (string apiDirectory in Directory.GetDirectories(LogSession.LogFolder))
            {
                foreach (var fileInfo in Directory.GetFileSystemEntries(apiDirectory).Select(f => new FileInfo(f)).OrderByDescending(fi => fi.CreationTime).Take(6))
                {
                    writer.Items.Add(new ZipWriterFileItem(fileInfo.FullName, logRoot));
                }
            }

            // Save the log to temp
            string tempZipFile = Path.Combine(DataPath.CreateUniqueTempPath(), "log.zip");
            writer.Save(tempZipFile);

            return tempZipFile;
        }

        /// <summary>
        /// Get the title of the exception
        /// </summary>
        private static string GetExceptionTitle(Exception ex)
        {
            BackgroundException backgroundEx = ex as BackgroundException;
            if (backgroundEx != null)
            {
                ex = backgroundEx.ActualException;
            }

            return string.Format("{0}: {1}", ex.GetType().Name, ex.Message);
        }

        /// <summary>
        /// Get a summary of the exception, that can be used as body of our fb case
        /// </summary>
        private static string GetExceptionSummary(Exception ex)
        {
            BackgroundException backgroundEx = ex as BackgroundException;
            if (backgroundEx != null)
            {
                ex = backgroundEx.ActualException;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("In [{0}]: {1}: {2}", GetFirstShipWorksMethod(ex.StackTrace) ?? "Unknown", ex.GetType().Name, ex.Message);

            Exception inner = ex.InnerException;
            while (inner != null)
            {
                sb.AppendLine();
                sb.AppendLine("InnerException:");
                sb.AppendFormat("In [{0}]: {1}: {2}", GetFirstShipWorksMethod(inner.StackTrace) ?? "Unknown", inner.GetType().Name, inner.Message);

                inner = inner.InnerException;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Append the detail of the given exception, and recursively any inner exceptions
        /// </summary>
        public static string GetExceptionDetail(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            AppendExceptionDetail(ex, sb, "");

            AppendRunningSqlCommands(sb);

            return sb.ToString();
        }

        /// <summary>
        /// Append the detail of the given exception, and recursively any inner exceptions
        /// </summary>
        private static void AppendExceptionDetail(Exception ex, StringBuilder sb, string tab)
        {
            const string indent = "   ";

            // Exception Properties
            sb.AppendFormat("{0}Exception [{1}]", tab, ex.GetType().Name);
            sb.AppendLine();

            BackgroundException backgroundEx = ex as BackgroundException;
            if (backgroundEx != null)
            {
                sb.AppendFormat("{0}{1}Actual Exception:", tab, indent);
                sb.AppendLine();

                AppendExceptionDetail(backgroundEx.ActualException, sb, tab + indent);
                sb.AppendFormat("{0}{1}------------------------------------------------------------------", tab, indent);
                sb.AppendLine();

                sb.AppendFormat("{0}{1}Invoking Callstack:", tab, indent);
                sb.AppendLine();

                AppendStackTrace(backgroundEx.InvokingThreadTrace.ToString(), sb, tab, indent);
            }
            else
            {
                // Show the value of each property in the exception
                foreach (PropertyInfo property in ex.GetType().GetProperties())
                {
                    if (!reUnwantedProperties.Match(property.Name).Success)
                    {
                        object value = property.GetValue(ex, null);
                        if (value != null)
                        {
                            string message = value.ToString().Replace("\r\n", string.Format("\r\n{0}{1}{1}", tab, indent));

                            sb.AppendFormat("{0}{1}{2} = {3}", tab, indent, property.Name, message);
                            sb.AppendLine();
                        }
                    }
                }

                // StackTrace
                sb.AppendFormat("{0}{1}Callstack:", tab, indent);
                sb.AppendLine();

                AppendStackTrace(ex.StackTrace, sb, tab, indent);

                if (ex.InnerException != null)
                {
                    sb.AppendFormat("{0}{1}InnerException: ------------------------------------------------------------------", tab, indent);
                    sb.AppendLine();

                    AppendExceptionDetail(ex.InnerException, sb, tab + indent);
                }
            }
        }

        /// <summary>
        /// Append a formatted version of the given stack trace
        /// </summary>
        private static void AppendStackTrace(string stackTrace, StringBuilder sb, string tab, string indent)
        {
            if (string.IsNullOrEmpty(stackTrace))
            {
                sb.AppendFormat("{0}{1}{1}(None)", tab, indent);
                sb.AppendLine();
            }
            else
            {
                foreach (string line in stackTrace.Split('\n', '\r'))
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        sb.AppendFormat("{0}{1}{1}{2}", tab, indent, line.Trim());
                        sb.AppendLine();
                    }
                }
            }
        }

        /// <summary>
        /// Get details about the current execution environment
        /// </summary>
        [NDependIgnoreLongMethod]
        private static string GetEnvironmentInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("OS: {0}\r\n", Environment.OSVersion.VersionString);
            sb.AppendFormat("x64 OS: {0}\r\n", MyComputer.Is64BitWindows);
            sb.AppendFormat("x64 Process: {0}\r\n", MyComputer.Is64BitProcess);
            sb.AppendFormat("IE Version: {0}\r\n", MyComputer.IEVersion);
            sb.AppendFormat("Windows Installer: {0}\r\n", MyComputer.WindowsInstallerVersion);
            sb.AppendFormat("CurrentCulture: {0}\r\n", Thread.CurrentThread.CurrentCulture);
            sb.AppendFormat("CurrentUICulture: {0}\r\n", Thread.CurrentThread.CurrentUICulture);

            sb.AppendFormat("Machine Name: {0}\r\n", Environment.MachineName);
            sb.AppendFormat("User Name: {0}\r\n", Environment.UserName);
            sb.AppendFormat("User Domain Name: {0}\r\n", Environment.UserDomainName);
            sb.AppendFormat("User Interactive: {0}\r\n", Environment.UserInteractive);

            AppendLineIgnoreException(() => sb.AppendFormat("Store Licenses: {0}\r\n", GetAllLicenses()));

            AppendLineIgnoreException(() => sb.AppendFormat("Execution Mode: {0}\r\n", Program.ExecutionMode.Name));
            AppendLineIgnoreException(() => sb.AppendFormat("Execution Mode IsUIDisplayed: {0}\r\n", Program.ExecutionMode.IsUIDisplayed));
            AppendLineIgnoreException(() => sb.AppendFormat("Execution Mode IsUISupported: {0}\r\n", Program.ExecutionMode.IsUISupported));
            
            AppendLineIgnoreException(() => sb.AppendFormat("Local IP Address: {0}\r\n", new NetworkUtility().GetIPAddress()));

            if (SqlSession.IsConfigured)
            {
                AppendLineIgnoreException(() => sb.AppendFormat("Sql Server Instance Name: {0}\r\n", SqlSession.Current.Configuration.ServerInstance));
                AppendLineIgnoreException(() => sb.AppendFormat("Sql Server Database Name: {0}\r\n", SqlSession.Current.Configuration.DatabaseName));
                AppendLineIgnoreException(() => sb.AppendFormat("Sql Server Is LocalDB: {0}\r\n", SqlSession.Current.Configuration.IsLocalDb()));

                AppendLineIgnoreException(() => sb.AppendFormat("Sql Server Machine Name: {0}\r\n", SqlSession.Current.GetServerMachineName()));
                AppendLineIgnoreException(() => sb.AppendFormat("Sql Server Version: {0}\r\n", SqlSession.Current.GetServerVersion()));
                AppendLineIgnoreException(() => sb.AppendFormat("Sql Server Is Local Server: {0}\r\n", SqlSession.Current.IsLocalServer()));
            }
            else
            {
                AppendLineIgnoreException(() => sb.AppendFormat("Sql Session reported that it is not configured."));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Joins all store licenses as a pipe delimited string
        /// </summary>
        private static string GetAllLicenses()
        {
            return StoreManager.GetAllStores().Select(s => s.License).Aggregate((i, j) => i + "|" + j);
        }

        /// <summary>
        /// Helper method to run code and ignore any exceptions if thrown.
        /// </summary>
        private static void AppendLineIgnoreException(Action method)
        {
            try
            {
                method();
            }
            catch 
            {
            }
        }

        /// <summary>
        /// Appends the list of loaded assemblies to the extra information list
        /// </summary>
        private static string GetLoadedAssemblyList()
        {
            StringBuilder assemblies = new StringBuilder();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                assemblies.AppendLine(assembly.FullName);
            }

            return assemblies.ToString();
        }
    }
}
