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
    public class ActionQueueGatewayDefaultPrintTest
    {
        private readonly DataContext context;
        private Mock<IConfigurationEntity> config = new Mock<IConfigurationEntity>();
        private Mock<IConfigurationData> configData = new Mock<IConfigurationData>();
        private readonly ActionEntity defaultPrintAction;
        private readonly ActionEntity nonDefaultPrintAction;
        private readonly ShipmentEntity shipment;
        private readonly ActionEntity finishProcessingBatch;

        public ActionQueueGatewayDefaultPrintTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            defaultPrintAction = ShippingActionUtility.GetPrintAction(ShipmentTypeCode.PostalWebTools);

            nonDefaultPrintAction = new ActionEntity()
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
            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                ActionManager.SaveAction(nonDefaultPrintAction, sqlAdapter);
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

            bool anyWorkToDo = true;

            ActionQueueGatewayDefaultPrint testObject = new ActionQueueGatewayDefaultPrint();
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
        public void AnyWorkToDo_ReturnsTrue_WhenActionQueueHasDefaultPrintEntry()
        {
            bool anyWorkToDo = true;

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                DispatchAction(nonDefaultPrintAction, shipment.OrderID, sqlAdapter, "");
                DispatchAction(defaultPrintAction, shipment.ShipmentID, sqlAdapter, "");
                DispatchAction(finishProcessingBatch, null, sqlAdapter, "");

                config.Setup(c => c.UseParallelActionQueue).Returns(false);
                configData.Setup(cd => cd.FetchReadOnly()).Returns(config.Object);

                ActionQueueGatewayDefaultPrint testObject = new ActionQueueGatewayDefaultPrint();

                using (SqlConnection sqlConnection = new SqlConnection(sqlAdapter.ConnectionString))
                {
                    sqlConnection.Open();
                    anyWorkToDo = testObject.AnyWorkToDo(sqlConnection);
                }
            }

            Assert.True(anyWorkToDo);
        }

        [Fact]
        public void AnyWorkToDo_ReturnsFalse_WhenActionQueueDoesNotHasDefaultPrintEntry()
        {
            bool anyWorkToDo = true;

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                DispatchAction(nonDefaultPrintAction, shipment.OrderID, sqlAdapter, "");

                config.Setup(c => c.UseParallelActionQueue).Returns(false);
                configData.Setup(cd => cd.FetchReadOnly()).Returns(config.Object);

                ActionQueueGatewayDefaultPrint testObject = new ActionQueueGatewayDefaultPrint();

                using (SqlConnection sqlConnection = new SqlConnection(sqlAdapter.ConnectionString))
                {
                    sqlConnection.Open();
                    anyWorkToDo = testObject.AnyWorkToDo(sqlConnection);
                }
            }

            Assert.False(anyWorkToDo);
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
