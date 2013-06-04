using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model;

namespace ShipWorks.Actions.Triggers
{
    /// <summary>
    /// Trigger that happens when a download for a store completes
    /// </summary>
    public class DownloadFinishedTrigger : ActionTrigger
    {
        DownloadResult? requiredResult = null;
        bool onlyIfNewOrders = false;

        /// <summary>
        /// Constructor for default settings
        /// </summary>
        public DownloadFinishedTrigger()
            : base(null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DownloadFinishedTrigger(string xmlSettings)
            : base(xmlSettings)
        {

        }

        /// <summary>
        /// Return the enumerate trigger type value represented by the type
        /// </summary>
        public override ActionTriggerType TriggerType
        {
            get { return ActionTriggerType.DownloadFinished; }
        }

        /// <summary>
        /// Create the editor for editing the settings of the trigger
        /// </summary>
        public override ActionTriggerEditor CreateEditor()
        {
            return new DownloadFinishedTriggerEditor(this);
        }

        /// <summary>
        /// The entity type that triggers this trigger
        /// </summary>
        public override EntityType? TriggeringEntityType
        {
            get { return null; }
        }

        /// <summary>
        /// The DownloadResult required for the trigger to activate, or null if any result is OK.
        /// </summary>
        public DownloadResult? RequiredResult
        {
            get { return requiredResult; }
            set { requiredResult = value; }
        }

        /// <summary>
        /// Indicates that the trigger should activate only if there are new orders during the download.
        /// </summary>
        public bool OnlyIfNewOrders
        {
            get { return onlyIfNewOrders; }
            set { onlyIfNewOrders = value; }
        }
    }
}
