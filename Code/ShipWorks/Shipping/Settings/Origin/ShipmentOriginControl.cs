using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Data.Controls;
using Interapptive.Shared.Business;

namespace ShipWorks.Shipping.Settings.Origin
{
    /// <summary>
    /// User control for selecting the origin of a postal shipment
    /// </summary>
    public partial class ShipmentOriginControl : UserControl
    {
        Dictionary<ShipmentEntity, PersonAdapter> loadedShipments = new Dictionary<ShipmentEntity, PersonAdapter>();

        /// <summary>
        /// Raised when any part of the configured origin changes
        /// </summary>
        public event EventHandler OriginChanged;

        /// <summary>
        /// Raised when the destination of the origin changes
        /// </summary>
        public event EventHandler DestinationChanged;

        // The ShipmentType for which we are displaying data
        ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentOriginControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control
        /// </summary>
        public void Initialize(ShipmentTypeCode code)
        {
            shipmentType = ShipmentTypeManager.GetType(code);

            List<KeyValuePair<string, long>> origins = shipmentType.GetOrigins();
       
            originCombo.DisplayMember = "Key";
            originCombo.ValueMember = "Value";
            originCombo.DataSource = origins;
        }

        /// <summary>
        /// The text to display for the "Origin" label
        /// </summary>
        [DefaultValue("Origin")]
        public string OriginLabel
        {
            get
            {
                return titleOrigin.Text;
            }
            set
            {
                titleOrigin.Text = value;
                labelOrigin.Text = value + ":";
            }
        }

        /// <summary>
        /// Controls what fields are visible to the user for editing
        /// </summary>
        [DefaultValue(PersonFields.All)]
        public PersonFields AvailableFields
        {
            get { return personControl.AvailableFields; }
            set { personControl.AvailableFields = value; }
        }

        /// <summary>
        /// Load the values from the entities into the control
        /// </summary>
        public void LoadShipments(IEnumerable<ShipmentEntity> shipments)
        {
            LoadShipments(shipments, s => new PersonAdapter(s, "Origin"));
        }

        /// <summary>
        /// Load the values from the entities into the control
        /// </summary>
        public void LoadShipments(IEnumerable<ShipmentEntity> shipments, Func<ShipmentEntity, PersonAdapter> adapterCreator)
        {
            originCombo.SelectedIndexChanged -= this.OnChangeSender;

            loadedShipments.Clear();

            // We'll need to know which entities are loaded to properly handle the origin source changing
            foreach (ShipmentEntity shipment in shipments)
            {
                loadedShipments[shipment] = adapterCreator(shipment);
            }

            // Load the entities into the address control
            personControl.LoadEntities(loadedShipments.Values.ToList());

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (PersonAdapter person in loadedShipments.Values)
                {
                    originCombo.ApplyMultiValue(person.OriginID);
                }
            }

            UpdateEditableState();

            originCombo.SelectedIndexChanged += this.OnChangeSender;

            RaiseOriginChanged();
        }

        /// <summary>
        /// Save the values from the control into the entities
        /// </summary>
        public void SaveToEntities()
        {
            foreach (PersonAdapter person in loadedShipments.Values)
            {
                originCombo.ReadMultiValue(v => person.OriginID = (long) v);
            }

            personControl.SaveToEntities(loadedShipments.Values);
        }

        /// <summary>
        /// Can be called to notify the control that the selected "Account" has changed, which means the displayed address needs to be reloaded
        /// </summary>
        public void NotifySelectedAccountChanged()
        {
            OnChangeSender(originCombo, EventArgs.Empty);
        }

        /// <summary>
        /// Changing the selected sender
        /// </summary>
        private void OnChangeSender(object sender, EventArgs e)
        {
            List<PersonAdapter> prototypes = new List<PersonAdapter>();

            // We need one prototype for loaded shipment, to be able to properly figure out what the multi select should be
            foreach (var pair in loadedShipments)
            {
                ShipmentEntity shipment = pair.Key;
                PersonAdapter person = pair.Value;

                PersonAdapter prototype = new PersonAdapter();
                PersonAdapter.Copy(person, prototype);

                // Save what's in the address control now to the prototype, in case of Manual
                personControl.SaveToEntity(prototype);

                long originID = person.OriginID;

                if (!originCombo.MultiValued)
                {
                    originID = (long) originCombo.SelectedValue;
                }

                // Update the origin address of our prototype shipment based on the new combobox selection
                shipmentType.UpdatePersonAddress(shipment, prototype, originID);

                // Add to the prototypes
                prototypes.Add(prototype);
            }

            // Load the values into the person control
            personControl.LoadEntities(prototypes);

            UpdateEditableState();

            RaiseOriginChanged();
        }

        /// <summary>
        /// Update the editable state of the address control
        /// </summary>
        private void UpdateEditableState()
        {
            bool canEdit = !originCombo.MultiValued && (ShipmentOriginSource) (long) originCombo.SelectedValue == ShipmentOriginSource.Other;

            personControl.Enabled = canEdit;
        }

        /// <summary>
        /// Values in the person control have changed
        /// </summary>
        private void OnPersonContentChanged(object sender, EventArgs e)
        {
            RaiseOriginChanged();
        }

        /// <summary>
        /// Indicate that some part of the origin data has changed
        /// </summary>
        private void RaiseOriginChanged()
        {
            if (OriginChanged != null)
            {
                OriginChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Destination in the person control have changed
        /// </summary>
        private void OnDestinationChanged(object sender, EventArgs e)
        {
            RaiseDestinationChange();
        }

        /// <summary>
        /// Indicate that the destination of the origin data has changed
        /// </summary>
        private void RaiseDestinationChange()
        {
            if (DestinationChanged != null)
            {
                DestinationChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the selected origin. A null value is returned if the item is multivalued
        /// </summary>
        /// <value>The selected origin.</value>
        public ShipmentOriginSource? SelectedOrigin
        {
            get 
            { 
                if (originCombo.MultiValued)
                {
                    return null;
                }

                return (ShipmentOriginSource) (long) originCombo.SelectedValue; 
            }
        }

        /// <summary>
        /// Get a descriptive text describing the origin the user has selected
        /// </summary>
        public string OriginDescription
        {
            get
            {
                if (originCombo.MultiValued)
                {
                    return "Address: (Multiple)";
                }

                if (originCombo.SelectedValue == null)
                {
                    return "Address: (None)";
                }

                long originID = (long) originCombo.SelectedValue;

                if (originID == (long) ShipmentOriginSource.Store)
                {
                    return "Store Address";
                }

                if (originID == (long) ShipmentOriginSource.Account)
                {
                    return "Account Address";
                }

                ShippingOriginEntity shipper = ShippingOriginManager.GetOrigin(originID);
                if (shipper != null)
                {
                    return shipper.Description;
                }

                return "Other Address";
            }
        }
    }
}
