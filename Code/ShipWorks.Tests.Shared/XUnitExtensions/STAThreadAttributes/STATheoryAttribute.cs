// From: https://github.com/xunit/samples.xunit/tree/master/STAExamples
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace ShipWorks.Tests.Shared.XUnitExtensions.STAThreadAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("ShipWorks.Tests.Shared.XUnitExtensions.STAThreadAttributes.STATheoryDiscoverer", "ShipWOrks.Tests.Shared")]
    public class STATheoryAttribute : TheoryAttribute { }
}
