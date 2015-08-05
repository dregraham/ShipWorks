using System;
using System.Linq.Expressions;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Extensions on the object object
    /// </summary>
    public static class ObjectUtility
    {
        /// <summary>
        /// Get the name of a member identified by the expression
        /// </summary>
        /// <remarks>If/When we switch to VS2015, this should be removed in favor of the nameof operator</remarks>
        public static string Nameof<T>(Expression<Func<T>> expression)
        {
            MemberExpression memberExpress = expression.Body as MemberExpression;
            if (memberExpress != null)
            {
                return memberExpress.Member.Name;
            }

            MethodCallExpression methodCallExpress = expression.Body as MethodCallExpression;
            if (methodCallExpress != null)
            {
                return methodCallExpress.Method.Name;
            }

            return null;
        }
    }
}
