﻿namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// Provides a name for delegates generated by FlowPreservation infrastructure and checks for it
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    static class MethodNameHelper
    {
        /// <summary>
        /// Default method name
        /// </summary>
        private static readonly string defaultMethodName = "<" + typeof(MethodNameHelper).Namespace + ">lambda_method";

        /// <summary>
        /// Get a lambda method name
        /// </summary>
        public static string GetLambdaMethodName() => defaultMethodName;

        /// <summary>
        /// Is this our lambda method
        /// </summary>
        public static bool IsOurLambdaMethod(string methodName) => defaultMethodName.Equals(methodName);
    }
}