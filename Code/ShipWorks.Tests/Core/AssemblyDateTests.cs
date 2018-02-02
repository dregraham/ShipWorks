using System;
using System.Reflection;
using Interapptive.Shared;
using Xunit;

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
        public void ReadAssemblyDateNonexistent()
        {
            // This will read it off the current assembly, which does not have the date
            Assert.Throws<InvalidOperationException>(() => AssemblyDateAttribute.Read());
        }

        [Fact]
        public void ReadAssemblyDateForNullAssembly()
        {
            Assert.Throws<ArgumentNullException>(() => AssemblyDateAttribute.Read(null));
        }

        [Fact]
        public void ReadResultAssemblyDate()
        {
            // If it can be read, it works.
            var result = AssemblyDateAttribute.ReadResult(Assembly.GetAssembly(typeof(AssemblyDateAttribute)));
            Assert.True(result.Success);
        }

        [Fact]
        public void ReadResultAssemblyDateNonexistent()
        {
            // This will read it off the current assembly, which does not have the date
            var result = AssemblyDateAttribute.ReadResult(Assembly.GetCallingAssembly());
            Assert.True(result.Failure);
        }

        [Fact]
        public void ReadResultAssemblyDateForNullAssembly()
        {
            var result = AssemblyDateAttribute.ReadResult(null);
            Assert.True(result.Failure);
        }
    }
}

