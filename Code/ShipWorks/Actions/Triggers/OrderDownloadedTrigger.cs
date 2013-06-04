using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Data.Model;

namespace ShipWorks.Actions.Triggers
{
    /// <summary>
    /// Trigger that happens when a new order is downloaded
    /// </summary>
    public class OrderDownloadedTrigger : ActionTrigger
    {
        OrderDownloadedRestriction restriction = OrderDownloadedRestriction.None;

        /// <summary>
        /// Constructor for default settings
        /// </summary>
        public OrderDownloadedTrigger()
            : base(null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDownloadedTrigger(string xmlSettings)
            : base(xmlSettings)
        {

        }

        /// <summary>
        /// Return the enumerate trigger type value represented by the type
        /// </summary>
        public override ActionTriggerType TriggerType
        {
            get { return ActionTriggerType.OrderDownloaded; }
        }

        /// <summary>
        /// Create the editor for editing the settings of the trigger
        /// </summary>
        public override ActionTriggerEditor CreateEditor()
        {
            return new OrderDownloadedTriggerEditor(this);
        }

        /// <summary>
        /// The entity type that triggers this trigger
        /// </summary>
        public override EntityType? TriggeringEntityType
        {
            get { return EntityType.OrderEntity; }
        }

        /// <summary>
        /// Indicates if the trigger only applies for orders on their initial download into ShipWorks.
        /// </summary>
        public OrderDownloadedRestriction Restriction
        {
            get { return restriction; }
            set { restriction = value; }
        }
    }
}
