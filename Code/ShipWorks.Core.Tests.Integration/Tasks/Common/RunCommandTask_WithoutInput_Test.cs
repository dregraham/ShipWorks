using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Interapptive.Shared.UI;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Enums;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Shared.Database.Tasks
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class RunCommandTask_WithInput_Test
    {
        private readonly ActionStepContext stepContext;
        private readonly string currentDirectory;
        private readonly DataContext context;
        private readonly RunCommandTask testObject;

        public RunCommandTask_WithInput_Test(DatabaseFixture db)
        {
            stepContext = new ActionStepContext(
                new ActionQueueEntity { ActionVersion = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 } },
                new ActionQueueStepEntity { InputSource = (int) ActionTaskInputSource.Nothing },
                null);

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
            });

            currentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "results");

            DataPath.Initialize(
                instancePath: Path.Combine(currentDirectory, "instance"),
                commonSettingsPath: Path.Combine(currentDirectory, "common"),
                tempPath: Path.Combine(currentDirectory, "temp")
            );

            LogSession.Initialize("integration-test");

            testObject = context.Mock.Create<RunCommandTask>();
        }

        [Fact]
        public void Run_ExecutesCommandWithNoOrders_LogsOutput()
        {
            var date = DateTime.Now.ToString();
            testObject.Command = $"echo {date}";

            testObject.Run(new List<long>(), stepContext);

            Assert.Contains("O> " + date, File.ReadAllLines(LatestLogFile()));
        }

        [Fact]
        public void Run_ExecutesCommandWithNoOrders_LogsErorr()
        {
            var date = DateTime.Now.ToString();
            testObject.Command = $"echo {date} 1>&2";

            testObject.Run(new List<long>(), stepContext);

            Assert.Contains("E> " + date + " ", File.ReadAllLines(LatestLogFile()));
        }

        [Fact]
        public void Run_ExecutesCommandWithOneOrder_LogsOutput()
        {
            var order = Create.Order(context.Store, context.Customer)
                .Set(x => x.OrderNumber = 1234)
                .Save();

            stepContext.Step.InputSource = (int) ActionTaskInputSource.Selection;

            testObject.Command = "echo Order #{//Order/Number}";

            testObject.Run(new List<long> { order.OrderID }, stepContext);

            Assert.Contains("O> Order #1234", File.ReadAllLines(LatestLogFile()));
        }

        [Fact]
        public void Run_ExecutesCommandWithTwoOrders_AndSingleCardinality_LogsOutput()
        {
            var order = Create.Order(context.Store, context.Customer)
                .Set(x => x.OrderNumber = 1234)
                .Save();
            var order2 = Create.Order(context.Store, context.Customer)
                .Set(x => x.OrderNumber = 5678)
                .Save();

            stepContext.Step.InputSource = (int) ActionTaskInputSource.Selection;
            testObject.RunCardinality = RunCommandCardinality.OneTime;

            testObject.Command = "echo <xsl:for-each select=\"//Order\"><xsl:text>Order #</xsl:text><xsl:value-of select=\"Number\" /><xsl:text> </xsl:text></xsl:for-each>";

            testObject.Run(new List<long> { order.OrderID, order2.OrderID }, stepContext);

            Assert.Contains("O> Order #1234 Order #5678 ", File.ReadAllLines(LatestLogFile()));
        }

        [Fact]
        public void Run_ExecutesCommandWithTwoOrders_AndMultipleCardinality_LogsMultipleOutput()
        {
            var order = Create.Order(context.Store, context.Customer)
                .Set(x => x.OrderNumber = 1234)
                .Save();
            var order2 = Create.Order(context.Store, context.Customer)
                .Set(x => x.OrderNumber = 5678)
                .Save();

            stepContext.Step.InputSource = (int) ActionTaskInputSource.Selection;
            testObject.RunCardinality = RunCommandCardinality.OncePerFilterEntry;

            testObject.Command = "echo Order #{//Order/Number}";

            testObject.Run(new List<long> { order.OrderID, order2.OrderID }, stepContext);

            var logFiles = LatestLogFiles(2);
            Assert.Equal(2, logFiles.Count());
            Assert.Contains("O> Order #1234", File.ReadAllLines(logFiles.Last()));
            Assert.Contains("O> Order #5678", File.ReadAllLines(logFiles.First()));
        }

        [Fact]
        public void Run_FinishesBeforeTimeout_FinishesSuccessfully()
        {
            testObject.Command = $"ping localhost -n 3";
            testObject.CommandTimeout = TimeSpan.FromSeconds(4);
            testObject.ShouldStopCommandOnTimeout = true;

            testObject.Run(new List<long>(), stepContext);

            Assert.Contains("O> Approximate round trip times in milli-seconds:", File.ReadAllLines(LatestLogFile()));
        }

        [Fact]
        public void Run_TakesLongerThanTimeout_ProcessIsKilled()
        {
            testObject.Command = $"ping localhost -n 9";
            testObject.CommandTimeout = TimeSpan.FromSeconds(2);
            testObject.ShouldStopCommandOnTimeout = true;

            var exception = Assert.Throws<ActionTaskRunException>(() => testObject.Run(new List<long>(), stepContext));

            Assert.StartsWith("The command took longer than", exception.Message);
        }

        [Fact]
        public void Run_TakesLongerThanTimeout_ProcessWithSubprocessesIsKilled()
        {
            testObject.Command = $"start /B /wait ping localhost -n 9";
            testObject.CommandTimeout = TimeSpan.FromSeconds(2);
            testObject.ShouldStopCommandOnTimeout = true;

            var exception = Assert.Throws<ActionTaskRunException>(() => testObject.Run(new List<long>(), stepContext));

            Assert.StartsWith("The command took longer than", exception.Message);
        }

        private static IEnumerable<string> LatestLogFiles(int count) =>
            Directory.EnumerateFiles(RunCommandTask.CommandLogFolder)
                .Where(x => x.EndsWith(".output.log", StringComparison.Ordinal))
                .OrderByDescending(x => x)
                .Take(count);

        private static string LatestLogFile() => LatestLogFiles(1).Single();
    }
}