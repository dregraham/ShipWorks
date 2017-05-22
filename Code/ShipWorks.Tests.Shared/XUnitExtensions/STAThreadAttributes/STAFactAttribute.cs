using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace ShipWorks.Tests.Shared.XUnitExtensions.STAThreadAttributes
{
    /// <summary>
    /// STA Fact test
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("ShipWorks.Tests.Shared.XUnitExtensions.STAThreadAttributes.STAFactDiscoverer", "ShipWorks.Tests.Shared")]
    public class STAFactAttribute : FactAttribute { }
}
