using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Interapptive.Shared;

namespace ShipWorks.Tests.Core
{
    [TestClass]
    public class AssemblyDateTests
    {
        [TestMethod]
        public void ReadAssemblyDate()
        {
            // If it can be read, it works.
            DateTime buildDate = AssemblyDateAttribute.Read(Assembly.GetAssembly(typeof(AssemblyDateAttribute)));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReadAssemblyDateNonexistent()
        {
            // This will read it off the current assembly, which does not have the date
            DateTime buildDate = AssemblyDateAttribute.Read();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadAssemblyDateForNullAssembly()
        {
            AssemblyDateAttribute.Read(null);
        }
    }
}

