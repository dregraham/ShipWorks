// From: https://github.com/xunit/samples.xunit/tree/master/STAExamples
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ShipWorks.Tests.Shared.XUnitExtensions.STAThreadAttributes
{
    /// <summary>
    /// Discoverer for STA Theory tests
    /// </summary>
    public class STATheoryDiscoverer : IXunitTestCaseDiscoverer
    {
        readonly TheoryDiscoverer theoryDiscoverer;

        /// <summary>
        /// Constructor
        /// </summary>
        public STATheoryDiscoverer(IMessageSink diagnosticMessageSink)
        {
            theoryDiscoverer = new TheoryDiscoverer(diagnosticMessageSink);
        }

        /// <summary>
        /// Discover the STA Theory tests
        /// </summary>
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            return theoryDiscoverer.Discover(discoveryOptions, testMethod, factAttribute)
                                   .Select(testCase => new STATestCase(testCase));
        }
    }
}
