using Moq;
using System;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Houses a Mocked function and the Mocked returned value.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    public class MockedFuncAndOutput<TInput, TOutput> where TOutput : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockedFuncAndOutput{TInput, TOutput}"/> class.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="functionOutput">The function output.</param>
        public MockedFuncAndOutput(Mock<Func<TInput, TOutput>> function, Mock<TOutput> functionOutput)
        {
            Function = function;
            FunctionOutput = functionOutput;
        }

        /// <summary>
        /// Gets the mocked function.
        /// </summary>
        public Mock<Func<TInput, TOutput>> Function { get; }
        
        /// <summary>
        /// Gets the mocked function output.
        /// </summary>
        public Mock<TOutput> FunctionOutput { get; }
    }
}