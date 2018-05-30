using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Actions;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Triggers;
using ShipWorks.Core.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Archive.Errors;

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
        private int numberOfDaysToKeep;
        private bool enabled;
        private DayOfWeek dayOfWeek;
        private const string AutoArchiveActionTaskName = "Auto archive action";

        /// <summary>
        /// Constructor
        /// </summary>
        public ScheduleOrderArchiveViewModel(
            IAsyncMessageHelper messageHelper,
            IScheduleOrderArchiveDialog scheduleArchiveOrdersDialog,
            ILifetimeScope lifetimeScope)
        {
            this.scheduleArchiveOrdersDialog = scheduleArchiveOrdersDialog;
            this.messageHelper = messageHelper;
            this.lifetimeScope = lifetimeScope;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            ConfirmSchedule = new RelayCommand(ConfirmScheduleAction, CanSchedule);
            CancelSchedule = new RelayCommand(CancelScheduleAction);

            Enabled = true;
            NumberOfDaysToKeep = 90;
            DayOfWeek = DayOfWeek.Thursday;

            var actionAndTask = LoadAction();

            if (actionAndTask.action != null)
            {
                Enabled = actionAndTask.action.Enabled;
                NumberOfDaysToKeep = actionAndTask.autoArchiveTask.NumberOfDaysToKeep;
                DayOfWeek = actionAndTask.autoArchiveTask.ExecuteOnDayOfWeek;
            }
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
        /// Day of week for the archive to be executed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public DayOfWeek DayOfWeek
        {
            get { return dayOfWeek; }
            set { handler.Set(nameof(DayOfWeek), ref dayOfWeek, value); }
        }

        /// <summary>
        /// Save the schedule info
        /// </summary>
        public Task Show()
        {
            return messageHelper
                .ShowDialog(SetupDialog)
                .Bind(x => x == true ?
                    Task.FromResult(Enabled) :
                    Task.FromException<bool>(Error.Canceled));
        }

        /// <summary>
        /// Load the auto archive action and task.
        /// </summary>
        private (ActionEntity action, AutoArchiveTask autoArchiveTask) LoadAction()
        {
            AutoArchiveTask autoArchiveTask = null;
            var action = ActionManager.Actions.FirstOrDefault(a => a.Name == AutoArchiveActionTaskName);

            if (action != null)
            {
                Enabled = action.Enabled;

                autoArchiveTask = (AutoArchiveTask) ActionManager.LoadTasks(lifetimeScope, action).FirstOrDefault();
                NumberOfDaysToKeep = autoArchiveTask.NumberOfDaysToKeep;
                DayOfWeek = autoArchiveTask.ExecuteOnDayOfWeek;
            }

            return (action, autoArchiveTask);
        }

        /// <summary>
        /// Persist the action
        /// </summary>
        private bool Save()
        {
            var actionAndTask = LoadAction();

            if (actionAndTask.action == null)
            {
                CreateAction();
            }
            else
            {
                actionAndTask.action.Enabled = Enabled;
                actionAndTask.autoArchiveTask.NumberOfDaysToKeep = NumberOfDaysToKeep;
                actionAndTask.autoArchiveTask.ExecuteOnDayOfWeek = DayOfWeek;

                ScheduledTrigger actionTrigger = (ScheduledTrigger) ActionManager.LoadTrigger(actionAndTask.action);
                MonthlyActionSchedule schedule = (MonthlyActionSchedule) actionTrigger.Schedule;
                schedule.ExecuteOnDay = DayOfWeek;

                // Transacted since we affect multiple action tables
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    actionAndTask.autoArchiveTask.Save(actionAndTask.action, adapter);

                    // Give the new trigger a chance to save its state
                    actionTrigger.SaveExtraState(actionAndTask.action, adapter);

                    // Save the action
                    ActionManager.SaveAction(actionAndTask.action, adapter);

                    adapter.Commit();
                }
            }

            return true;
        }

        /// <summary>
        /// Create the auto archive action
        /// </summary>
        private void CreateAction()
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
                TaskSummary = "Auto archive task summary"
            };
            ActionTaskEntity actionTaskEntity = new ActionTaskEntity();
            AutoArchiveTask actionTask = new AutoArchiveTask();

            ActionTaskDescriptor descriptor = new ActionTaskDescriptor(actionTask.GetType());
            actionTaskEntity.TaskIdentifier = descriptor.Identifier;
            actionTaskEntity.FlowError = (int) ActionTaskFlowOption.NextStep;
            actionTaskEntity.StepIndex = 0;
            actionTaskEntity.InputSource = (int) ActionTaskInputSource.Nothing;
            actionTaskEntity.FlowSuccess = (int) ActionTaskFlowOption.NextStep;
            actionTaskEntity.FlowSkipped = (int) ActionTaskFlowOption.NextStep;
            actionTaskEntity.InputFilterNodeID = -1;
            actionTaskEntity.FilterCondition = false;
            actionTaskEntity.FilterConditionNodeID = -1;

            actionTaskEntity.TaskSettings = triggerXml;
            actionTask = (AutoArchiveTask) ActionManager.InstantiateTask(lifetimeScope, actionTaskEntity);

            actionTask.NumberOfDaysToKeep = NumberOfDaysToKeep;

            // Transacted since we affect multiple action tables
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
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
                ActionManager.SaveAction(action, adapter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Setup the archive orders dialog
        /// </summary>
        private IDialog SetupDialog()
        {
            scheduleArchiveOrdersDialog.DataContext = this;
            return scheduleArchiveOrdersDialog;
        }

        /// <summary>
        /// Handle the confirmation of archiving orders
        /// </summary>
        private void ConfirmScheduleAction()
        {
            Save();

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
