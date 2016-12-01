using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// A preprocessor that can be called prior to processing a shipment.
    /// </summary>
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.Amazon)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.Endicia)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.Express1Endicia)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.Express1Usps)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.FedEx)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.iParcel)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.None)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.OnTrac)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.Other)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.PostalWebTools)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.UpsOnLineTools)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.UpsWorldShip)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.Usps)]
    public class GenericShipmentPreProcessor : IShipmentPreProcessor
    {
        private readonly ICarrierAccountRetrieverFactory accountRetrieverFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IShippingManager shippingManager;
        private readonly IShippingSettings shippingSettings;
        private readonly IShipmentTypeSetupWizardFactory createSetupWizard;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public GenericShipmentPreProcessor(IShippingManager shippingManager,
            IShipmentTypeManager shipmentTypeManager,
            IShippingSettings shippingSettings,
            ICarrierAccountRetrieverFactory accountRetrieverFactory,
            IMessageHelper messageHelper,
            IShipmentTypeSetupWizardFactory createSetupWizard,
            ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.shippingManager = shippingManager;
            this.shipmentTypeManager = shipmentTypeManager;
            this.shippingSettings = shippingSettings;
            this.accountRetrieverFactory = accountRetrieverFactory;
            this.messageHelper = messageHelper;
            this.createSetupWizard = createSetupWizard;
        }

        /// <summary>
        /// Uses the synchronizer to check whether an account exists and call the counterRatesProcessing callback
        /// provided when trying to process a shipment without any accounts for this shipment type in ShipWorks,
        /// otherwise the shipment is unchanged.
        /// </summary>
        public virtual IEnumerable<ShipmentEntity> Run(ShipmentEntity shipment, RateResult selectedRate, Action configurationCallback)
        {
            ICarrierAccountRetriever accountRetriever = accountRetrieverFactory.Create(shipment.ShipmentTypeCode);

            if (IsReadyToShip(shipment.ShipmentTypeCode, accountRetriever))
            {
                shippingManager.EnsureShipmentLoaded(shipment);
                return new[] { shipment };
            }

            // Invoke the counter rates callback
            if (CounterRatesProcessing(shipment, configurationCallback) != DialogResult.OK)
            {
                // The user canceled, so we need to stop processing
                return null;
            }

            // The user created an account, so try to grab the account and use it
            // to process the shipment
            shippingSettings.CheckForChangesNeeded();

            if (IsReadyToShip(shipment.ShipmentTypeCode, accountRetriever))
            {
                // Assign the account ID and save the shipment
                ICarrierAccount carrierAccount = accountRetriever.AccountsReadOnly.FirstOrDefault();
                if (carrierAccount == null)
                {
                    throw new CarrierException($"An account for {EnumHelper.GetDescription(shipment.ShipmentTypeCode)} must be created to process this shipment.");
                }

                carrierAccount.ApplyTo(shipment);

                using (ISqlAdapter adapter = sqlAdapterFactory.CreateTransacted())
                {
                    adapter.SaveAndRefetch(shipment);
                    adapter.Commit();
                }

                return new[] { shipment };
            }

            // There still aren't any accounts for some reason, so throw an exception
            throw new ShippingException("An account must be created to process this shipment.");
        }

        /// <summary>
        /// Is the carrier ready to ship
        /// </summary>
        private bool IsReadyToShip(ShipmentTypeCode shipmentType, ICarrierAccountRetriever accountRetriever)
        {
            return shippingSettings.IsConfigured(shipmentType) && accountRetriever.AccountsReadOnly.Any();
        }

        /// <summary>
        /// Method used when processing a (non-best rate) shipment for a provider that does not have any
        /// accounts setup, and we need to provide the user with a way to sign up for the carrier.
        /// </summary>
        /// <returns></returns>
        private DialogResult CounterRatesProcessing(ShipmentEntity shipment, Action configurationCallback)
        {
            // This is for a specific shipment type, so we're always going to need to show the wizard
            // since the user explicitly chose to process with this provider
            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);

            DialogResult result = DialogResult.Cancel;

            // If this shipment type is not allowed to have new registrations, cancel out.
            if (!shipmentType.IsAccountRegistrationAllowed)
            {
                messageHelper.ShowWarning($"Account registration is disabled for {EnumHelper.GetDescription(shipment.ShipmentTypeCode)}");
                return DialogResult.Cancel;
            }

            result = messageHelper.ShowDialog(() => createSetupWizard.Create(shipment.ShipmentTypeCode));

            if (result == DialogResult.OK)
            {
                shippingSettings.MarkAsConfigured(shipment.ShipmentTypeCode);

                // Make sure we've got the latest data for the shipment since the requested label format may have changed
                shippingManager.RefreshShipment(shipment);
                shippingManager.EnsureShipmentLoaded(shipment);

                configurationCallback?.Invoke();
            }

            return result;
        }
    }
}
