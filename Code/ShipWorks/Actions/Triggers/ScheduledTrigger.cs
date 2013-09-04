using System;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using Interapptive.Shared.Utility;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ShipWorks.Actions.Scheduling;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Actions.Triggers
{
    public class ScheduledTrigger : ActionTrigger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTrigger"/> class.
        /// </summary>
        public ScheduledTrigger()
            : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTrigger"/> class.
        /// </summary>
        /// <param name="xmlSettings"></param>
        public ScheduledTrigger(string xmlSettings) : base(string.Empty)
        {
            if (!string.IsNullOrEmpty(xmlSettings))
            {
                Schedule = DeserializeSchedule(xmlSettings);
            }
            
            if (Schedule == null)
            {
                Schedule = new OneTimeActionSchedule();
            }
        }

        /// <summary>
        /// Overridden to provide the trigger type
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ActionTriggerType TriggerType
        {
            get { return ActionTriggerType.Scheduled; }
        }

        /// <summary>
        /// Creates the editor that is used to edit the condition.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ActionTriggerEditor CreateEditor()
        {
            return new ScheduledTriggerEditor(this);
        }

        /// <summary>
        /// The type of entity that causes the trigger to fire
        /// </summary>
        public override EntityType? TriggeringEntityType
        {
            // This doesn't map to any ShipWorks database entity at this time
            get { return null; }
        }

        /// <summary>
        /// Gets or sets the schedule for this trigger.
        /// </summary>
        public ActionSchedule Schedule
        {
            get;
            set;
        }

        /// <summary>
        /// Deserializes the schedule.
        /// </summary>
        /// <param name="xmlSettings">The XML settings.</param>
        /// <returns>An instance of an ActionSchedule object.</returns>
        /// <exception cref="ActionScheduleException"></exception>
        private ActionSchedule DeserializeSchedule(string xmlSettings)
        {
            XDocument settingsXDoc = XDocument.Parse(xmlSettings);

            ActionScheduleType actionScheduleType = ActionScheduleType.OneTime;
            if (!settingsXDoc.Descendants("ScheduleType").Any())
            {
                return null;
            }

            actionScheduleType = (ActionScheduleType)int.Parse(settingsXDoc.Descendants("ScheduleType").First().Value);

            string xml = string.Empty;
            switch (actionScheduleType)
            {
                case ActionScheduleType.OneTime:
                    xml = string.Join("", settingsXDoc.Descendants("OneTimeActionSchedule").First()).Trim();
                    Schedule = SerializationUtility.DeserializeFromXml<OneTimeActionSchedule>(xml);
                    break;

                case ActionScheduleType.Hourly:
                    xml = string.Join("", settingsXDoc.Descendants("HourlyActionSchedule").First()).Trim();
                    Schedule = SerializationUtility.DeserializeFromXml<HourlyActionSchedule>(xml);
                    break;

                case ActionScheduleType.Daily:
                    xml = string.Join("", settingsXDoc.Descendants("DailyActionSchedule").First()).Trim();
                    Schedule = SerializationUtility.DeserializeFromXml<DailyActionSchedule>(xml);
                    break;

                case ActionScheduleType.Weekly:
                    xml = string.Join("", settingsXDoc.Descendants("WeeklyActionSchedule").First()).Trim();
                    Schedule = SerializationUtility.DeserializeFromXml<WeeklyActionSchedule>(xml);
                    break;

                case ActionScheduleType.Monthly:
                    xml = string.Join("", settingsXDoc.Descendants("MonthlyActionSchedule").First()).Trim();
                    Schedule = SerializationUtility.DeserializeFromXml<MonthlyActionSchedule>(xml);
                    break;
                default:
                    throw new ActionScheduleException(string.Format("{0} is an unknown schedule type and can't be deserialized in ScheduledTrigger.cs", EnumHelper.GetDescription(actionScheduleType)));
            }

            return Schedule;
        }

        /// <summary>
        /// Load the object data from the given XPath
        /// </summary>
        /// <param name="xpath"></param>
        public override void DeserializeXml(System.Xml.XPath.XPathNavigator xpath)
        {
            if (xpath == null)
            {
                throw new ArgumentNullException("xpath", "xpath is null");
            }

            Schedule = DeserializeSchedule(xpath.OuterXml);
        }

        /// <summary>
        /// Save the instance as XML to the given writer
        /// </summary>
        /// <param name="xmlWriter"></param>
        public override void SerializeXml(System.Xml.XmlTextWriter xmlWriter)
        {
            if (xmlWriter == null)
            {
                throw new ArgumentNullException("xmlWriter", "xmlWriter is null");
            }
            
            string xml = SerializationUtility.SerializeToXml(Schedule, true);

            xmlWriter.WriteRaw(xml);
        }

        /// <summary>
        /// Some triggers may have extra database state they need to save in addition to what get's persisted in the XML settings.
        /// Such as the sound file resource for the PlaySound task.  This gives the trigger a chance to save it.
        /// </summary>
        public override void SaveExtraState(ActionEntity action, SqlAdapter adapter)
        {
            base.SaveExtraState(action, adapter);

            // And finally schedule the action
            Scheduler scheduler = new Scheduler();
            scheduler.UnscheduleAction(action);

            if (action.Enabled)
            {
                scheduler.ScheduleAction(action, Schedule);
            }
        }

        /// <summary>
        /// The trigger is being deleted.  Any database extra state saved by the trigger in SaveExtraState should be cleaned up.
        /// </summary>
        public override void DeleteExtraState(ActionEntity action, SqlAdapter adapter)
        {
            new Scheduler().UnscheduleAction(action);

            base.DeleteExtraState(action, adapter);
        }

        /// <summary>
        /// Validates the schedule.
        /// </summary>
        public override void Validate()
        {
            if (null == Schedule)
            {
                throw new SchedulingException("No schedule has been set for this trigger.");   
            }

            Schedule.Validate();
        }
    }
}
