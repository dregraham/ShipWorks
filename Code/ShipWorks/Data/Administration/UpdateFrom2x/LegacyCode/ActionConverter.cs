using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Common.Threading;
using System.Data;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Data;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using System.Xml.Linq;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using System.Transactions;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Actions;
using ShipWorks.Stores.Communication;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Infopia.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.AmeriCommerce.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Magento.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Volusion.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.OrderMotion.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.Amazon.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.CommerceInterface.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.NetworkSolutions.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor;
using System.IO;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data.Administration.UpdateFrom2x.LegacyCode
{
    /// <summary>
    /// Responsible for converting 2x actions into 3x actions
    /// </summary>
    public static class ActionConverter
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ActionConverter));

        /// <summary>
        /// Convert all the 2x actions in the current database to 3x actions
        /// </summary>
        public static void ConvertActions(ProgressItem progress)
        {
            progress.Starting();
            progress.Detail = "Converting actions...";

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, SqlCommandProvider.DefaultTimeout))
            {
                DataTable actions = LoadOldActions();

                int count = 0;
                foreach (DataRow action in actions.Rows)
                {
                    int originalStoreID = (int) action["StoreID"];
                    long storeID;

                    using (SqlConnection con = SqlSession.Current.OpenConnection())
                    {
                        storeID = MigrationRowKeyTranslator.TranslateKeyToV3(originalStoreID, MigrationRowKeyType.Store, con);
                    }

                    StoreEntity store = new StoreEntity(storeID);
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.FetchEntity(store);

                        if (store.Fields.State != EntityState.Fetched)
                        {
                            throw new InvalidOperationException(string.Format("Could not load store {0}", storeID));
                        }
                    }

                    CreateAction(
                        store,
                        (string) action["ActionName"],
                        Convert.ToInt32(action["TriggerType"]),
                        (string) action["TriggerSettings"],
                        (string) action["TasksXml"],
                        (bool) action["Enabled"]);

                    progress.PercentComplete = (100 * ++count) / actions.Rows.Count;
                }

                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    using (SqlCommand cmd = SqlCommandProvider.Create(con))
                    {
                        cmd.CommandText = "DELETE v2m_Actions";
                        cmd.ExecuteNonQuery();
                    }
                }

                // done with transaction
                scope.Complete();
            }

            progress.Detail = "Done";
            progress.PercentComplete = 100;
            progress.Completed();
        }

        /// <summary>
        /// Create a V3 action from the given v2 action properties
        /// </summary>
        private static void CreateAction(StoreEntity store, string name, int triggerType, string triggerSettings, string tasksXml, bool enabled)
        {
            ActionEntity action = new ActionEntity();
            action.Name = name;
            action.Enabled = enabled;
            action.TaskSummary = "";

            action.ComputerLimitedType = (int) ComputerLimitedType.TriggeringComputer;
            action.InternalComputerLimitedList = string.Empty;

            action.StoreLimited = true;
            action.StoreLimitedList = new long[] { store.StoreID };

            LoadTrigger(action, triggerType, triggerSettings);

            // Need to save upfront so tasks have a parent ID to save to
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(action);
            }

            long? restrictFilterNodeID = null;

            // The OrderDownloaded trigger had an option filter.  But in v3 its not on the trigger, it's on each task.  We may need to apply it to tasks.
            if (action.TriggerType == (int) ActionTriggerType.OrderDownloaded)
            {
                XElement xSettings = XElement.Parse(triggerSettings);
                string filterName = (string) xSettings.Element("Filter");

                if (!string.IsNullOrWhiteSpace(filterName))
                {
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        RelationPredicateBucket bucket = new RelationPredicateBucket(FilterFields.Name == filterName);
                        bucket.Relations.Add(FilterNodeEntity.Relations.FilterSequenceEntityUsingFilterSequenceID);
                        bucket.Relations.Add(FilterSequenceEntity.Relations.FilterEntityUsingFilterID);

                        FilterNodeCollection nodes = new FilterNodeCollection();
                        adapter.FetchEntityCollection(nodes, bucket);

                        if (nodes.Count > 0)
                        {
                            restrictFilterNodeID = nodes[0].FilterNodeID;
                        }
                    }
                }
            }

            List<ActionTask> tasks = LoadTasks(action, tasksXml, store, restrictFilterNodeID);

            // Update the tasks xml description and save again
            using (SqlAdapter adapter = new SqlAdapter())
            {
                action.TaskSummary = ActionManager.GetTaskSummary(tasks);
                adapter.SaveEntity(action);
            }
        }

        /// <summary>
        /// Load the v2 trigger data into the given v3 action entity
        /// </summary>
        private static void LoadTrigger(ActionEntity action, int v2TriggerType, string v2TriggerSettings)
        {
            ActionTriggerType triggerType;

            XElement xSettings = XElement.Parse(v2TriggerSettings);

            switch (v2TriggerType)
            {
                case 0: triggerType = ActionTriggerType.OrderDownloaded; break;
                case 1: triggerType = ActionTriggerType.ShipmentProcessed; break;
                case 2: triggerType = ActionTriggerType.DownloadFinished; break;
                case 3: triggerType = ActionTriggerType.ShipmentVoided; break;
                default: throw new InvalidOperationException("Unknown V2 action trigger type " + v2TriggerType);
            }

            action.TriggerType = (int) triggerType;

            switch ((ActionTriggerType) action.TriggerType)
            {
                case ActionTriggerType.OrderDownloaded:
                    {
                        OrderDownloadedTrigger trigger = new OrderDownloadedTrigger();
                        trigger.Restriction = OrderDownloadedRestriction.OnlyInitial;

                        action.TriggerSettings = trigger.GetXml();
                    }
                    break;

                case ActionTriggerType.DownloadFinished:
                    {
                        DownloadFinishedTrigger trigger = new DownloadFinishedTrigger();
                        trigger.OnlyIfNewOrders = (bool) xSettings.Element("NewOrders");

                        int resultType = (int) xSettings.Element("DownloadResult");
                        trigger.RequiredResult = (resultType != -1) ? (DownloadResult?) resultType : (DownloadResult?) null;

                        action.TriggerSettings = trigger.GetXml();
                    }
                    break;

                case ActionTriggerType.ShipmentProcessed:
                    {
                        ShipmentProcessedTrigger trigger = new ShipmentProcessedTrigger();

                        int shipmentType = (int) xSettings.Element("ShipmentType");

                        trigger.RestrictType = shipmentType >= 0 && shipmentType < 7;
                        if (trigger.RestrictType)
                        {
                            trigger.ShipmentType = (ShipmentTypeCode) shipmentType;
                        }

                        trigger.RestrictStandardReturn = true;
                        trigger.ReturnShipmentsOnly = false;

                        action.TriggerSettings = trigger.GetXml();
                    }
                    break;

                case ActionTriggerType.ShipmentVoided:
                    {
                        ShipmentVoidedTrigger trigger = new ShipmentVoidedTrigger();

                        int shipmentType = (int) xSettings.Element("ShipmentType");

                        trigger.RestrictType = shipmentType >= 0 && shipmentType < 7;
                        if (trigger.RestrictType)
                        {
                            trigger.ShipmentType = (ShipmentTypeCode) shipmentType;
                        }

                        trigger.RestrictStandardReturn = true;
                        trigger.ReturnShipmentsOnly = false;

                        action.TriggerSettings = trigger.GetXml();
                    }
                    break;
            }
        }

        /// <summary>
        /// Create a V3 task for each task represented by the v2 xml
        /// </summary>
        private static List<ActionTask> LoadTasks(ActionEntity action, string tasksXml, StoreEntity store, long? restrictFilterNodeID)
        {
            List<ActionTask> tasks = new List<ActionTask>();

            using (SqlAdapter adapter = new SqlAdapter())
            {
                int stepIndex = 0;

                XElement xTasks = XElement.Parse(tasksXml);
                foreach (var xTask in xTasks.Elements("Task"))
                {
                    ActionTask task = InstantiateTask(xTask, store);

                    // Some tasks may not be convertable, ConvertTaskType will return null if that's the case
                    if (task == null)
                    {
                        continue;
                    }

                    ActionTaskEntity taskEntity = task.Entity;

                    taskEntity.ActionID = action.ActionID;
                    taskEntity.StepIndex = stepIndex++;

                    if (restrictFilterNodeID != null)
                    {
                        taskEntity.FilterCondition = true;
                        taskEntity.FilterConditionNodeID = (long) restrictFilterNodeID;
                    }

                    // Save the task
                    task.Save(action, adapter);
                    tasks.Add(task);
                }
            }

            return tasks;
        }

        /// <summary>
        /// Instantiate an instance of the given task identifier, bound to the given store if necessary
        /// </summary>
        private static ActionTask InstantiateTask(XElement xTask, StoreEntity store)
        {
            string v2TypeName = (string) xTask.Attribute("type");

            switch (v2TypeName)
            {
                case "PrintTask": return LoadPrintTaskSettings(CreateTaskType<PrintTask>(store), xTask);
                case "EmailTask": return LoadEmailTaskSettings(CreateTaskType<EmailTask>(store), xTask);
                case "PlaySoundTask": return LoadPlaySoundTaskSettings(CreateTaskType<PlaySoundTask>(store), xTask);
                case "SetOrderStatusTask": return LoadOrderStatusTaskSettings(CreateTaskType<SetOrderStatusTask>(store), xTask);
                case "PrintDownloadedLabelsTask": return CreateTaskType<PrintShipmentsTask>(store);
                case "eBayOnlineUpdateTask": return LoadEbayOnlineUpdateTask(CreateTaskType<EbayOnlineUpdateTask>(store), xTask);
                case "oscShipmentUpdateTask": return LoadGenericOrderUpdateTask(CreateTaskType<GenericStoreOrderUpdateTask>(store), xTask);
                case "InfopiaOrderUpdateTask": return LoadInfopiaOrderUpdateTask(CreateTaskType<InfopiaOrderUpdateTask>(store), xTask);
                case "InfopiaShipmentUpdateTask": return CreateTaskType<InfopiaShipmentUploadTask>(store);
                case "AmeriCommerceUpdateStatusTask": return LoadAmeriCommerceUpdateStatusTask(CreateTaskType<AmeriCommerceOrderUpdateTask>(store), xTask);
                case "AmeriCommerceShipmentUpdateTask": return CreateTaskType<AmeriCommerceShipmentUploadTask>(store);
                case "MagentoShipmentUpdateTask": return LoadMagentoShipmentUpdateTask(CreateTaskType<MagentoShipmentUploadTask>(store), xTask);
                case "VolusionShipmentUpdateTask": return LoadVolusionShipmentUploadTask(CreateTaskType<VolusionShipmentUploadTask>(store), xTask);
                case "OrderMotionShipmentUpdateTask": return CreateTaskType<OrderMotionShipmentUploadTask>(store);
                case "ChannelAdvisorShipmentUpdateTask": return CreateTaskType<ChannelAdvisorShipmentUploadTask>(store);
                case "MarketWorksUpdateShipmentTask": return CreateTaskType<MarketplaceAdvisorShipmentUploadTask>(store);
                case "MarketWorksOrderPromoteTask": return CreateTaskType<MarketplaceAdvisorPromoteOrderTask>(store);
                case "MarketWorksParcelPromoteTask": return CreateTaskType<MarketplaceAdvisorPromoteParcelTask>(store);
                case "MarketWorksOrderFlagsTask": return LoadMarketworksFlagsTask(CreateTaskType<MarketplaceAdvisorChangeOrderFlagsTask>(store), xTask);
                case "MarketWorksParcelFlagsTask": return LoadMarketworksFlagsTask(CreateTaskType<MarketplaceAdvisorChangeParcelFlagsTask>(store), xTask);
                case "YahooShipmentUpdateTask": return CreateTaskType<YahooEmailShipmentUploadTask>(store);
                case "AmazonShipmentUpdateTask": return CreateTaskType<AmazonShipmentUploadTask>(store);
                case "AuctionSoundShipmentUpdateTask": return CreateTaskType<GenericStoreShipmentUploadTask>(store);
                case "ClickCartProShipmentUpdateTask": return LoadGenericOrderUpdateTask(CreateTaskType<GenericStoreOrderUpdateTask>(store), xTask);
                case "CommerceInterfaceShipmentUpdateTask": return LoadCommerceInterfaceShipmentUploadTask(CreateTaskType<CommerceInterfaceShipmentUploadTask>(store), xTask);
                case "MivaShipmentUpdateTask": return LoadMivaShipmentUploadTask(CreateTaskType<GenericStoreOrderUpdateTask>(store), xTask);
                case "XCartOrderUpdateTask": return LoadGenericOrderUpdateTask(CreateTaskType<GenericStoreOrderUpdateTask>(store), xTask);
                case "XCartShipmentUpdateTask": return CreateTaskType<GenericStoreShipmentUploadTask>(store);
                case "NetworkSolutionsOrderUpdateTask": return LoadNetworkSolutionsOrderUpdateTask(CreateTaskType<NetworkSolutionsOrderUpdateTask>(store), xTask);
                case "NetworkSolutionsShipmentUpdateTask": return CreateTaskType<NetworkSolutionsShipmentUploadTask>(store);
            }

            // Throw once we get all of them mapped
            return null;
            // throw new InvalidOperationException("Unhandled v2 task type name: " + v2TypeName);
        }

        /// <summary>
        /// Load NetSol settings into the task
        /// </summary>
        private static ActionTask LoadNetworkSolutionsOrderUpdateTask(NetworkSolutionsOrderUpdateTask task, XElement xTask)
        {
            task.Comment = (string) xTask.Element("Comments");
            task.StatusCode = (long) xTask.Element("Code");

            return task;
        }

        /// <summary>
        /// Load the settings from the V2 miva shipment update task
        /// </summary>
        private static ActionTask LoadMivaShipmentUploadTask(GenericStoreOrderUpdateTask task, XElement xTask)
        {
            task.Comment = "";
            task.StatusCode = (string) xTask.Element("Status");

            return task;
        }

        /// <summary>
        /// Load the task settings for the CommerceInterface task
        /// </summary>
        private static ActionTask LoadCommerceInterfaceShipmentUploadTask(CommerceInterfaceShipmentUploadTask task, XElement xTask)
        {
            task.StatusCode = (int) xTask.Element("Code");
            return task;
        }

        /// <summary>
        /// Load marketworks flag-based tasks
        /// </summary>
        private static ActionTask LoadMarketworksFlagsTask(MarketplaceAdvisorChangeFlagsTaskBase task, XElement xTask)
        {
            task.FlagsOn = (MarketplaceAdvisorOmsFlagTypes) (int) xTask.Element("OnFlags");
            task.FlagsOff = (MarketplaceAdvisorOmsFlagTypes) (int) xTask.Element("OffFlags");

            return task;
        }

        /// <summary>
        /// Load the volusion shipment upload task settings
        /// </summary>
        private static ActionTask LoadVolusionShipmentUploadTask(VolusionShipmentUploadTask task, XElement xTask)
        {
            task.SendEmail = (bool) xTask.Element("SendEmail");
            return task;
        }

        /// <summary>
        /// Load settings for the magento upload task
        /// </summary>
        private static ActionTask LoadMagentoShipmentUpdateTask(MagentoShipmentUploadTask task, XElement xTask)
        {
            task.Comment = (string) xTask.Element("Comments");
            return task;
        }

        /// <summary>
        /// Load the AmeriCommerce order update task
        /// </summary>
        private static ActionTask LoadAmeriCommerceUpdateStatusTask(AmeriCommerceOrderUpdateTask task, XElement xTask)
        {
            task.StatusCode = (int) xTask.Element("Status");
            return task;
        }

        /// <summary>
        /// Load settings for the infopia task
        /// </summary>
        private static ActionTask LoadInfopiaOrderUpdateTask(InfopiaOrderUpdateTask task, XElement xTask)
        {
            task.Status = (string) xTask.Element("Status");
            return task;
        }

        /// <summary>
        /// Load the task specific settings for OSC
        /// </summary>
        private static ActionTask LoadGenericOrderUpdateTask(GenericStoreOrderUpdateTask task, XElement xTask)
        {
            task.Comment = (string) xTask.Element("Comments");
            task.StatusCode = (string) xTask.Element("Code");

            return task;
        }

        /// <summary>
        /// Load the ebay action task 
        /// </summary>
        private static ActionTask LoadEbayOnlineUpdateTask(EbayOnlineUpdateTask task, XElement xTask)
        {
            task.MarkShipped = (bool) xTask.Element("Shipped");
            task.MarkPaid = (bool) xTask.Element("Paid");

            return task;
        }

        /// <summary>
        /// Load the v2 task settings into the given v3 object
        /// </summary>
        private static PrintTask LoadPrintTaskSettings(PrintTask printTask, XElement xTask)
        {
            // TODO: translate into converted templateID
            string templateName = (string) xTask.Element("Template");

            // In V3 copies are at the template level not the task level
            int copies = (int) xTask.Element("Copies");
            if (copies > 1)
            {
                log.Warn("Ignore Copies for PrintTask since in V3 it's stored in the template");
            }

            return printTask;
        }

        /// <summary>
        /// Load the v2 task settings into the given v3 object
        /// </summary>
        private static EmailTask LoadEmailTaskSettings(EmailTask emailTask, XElement xTask)
        {
            // TODO: translate into converted templateID
            string templateName = (string) xTask.Element("Template");

            return emailTask;
        }

        /// <summary>
        /// Load the v2 task settings into the given v3 object
        /// </summary>
        private static PlaySoundTask LoadPlaySoundTaskSettings(PlaySoundTask playSoundTask, XElement xTask)
        {
            string soundFile = (string) xTask.Element("SoundFile");

            if (File.Exists(soundFile))
            {
                playSoundTask.PendingSoundFile = soundFile;
            }
            else
            {
                log.WarnFormat("PlaySound task could not find file '{0}'", soundFile);
            }

            playSoundTask.Entity.InputSource = (int) ActionTaskInputSource.Nothing;

            return playSoundTask;
        }

        /// <summary>
        /// Load the v2 task settings into the given v3 object
        /// </summary>
        private static SetOrderStatusTask LoadOrderStatusTaskSettings(SetOrderStatusTask setOrderStatusTask, XElement xTask)
        {
            string status = (string) xTask.Element("Status");

            setOrderStatusTask.Status = status;

            return setOrderStatusTask;
        }

        /// <summary>
        /// Instantiate an ActionTask of type T
        /// </summary>
        private static T CreateTaskType<T>(StoreEntity store)
        {
            ActionTaskDescriptorBinding binding = new ActionTaskDescriptorBinding(typeof(T), store);

            return (T) (object) binding.CreateInstance();
        }

        /// <summary>
        /// Load the old actions
        /// </summary>
        private static DataTable LoadOldActions()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
                {
                    cmd.CommandText = "SELECT * FROM v2m_Actions";

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    return table;
                }
            }
        }
    }
}
