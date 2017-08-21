using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// Filters out boilerplate eventing and framework code
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    sealed class ExternalCodeHelper
    {
        private static readonly string myNamespace = typeof(StackFrameSlim).Namespace;

        private static readonly Dictionary<string, string> eventInfrastructureMethods =
            new Dictionary<string, string>
            {
                { typeof(System.Diagnostics.Tracing.EventSource).FullName, null },
                { typeof(System.Runtime.CompilerServices.TaskAwaiter).FullName, "OutputWaitEtwEvents" },
                { typeof(StackTrace).FullName, "GetStackFramesInternal" },
                { "System.Threading.Tasks.TplEtwProvider", null },
                { "System.Diagnostics.Tracing.FrameworkEventSource", null }
            };

        private static readonly HashSet<string> externalClassesAndNamespaces =
            new HashSet<string>
            {
                typeof(System.Action).Namespace,
                typeof(System.Runtime.CompilerServices.AsyncTaskMethodBuilder).Namespace,
                typeof(System.Threading.ThreadPool).Namespace,
                typeof(System.Threading.Tasks.Task).Namespace,
                typeof(System.Threading.Tasks.Task).Namespace + ".Dataflow",
                typeof(System.Threading.Tasks.Task).Namespace + ".Dataflow.Internal",
                "Microsoft.VisualStudio.HostingProcess.HostProc",
                "Microsoft.Win32.SystemEvents",
                "System.AppDomain",
                "BaseThreadInitThunk",
                "_RtlUserThreadStart",
                "DestroyThread",
                "_NtWaitForSingleObject@",
                "_WaitForSingleObjectEx@",
                "_NtWaitForMultipleObjects@",
                "_WaitForMultipleObjectsEx@"
            };

        /// <summary>
        /// Whether method belongs to eventing infrastructure
        /// </summary>
        /// <param name="method">Method</param>
        /// <returns>Whether method belongs to eventing infrastructure</returns>
        public static bool IsEventInfrastructure(MethodBase method)
        {
            if (method == null)
            {
                return false;
            }

            if (method.DeclaringType == null)
            {
                return MethodNameHelper.IsOurLambdaMethod(method.Name);
            }

            if (method.DeclaringType.Namespace == myNamespace)
            {
                return true;
            }

            string expectedName = null;
            if (!eventInfrastructureMethods.TryGetValue(method.DeclaringType.FullName, out expectedName))
            {
                return false;
            }

            return expectedName == null || expectedName == method.Name;
        }

        /// <summary>
        /// Whether method belongs to eventing infrastructure
        /// </summary>
        /// <param name="method">Method</param>
        /// <returns>Whether method belongs to eventing infrastructure</returns>
        public static bool IsEventInfrastructure(string method)
        {
            if (method == null)
            {
                return false;
            }

            if (method.StartsWith(myNamespace + "."))
            {
                return true;
            }

            foreach (var pair in eventInfrastructureMethods)
            {
                if (method.StartsWith(pair.Key + "." + pair.Value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Whether method is boilerplate framework code
        /// </summary>
        /// <param name="method">Method</param>
        /// <returns>Whether method is boilerplate framework code</returns>
        public static bool IsExternalCode(MethodBase method)
        {
            if (method == null || method.DeclaringType == null)
            {
                return true;
            }

            return externalClassesAndNamespaces.Contains(method.DeclaringType.FullName) ||
                externalClassesAndNamespaces.Any(x => method.DeclaringType.Namespace.StartsWith(x, StringComparison.Ordinal));
        }

        /// <summary>
        /// Whether method is boilerplate framework code
        /// </summary>
        /// <param name="method">Method</param>
        /// <returns>Whether method is boilerplate framework code</returns>
        public static bool IsExternalCode(string method)
        {
            if (method == null)
            {
                return true;
            }

            if (method.StartsWith("@") || method.StartsWith("__") || method.Contains("::"))
            {
                return true;
            }

            foreach (var item in externalClassesAndNamespaces)
            {
                if (method.StartsWith(item.Contains(".") ? item + "." : item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}