using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Moq;

namespace ShipWorks.Tests
{
    /// <summary>
    /// Extension methods for Moq functionality
    /// </summary>
    public static class MoqExtensions
    {
        /// <summary>
        /// Setup multiple return values based on how many times a method is called
        /// </summary>
        /// <typeparam name="TSvc">Type for which the Mock has been generated</typeparam>
        /// <typeparam name="TReturn">Type of the return value</typeparam>
        /// <param name="mock">The mock that will be setup with return values</param>
        /// <param name="expression">Expression for which to return values</param>
        /// <param name="args">The list of values that will be returned in order</param>
        public static void SetupMany<TSvc, TReturn>(this Mock<TSvc> mock,
            Expression<Func<TSvc, TReturn>> expression,
            params TReturn[] args)
            where TSvc : class
        {
            var numCalls = 0;

            mock.Setup(expression)
                .Returns(() => numCalls < args.Length ? args[numCalls] : args[args.Length - 1])
                .Callback(() => numCalls++);
        }
    }
}
