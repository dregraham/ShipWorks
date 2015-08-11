using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Reflection;
using Interapptive.Shared;

namespace ShipWorks.Tests.Core
{
    public class AssemblyDateTests
    {
        [Fact]
        public void ReadAssemblyDate()
        {
            // If it can be read, it works.
            DateTime buildDate = AssemblyDateAttribute.Read(Assembly.GetAssembly(typeof(AssemblyDateAttribute)));
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReadAssemblyDateNonexistent()
        {
            // This will read it off the current assembly, which does not have the date
            DateTime buildDate = AssemblyDateAttribute.Read();
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadAssemblyDateForNullAssembly()
        {
            AssemblyDateAttribute.Read(null);
        }
    }
}

