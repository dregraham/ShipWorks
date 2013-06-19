using System;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using Interapptive.Shared.Utility;
using System.Xml.Linq;
using System.IO;

namespace ShipWorks.Actions.Triggers
{
    public class ScheduledTrigger : ActionTrigger
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTrigger"/> class.
        /// </summary>
        public ScheduledTrigger()
            : this(null)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTrigger"/> class.
        /// </summary>
        /// <param name="xmlSettings"></param>
        public ScheduledTrigger(string xmlSettings)
            :base(string.Empty)
        {
            if (!string.IsNullOrEmpty(xmlSettings))
            {
                Schedule = DeserializeSchedule(xmlSettings);
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
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private ActionSchedule DeserializeSchedule(string xmlSettings)
        {
            string localRootName;

            using (var settingsStream = new MemoryStream(Encoding.ASCII.GetBytes(xmlSettings)))
            {

                XDocument settingsXDoc = XDocument.Load(settingsStream);

                Debug.Assert(settingsXDoc.Root != null, "settingsXDoc.Root != null");

                localRootName = settingsXDoc.Root.Name.LocalName;
            }

            switch (localRootName)
            {
                case "HourlyActionSchedule":
                    Schedule = SerializationUtility.DeserializeFromXml<HourlyActionSchedule>(xmlSettings);
                    break;
                case "OneTimeActionSchedule":
                    Schedule = SerializationUtility.DeserializeFromXml<OneTimeActionSchedule>(xmlSettings);
                    break;
                default:
                    throw new Exception(string.Format("{0} is an unknown schedule type and can't be deserialized in ScheduledTrigger.cs", localRootName));
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
            
            string xml = SerializationUtility.SerializeToXml(Schedule);
            
            xmlWriter.WriteRaw(xml);
        }
    }
}
