using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Actions;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Triggers;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// View Model for the scheduling archive orders dialog
    /// </summary>
    [Component(Service = typeof(IScheduleOrderArchiveViewModel))]
    public class ScheduleOrderArchiveViewModel : IScheduleOrderArchiveViewModel, INotifyPropertyChanged
    {
        private readonly IAsyncMessageHelper messageHelper;
        private readonly IScheduleOrderArchiveDialog scheduleArchiveOrdersDialog;
        private readonly PropertyChangedHandler handler;
        private readonly ILifetimeScope lifetimeScope;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IActionManager actionManager;
        private int numberOfDaysToKeep;
        private bool enabled;
        private bool saving;
        private DayOfWeek dayOfWeek;
        private const string AutoArchiveActionTaskName = "Auto archive action";

        /// <summary>
        /// Constructor
        /// </summary>
        public ScheduleOrderArchiveViewModel(
            IAsyncMessageHelper messageHelper,
            IScheduleOrderArchiveDialog scheduleArchiveOrdersDialog,
            ISqlAdapterFactory sqlAdapterFactory,
            IActionManager actionManager,
            ILifetimeScope lifetimeScope)
        {
            this.scheduleArchiveOrdersDialog = scheduleArchiveOrdersDialog;
            this.messageHelper = messageHelper;
            this.lifetimeScope = lifetimeScope;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.actionManager = actionManager;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            ConfirmSchedule = new RelayCommand(() => ConfirmScheduleAction().Forget(), CanSchedule);
            CancelSchedule = new RelayCommand(CancelScheduleAction);

            Enabled = false;
            NumberOfDaysToKeep = 90;
            DayOfWeek = DayOfWeek.Sunday;
        }

        /// <summary>
        /// A property value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Confirm archive of the orders
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand ConfirmSchedule { get; }

        /// <summary>
        /// Cancel archive orders
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CancelSchedule { get; }

        /// <summary>
        /// Selected number of days of order data to keep
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int NumberOfDaysToKeep
        {
            get { return numberOfDaysToKeep; }
            set { handler.Set(nameof(NumberOfDaysToKeep), ref numberOfDaysToKeep, value); }
        }

        /// <summary>
        /// Is auto archiving enabled?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Enabled
        {
            get { return enabled; }
            set { handler.Set(nameof(Enabled), ref enabled, value); }
        }

        /// <summary>
        /// Are we saving?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Saving
        {
            get { return saving; }
            set { handler.Set(nameof(Saving), ref saving, value); }
        }

        /// <summary>
        /// Day of week for the archive to be executed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public DayOfWeek DayOfWeek
        {
            get { return dayOfWeek; }
            set { handler.Set(nameof(DayOfWeek), ref dayOfWeek, value); }
        }

        /// <summary>
        /// Bind the UI elements to the entities
        /// </summary>
        private void BindToUi()
        {
            var actionAndTask = LoadAction();

            if (actionAndTask.action != null)
            {
                Enabled = actionAndTask.action.Enabled;
                NumberOfDaysToKeep = actionAndTask.autoArchiveTask.NumberOfDaysToKeep;
                DayOfWeek = actionAndTask.autoArchiveTask.ExecuteOnDayOfWeek;
            }
        }

        /// <summary>
        /// Save the schedule info
        /// </summary>
        public Task Show()
        {
            return messageHelper
                .ShowDialog(SetupDialog);
        }

        /// <summary>
        /// Load the auto archive action and task.
        /// </summary>
        private (ActionEntity action, AutoArchiveTask autoArchiveTask) LoadAction()
        {
            actionManager.CheckForChangesNeeded();
            AutoArchiveTask autoArchiveTask = null;
            ActionEntity action = actionManager.Actions.FirstOrDefault(a => a.Name == AutoArchiveActionTaskName);

            if (action != null)
            {
                autoArchiveTask = (AutoArchiveTask) actionManager.LoadTasks(lifetimeScope, action).FirstOrDefault();
            }

            return (action, autoArchiveTask);
        }

        /// <summary>
        /// Persist the action
        /// </summary>
        private async Task Save()
        {
            var actionAndTask = LoadAction();

            if (actionAndTask.action == null)
            {
                await CreateAction().ConfigureAwait(false);
            }
            else
            {
                actionAndTask.action.Enabled = Enabled;
                actionAndTask.autoArchiveTask.NumberOfDaysToKeep = NumberOfDaysToKeep;
                actionAndTask.autoArchiveTask.ExecuteOnDayOfWeek = DayOfWeek;

                ScheduledTrigger actionTrigger = (ScheduledTrigger) actionManager.LoadTrigger(actionAndTask.action);
                MonthlyActionSchedule schedule = (MonthlyActionSchedule) actionTrigger.Schedule;
                schedule.ExecuteOnDay = DayOfWeek;

                actionAndTask.action.TriggerSettings = actionTrigger.GetXml();

                // Transacted since we affect multiple action tables
                await sqlAdapterFactory.WithPhysicalTransactionAsync((transaction, sqlAdapter) =>
                {
                    SqlAdapter adapter = (SqlAdapter) sqlAdapter;
                    actionAndTask.autoArchiveTask.Save(actionAndTask.action, adapter);

                    // Give the new trigger a chance to save its state
                    actionTrigger.SaveExtraState(actionAndTask.action, adapter);

                    // Save the action
                    actionManager.SaveAction(actionAndTask.action, adapter);

                    transaction.Commit();

                    return Task.FromResult(true);
                }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Create the auto archive action
        /// </summary>
        private async Task CreateAction()
        {
            ActionTriggerType actionTriggerType = ActionTriggerType.Scheduled;

            // Create a new trigger of the selected type with the trigger's default settings
            ScheduledTrigger trigger = (ScheduledTrigger) ActionTriggerFactory.CreateTrigger(actionTriggerType, null);

            MonthlyActionSchedule schedule = new MonthlyActionSchedule()
            {
                StartDateTimeInUtc = DateTime.Now.Date.ToUniversalTime(),
                EndsOnType = ActionEndsOnType.Never,
                CalendarType = MonthlyCalendarType.Day,
                ExecuteOnDay = DayOfWeek,
                ExecuteOnAnyDay = false,
                ExecuteOnWeek = WeekOfMonthType.First,
                ExecuteOnDayMonths = EnumHelper.GetEnumList<MonthType>().Select(mt => mt.Value).ToList()
            };

            trigger.Schedule = schedule;

            string triggerXml = trigger.GetXml();

            ActionEntity action = new ActionEntity()
            {
                Name = AutoArchiveActionTaskName,
                Enabled = true,
                ComputerLimitedType = (int) ComputerLimitedType.None,
                ComputerLimitedList = new long[0],
                StoreLimited = false,
                StoreLimitedList = new long[0],
                TriggerType = (int) ActionTriggerType.Scheduled,
                TriggerSettings = triggerXml,
                TaskSummary = "Auto archive task summary",
                InternalOwner = "AutoArchive"
            };
            AutoArchiveTask actionTask = new AutoArchiveTask();

            ActionTaskDescriptor descriptor = new ActionTaskDescriptor(actionTask.GetType());

            ActionTaskEntity actionTaskEntity = new ActionTaskEntity
            {
                TaskIdentifier = descriptor.Identifier,
                FlowError = (int) ActionTaskFlowOption.NextStep,
                StepIndex = 0,
                InputSource = (int) ActionTaskInputSource.Nothing,
                FlowSuccess = (int) ActionTaskFlowOption.NextStep,
                FlowSkipped = (int) ActionTaskFlowOption.NextStep,
                InputFilterNodeID = -1,
                FilterCondition = false,
                FilterConditionNodeID = -1,
                TaskSettings = triggerXml
            };

            actionTask = (AutoArchiveTask) actionManager.InstantiateTask(lifetimeScope, actionTaskEntity);

            actionTask.NumberOfDaysToKeep = NumberOfDaysToKeep;
            actionTask.ExecuteOnDayOfWeek = DayOfWeek;

            // Transacted since we affect multiple action tables
            await sqlAdapterFactory.WithPhysicalTransactionAsync((transaction, sqlAdapter) =>
            {
                SqlAdapter adapter = (SqlAdapter) sqlAdapter;

                // If the action is new, we have to save it once up front to get its PK.  Require's two trip's to save,
                // but some of the SaveExtraState functions can require the ID... but then that can affect there XML Settings serialization,
                // which requires the final save to the DB
                if (action.IsNew)
                {
                    adapter.SaveAndRefetch(action);
                }

                actionTaskEntity.ActionID = action.ActionID;

                // Give the new trigger a chance to save its state
                trigger.SaveExtraState(action, adapter);

                actionTask.Save(action, adapter);

                action.Fields[(int) ActionFieldIndex.Enabled].IsChanged = true;
                action.IsDirty = true;

                // Save the action
                actionManager.SaveAction(action, adapter);

                transaction.Commit();

                return Task.FromResult(true);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Setup the archive orders dialog
        /// </summary>
        private IDialog SetupDialog()
        {
            BindToUi();

            scheduleArchiveOrdersDialog.DataContext = this;
            return scheduleArchiveOrdersDialog;
        }

        /// <summary>
        /// Handle the confirmation of archiving orders
        /// </summary>
        private async Task ConfirmScheduleAction()
        {
            Saving = true;

            try
            {
                await Save().ConfigureAwait(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Saving = false;

            scheduleArchiveOrdersDialog.DialogResult = true;
            scheduleArchiveOrdersDialog.Close();
        }

        /// <summary>
        /// Can the user start the archive
        /// </summary>
        private bool CanSchedule() => NumberOfDaysToKeep > 0;

        /// <summary>
        /// Cancel archiving orders
        /// </summary>
        private void CancelScheduleAction() =>
            scheduleArchiveOrdersDialog.Close();
    }
}
