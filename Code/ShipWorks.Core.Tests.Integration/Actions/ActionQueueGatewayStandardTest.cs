using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Moq;
using ShipWorks.Actions;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Actions
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ActionQueueGatewayStandardTest
    {
        private readonly DataContext context;
        private Mock<IConfigurationEntity> config = new Mock<IConfigurationEntity>();
        private Mock<IConfigurationData> configData = new Mock<IConfigurationData>();
        private readonly ActionEntity defaultPrintEnabledAction;
        private readonly ActionEntity nondefaultPrintEnabledAction;
        private readonly ActionEntity scheduledPrintAction;
        private readonly ShipmentEntity shipment;
        private readonly ActionEntity finishProcessingBatch;

        public ActionQueueGatewayStandardTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            defaultPrintEnabledAction = ShippingActionUtility.GetPrintAction(ShipmentTypeCode.PostalWebTools);

            nondefaultPrintEnabledAction = new ActionEntity()
            {
                Name = "Test Action",
                Enabled = true,
                ComputerLimitedType = 0,
                ComputerLimitedList = Enumerable.Empty<long>().ToArray(),
                StoreLimited = false,
                StoreLimitedList = Enumerable.Empty<long>().ToArray(),
                TriggerType = (int) ActionTriggerType.UserInitiated,
                InternalOwner = null,
                TriggerSettings = "<Settings/>",
                TaskSummary = "Test action task summary"
            };

            scheduledPrintAction = new ActionEntity()
            {
                Name = "Scheduled Test Action",
                Enabled = true,
                ComputerLimitedType = 0,
                ComputerLimitedList = Enumerable.Empty<long>().ToArray(),
                StoreLimited = false,
                StoreLimitedList = Enumerable.Empty<long>().ToArray(),
                TriggerType = (int) ActionTriggerType.Scheduled,
                InternalOwner = null,
                TriggerSettings = "<Settings/>",
                TaskSummary = "Scheduled Test action task summary"
            };

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                ActionManager.SaveAction(nondefaultPrintEnabledAction, sqlAdapter);
                ActionManager.SaveAction(scheduledPrintAction, sqlAdapter);
            }

            finishProcessingBatch = ActionManager.Actions.FirstOrDefault(a => a.Name == "Finish Processing Batch");

            shipment = Create.Shipment(context.Order).AsPostal().Save();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AnyWorkToDo_ReturnsFalse_WhenEmptyActionQueue(bool useParallelActionQueue)
        {
            config = new Mock<IConfigurationEntity>();
            configData = new Mock<IConfigurationData>();

            config.Setup(c => c.UseParallelActionQueue).Returns(useParallelActionQueue);
            configData.Setup(cd => cd.FetchReadOnly()).Returns(config.Object);
            configData.Setup(cd => cd.ExecutionModeActionQueueType).Returns(ActionQueueType.UserInterface);
            configData.Setup(cd => cd.IncludeUserInterfaceActions).Returns(true);

            bool anyWorkToDo = true;

            ActionQueueGatewayStandard testObject = new ActionQueueGatewayStandard(configData.Object);
            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlAdapter.ConnectionString))
                {
                    sqlConnection.Open();
                    anyWorkToDo = testObject.AnyWorkToDo(sqlConnection);
                }
            }

            Assert.False(anyWorkToDo);
        }

        [Fact]
        public void AnyWorkToDo_ReturnsFalse_WhenDefaultPrintEnabledEnabledAndActionQueueHasdefaultPrintEnabledEntryButNoStandardEntry()
        {
            bool anyWorkToDo = true;

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                DispatchAction(defaultPrintEnabledAction, shipment.ShipmentID, sqlAdapter, "");
                DispatchAction(finishProcessingBatch, null, sqlAdapter, "");

                config.Setup(c => c.UseParallelActionQueue).Returns(true);
                configData.Setup(cd => cd.FetchReadOnly()).Returns(config.Object);
                configData.Setup(cd => cd.ExecutionModeActionQueueType).Returns(ActionQueueType.UserInterface);
                configData.Setup(cd => cd.IncludeUserInterfaceActions).Returns(true);

                ActionQueueGatewayStandard testObject = new ActionQueueGatewayStandard(configData.Object);

                using (SqlConnection sqlConnection = new SqlConnection(sqlAdapter.ConnectionString))
                {
                    sqlConnection.Open();
                    anyWorkToDo = testObject.AnyWorkToDo(sqlConnection);
                }
            }

            Assert.False(anyWorkToDo);
        }

        [Theory]
        [InlineData(true, ActionQueueType.DefaultPrint, true, true)]
        [InlineData(true, ActionQueueType.DefaultPrint, false, true)]
        [InlineData(true, ActionQueueType.Scheduled, true, true)]
        [InlineData(true, ActionQueueType.Scheduled, false, true)]
        [InlineData(true, ActionQueueType.UserInterface, true, true)]
        [InlineData(true, ActionQueueType.UserInterface, false, true)]
        [InlineData(false, ActionQueueType.DefaultPrint, true, true)]
        [InlineData(false, ActionQueueType.DefaultPrint, false, true)]
        [InlineData(false, ActionQueueType.Scheduled, true, true)]
        [InlineData(false, ActionQueueType.Scheduled, false, true)]
        [InlineData(false, ActionQueueType.UserInterface, true, true)]
        [InlineData(false, ActionQueueType.UserInterface, false, true)]
        public void AnyWorkToDo_ExpectedValueIsCorrect_WhenAllQueueTypes(bool defaultPrintEnabled,
                ActionQueueType actionQueueType,
                bool includeUserInterfaceActions,
                bool expectedValue)
        {
            bool anyWorkToDo = true;

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                DispatchAction(nondefaultPrintEnabledAction, shipment.ShipmentID, sqlAdapter, "");
                DispatchAction(scheduledPrintAction, shipment.ShipmentID, sqlAdapter, "");
                DispatchAction(defaultPrintEnabledAction, shipment.ShipmentID, sqlAdapter, "");
                DispatchAction(finishProcessingBatch, null, sqlAdapter, "");

                config.Setup(c => c.UseParallelActionQueue).Returns(defaultPrintEnabled);
                configData.Setup(cd => cd.FetchReadOnly()).Returns(config.Object);
                configData.Setup(cd => cd.ExecutionModeActionQueueType).Returns(actionQueueType);
                configData.Setup(cd => cd.IncludeUserInterfaceActions).Returns(includeUserInterfaceActions);

                ActionQueueGatewayStandard testObject = new ActionQueueGatewayStandard(configData.Object);

                using (SqlConnection sqlConnection = new SqlConnection(sqlAdapter.ConnectionString))
                {
                    sqlConnection.Open();
                    anyWorkToDo = testObject.AnyWorkToDo(sqlConnection);
                }
            }

            Assert.Equal(expectedValue, anyWorkToDo);
        }

        [Theory]
        [InlineData(true, ActionQueueType.DefaultPrint, true, true)]
        [InlineData(true, ActionQueueType.DefaultPrint, false, false)]
        [InlineData(true, ActionQueueType.Scheduled, true, true)]
        [InlineData(true, ActionQueueType.Scheduled, false, true)]
        [InlineData(true, ActionQueueType.UserInterface, true, true)]
        [InlineData(true, ActionQueueType.UserInterface, false, true)]
        [InlineData(false, ActionQueueType.DefaultPrint, true, true)]
        [InlineData(false, ActionQueueType.DefaultPrint, false, false)]
        [InlineData(false, ActionQueueType.Scheduled, true, true)]
        [InlineData(false, ActionQueueType.Scheduled, false, true)]
        [InlineData(false, ActionQueueType.UserInterface, true, true)]
        [InlineData(false, ActionQueueType.UserInterface, false, true)]
        public void AnyWorkToDo_ExpectedValueIsCorrect_WhenNoDefaultPrintEnabledQueueEntries(bool defaultPrintEnabled,
                ActionQueueType actionQueueType,
                bool includeUserInterfaceActions,
                bool expectedValue)
        {
            bool anyWorkToDo = true;

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                DispatchAction(nondefaultPrintEnabledAction, shipment.ShipmentID, sqlAdapter, "");
                DispatchAction(scheduledPrintAction, shipment.ShipmentID, sqlAdapter, "");

                config.Setup(c => c.UseParallelActionQueue).Returns(defaultPrintEnabled);
                configData.Setup(cd => cd.FetchReadOnly()).Returns(config.Object);
                configData.Setup(cd => cd.ExecutionModeActionQueueType).Returns(actionQueueType);
                configData.Setup(cd => cd.IncludeUserInterfaceActions).Returns(includeUserInterfaceActions);

                ActionQueueGatewayStandard testObject = new ActionQueueGatewayStandard(configData.Object);

                using (SqlConnection sqlConnection = new SqlConnection(sqlAdapter.ConnectionString))
                {
                    sqlConnection.Open();
                    anyWorkToDo = testObject.AnyWorkToDo(sqlConnection);
                }
            }

            Assert.Equal(expectedValue, anyWorkToDo);
        }

        [Theory]
        [InlineData(true, ActionQueueType.DefaultPrint, true, true)]
        [InlineData(true, ActionQueueType.DefaultPrint, false, true)]
        [InlineData(true, ActionQueueType.Scheduled, true, false)]
        [InlineData(true, ActionQueueType.Scheduled, false, false)]
        [InlineData(true, ActionQueueType.UserInterface, true, false)]
        [InlineData(true, ActionQueueType.UserInterface, false, false)]
        [InlineData(false, ActionQueueType.DefaultPrint, true, true)]
        [InlineData(false, ActionQueueType.DefaultPrint, false, true)]
        [InlineData(false, ActionQueueType.Scheduled, true, false)]
        [InlineData(false, ActionQueueType.Scheduled, false, false)]
        [InlineData(false, ActionQueueType.UserInterface, true, true)]
        [InlineData(false, ActionQueueType.UserInterface, false, true)]
        public void AnyWorkToDo_ExpectedValueIsCorrect_WhenOnlyDefaultPrintEnabledQueueEntries(bool defaultPrintEnabled,
                ActionQueueType actionQueueType,
                bool includeUserInterfaceActions,
                bool expectedValue)
        {
            bool anyWorkToDo = true;

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                DispatchAction(defaultPrintEnabledAction, shipment.ShipmentID, sqlAdapter, "");
                DispatchAction(finishProcessingBatch, null, sqlAdapter, "");

                config.Setup(c => c.UseParallelActionQueue).Returns(defaultPrintEnabled);
                configData.Setup(cd => cd.FetchReadOnly()).Returns(config.Object);
                configData.Setup(cd => cd.ExecutionModeActionQueueType).Returns(actionQueueType);
                configData.Setup(cd => cd.IncludeUserInterfaceActions).Returns(includeUserInterfaceActions);

                ActionQueueGatewayStandard testObject = new ActionQueueGatewayStandard(configData.Object);

                using (SqlConnection sqlConnection = new SqlConnection(sqlAdapter.ConnectionString))
                {
                    sqlConnection.Open();
                    anyWorkToDo = testObject.AnyWorkToDo(sqlConnection);
                }
            }

            Assert.Equal(expectedValue, anyWorkToDo);
        }

        /// <summary>
        /// A valid trigger has been met and the given action is ready to be dispatched
        /// </summary>
        private static long DispatchAction(ActionEntity action, long? objectID, ISqlAdapter adapter, string extraData)
        {
            ActionQueueEntity entity = new ActionQueueEntity();
            entity.ActionID = action.ActionID;
            entity.ActionQueueType = (int) ActionDispatcher.DetermineActionQueueType(action); ;
            entity.ActionName = action.Name;
            entity.ActionVersion = action.RowVersion;
            entity.EntityID = objectID;
            entity.TriggerComputerID = UserSession.Computer.ComputerID;
            entity.ExtraData = extraData;

            if (action.ComputerLimitedType == (int) ComputerLimitedType.TriggeringComputer)
            {
                // It's limited to only running on this computer, so use this computer ID as
                // the only computer that can execute the action
                entity.InternalComputerLimitedList = UserSession.Computer.ComputerID.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                // Just copy over the list of computers that are able to execute the action
                entity.InternalComputerLimitedList = action.InternalComputerLimitedList;
            }

            // Set the initial status and the first step
            entity.Status = (int) ActionQueueStatus.Dispatched;
            entity.NextStep = 0;

            adapter.SaveAndRefetch(entity);

            return entity.ActionQueueID;
        }
        
    }
}
