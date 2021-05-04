using System;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Extensions;
using ShipWorks.ApplicationCore;
using ShipWorks.Common;
using ShipWorks.Tests.Shared;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Common
{
    public class ManagerBaseTest
    {
        private readonly ITestOutputHelper output;

        public ManagerBaseTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(typeof(IInitializeForCurrentSession))]
        [InlineData(typeof(ICheckForChangesNeeded))]
        public void Verify_AllImplementingClasses_ImplementExpectedInterfaces(Type toCheck)
        {
            var testType = typeof(ManagerBase<,>);

            var missingClasses =
                AssemblyProvider
                    .GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => IsSubclassOfRawGeneric(testType, t))
                    .Where(t => !t.GetInterfaces().Contains(toCheck))
                    .ToList();

            if (missingClasses.Any())
            {
                output.WriteLine($"Classes missing {toCheck.Name}:");
                foreach (var missingClass in missingClasses)
                {
                    output.WriteLine($" - {missingClass}");
                }
            }

            Assert.Empty(missingClasses);
        }

        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
            if (generic == toCheck)
            {
                return false;
            }
            
            while (toCheck != null && toCheck != typeof(object)) {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}