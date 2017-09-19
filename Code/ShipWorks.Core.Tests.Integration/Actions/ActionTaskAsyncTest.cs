using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Tasks;
using ShipWorks.ApplicationCore;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms
{
    [Trait("Category", "IoCRegistration")]
    [Trait("Category", "ContinuousIntegration")]
    public class ActionTaskAsyncTest : IDisposable
    {
        readonly ITestOutputHelper testOutputHelper;
        readonly IContainer container;

        public ActionTaskAsyncTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            container = new ContainerBuilder().Build();
            ContainerInitializer.Initialize(container);
        }

        [Fact]
        public void CanResolveActionTasks()
        {
            var results = AssemblyProvider.GetShipWorksTypesInAppDomain()
                .Where(HasActionTaskAttribute)
                .Select(x => new { Type = x, Resolved = Resolve(x) })
                .Where(x => x.Resolved.Failure)
                .ToList();

            foreach (var result in results)
            {
                testOutputHelper.WriteLine($"Could not resolve {result.Type.Name}");
            }

            foreach (var result in results)
            {
                testOutputHelper.WriteLine(result.Resolved.Message);
            }

            Assert.Empty(results);
        }

        [Fact]
        public void EnsureCorrectRunMethodIsImplemented()
        {
            var results = AssemblyProvider.GetShipWorksTypesInAppDomain()
                .Where(HasActionTaskAttribute)
                .Select(x => new { Type = x, Resolved = Resolve(x) })
                .Select(x => new { x.Type, Instance = x.Resolved.Value })
                .Where(x => x.Instance != null)
                .Select(x => EnsureCorrectRunOverride(x.Type, x.Instance.IsAsync))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            foreach (var result in results)
            {
                testOutputHelper.WriteLine(result);
            }

            Assert.Empty(results);
        }

        private string EnsureCorrectRunOverride(Type type, bool isAsync)
        {
            ActionTask dummyAction = null;
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            var runMethodsOverridden = type.GetMethods(bindingFlags)
                .Where(x => x.Name == nameof(dummyAction.Run))
                .Select(x => new { BaseDef = x.GetBaseDefinition(), Actual = x })
                .Where(x => x.BaseDef.DeclaringType != x.Actual.DeclaringType)
                .Any();

            var runAsyncMethodsOverridden = type.GetMethods(bindingFlags)
                .Where(x => x.Name == nameof(dummyAction.RunAsync))
                .Where(x => x.GetBaseDefinition().DeclaringType != x.DeclaringType)
                .Any();

            if (isAsync)
            {
                if (runMethodsOverridden)
                {
                    return $"{type.Name} overrides a Run method, but has IsAsync set to true";
                }

                if (!runAsyncMethodsOverridden)
                {
                    return $"{type.Name} does not override a RunAsync method, and has IsAsync set to true";
                }
            }
            else
            {
                if (runAsyncMethodsOverridden)
                {
                    return $"{type.Name} overrides a RunAsync method, but has IsAsync set to false";
                }

                if (!runMethodsOverridden)
                {
                    return $"{type.Name} does not override a Run method, and has IsAsync set to false";
                }
            }

            return string.Empty;
        }

        private GenericResult<ActionTask> Resolve(Type x)
        {
            try
            {
                var instance = IoC.UnsafeGlobalLifetimeScope.Resolve(x);
                var typedInstance = instance as ActionTask;

                return typedInstance == null ?
                    GenericResult.FromError<ActionTask>($"{x.Name} could not be cast to ActionTask") :
                    GenericResult.FromSuccess(typedInstance);
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<ActionTask>(ex.GetBaseException());
            }
        }

        private bool HasActionTaskAttribute(Type arg) =>
            arg.GetCustomAttributes(typeof(ActionTaskAttribute), false)
                .OfType<ActionTaskAttribute>()
                .Any();

        public void Dispose() => container?.Dispose();
    }
}
