using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ShipWorks.Tests.Shared.Expressions
{
    /// <summary>
    /// Class helper for testing various things about a class
    /// </summary>
    public class ClassHelper<TClass>
    {
        /// <summary>
        /// Get the return type of a property or method
        /// </summary>
        public Type GetReturnTypeOf<T>(Expression<Func<TClass, T>> expr)
        {
            var memberExpression = expr.Body as MemberExpression;
            return memberExpression.Member is MethodInfo ?
                ((MethodInfo) memberExpression.Member).ReturnType :
                ((PropertyInfo) memberExpression.Member).PropertyType;
        }
    }
}