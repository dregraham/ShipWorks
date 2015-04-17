using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Controls.Design;
using ShipWorks.UI.Utility;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// UserControl for editing the details of a postal shipment
    /// </summary>
    public partial class PostalServiceControlBase : ServiceControlBase
    {
        /// <summary>
        /// Constructor that is needed for the designer to work
        /// </summary>
        protected PostalServiceControlBase()
        {
            if (!DesignModeDetector.IsDesignerHosted())
            {
                throw new InvalidOperationException("The default constructor for PostalServiceControlBase should only be used by the VS designer");
            }

            InitializeComponent();
        }

        /// <summary>
        /// For inherited designer support
        /// </summary>
        /// <param name="shipmentTypeCode"></param>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected PostalServiceControlBase(ShipmentTypeCode shipmentTypeCode, RateControl rateControl) 
            : base(shipmentTypeCode, rateControl)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the comboboxes
        /// </summary>
        public override void Initialize()
        {
            dimensionsControl.Initialize();

            service.DisplayMember = "Key";
            service.ValueMember = "Value";

            confirmation.DisplayMember = "Key";
            confirmation.ValueMember = "Value";

            // Only Express 1 Endicia should see the cubic packaging type
            packagingType.SelectedIndexChanged -= this.OnChangePackaging;
            EnumHelper.BindComboBox<PostalPackagingType>(packagingType, p => (p != PostalPackagingType.Cubic || ShipmentTypeCode == ShipmentTypeCode.Express1Endicia));
            packagingType.SelectedIndexChanged += this.OnChangePackaging;
        }

        /// <summary>
        /// Load the shipment data into the ui
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            // Load the base
            base.RecipientDestinationChanged -= new EventHandler(OnRecipientDestinationChanged);
            base.LoadShipments(shipments, enableEditing, enableShippingAddress);
            base.RecipientDestinationChanged += new EventHandler(OnRecipientDestinationChanged);

            // Stop the dimensions control from listening to weight changes
            dimensionsControl.ShipmentWeightBox = null;

            // Load shipment details
            LoadShipmentDetails();

            // Start the dimensiosn control listening to weight changes
            dimensionsControl.ShipmentWeightBox = weight;

            // Load the dimensions
            dimensionsControl.LoadDimensions(shipments.Select(s => new DimensionsAdapter(s.Postal)));

            // Insurance
            UpdateInsuranceDisplay();

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Update the services available based on the destination of the selected shipments
        /// </summary>
        private void LoadShipmentDetails()
        {
            bool allDomestic = true;
            bool allInternational = true;

            List<PostalConfirmationType> availableConfirmations = EnumHelper.GetEnumList<PostalConfirmationType>().Select(e => e.Value).ToList();

            bool allExpressMail = true;

            // Determine if all shipments will have the same destination service types
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                // Need to check with the store  to see if anything about the shipment was overridden in case
                // it may have effected the shipping services available (i.e. the eBay GSP program)
                ShipmentEntity overriddenShipment = ShippingManager.GetOverriddenStoreShipment(shipment);

                // Check the overridden shipment to see if all are international or domestic
                if (overriddenShipment.ShipPerson.IsDomesticCountry())
                {
                    allInternational = false;
                }
                else
                {
                    allDomestic = false;
                }
                
                PostalServiceType postalServiceType = (PostalServiceType) shipment.Postal.Service;

                PostalShipmentType postalShipmentType = ShipmentTypeManager.GetType(shipment) as PostalShipmentType;
                if (postalShipmentType == null)
                {
                    throw new ShippingException("ShipWorks encountered an unexpected shipment type.");
                }

                // See if all have confirmation as an option or not
                availableConfirmations = availableConfirmations.Intersect(
                    postalShipmentType.GetAvailableConfirmationTypes(shipment.ShipCountryCode, postalServiceType, (PostalPackagingType)shipment.Postal.PackagingType))
                        .ToList();

                if (postalServiceType != PostalServiceType.ExpressMail)
                {
                    allExpressMail = false;
                }
            }

            // Unhook events
            service.SelectedIndexChanged -= new EventHandler(OnServiceChanged);
            confirmation.SelectedIndexChanged -= OnConfirmationChanged;

            // If they are all international we can load up all the international services
            if (allInternational)
            {
                service.DataSource = ActiveEnumerationBindingSource
                    .Create<PostalServiceType>(PostalUtility.GetInternationalServices(ShipmentTypeCode)
                    .Select(type => new KeyValuePair<string, PostalServiceType>(PostalUtility.GetPostalServiceTypeDescription(type), type))
                    .ToList());
            }
            // If they are all domestic we can load up all the domestic services
            else if (allDomestic)
            {
                service.DataSource = ActiveEnumerationBindingSource.Create<PostalServiceType>(PostalUtility.GetDomesticServices(ShipmentTypeCode)
                    .Select(type => new KeyValuePair<string, PostalServiceType>(PostalUtility.GetPostalServiceTypeDescription(type), type))
                    .ToList());
            }
            // Otherwise there is nothing to choose from
            else
            {
                service.DataSource = new KeyValuePair<string, PostalServiceType>[0];
            }
            service.DisplayMember = "Key";
            service.ValueMember = "Value";

            // If they all have confirmation load the confirmation types
            if (availableConfirmations.Count > 0)
            {
                UpdateConfirmationTypes(availableConfirmations);
            }
            // Otherwise load nothing
            else
            {
                confirmation.DataSource = new KeyValuePair<string, PostalConfirmationType>[0];
                confirmation.Enabled = true;
            }
            confirmation.DisplayMember = "Key";
            confirmation.ValueMember = "Value";

            // Load all the shipment values
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    service.ApplyMultiValue((PostalServiceType) shipment.Postal.Service);
                    confirmation.ApplyMultiValue((PostalConfirmationType) shipment.Postal.Confirmation);

                    shipDate.ApplyMultiDate(shipment.ShipDate);
                    weight.ApplyMultiWeight(shipment.ContentWeight);

                    packagingType.ApplyMultiValue((PostalPackagingType) shipment.Postal.PackagingType);

                    nonRectangular.ApplyMultiCheck(shipment.Postal.NonRectangular);
                    nonMachinable.ApplyMultiCheck(shipment.Postal.NonMachinable);

                    expressSignatureWaiver.ApplyMultiCheck(shipment.Postal.ExpressSignatureWaiver);
                }
            }

            // Make sure confirmation looks multi-valued if it's a mix
            if (availableConfirmations.Count == 0)
            {
                confirmation.MultiValued = true;
            }

            // Only show express section if necessary
            sectionExpress.Visible = allExpressMail;

            // Rehook events
            service.SelectedIndexChanged += new EventHandler(OnServiceChanged);
            confirmation.SelectedIndexChanged += OnConfirmationChanged;

            // Update the descriptive section text
            UpdateSectionDescription();

            SyncSelectedRate();
        }

        /// <summary>
        /// Update the available choices for confirmation
        /// </summary>
        private void UpdateConfirmationTypes(List<PostalConfirmationType> availableConfirmations)
        {
            bool previousMulti = confirmation.MultiValued;
            object previousValue = confirmation.SelectedValue;

            List<KeyValuePair<string, PostalConfirmationType>> confirmationTypes = new List<KeyValuePair<string, PostalConfirmationType>>();

            if (availableConfirmations.Count == 0 || (availableConfirmations.Count == 1 && availableConfirmations[0] == PostalConfirmationType.None))
            {
                confirmation.Enabled = false;

                confirmationTypes.Add(new KeyValuePair<string, PostalConfirmationType>("(Not Available)", PostalConfirmationType.None));
            }
            else
            {
                confirmation.Enabled = true;

                confirmationTypes.AddRange(availableConfirmations.Select(type => new KeyValuePair<string, PostalConfirmationType>(EnumHelper.GetDescription(type), type)));
            }

            confirmation.DataSource = confirmationTypes;

            // Set back the previous value
            if (previousMulti)
            {
                confirmation.MultiValued = true;
            }
            else if (previousValue != null)
            {
                confirmation.SelectedValue = previousValue;

                if (confirmation.SelectedIndex == -1)
                {
                    confirmation.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Update the insurance rate display
        /// </summary>
        public override void UpdateInsuranceDisplay()
        {
            insuranceControl.InsuranceOptionsChanged -= OnRateCriteriaChanged;
            insuranceControl.LoadInsuranceChoices(LoadedShipments.Select(shipment => ShipmentTypeManager.GetType(shipment).GetParcelDetail(shipment, 0).Insurance));
            insuranceControl.InsuranceOptionsChanged += OnRateCriteriaChanged;
        }

        /// <summary>
        /// Refresh the weight box with the latest weight information from the loaded shipments
        /// </summary>
        public override void RefreshContentWeight()
        {
            // Stop the dimensions control from listening to weight changes
            dimensionsControl.ShipmentWeightBox = null;

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    weight.ApplyMultiWeight(shipment.ContentWeight);
                }
            }

            // Start the dimensiosn control listening to weight changes
            dimensionsControl.ShipmentWeightBox = weight;
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            base.SaveToShipments();

            // Save the dimensions
            dimensionsControl.SaveToEntities();

            // Save our controls
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                service.ReadMultiValue(v => { if (v != null) shipment.Postal.Service = (int) v; });
                confirmation.ReadMultiValue(v => { if (v != null) shipment.Postal.Confirmation = (int) v; });

                shipDate.ReadMultiDate(d => shipment.ShipDate = d.Date.AddHours(12));
                weight.ReadMultiWeight(v => shipment.ContentWeight = v);

                packagingType.ReadMultiValue(v => shipment.Postal.PackagingType = (int) v);

                nonRectangular.ReadMultiCheck(c => shipment.Postal.NonRectangular = c);
                nonMachinable.ReadMultiCheck(c => shipment.Postal.NonMachinable = c);

                expressSignatureWaiver.ReadMultiCheck(c => shipment.Postal.ExpressSignatureWaiver = c);
            }

            insuranceControl.SaveToInsuranceChoices();

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Called whent the recipient country has changed.  We may have to switch from an international to domestic UI
        /// </summary>
        void OnRecipientDestinationChanged(object sender, EventArgs e)
        {
            SaveToShipments();
            LoadShipmentDetails();
        }

        /// <summary>
        /// Some aspect of the shipment that affects rates has changed
        /// </summary>
        private void OnRateCriteriaChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Some aspect of the shipment that affects ShipSense has changed
        /// </summary>
        private void OnShipSenseFieldChanged(object sender, EventArgs e)
        {
            RaiseShipSenseFieldChanged();
        }

        /// <summary>
        /// Something has changed that makes the shipment details header need updated
        /// </summary>
        private void OnServiceChanged(object sender, EventArgs e)
        {
            PostalServiceType serviceType = service.SelectedValue == null ? PostalServiceType.PriorityMail : (PostalServiceType)service.SelectedValue;

            // Update the available confirmation types based on the shipping provider
            PostalShipmentType postalShipmentType = ShipmentTypeManager.GetType(this.ShipmentTypeCode) as PostalShipmentType;            
            UpdateConfirmationTypes(postalShipmentType.GetAvailableConfirmationTypes(personControl.CountryCode, serviceType, (PostalPackagingType?) packagingType.SelectedValue));

            // Only show express options for express
            sectionExpress.Visible = (serviceType == PostalServiceType.ExpressMail);

            // Update section description
            UpdateSectionDescription();

            UpdateAvailableShipmentOptions((PostalPackagingType?) packagingType.SelectedValue);

            SyncSelectedRate();
        }
        
        /// <summary>
        /// Synchronizes the selected rate in the rate control.
        /// </summary>
        public override void SyncSelectedRate()
        {
            PostalServiceType serviceType = service.SelectedValue == null ? PostalServiceType.PriorityMail : (PostalServiceType)service.SelectedValue;
            PostalConfirmationType confirmationType = confirmation.SelectedValue == null ? PostalConfirmationType.None : (PostalConfirmationType)confirmation.SelectedValue;

            if (!service.MultiValued && !confirmation.MultiValued)
            {
                // Update the selected rate in the rate control to coincide with the service change
                PostalRateSelection rateSelection = new PostalRateSelection(serviceType, confirmationType);
                RateResult matchingRate = RateControl.RateGroup.Rates.Where(x => x.Selectable).FirstOrDefault(r =>
                {
                    if (r.Tag == null)
                    {
                        // The rates in the rates grid hasn't caught up or something else wacky is going on
                        return false;
                    }

                    PostalRateSelection current = r.OriginalTag as PostalRateSelection;
                    if (current == null)
                    {
                        // This isn't an actual rate - just a row in the grid for the section header
                        return false;
                    }

                    return current.ConfirmationType == rateSelection.ConfirmationType && current.ServiceType == rateSelection.ServiceType;
                });

                RateControl.SelectRate(matchingRate);
            }
            else
            {
                RateControl.ClearSelection();
            }
        }

        /// <summary>
        /// The selected packaging type is changing
        /// </summary>
        private void OnChangePackaging(object sender, EventArgs e)
        {
            OnRateCriteriaChanged(sender, e);

            // If we know the Service to be First (and only first) then make sure we are showing the right thing
            if (!service.MultiValued && packagingType.SelectedValue != null)
            {
                PostalServiceType serviceType = (PostalServiceType) service.SelectedValue;

                // Update the available confirmation types based on the shipping provider
                PostalShipmentType postalShipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode) as PostalShipmentType;
                UpdateConfirmationTypes(postalShipmentType.GetAvailableConfirmationTypes(personControl.CountryCode, serviceType, (PostalPackagingType) packagingType.SelectedValue));

                UpdateAvailableShipmentOptions((PostalPackagingType)packagingType.SelectedValue);
            }
        }

        /// <summary>
        /// Something has changed that makes the shipment details header need updated
        /// </summary>
        private void OnConfirmationChanged(object sender, EventArgs e)
        {
            // Update section description
            UpdateSectionDescription();

            SyncSelectedRate();
        }

        /// <summary>
        /// Updates the shipment options based on the configured shipment values
        /// </summary>
        protected virtual void UpdateAvailableShipmentOptions(PostalPackagingType? postalPackagingType)
        {
            // Do nothing here, but provide a hook for derived classes to implement custom functionality
        }

        /// <summary>
        /// Update the descriptive text for the section
        /// </summary>
        private void UpdateSectionDescription()
        {
            string text = null;

            if (service.MultiValued || confirmation.MultiValued)
            {
                text = "(Multiple)";
            }
            else
            {
                if (service.SelectedValue != null)
                {
                    text = EnumHelper.GetDescription((PostalServiceType) service.SelectedValue);
                }

                if (confirmation.SelectedValue != null)
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        text += ", ";
                    }

                    text += EnumHelper.GetDescription((PostalConfirmationType) confirmation.SelectedValue);
                }
            }

            sectionShipment.ExtraText = text;
        }

        /// <summary>
        /// A shipping rate has been selected
        /// </summary>
        public override void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            PostalRateSelection rate = e.Rate.OriginalTag as PostalRateSelection;

            service.SelectedValue = rate.ServiceType;

            confirmation.SelectedValue = rate.ConfirmationType;
            if (confirmation.SelectedIndex == -1)
            {
                confirmation.SelectedIndex = 0;
            }
        }
    }
}
