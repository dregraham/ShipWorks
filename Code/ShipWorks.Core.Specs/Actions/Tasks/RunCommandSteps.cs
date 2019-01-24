using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using BoDi;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Enums;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using TechTalk.SpecFlow;
using Xunit;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.Core.Specs.Actions.Tasks
{
    [Binding]
    public class RunCommandSteps : IDisposable
    {
        private ActionStepContext stepContext;
        private DataContext context;
        private RunCommandTask testObject;
        private Action<List<long>, ActionStepContext> runAction;
        private GenericResult<Unit> results;
        private readonly IObjectContainer objectContainer;
        private readonly DatabaseFixture db;

        public RunCommandSteps(IObjectContainer objectContainer, DatabaseFixture db)
        {
            this.objectContainer = objectContainer;
            this.db = db;
        }

        [BeforeScenario]
        public void SetupDatabaseContext()
        {
            stepContext = new ActionStepContext(
                new ActionQueueEntity { ActionVersion = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 } },
                new ActionQueueStepEntity { InputSource = (int) ActionTaskInputSource.Nothing },
                null);

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
            });

            objectContainer.RegisterInstanceAs(context);

            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "results");

            DataPath.Initialize(
                instancePath: Path.Combine(currentDirectory, "instance"),
                commonSettingsPath: Path.Combine(currentDirectory, "common"),
                tempPath: Path.Combine(currentDirectory, "temp")
            );

            LogSession.Initialize("integration-test");
            LogSession.Configure(new LogOptions { TraceToConsole = false });

            testObject = context.Mock.Create<RunCommandTask>();
            runAction = testObject.Run;
        }

        private IDictionary<string, OrderEntity> Orders =>
            objectContainer.Resolve<IDictionary<string, OrderEntity>>();

        [Given(@"the command ""(.*)""")]
        public void GivenTheCommand(string command) =>
            testObject.Command = command;

        [Given(@"the step input source ([a-zA-Z0-9_]*)")]
        public void GivenTheStepInputSourceSelection(ActionTaskInputSource inputSource) =>
            stepContext.Step.InputSource = (int) inputSource;

        [Given(@"a run cardinality of ([a-zA-Z0-9_]*)")]
        public void GivenARunCardinalityOfOneTime(RunCommandCardinality cardinality) =>
            testObject.RunCardinality = cardinality;

        [Given(@"a command timeout of (.*) seconds")]
        public void GivenACommandTimeoutOfSeconds(int timeoutInSeconds) =>
            testObject.CommandTimeout = TimeSpan.FromSeconds(timeoutInSeconds);

        [Given(@"should stop command on timeout is (.*)")]
        public void GivenShouldStopCommandOnTimeoutIsTrue(bool shouldTimeout) =>
            testObject.ShouldStopCommandOnTimeout = shouldTimeout;

        [When(@"I run the task with orders \((.*)\)")]
        public void WhenIRunTheTaskWithOrders(IEnumerable<string> orderNames) =>
            results = Try(() => runAction.ToFunc()(orderNames.Select(x => Orders[x].OrderID).ToList(), stepContext));

        [When(@"I run the task with no input")]
        public void WhenIRunTheTaskWithNoInput() =>
            results = Try(() => runAction.ToFunc()(new List<long>(), stepContext));

        [Then(@"the most recent log should contain ""(.*)""")]
        public void ThenTheMostRecentLogShouldContain(string value) =>
            Assert.Contains(value, File.ReadAllLines(LatestLogFile()).Select(x => x.Trim()));

        [Then(@"the ([0-9]+)(?:nd|rd|st|th)? most recent log should contain ""(.*)""")]
        public void ThenNthTheMostRecentLogShouldContain(int logNumber, string value) =>
            Assert.Contains(value, File.ReadAllLines(LatestLogFiles(logNumber).Last()).Select(x => x.Trim()));

        [Then(@"an error ""(.*)"" is shown")]
        public void ThenAnErrorIsShown(string message) =>
            results
                .Match(
                    x =>
                    {
                        Assert.False(true, "No exception was thrown when running the command");
                        return Unit.Default;
                    },
                    ex =>
                    {
                        Assert.IsType<ActionTaskRunException>(ex);
                        Assert.StartsWith(message, ex.Message);
                        return Unit.Default;
                    });

        [StepArgumentTransformation]
        public IEnumerable<string> TransformToListOfString(string commaSeparatedList) =>
            commaSeparatedList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());

        private static IEnumerable<string> LatestLogFiles(int count) =>
            Directory.EnumerateFiles(RunCommandTask.CommandLogFolder)
                .Where(x => x.EndsWith(".output.log", StringComparison.Ordinal))
                .OrderByDescending(x => x)
                .Take(count);

        private static string LatestLogFile() => LatestLogFiles(1).Single();

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
