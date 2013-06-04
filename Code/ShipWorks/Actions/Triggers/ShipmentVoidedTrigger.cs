using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Shipping;
using ShipWorks.Data.Model;

namespace ShipWorks.Actions.Triggers
{
    /// <summary>
    /// A shipment has been voided
    /// </summary>
    public class ShipmentVoidedTrigger : ActionTrigger
    {
        ShipmentTypeCode shipmentType = ShipmentTypeCode.Endicia;
        bool restrictType = false;

        // Return/Standard shipment
        bool restrictStandardReturn = true;
        bool returnShipmentsOnly = false;

        /// <summary>
        /// Constructor for default settings
        /// </summary>
        public ShipmentVoidedTrigger()
            : base(null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentVoidedTrigger(string xmlSettings)
            : base(xmlSettings)
        {

        }

        /// <summary>
        /// Return the enumerate trigger type value represented by the type
        /// </summary>
        public override ActionTriggerType TriggerType
        {
            get { return ActionTriggerType.ShipmentVoided; }
        }

        /// <summary>
        /// Create the editor for editing the trigger settings
        /// </summary>
        public override ActionTriggerEditor CreateEditor()
        {
            return new ShipmentVoidedTriggerEditor(this);
        }

        /// <summary>
        /// The entity type that triggers this trigger
        /// </summary>
        public override EntityType? TriggeringEntityType
        {
            get { return EntityType.ShipmentEntity; }
        }

        /// <summary>
        /// Indicates if the action should be restricted to running only for Return or Standard shipments
        /// </summary>
        public bool RestrictStandardReturn
        {
            get { return restrictStandardReturn; }
            set { restrictStandardReturn = value; }
        }

        /// <summary>
        /// When RestrictStandardReturn is true, specifies if the action will be for Return shipments or standard shipments
        /// </summary>
        public bool ReturnShipmentsOnly
        {
            get { return returnShipmentsOnly; }
            set { returnShipmentsOnly = value; }
        }

        /// <summary>
        /// Indicates if the action should be restricted to running only when a specified shipment type is voided
        /// </summary>
        public bool RestrictType
        {
            get { return restrictType; }
            set { restrictType = value; }
        }

        /// <summary>
        /// If RestrictType is true this controls which type the action applies to.
        /// </summary>
        public ShipmentTypeCode ShipmentType
        {
            get { return shipmentType; }
            set { shipmentType = value; }
        }
    }
}
