using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShipWorks.Tests.Shipping
{
    [TestClass]
    public class ChildShipmentEntityTest
    {
        [TestMethod]
        public void Verify_ChildShipmentEntitiesOverride_OnFieldValueChanged_Test()
        {
            List<string> ignoreShipmentTypeNames = new List<string>
            {
                "ShipmentEntity".ToUpperInvariant(),
                "PostalShipmentEntity".ToUpperInvariant(),
                "WorldShipShipmentEntity".ToUpperInvariant(),
                "OtherShipmentEntity".ToUpperInvariant()
            };

            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.IsClass && 
                    t.Namespace == @"ShipWorks.Data.Model.EntityClasses" && 
                    t.Name.ToUpperInvariant().Contains("ShipmentEntity".ToUpperInvariant()) &&
                    !ignoreShipmentTypeNames.Contains(t.Name.ToUpperInvariant()));

            foreach (Type type in types)
            {
                MethodInfo methodInfo = type.GetMethod("OnFieldValueChanged", BindingFlags.Instance |
                                                                              BindingFlags.DeclaredOnly |
                                                                              BindingFlags.Public |
                                                                              BindingFlags.NonPublic);

                Assert.IsNotNull(methodInfo, string.Format("Missing 'OnFieldValueChanged' for type {0}", type.Name));
            }
        }
    }
}
