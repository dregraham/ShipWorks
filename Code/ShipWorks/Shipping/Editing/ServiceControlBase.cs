﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Base UserControl for all ShipmentType specific stuff.
    /// </summary>
    public partial class ServiceControlBase : UserControl
    {
        // The type of shipment this instance is servicing
        readonly ShipmentTypeCode shipmentTypeCode;

        private BindingList<KeyValuePair<long, string>> includeReturnProfiles = new BindingList<KeyValuePair<long, string>>();
        private BindingSource bindingSource = new BindingSource();

        bool enableEditing;
        bool isLoading;
        bool allowIncludeReturn = true;

        // Counter for rate criteria change event suspension
        int suspendRateEvent = 0;

        // Counter for ShipSense criteria change event suspension
        int suspendShipSenseFieldChangedEvent = 0;

        // control for configuring returns
        ReturnsControlBase returnsControl;

        /// <summary>
        /// Raise when the country or state of the recipient has changed
        /// </summary>
        public event EventHandler RecipientDestinationChanged;

        /// <summary>
        /// Raise when the country or state of the origin has changed
        /// </summary>
        public event EventHandler OriginDestinationChanged;

        /// <summary>
        /// Occurs when [shipment service changed].
        /// </summary>
        public event EventHandler ShipmentServiceChanged;

        /// <summary>
        /// Occurs when return service changes
        /// </summary>
        public event EventHandler ReturnServiceChanged;

        /// <summary>
        /// Raise when new shipments are generated by the control
        /// </summary>
        public event ShipmentsAddedRemovedEventHandler ShipmentsAdded;

        /// <summary>
        /// Raised when a shipment value that affects rate data has changed, so that the shipping window can know to invalidate rate data.
        /// </summary>
        public event EventHandler RateCriteriaChanged;

        /// <summary>
        /// Raised when a shipment value that affects ShipSense data has changed, so that the shipping window can synchronize ShipSense data with other shipments.
        /// </summary>
        public event EventHandler ShipSenseFieldChanged;

        /// <summary>
        /// Raised when something occurs that causes the service control to change the shipment type. For example, this was created to support
        /// the best rate shipment type and allowing the selection of a rate to change the shipment type.
        /// </summary>
        public event EventHandler ShipmentTypeChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ServiceControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceControlBase(ShipmentTypeCode shipmentTypeCode,
            RateControl rateControl)
            : this()
        {
            this.shipmentTypeCode = shipmentTypeCode;

            // Make sure the rate control shows all rates by default; other
            // service controls (i.e. best rate) can override this as needed
            RateControl = rateControl;

            personControl.EnableValidationControls = true;
        }

        /// <summary>
        /// Sets up the rate control - Configures ShowAllRates and ActionLink visibility.
        /// </summary>
        public virtual void SetupRateControl()
        {
            RateControl.ShowAllRates = true;
            RateControl.ShowSingleRate = false;
            RateControl.ActionLinkVisible = false;
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
        /// A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control
        /// </summary>
        protected RateControl RateControl { get; private set; }

        /// <summary>
        /// The shipments last past to LoadShipments
        /// </summary>
        public List<ShipmentEntity> LoadedShipments { get; private set; }

        /// <summary>
        /// The enable editing value last past to LoadShipments
        /// </summary>
        protected bool EnableEditing
        {
            get { return enableEditing; }
        }

        /// <summary>
        /// Clear the rates in the main rates grid
        /// </summary>
        public Action<string> ClearRatesAction { get; set; }

        /// <summary>
        /// Initialization
        /// </summary>
        public virtual void Initialize(ILifetimeScope lifetimeScope)
        {
            personControl.ValidatedAddressScope = lifetimeScope.Resolve<IValidatedAddressScope>();
            Initialize();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        protected virtual void Initialize()
        {

        }

        /// <summary>
        /// Loads the accounts. This should be overridden in derived classes to account for a sign-up via
        // processing a shipment with a provider that doesn't have any accounts
        /// </summary>
        public virtual void LoadAccounts()
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
            SuspendShipSenseFieldChangeEvent();

            isLoading = true;

            LoadedShipments = shipments.ToList();

            EnumHelper.BindComboBox<ThermalLanguage>(labelFormat, ShouldIncludeLabelFormatInList);

            this.enableEditing = enableEditing;

            personControl.DestinationChanged -= this.OnRecipientDestinationChanged;
            personControl.ContentChanged -= this.OnPersonContentChanged;


            EnableContentPanels(enableEditing, enableShippingAddress);

            personControl.LoadEntities(shipments.Select(s => new PersonAdapter(s, "Ship")).ToList());

            LoadResidentialUI(shipments);

            LoadReturnsUI();

            using (MultiValueScope scope = new MultiValueScope())
            {
                // Go through and load the data from each shipment
                foreach (ShipmentEntity shipment in shipments)
                {
                    labelFormat.ApplyMultiValue((ThermalLanguage) shipment.RequestedLabelFormat);

                    LoadResidentialStatus(shipment);
                }
            }

            personControl.DestinationChanged += this.OnRecipientDestinationChanged;
            personControl.ContentChanged += this.OnPersonContentChanged;

            UpdateExtraText();

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();

            isLoading = false;
        }

        /// <summary>
        /// Enables the content panels.
        /// </summary>
        /// <param name="enableEditing">if set to <c>true</c> [enable editing].</param>
        /// <param name="enableShippingAddress">if set to <c>true</c> [enable shipping address].</param>
        private void EnableContentPanels(bool enableEditing, bool enableShippingAddress)
        {
            // Enable\disable the ContentPanels... not the groups themselves, so the groups can still be open\closed
            foreach (CollapsibleGroupControl group in Controls.OfType<CollapsibleGroupControl>())
            {
                group.ContentPanel.Enabled = enableEditing;
            }

            if (enableEditing)
            {
                sectionRecipient.ContentPanel.Enabled = enableShippingAddress;
            }
        }

        /// <summary>
        /// Loads Residential Status for shipment.
        /// </summary>
        /// <param name="shipment"></param>
        private void LoadResidentialStatus(ShipmentEntity shipment)
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

        /// <summary>
        /// Loads ResidentialUI if any shipment requires residential status.
        /// </summary>
        private void LoadResidentialUI(IEnumerable<ShipmentEntity> shipments)
        {
            Boolean anyNeedResidential = shipments.Any(shipment => ShipmentTypeManager.GetType(shipment).IsResidentialStatusRequired(shipment));

            UpdateResidentialDisplay(anyNeedResidential);

            // Load residential stuff only if necessary
            if (anyNeedResidential)
            {
                bool anyFedex = shipments.Any(s => s.ShipmentType == (int) ShipmentTypeCode.FedEx);
                LoadResidentialDeterminationOptions(anyFedex);
            }
        }

        /// <summary>
        /// Event raised when Rate is selected in rate control.
        /// </summary>
        public virtual void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
        }

        /// <summary>
        /// Called when the configure rate is clicked
        /// </summary>
        public virtual void OnConfigureRateClick(object sender, RateSelectedEventArgs e)
        {
            OnRateSelected(sender, e);
        }

        /// <summary>
        /// Rebinds labelformat dropdown taking LoadShipments into account.
        /// </summary>
        protected void UpdateLabelFormat()
        {
            labelFormat.BindToEnumAndPreserveSelection<ThermalLanguage>(ShouldIncludeLabelFormatInList);
        }

        /// <summary>
        /// Should the specified label format be included in the list of available formats
        /// </summary>
        protected virtual bool ShouldIncludeLabelFormatInList(ThermalLanguage format)
        {
            return true;
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
                    foreach (ShipmentEntity shipment in LoadedShipments)
                    {
                        returnShipment.ReadMultiCheck(v => shipment.ReturnShipment = v);
                        includeReturn.ReadMultiCheck(v => shipment.IncludeReturn = v);
                        applyReturnProfile.ReadMultiCheck(v => shipment.ApplyReturnProfile = v);
                        returnProfileID.ReadMultiValue(v => shipment.ReturnProfileID = (long) v);
                    }
                }
                returnsControl?.SaveToShipments();
            }
        }

        /// <summary>
        /// Loads the UI for Return Shipments
        /// </summary>
        [NDependIgnoreLongMethodAttribute]
        private void LoadReturnsUI()
        {
            ReturnsControlBase newReturnsControl = null;

            List<ShipmentType> loadedTypes = LoadedShipments.Select(s => s.ShipmentType).Distinct().Select(st => ShipmentTypeManager.GetType((ShipmentTypeCode) st)).ToList();

            bool anyReturnsSupported = loadedTypes.Any(st => st.SupportsReturns);

            if (anyReturnsSupported)
            {
                RefreshIncludeReturnProfileMenu(shipmentTypeCode);
                returnProfileID.DisplayMember = "Value";
                returnProfileID.ValueMember = "Key";

                bool allReturnsSupported = loadedTypes.All(st => st.SupportsReturns);

                // Always show it if any types support returns
                sectionReturns.Visible = true;

                // Checkbox for enabling returns editing
                using (MultiValueScope scope = new MultiValueScope())
                {
                    foreach (ShipmentEntity shipment in LoadedShipments)
                    {
                        returnShipment.ApplyMultiCheck(shipment.ReturnShipment);
                        includeReturn.ApplyMultiCheck(shipment.IncludeReturn);
                        applyReturnProfile.ApplyMultiCheck(shipment.ApplyReturnProfile);
                        returnProfileID.ApplyMultiValue(shipment.ReturnProfileID);
                    }
                }

                // Only if there all the same type can we create the type-specific control
                if (loadedTypes.Count == 1)
                {
                    newReturnsControl = loadedTypes[0].CreateReturnsControl();
                }
                else
                {
                    newReturnsControl = new MultiSelectReturnsControl();
                }

                newReturnsControl.LoadShipments(LoadedShipments);
                newReturnsControl.RateCriteriaChanged += OnRateCriteriaChanged;

                // add the control to the UI
                newReturnsControl.Location = new Point(0, 0);
                newReturnsControl.Width = returnsPanel.Width;
                newReturnsControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                returnsPanel.Controls.Add(newReturnsControl);

                // Sizing
                returnsPanel.Height = newReturnsControl.Height;
                sectionReturns.Height = returnsPanel.Bottom + 5 + (sectionReturns.Height - sectionReturns.ContentPanel.Height);

                // only enable for editing if all shipments are Returns
                returnsPanel.Enabled = (LoadedShipments.All(s => s.ReturnShipment));

                // Only enable automatic return labels for Endicia
                allowIncludeReturn = LoadedShipments.All(st => st.ShipmentTypeCode == ShipmentTypeCode.Endicia);

                // Only enable returns controls if all selected shipments support it
                includeReturn.Enabled = allowIncludeReturn && allReturnsSupported && !returnShipment.Checked;
                returnShipment.Enabled = allReturnsSupported && !includeReturn.Checked;
                applyReturnProfile.Enabled = allReturnsSupported && includeReturn.Checked;
                returnProfileID.Enabled = allReturnsSupported && applyReturnProfile.Checked;
                returnProfileIDLabel.Enabled = allReturnsSupported && applyReturnProfile.Checked;
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
        /// Load the options for selecting residential determination based on if any are fedex.  For now only fedex offers residential\commercial
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
        /// Sometimes the weight can change through other means, like through the customs editor.  This should be overriden by derived controls to update the
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
            SuspendShipSenseFieldChangeEvent();

            personControl.SaveToEntity();

            // Save the data to each selected shipment
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

                if (sectionLabelOptions.Visible)
                {
                    labelFormat.ReadMultiValue(v => shipmentType.SaveRequestedLabelFormat((ThermalLanguage) v, shipment));
                }

                // Residential
                if (shipmentType.IsResidentialStatusRequired(shipment))
                {
                    ResidentialDeterminationType? type = null;

                    // Read the selected type
                    residentialDetermination.ReadMultiValue(v => { if (v != null) type = (ResidentialDeterminationType) v; });

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
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Suspend raising the event that rate criteria has changed
        /// </summary>
        public void SuspendRateCriteriaChangeEvent()
        {
            suspendRateEvent++;
        }

        /// <summary>
        /// Resume raising the event that the rate criteria has changed.  This function does not raise the event
        /// </summary>
        protected void ResumeRateCriteriaChangeEvent()
        {
            suspendRateEvent--;
        }

        /// <summary>
        /// One of the values that affects rates has changed
        /// </summary>
        /// <remarks>
        /// This is private and not shared because Visual Studio's designer deletes event wiring that
        /// uses handlers defined in a base class. Subclasses will need to copy and paste this method.</remarks>
        private void OnRateCriteriaChanged(object sender, EventArgs e) => RaiseRateCriteriaChanged();

        /// <summary>
        /// Raise the event to notify listeners that data that affects rates has changed
        /// </summary>
        protected void RaiseRateCriteriaChanged()
        {
            if (suspendRateEvent == 0)
            {
                RateCriteriaChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Suspend raising the event that ShipSense criteria has changed
        /// </summary>
        protected void SuspendShipSenseFieldChangeEvent()
        {
            suspendShipSenseFieldChangedEvent++;
        }

        /// <summary>
        /// Resume raising the event that the rate ShipSense has changed.  This function does not raise the event
        /// </summary>
        protected void ResumeShipSenseFieldChangeEvent()
        {
            suspendShipSenseFieldChangedEvent--;
        }

        /// <summary>
        /// Raise the event to notify listeners that data that affects ShipSense has changed
        /// </summary>
        protected void RaiseShipSenseFieldChanged()
        {
            if (suspendShipSenseFieldChangedEvent == 0)
            {
                ShipSenseFieldChanged?.Invoke(this, EventArgs.Empty);
            }
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
        protected void RaiseShipmentsAdded(List<ShipmentEntity> shipments) =>
            ShipmentsAdded?.Invoke(this, new ShipmentsAddedRemovedEventArgs(shipments));

        /// <summary>
        /// Synchronizes the selected rate in the rate control.
        /// </summary>
        public virtual void SyncSelectedRate()
        { }

        /// <summary>
        /// User has changed the recipient state\country
        /// </summary>
        private void OnRecipientDestinationChanged(object sender, EventArgs e) =>
            RecipientDestinationChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// User has changed the recipient state\country
        /// </summary>
        protected virtual void OnOriginDestinationChanged(object sender, EventArgs e) =>
            OriginDestinationChanged?.Invoke(this, EventArgs.Empty);

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
                country = ShipmentTypeManager.GetType(LoadedShipments[0]).IsDomestic(LoadedShipments[0]) ? "Domestic" : "International";
            }

            sectionRecipient.ExtraText = string.Format("{0}, {1}", name, country);
        }

        /// <summary>
        /// Click of the Include Return checkbox
        /// </summary>
        private void OnIncludeReturnChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                if (!includeReturn.Checked)
                {
                    applyReturnProfile.Checked = false;
                }
                returnShipment.Enabled = !includeReturn.Checked;
                applyReturnProfile.Enabled = includeReturn.Checked;
                returnProfileID.Enabled = applyReturnProfile.Checked;
                returnProfileIDLabel.Enabled = applyReturnProfile.Checked;
                SaveReturnsToShipments();
            }
        }

        /// <summary>
        /// Click of the Apply Return Profile checkbox
        /// </summary>
        private void OnApplyReturnChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                returnProfileID.Enabled = applyReturnProfile.Checked;
                returnProfileIDLabel.Enabled = applyReturnProfile.Checked;
                SaveReturnsToShipments();
            }
        }

        /// <summary>
        /// Opening the return profiles menu
        /// </summary>
        private void OnReturnProfileIDOpened(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                // Populate the list of profiles
                RefreshIncludeReturnProfileMenu(shipmentTypeCode);
            }
        }

        /// <summary>
        /// A return profile is selected
        /// </summary>
        private void OnReturnProfileSelected(object sender, EventArgs e)
        {
            SaveReturnsToShipments();
        }

        /// <summary>
        /// Open the profiles manager
        /// </summary>
        private void OnManageProfiles(object sender, EventArgs e)
        {
            Messenger.Current.Send(new OpenProfileManagerDialogMessage(this));
        }

        /// <summary>
        /// When ReturnProfileID dropdown is enabled
        /// </summary>
        protected void OnReturnProfileIDEnabledChanged(object sender, EventArgs e)
        {
            if (returnProfileID.Enabled)
            {
                RefreshIncludeReturnProfileMenu(shipmentTypeCode);
            }
        }

        /// <summary>
        /// Add applicable profiles for the given shipment type to the context menu
        /// </summary>
        private void RefreshIncludeReturnProfileMenu(ShipmentTypeCode shipmentTypeCode)
        {
            BindingList<KeyValuePair<long, string>> newReturnProfiles = new BindingList<KeyValuePair<long, string>>();

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShippingProfileService shippingProfileService = lifetimeScope.Resolve<IShippingProfileService>();

                List<KeyValuePair<long, string>> returnProfiles = shippingProfileService
                    .GetConfiguredShipmentTypeProfiles()
                    .Where(p => p.ShippingProfileEntity.ShipmentType.HasValue)
                    .Where(p => p.IsApplicable(shipmentTypeCode))
                    .Where(p => p.ShippingProfileEntity.ShipmentType == shipmentTypeCode)
                    .Where(p => p.ShippingProfileEntity.ReturnShipment == true)
                    .Select(s => new KeyValuePair<long, string>(s.ShippingProfileEntity.ShippingProfileID, s.ShippingProfileEntity.Name))
                    .OrderBy(g => g.Value)
                    .ToList<KeyValuePair<long, string>>();

                newReturnProfiles = new BindingList<KeyValuePair<long, string>>(returnProfiles);
            }

            // Always add No Profile so if a selected profile was deleted, this becomes the default
            newReturnProfiles.Insert(0, new KeyValuePair<long, string>(-1, "(No Profile)"));

            includeReturnProfiles = newReturnProfiles;

            // Reset data sources because calling resetbindings() doesn't work
            bindingSource.DataSource = includeReturnProfiles;
            returnProfileID.DataSource = bindingSource;
        }

        /// <summary>
        /// Click of the Return Shipment checkbox
        /// </summary>
        protected virtual void OnReturnShipmentChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                includeReturn.Enabled = allowIncludeReturn && !returnShipment.Checked;
                returnsPanel.Enabled = returnShipment.Checked;
                SaveReturnsToShipments();
                ReturnServiceChanged?.Invoke(this, EventArgs.Empty);
            }
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
        protected void RaiseShipmentServiceChanged() => ShipmentServiceChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Raises the shipment type changed event.
        /// </summary>
        protected void RaiseShipmentTypeChanged() => ShipmentTypeChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Show the knowledge base article for thermal settings
        /// </summary>
        private void OnHelpClick(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://shipworks.zendesk.com/hc/en-us/articles/360022467092", this);
        }

        /// <summary>
        /// Pre select a rate
        /// </summary>
        public virtual void PreSelectRate(RateSelectedEventArgs args) => OnConfigureRateClick(this, args);

        /// <summary>
        /// Flush any in-progress changes before saving
        /// </summary>
        /// <remarks>This should cause weight controls to finish, etc.</remarks>
        public virtual void FlushChanges()
        {

        }

        /// <summary>
        /// Unload shipments
        /// </summary>
        internal void UnloadShipments()
        {
            LoadedShipments.Clear();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();

                RecipientDestinationChanged = null;
                OriginDestinationChanged = null;
                ShipmentServiceChanged = null;
                RateCriteriaChanged = null;
                ShipSenseFieldChanged = null;
                ShipmentsAdded = null;
                ShipmentTypeChanged = null;
                ClearRatesAction = null;

                if (!DesignMode && !DesignModeDetector.IsDesignerHosted())
                {
                    UnloadShipments();
                }
            }

            base.Dispose(disposing);
        }
    }
}
