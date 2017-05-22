using System;
// From: https://github.com/xunit/samples.xunit/tree/master/STAExamples
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ShipWorks.Tests.Shared.XUnitExtensions.STAThreadAttributes
{
    /// <summary>
    /// Discover STA Fact tests
    /// </summary>
    public class STAFactDiscoverer : IXunitTestCaseDiscoverer
    {
        readonly FactDiscoverer factDiscoverer;

        /// <summary>
        /// Constructor
        /// </summary>
        public STAFactDiscoverer(IMessageSink diagnosticMessageSink)
        {
            factDiscoverer = new FactDiscoverer(diagnosticMessageSink);
        }

        /// <summary>
        /// Discover test cases
        /// </summary>
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            return factDiscoverer.Discover(discoveryOptions, testMethod, factAttribute)
                                 .Select(testCase => new STATestCase(testCase));
        }
    }
}
