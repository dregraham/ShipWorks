﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;
using ShipWorks.Data;
using Divelements.SandGrid;
using ShipWorks.Data.Grid.DetailView;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Net;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Base UserControl for all ShipmentType specific stuff.
    /// </summary>
    public partial class ServiceControlBase : UserControl
    {
        // The type of shipment this instance is servicing 
        readonly ShipmentTypeCode shipmentTypeCode;

        // The values last past to the LoadShipments function
        List<ShipmentEntity> loadedShipments;
        bool enableEditing;

        // Counter for rate criteria change even suspension
        int suspendRateEvent = 0;

        // control for configuring returns
        ReturnsControlBase returnsControl;

        /// <summary>
        /// Raise when the country or state of the recipient has changed
        /// </summary>
        public event EventHandler RecipientDestinationChanged;

        /// <summary>
        /// Occurs when [shipment service changed].
        /// </summary>
        public event EventHandler ShipmentServiceChanged;

        /// <summary>
        /// Raise when new shipments are generated by the control
        /// </summary>
        public event ShipmentsAddedRemovedEventHandler ShipmentsAdded;

        /// <summary>
        /// Raised when a shipment value that affects rate data has changed, so that the shipping window can know to invalidate rate data.
        /// </summary>
        public event EventHandler RateCriteriaChanged;

        /// <summary>
        /// Raised when something occurs that causes the service control to change the shipment type. For example, this was created to support 
        /// the best rate shipment type and allowing the selection of a rate to change the shipment type.
        /// </summary>
        public event EventHandler ShipmentTypeChanged;

        /// <summary>
        /// Raised when the rates have been cleared from the rates grid.
        /// </summary>
        public event EventHandler RatesCleared;

        /// <summary>
        /// Constructor
        /// </summary>
        private ServiceControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceControlBase(ShipmentTypeCode shipmentTypeCode)
            : this()
        {
            this.shipmentTypeCode = shipmentTypeCode;
        }

        /// <summary>
        /// The shipment type this instance is servicing
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return shipmentTypeCode;
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Load the data for the list of shipments into the control
        /// </summary>
        public virtual void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            if (shipments == null)
            {
                throw new ArgumentNullException("shipments");
            }

            SuspendRateCriteriaChangeEvent();

            this.loadedShipments = shipments.ToList();
            this.enableEditing = enableEditing;

            personControl.DestinationChanged -= this.OnRecipientDestinationChanged;
            personControl.ContentChanged -= this.OnPersonContentChanged;

            // Enable\disable the ContentPanels... not the groups themselves, so the groups can still be open\closed
            foreach (CollapsibleGroupControl group in Controls.OfType<CollapsibleGroupControl>())
            {
                group.ContentPanel.Enabled = enableEditing;
            }

            if (enableEditing)
            {
                sectionRecipient.ContentPanel.Enabled = enableShippingAddress;
            }

            personControl.LoadEntities(shipments.Select(s => new PersonAdapter(s, "Ship")).ToList());

            bool anyNeedResidential = false;

            // Determine if any shipments require the residential setting
            foreach (ShipmentEntity shipment in shipments)
            {
                anyNeedResidential = ShipmentTypeManager.GetType(shipment).IsResidentialStatusRequired(shipment);

                if (anyNeedResidential)
                {
                    break;
                }
            }

            UpdateResidentialDisplay(anyNeedResidential);

            // Load residential stuff only if neccearry
            if (anyNeedResidential)
            {
                bool anyFedex = shipments.Any(s => s.ShipmentType == (int) ShipmentTypeCode.FedEx);
                LoadResidentialDeterminationOptions(anyFedex);
            }

            LoadReturnsUI();

            using (MultiValueScope scope = new MultiValueScope())
            {
                // Go through and load the data from each shipment
                foreach (ShipmentEntity shipment in shipments)
                {
                    // Residential status info
                    if (ShipmentTypeManager.GetType(shipment).IsResidentialStatusRequired(shipment))
                    {
                        ResidentialDeterminationType residentialType = (ResidentialDeterminationType) shipment.ResidentialDetermination;

                        // If its processed, use its final determined value
                        if (shipment.Processed)
                        {
                            residentialType = shipment.ResidentialResult ? ResidentialDeterminationType.Residential : ResidentialDeterminationType.Commercial;
                        }

                        residentialDetermination.ApplyMultiValue(residentialType);
                    }
                }
            }

            personControl.DestinationChanged += this.OnRecipientDestinationChanged;
            personControl.ContentChanged += this.OnPersonContentChanged;

            UpdateExtraText();

            ResumeRateCriteriaChangeEvent();
        }

        /// <summary>
        /// Save returns
        /// </summary>
        private void SaveReturnsToShipments()
        {
            if (sectionReturns.Visible)
            {
                using (MultiValueScope scope = new MultiValueScope())
                {
                    foreach (ShipmentEntity shipment in loadedShipments)
                    {
                        returnShipment.ReadMultiCheck(v => shipment.ReturnShipment = v);
                    }
                }

                if (returnsControl != null)
                {
                    returnsControl.SaveToShipments();
                }
            }
        }

        /// <summary>
        /// Loads the UI for Return Shipments
        /// </summary>
        private void LoadReturnsUI()
        {
            ReturnsControlBase newReturnsControl = null;

            List<ShipmentType> loadedTypes = loadedShipments.Select(s => s.ShipmentType).Distinct().Select(st => ShipmentTypeManager.GetType((ShipmentTypeCode) st)).ToList();

            bool anyReturnsSupported = loadedTypes.Any(st => st.SupportsReturns);

            if (anyReturnsSupported)
            {
                bool allReturnsSupported = loadedTypes.All(st => st.SupportsReturns);

                // Always show it if any types support returns
                sectionReturns.Visible = true;

                // Checkbox for enabling returns editing 
                using (MultiValueScope scope = new MultiValueScope())
                {
                    foreach (ShipmentEntity shipment in loadedShipments)
                    {
                        returnShipment.ApplyMultiCheck(shipment.ReturnShipment);
                    }
                }

                // But only support turning "Returns" on or off if all types support it
                returnShipment.Enabled = allReturnsSupported;

                // Only if there all the same type can we create the type-specific control
                if (loadedTypes.Count == 1)
                {
                    newReturnsControl = loadedTypes[0].CreateReturnsControl();
                }
                else
                {
                    newReturnsControl = new MultiSelectReturnsControl();
                }

                newReturnsControl.LoadShipments(loadedShipments);

                // add the control to the UI
                newReturnsControl.Location = new Point(0, 0);
                newReturnsControl.Width = returnsPanel.Width;
                newReturnsControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                returnsPanel.Controls.Add(newReturnsControl);

                // Sizing
                returnsPanel.Height = newReturnsControl.Height;
                sectionReturns.Height = returnsPanel.Bottom + 5 + (sectionReturns.Height - sectionReturns.ContentPanel.Height);

                // only enable for editing if all shipments are Returns
                returnsPanel.Enabled = (loadedShipments.All(s => s.ReturnShipment));
            }
            else
            {
                sectionReturns.Visible = false;
            }

            // Cleanup
            if (returnsControl != null)
            {
                returnsControl.Dispose();
                returnsControl = null;
            }

            // Update
            returnsControl = newReturnsControl;
        }

        /// <summary>
        /// Update the display of the residential information
        /// </summary>
        private void UpdateResidentialDisplay(bool showResidential)
        {
            labelResidentialCommercial.Visible = showResidential;
            labelAddress.Visible = showResidential;
            residentialDetermination.Visible = showResidential;

            var controlsToConsider = sectionRecipient.ContentPanel.Controls.Cast<Control>().Where(c => showResidential || (c != labelResidentialCommercial && c != residentialDetermination && c != labelAddress)).ToList();
            if (controlsToConsider.Any(c => c.Visible))
            {
                controlsToConsider = controlsToConsider.Where(c => c.Visible).ToList();
            }

            sectionRecipient.Height = controlsToConsider.Max(c => c.Bottom)
                 + 8 + (sectionRecipient.Height - sectionRecipient.ContentPanel.Height);
        }

        /// <summary>
        /// Load the options for selecting residential determinatino based on if any are fedex.  For now only fedex offers residential\commercial
        /// address lookup.
        /// </summary>
        private void LoadResidentialDeterminationOptions(bool anyFedEx)
        {
            EnumHelper.BindComboBox<ResidentialDeterminationType>
                (
                    residentialDetermination,
                    t => anyFedEx || t != ResidentialDeterminationType.FedExAddressLookup
                );
        }

        /// <summary>
        /// Somtimes the weight can change through other means, like through the customs editor.  This should be overriden by derived controls to update the
        /// displayed weight.
        /// </summary>
        public virtual void RefreshContentWeight()
        {
            // Makes sure we don't forget to implement this in derived classes
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save the current content of the service control to the given shipments.
        /// </summary>
        public virtual void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();

            personControl.SaveToEntity();

            // Save the data to each selected shipment
            foreach (ShipmentEntity shipment in loadedShipments)
            {
                // Residential
                if (ShipmentTypeManager.GetType(shipment).IsResidentialStatusRequired(shipment))
                {
                    ResidentialDeterminationType? type = null;

                    // Read the selected type
                    residentialDetermination.ReadMultiValue(v => { if (v != null) type = (ResidentialDeterminationType) v; } );

                    if (type != null)
                    {
                        // Only fedex can use fedex address lookup
                        if (type != ResidentialDeterminationType.FedExAddressLookup || shipment.ShipmentType == (int) ShipmentTypeCode.FedEx)
                        {
                            shipment.ResidentialDetermination = (int) type;
                        }
                    }
                }
            }

            SaveReturnsToShipments();

            ResumeRateCriteriaChangeEvent();
        }

        /// <summary>
        /// Suspend raising the event that rate criteria has changed
        /// </summary>
        protected void SuspendRateCriteriaChangeEvent()
        {
            suspendRateEvent++;
        }
        
        /// <summary>
        /// Resume raising the event that the rate critiera has changed.  This function does not raise the event
        /// </summary>
        protected void ResumeRateCriteriaChangeEvent()
        {
            suspendRateEvent--;
        }

        /// <summary>
        /// Raise the event to notify listeners that data that affects rates has changed
        /// </summary>
        protected void RaiseRateCriteriaChanged()
        {
            if (suspendRateEvent == 0)
            {
                if (RateCriteriaChanged != null)
                {
                    RateCriteriaChanged(this, EventArgs.Empty);
                }
            }
        }


        /// <summary>
        /// The shipments last past to LoadShipments
        /// </summary>
        public List<ShipmentEntity> LoadedShipments
        {
            get { return loadedShipments; }
        }

        /// <summary>
        /// The enable editing value last past to LoadShipments
        /// </summary>
        protected bool EnableEditing
        {
            get { return enableEditing; }
        }

        /// <summary>
        /// Laying out controls
        /// </summary>
        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            int location = AutoScrollPosition.Y + 5;

            foreach (CollapsibleGroupControl group in Controls.OfType<CollapsibleGroupControl>())
            {
                if (group.Visible)
                {
                    group.Location = new Point(group.Location.X, location);
                    location = group.Bottom + 5;
                }
            }
        }

        /// <summary>
        /// One or more shipments have been created by the control
        /// </summary>
        protected void RaiseShipmentsAdded(List<ShipmentEntity> shipments)
        {
            if (ShipmentsAdded != null)
            {
                ShipmentsAdded(this, new ShipmentsAddedRemovedEventArgs(shipments));
            }
        }

        /// <summary>
        /// User has changed the recipient state\country
        /// </summary>
        private void OnRecipientDestinationChanged(object sender, EventArgs e)
        {
            if (RecipientDestinationChanged != null)
            {
                RecipientDestinationChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Something changed about the data typed in for the person
        /// </summary>
        void OnPersonContentChanged(object sender, EventArgs e)
        {
            UpdateExtraText();

            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Update the display of the extra text area
        /// </summary>
        private void UpdateExtraText()
        {
            string name = personControl.FullName ?? "(Multiple Names)";

            string country = "Domestic\\International";
            if (personControl.CountryCode != null)
            {
                country = ShipmentType.IsDomestic(loadedShipments[0]) ? "Domestic" : "International";
            }

            sectionRecipient.ExtraText = string.Format("{0}, {1}", name, country);
        }

        /// <summary>
        /// Click of the Return Shipment checkbox
        /// </summary>
        private void OnReturnShipmentChanged(object sender, EventArgs e)
        {
            returnsPanel.Enabled = returnShipment.Checked;
        }

        /// <summary>
        /// Any derived classes should override to update their display of insurance rates
        /// </summary>
        public virtual void UpdateInsuranceDisplay()
        {

        }

        /// <summary>
        /// Raises the shipment service changed Event
        /// </summary>
        protected void RaiseShipmentServiceChanged()
        {
            if (ShipmentServiceChanged != null)
            {
                ShipmentServiceChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the shipment type changed event.
        /// </summary>
        protected void RaiseShipmentTypeChanged()
        {
            if (ShipmentTypeChanged != null)
            {
                ShipmentTypeChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the rates cleared event.
        /// </summary>
        protected void RaiseRatesCleared()
        {
            if (RatesCleared != null)
            {
                RatesCleared(this, EventArgs.Empty);
            }
        }

    }
}
