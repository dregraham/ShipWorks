using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Filters;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.XPath;
using log4net;
using ShipWorks.Data.Model;
using System.Data.SqlClient;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Actions.Triggers
{
    /// <summary>
    /// Trigger based on an order\customer entering a filter
    /// </summary>
    public class FilterContentTrigger : ActionTrigger
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FilterContentTrigger));

        FilterTarget filterTarget = FilterTarget.Orders;
        long filterNodeID = 0;

        // The filter node ID when the object is first loaded
        long initialFilterNodeID;

        // If the trigger is from an object entering or leaving the filter
        FilterContentChangeDirection direction = FilterContentChangeDirection.Entering;

        // The key to use for the ObjectReferenceManager
        readonly string referenceKey = "FilterTrigger";

        /// <summary>
        /// Constructor for default settings
        /// </summary>
        public FilterContentTrigger()
            : base(null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterContentTrigger(string xmlSettings)
            : base(xmlSettings)
        {

        }

        /// <summary>
        /// Return the enumerate trigger type value represented by the type
        /// </summary>
        public override ActionTriggerType TriggerType
        {
            get { return ActionTriggerType.FilterContentChanged; }
        }

        /// <summary>
        /// Intercept the deserialization process
        /// </summary>
        public override void DeserializeXml(XPathNavigator xpath)
        {
            base.DeserializeXml(xpath);

            // Save the original one so we know if it changed
            initialFilterNodeID = filterNodeID;
        }

        /// <summary>
        /// Create the editor for the trigger settings
        /// </summary>
        public override ActionTriggerEditor CreateEditor()
        {
            return new FilterContentTriggerEditor(this);
        }

        /// <summary>
        /// The entity type that triggers this trigger
        /// </summary>
        public override EntityType? TriggeringEntityType
        {
            get { return FilterHelper.GetEntityType(filterTarget); }
        }

        /// <summary>
        /// The target filter type that the filter is for
        /// </summary>
        public FilterTarget FilterTarget
        {
            get { return filterTarget; }
            set 
            {
                if (filterTarget == value)
                {
                    return;
                }

                filterTarget = value;

                RaiseTriggeringEntityTypeChanged();
            }
        }
        
        /// <summary>
        /// The filter node that we are monitoring for a triggering change
        /// </summary>
        public long FilterNodeID
        {
            get { return filterNodeID; }
            set { filterNodeID = value; }
        }

        /// <summary>
        /// Controls if the trigger is when the object enters or leaves the filter
        /// </summary>
        public FilterContentChangeDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// Validates the state of the trigger.  An exception is thrown to indicate validation failure.
        /// </summary>
        /// <exception cref="ShipWorks.Actions.Triggers.FilterContentActionTriggerException">A disabled filter has been selected as a trigger.</exception>
        public override void Validate()
        {
            base.Validate();

            // If the selected filter didn't change, don't bother validating it
            if (initialFilterNodeID == FilterNodeID)
            {
                return;
            }

            // We want to throw an exception if the trigger is using a disabled filter
            FilterNodeEntity filterNode = FilterLayoutContext.Current.FindNode(FilterNodeID);
            if (filterNode.Filter.State == (byte)FilterState.Disabled)
            {
                throw new FilterContentActionTriggerException("A disabled filter has been selected as a trigger.");
            }
        }

        /// <summary>
        /// Save additional data required by the trigger to the database
        /// </summary>
        public override void SaveExtraState(ActionEntity action, SqlAdapter adapter)
        {
            base.SaveExtraState(action, adapter);

            // If the user has changed the selected filter node, delete our reference holding on to the old one
            if (initialFilterNodeID != filterNodeID)
            {
                log.DebugFormat("Deleting old reference {0} ({1})", initialFilterNodeID, action.Name);
                DeleteFilterReference(action.ActionID);
            }

            adapter.DeleteEntitiesDirectly(
                typeof(ActionFilterTriggerEntity),
                new RelationPredicateBucket(ActionFilterTriggerFields.ActionID == action.ActionID));

            if (filterNodeID != 0)
            {
                log.DebugFormat("Adding reference {0} ({1})", filterNodeID, action.Name);
                ObjectReferenceManager.SetReference(action.ActionID, referenceKey, filterNodeID, string.Format("The trigger for action '{0}'", action.Name));

                // Only add the trigger record if the action is enabled
                if (action.Enabled)
                {
                    ActionFilterTriggerEntity filterTrigger = new ActionFilterTriggerEntity();
                    filterTrigger.ActionID = action.ActionID;
                    filterTrigger.FilterNodeID = filterNodeID;
                    filterTrigger.Direction = (int) Direction;
                    filterTrigger.ComputerLimitedType = action.ComputerLimitedType;
                    filterTrigger.InternalComputerLimitedList = action.InternalComputerLimitedList;

                    adapter.SaveEntity(filterTrigger);
                }
            }
        }

        /// <summary>
        /// Cleanup any additional data added by the trigger
        /// </summary>
        public override void DeleteExtraState(ActionEntity action, SqlAdapter adapter)
        {
            // We send in the initial one - if the current one is not the initial one it doesn't matter, as it hasn't been saved yet.
            log.DebugFormat("Deleting reference {0} ({1})", initialFilterNodeID, action.Name);
            DeleteFilterReference(action.ActionID);

            adapter.DeleteEntitiesDirectly(
                typeof(ActionFilterTriggerEntity),
                new RelationPredicateBucket(ActionFilterTriggerFields.ActionID == action.ActionID));

            base.DeleteExtraState(action, adapter);
        }

        /// <summary>
        /// Delete our references to the given filter node
        /// </summary>
        private void DeleteFilterReference(long actionID)
        {
            ObjectReferenceManager.ClearReference(actionID, referenceKey);
        }
    }
}
