using ShipWorks.Actions.Tasks;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Actions.Tasks
{
    public class ActionTaskTest
    {
        private readonly ITestOutputHelper outputHelper;

        public ActionTaskTest(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        [Fact]
        public void ActionTasksHaveEmptyConstructor()
        {

            var assemblies = AssemblyProvider.GetAssemblies();

            List<Type> actionTasks = assemblies
                .SelectMany(t => t.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(ActionTask)))
                .Where(t => !t.IsAbstract)
                .Where(t => t.GetConstructor(Type.EmptyTypes) == null)
                .OrderBy(t => t.FullName)
                .ToList();

            actionTasks.ForEach(t=>outputHelper.WriteLine(t.FullName));

            Assert.Empty(actionTasks);
        }
    }
}
