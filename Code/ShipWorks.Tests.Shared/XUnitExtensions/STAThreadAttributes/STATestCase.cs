// From: https://github.com/xunit/samples.xunit/tree/master/STAExamples
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Interapptive.Shared.Utility;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ShipWorks.Tests.Shared.XUnitExtensions.STAThreadAttributes
{
    /// <summary>
    /// Wraps test cases for FactAttribute and TheoryAttribute so the test case runs on the STA thread
    /// </summary>
    [DebuggerDisplay(@"\{ class = {TestMethod.TestClass.Class.Name}, method = {TestMethod.Method.Name}, display = {DisplayName}, skip = {SkipReason} \}")]
    public class STATestCase : LongLivedMarshalByRefObject, IXunitTestCase
    {
        IXunitTestCase testCase;

        public STATestCase(IXunitTestCase testCase)
        {
            this.testCase = testCase;
        }

        /// <summary/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", error: true)]
        public STATestCase() { }

        public IMethodInfo Method => testCase.Method;

        [SuppressMessage("SonarQube", "SW0001:Wrap in exception monitor",
            Justification = "Errors will be handled correctly by the test framework")]
        public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                         IMessageBus messageBus,
                                         object[] constructorArguments,
                                         ExceptionAggregator aggregator,
                                         CancellationTokenSource cancellationTokenSource)
        {
            var tcs = new TaskCompletionSource<RunSummary>();
            var thread = new Thread(() =>
            {
                try
                {
                    // Set up the SynchronizationContext so that any awaits
                    // resume on the STA thread as they would in a GUI app.
                    SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());

                    // Start off the test method.
                    var testCaseTask = this.testCase.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource);

                    // Arrange to pump messages to execute any async work associated with the test.
                    var frame = new DispatcherFrame();
                    Task.Run(async delegate
                    {
                        try
                        {
                            await testCaseTask;
                        }
                        finally
                        {
                            // The test case's execution is done. Terminate the message pump.
                            frame.Continue = false;
                        }
                    });
                    Dispatcher.PushFrame(frame);

                    // Report the result back to the Task we returned earlier.
                    CopyTaskResultFrom(tcs, testCaseTask);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        public string DisplayName
        {
            get { return testCase.DisplayName; }
        }

        public string SkipReason
        {
            get { return testCase.SkipReason; }
        }

        public ISourceInformation SourceInformation
        {
            get { return testCase.SourceInformation; }
            set { testCase.SourceInformation = value; }
        }

        public ITestMethod TestMethod
        {
            get { return testCase.TestMethod; }
        }

        public object[] TestMethodArguments
        {
            get { return testCase.TestMethodArguments; }
        }

        public Dictionary<string, List<string>> Traits
        {
            get { return testCase.Traits; }
        }

        public string UniqueID
        {
            get { return testCase.UniqueID; }
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            testCase = info.GetValue<IXunitTestCase>("InnerTestCase");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("InnerTestCase", testCase);
        }

        [SuppressMessage("ShipWorks", "SW0002:Do not obfuscate variables",
            Justification = "We don't obfuscate tests, but if we did, we'd still want the actual name of the parameter")]
        private static void CopyTaskResultFrom<T>(TaskCompletionSource<T> tcs, Task<T> template)
        {
            MethodConditions.EnsureArgumentIsNotNull(tcs, nameof(tcs));
            MethodConditions.EnsureArgumentIsNotNull(template, nameof(template));

            if (!template.IsCompleted)
            {
                throw new ArgumentException("Task must be completed first.", nameof(template));
            }

            if (template.IsFaulted)
            {
                tcs.SetException(template.Exception);
            }
            else if (template.IsCanceled)
            {
                tcs.SetCanceled();
            }
            else
            {
                tcs.SetResult(template.Result);
            }
        }
    }
}
