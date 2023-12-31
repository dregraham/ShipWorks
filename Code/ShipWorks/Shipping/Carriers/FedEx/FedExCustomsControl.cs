﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Customized customs control for FedEx
    /// </summary>
    public partial class FedExCustomsControl : CustomsControlBase
    {
        private IDisposable fedExServiceChangedToken;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExCustomsControl()
        {
            InitializeComponent();

            fedExServiceChangedToken = Messenger.Current.OfType<FedExServiceTypeChangedMessage>().Subscribe(OnServiceChanged);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            //this.RemoveWeight();

            EnumHelper.BindComboBox<FedExPhysicalPackagingType>(admissibilityPackaging);
            EnumHelper.BindComboBox<FedExTermsOfSale>(ciTermsOfSale);
            EnumHelper.BindComboBox<FedExCommercialInvoicePurpose>(ciPurpose);
            EnumHelper.BindComboBox<FedExCustomsExportFilingOption>(filingOption);
            EnumHelper.BindComboBox<FedExNaftaDeterminationCode>(naftaProducerDetermination);
            EnumHelper.BindComboBox<FedExNaftaPreferenceCriteria>(naftaPreference);
            EnumHelper.BindComboBox<FedExNaftaNetCostMethod>(naftaNetCostMethod);
            EnumHelper.BindComboBox<FedexTINType>(customsRecipientTINType);
        }

        /// <summary>
        /// Load the shipment data into the control
        /// </summary>
        [NDependIgnoreLongMethod]
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing)
        {
            base.LoadShipments(shipments, enableEditing);

            // Broker content
            brokerControl.LoadEntities(LoadedShipments.Select(s => new PersonAdapter(s.FedEx, "Broker")).ToList());

            // Importer of record
            iorPersonControl.LoadEntities(LoadedShipments.Select(s => new PersonAdapter(s.FedEx, "Importer")).ToList());

            bool anyCanada = false;

            naftaEnabled.CheckedChanged -= new EventHandler(OnNaftaEnabled);
            
            if (customsTinIssuingAuthority.DataSource == null)
            {
                customsTinIssuingAuthority.DataSource = Geography.Countries;
            }

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    if (shipment.AdjustedShipCountryCode() == "CA")
                    {
                        anyCanada = true;
                    }

                    brokerEnabled.ApplyMultiCheck(shipment.FedEx.BrokerEnabled);
                    brokerAccount.ApplyMultiText(shipment.FedEx.BrokerAccount);

                    documentsOnly.ApplyMultiCheck(shipment.FedEx.CustomsDocumentsOnly);

                    recipientTaxID.ApplyMultiText(shipment.FedEx.CustomsRecipientTIN);
                    customsRecipientTINType.ApplyMultiValue((FedexTINType) shipment.FedEx.CustomsRecipientTINType);

                    admissibilityPackaging.ApplyMultiValue((FedExPhysicalPackagingType) shipment.FedEx.CustomsAdmissibilityPackaging);

                    filingOption.ApplyMultiValue((FedExCustomsExportFilingOption) shipment.FedEx.CustomsExportFilingOption);
                    electronicExportInfo.ApplyMultiText(shipment.FedEx.CustomsAESEEI);

                    commercialInvoice.ApplyMultiCheck(shipment.FedEx.CommercialInvoice);
                    electronicTradeDocuments.ApplyMultiCheck(shipment.FedEx.CommercialInvoiceFileElectronically);
                    ciTermsOfSale.ApplyMultiValue((FedExTermsOfSale) shipment.FedEx.CommercialInvoiceTermsOfSale);
                    ciPurpose.ApplyMultiValue((FedExCommercialInvoicePurpose) shipment.FedEx.CommercialInvoicePurpose);
                    ciComments.ApplyMultiText(shipment.FedEx.CommercialInvoiceComments);
                    ciFreight.ApplyMultiAmount(shipment.FedEx.CommercialInvoiceFreight);
                    ciInsurance.ApplyMultiAmount(shipment.FedEx.CommercialInvoiceInsurance);
                    ciAdditional.ApplyMultiAmount(shipment.FedEx.CommercialInvoiceOther);
                    commercialInvoiceReference.ApplyMultiText(shipment.FedEx.CommercialInvoiceReference);

                    importerOfRecord.ApplyMultiCheck(shipment.FedEx.ImporterOfRecord);
                    iorTaxID.ApplyMultiText(shipment.FedEx.ImporterTIN);
                    iorFedExAccount.ApplyMultiText(shipment.FedEx.ImporterAccount);

                    naftaEnabled.ApplyMultiCheck(shipment.FedEx.CustomsNaftaEnabled);
                    naftaPreference.ApplyMultiValue((FedExNaftaPreferenceCriteria) shipment.FedEx.CustomsNaftaPreferenceType);
                    naftaProducerDetermination.ApplyMultiValue((FedExNaftaDeterminationCode) shipment.FedEx.CustomsNaftaDeterminationCode);
                    naftaNetCostMethod.ApplyMultiValue((FedExNaftaNetCostMethod) shipment.FedEx.CustomsNaftaNetCostMethod);
                    naftaProducerId.ApplyMultiText(shipment.FedEx.CustomsNaftaProducerId);
                    customsTinIssuingAuthority.ApplyMultiText(Geography.GetCountryName(shipment.FedEx.CustomsTinIssuingAuthority ?? "US"));
                }
            }

            // Admissibility is only for canada
            admissibilityPackaging.Visible = anyCanada;
            labelAdmissibilityPackaging.Visible = anyCanada;

            // Update the height based on admissibility visibility
            sectionGeneral.Height = (anyCanada ? admissibilityPackaging.Bottom : electronicExportInfo.Bottom) + 6 + (sectionGeneral.Height - sectionGeneral.ContentPanel.Height);

            //Update the NAFTA controls
            OnNaftaEnabled(naftaEnabled, EventArgs.Empty);
            naftaEnabled.CheckedChanged += OnNaftaEnabled;

            UpdateEnabledUI(enableEditing);

            UpdateControlVisibility(shipments.All(x => FedExUtility.IsFimsService((FedExServiceType) x.FedEx.Service)));
            
        }

        /// <summary>
        /// Update the editable state of the control
        /// </summary>
        private void UpdateEnabledUI(bool enableEditing)
        {
            numberOfPieces.Enabled = enableEditing;
            unitPrice.Enabled = enableEditing;
        }

        /// <summary>
        /// Handle when the service type changes
        /// </summary>
        private void OnServiceChanged(FedExServiceTypeChangedMessage message)
        {
            UpdateControlVisibility(FedExUtility.IsFimsService(message.ServiceType));
        }

        /// <summary>
        /// Change the visibility of panels
        /// </summary>
        private void UpdateControlVisibility(bool isFims)
        {
            sectionNafta.Visible = !isFims;
            sectionCommercialInvoice.Visible = !isFims;
            sectionBroker.Visible = !isFims;
        }

        /// <summary>
        /// Loads the form data.
        /// </summary>
        /// <param name="customsItem">The customs item.</param>
        protected override void LoadFormData(ShipmentCustomsItemEntity customsItem)
        {
            base.LoadFormData(customsItem);

            numberOfPieces.ApplyMultiText(customsItem.NumberOfPieces.ToString());
            unitPrice.ApplyMultiAmount(customsItem.UnitPriceAmount);
        }

        /// <summary>
        /// Saves the customs item.
        /// </summary>
        /// <param name="customsItem">The customs item.</param>
        /// <param name="changedWeights">The changed weights.</param>
        /// <param name="changedValues">The changed values.</param>
        protected override void SaveCustomsItem(ShipmentCustomsItemEntity customsItem, List<long> changedWeights, List<long> changedValues)
        {
            base.SaveCustomsItem(customsItem, changedWeights, changedValues);

            numberOfPieces.ReadMultiText(s =>
            {
                int parsedNumberOfPieces;
                if (int.TryParse(s, out parsedNumberOfPieces))
                {
                    customsItem.NumberOfPieces = parsedNumberOfPieces;
                }
            });

            unitPrice.ReadMultiText(s =>
            {
                decimal parsedUnitPrice;
                if (decimal.TryParse(s, NumberStyles.Any, null, out parsedUnitPrice))
                {
                    customsItem.UnitPriceAmount = parsedUnitPrice;
                }
            });
        }

        /// <summary>
        /// Handle when Nafta is enabled
        /// </summary>
        private void OnNaftaEnabled(object sender, EventArgs e)
        {
            EnableNaftaControls(naftaEnabled.Checked);
        }

        /// <summary>
        /// Enables the controls in the NAFTA group.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        private void EnableNaftaControls(bool enable)
        {
            naftaNetCostMethod.Enabled = enable;
            naftaPreference.Enabled = enable;
            naftaProducerDetermination.Enabled = enable;
            naftaProducerId.Enabled = enable;
        }

        /// <summary>
        /// Save the data in the control to the shipments
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public override void SaveToShipments()
        {
            base.SaveToShipments();

            // Broker address
            brokerControl.SaveToEntity();

            // Importer of record
            iorPersonControl.SaveToEntity();

            // Save the data to the shipment entity
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                brokerEnabled.ReadMultiCheck(c => shipment.FedEx.BrokerEnabled = c);
                brokerAccount.ReadMultiText(t => shipment.FedEx.BrokerAccount = t);

                documentsOnly.ReadMultiCheck(c => shipment.FedEx.CustomsDocumentsOnly = c);

                recipientTaxID.ReadMultiText(t => shipment.FedEx.CustomsRecipientTIN = t);
                customsRecipientTINType.ReadMultiValue(v => shipment.FedEx.CustomsRecipientTINType = (int) v);

                admissibilityPackaging.ReadMultiValue(v => shipment.FedEx.CustomsAdmissibilityPackaging = (int) v);

                filingOption.ReadMultiValue(v => shipment.FedEx.CustomsExportFilingOption = (int) (v ?? FedExCustomsExportFilingOption.NotRequired)) ;
                electronicExportInfo.ReadMultiText(t => shipment.FedEx.CustomsAESEEI = t);

                commercialInvoice.ReadMultiCheck(c => shipment.FedEx.CommercialInvoice = c);
                electronicTradeDocuments.ReadMultiCheck(c => shipment.FedEx.CommercialInvoiceFileElectronically = c);
                ciTermsOfSale.ReadMultiValue(v => shipment.FedEx.CommercialInvoiceTermsOfSale = (int) (v ?? FedExTermsOfSale.FOB_or_FCA));
                ciPurpose.ReadMultiValue(v => shipment.FedEx.CommercialInvoicePurpose = (int) (v ?? FedExCommercialInvoicePurpose.Sold));
                ciComments.ReadMultiText(t => shipment.FedEx.CommercialInvoiceComments = t);
                ciFreight.ReadMultiAmount(a => shipment.FedEx.CommercialInvoiceFreight = a);
                ciInsurance.ReadMultiAmount(a => shipment.FedEx.CommercialInvoiceInsurance = a);
                ciAdditional.ReadMultiAmount(a => shipment.FedEx.CommercialInvoiceOther = a);
                commercialInvoiceReference.ReadMultiText(t => shipment.FedEx.CommercialInvoiceReference = t);

                importerOfRecord.ReadMultiCheck(c => shipment.FedEx.ImporterOfRecord = c);
                iorFedExAccount.ReadMultiText(t => shipment.FedEx.ImporterAccount = t);
                iorTaxID.ReadMultiText(t => shipment.FedEx.ImporterTIN = t);

                naftaEnabled.ReadMultiCheck(c => shipment.FedEx.CustomsNaftaEnabled = c);
                naftaPreference.ReadMultiValue(v => shipment.FedEx.CustomsNaftaPreferenceType = (int) v);
                naftaProducerDetermination.ReadMultiValue(v => shipment.FedEx.CustomsNaftaDeterminationCode = (int) v);
                naftaNetCostMethod.ReadMultiValue(v => shipment.FedEx.CustomsNaftaNetCostMethod = (int) v);
                naftaProducerId.ReadMultiText(v => shipment.FedEx.CustomsNaftaProducerId = v);
                customsTinIssuingAuthority.ReadMultiText(v => shipment.FedEx.CustomsTinIssuingAuthority = Geography.GetCountryCode(v));
            }
        }
    }
}
