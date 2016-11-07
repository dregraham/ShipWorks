using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Stores;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Services.ShipmentProcessorPhases
{
    /// <summary>
    /// Get the label for the shipment
    /// </summary>
    [Component(RegistrationType.Self)]
    public class GetLabelPhase
    {
        private readonly IShippingSettings shippingSettings;
        private readonly IShippingManager shippingManager;
        private readonly ILicenseService licenseService;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IStoreTypeManager storeTypeManager;
        private readonly ILabelServiceFactory labelServiceFactory;
        private readonly IInsuranceUtility insuranceUtility;
        private readonly IResidentialDeterminationService residentialDeterminationService;
        private readonly IKnowledgebase knowledgebase;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public GetLabelPhase(IShippingSettings shippingSettings,
            IShippingManager shippingManager,
            ILicenseService licenseService,
            IShipmentTypeManager shipmentTypeManager,
            IStoreTypeManager storeTypeManager,
            ILabelServiceFactory labelServiceFactory,
            IInsuranceUtility insuranceUtility,
            IResidentialDeterminationService residentialDeterminationService,
            IKnowledgebase knowledgebase,
            Func<Type, ILog> createLogger)
        {
            this.residentialDeterminationService = residentialDeterminationService;
            this.knowledgebase = knowledgebase;
            this.insuranceUtility = insuranceUtility;
            this.labelServiceFactory = labelServiceFactory;
            this.storeTypeManager = storeTypeManager;
            this.shipmentTypeManager = shipmentTypeManager;
            this.licenseService = licenseService;
            this.shippingManager = shippingManager;
            this.shippingSettings = shippingSettings;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Get a label for a shipment
        /// </summary>
        public GetLabelResult GetLabel(PrepareShipmentResult result)
        {
            if (!result.Success)
            {
                return new GetLabelResult(result);
            }

            ShippingException lastException = null;

            foreach (ShipmentEntity shipment in result.Shipments)
            {
                try
                {
                    // We have to test this here because at this point the shipment has been converted to its
                    // real time if it was originally best rate
                    EnsureShipmentTypesAreAllowed(shipment.ShipmentTypeCode, licenseService);
                    IDownloadedLabelData labelData = null;

                    // Get the ShipmentType instance
                    ShipmentType shipmentType = shipmentTypeManager.Get(shipment);

                    // A null value returned from the preprocess method means the user has opted to not continue
                    // processing after a counter rate was selected as the best rate, so the processing of the shipment should be aborted
                    if (shipmentType == null)
                    {
                        return new GetLabelResult(result, true);
                    }

                    // Ensure the carrier specific data has been loaded
                    log.InfoFormat("Shipment {0}  - Ensuring loaded", shipment.ShipmentID);
                    shippingManager.EnsureShipmentLoaded(shipment);

                    // Update the dynamic data of the shipment
                    shipmentType.UpdateDynamicShipmentData(shipment);

                    // Apply the blank recipient phone# option.  We apply it right to the entity so that
                    // its transparent to all the shipping carrier processing.  But we reset it back
                    // after processing, so it doesn't look like that's the phone the customer entered for the shipment.
                    if (shipment.ShipPhone.Trim().Length == 0)
                    {
                        IShippingSettingsEntity settings = shippingSettings.FetchReadOnly();
                        if (settings.BlankPhoneOption == (int) ShipmentBlankPhoneOption.SpecifiedPhone)
                        {
                            shipment.ShipPhone = settings.BlankPhoneNumber;
                        }
                        else
                        {
                            shipment.ShipPhone = shipment.OriginPhone;
                        }

                        log.InfoFormat("Shipment {1} - Using phone '{0}' for  in place of blank phone.",
                            shipment.ShipPhone, shipment.ShipmentID);
                    }

                    // Determine residential status
                    if (shipmentType.IsResidentialStatusRequired(shipment))
                    {
                        shipment.ResidentialResult = residentialDeterminationService.IsResidentialAddress(shipment);
                    }

                    insuranceUtility.ValidateShipment(shipment);

                    // Check against the postal restriction for APO/FPO only
                    if (licenseService.CheckRestriction(EditionFeature.PostalApoFpoPoboxOnly, shipment) != EditionRestrictionLevel.None)
                    {
                        throw new ShippingException(
                            "Your ShipWorks account is only enabled for using APO, FPO, and P.O. " +
                            "Box postal services.  Please contact Interapptive to enable use of all postal services.");
                    }

                    if (licenseService.CheckRestriction(EditionFeature.ShipmentType, shipmentType.ShipmentTypeCode) != EditionRestrictionLevel.None)
                    {
                        throw new ShippingException(
                            $"Your edition of ShipWorks does not support shipping with '{shipmentType.ShipmentTypeName}'.");
                    }

                    // If they had set this shipment to be a return - we want to make sure it's not processed as one if they switched to something that doesn't support it
                    if (!shipmentType.SupportsReturns)
                    {
                        shipment.ReturnShipment = false;
                    }

                    // We're going to allow the store to confirm the shipping address for the shipping label, but we want to
                    // make a note of the original shipping address first, so we can reset the address back after the label
                    // has been generated. This will result in the customer still being able to see where the package went
                    // according to the original cart order
                    ShipmentEntity clone = EntityUtility.CloneEntity(shipment);

                    // Instantiate the store class to allow it a chance to confirm the shipping address before
                    // the shipping label is created. We don't use the method on the ShippingManager to do this
                    // since we want to track the fields that changed.
                    StoreType storeType = storeTypeManager.GetType(result.Store);
                    List<ShipmentFieldIndex> fieldsToRestore = storeType.OverrideShipmentDetails(shipment);

                    log.InfoFormat("Shipment {0}  - ShipmentType.Process Start", shipment.ShipmentID);

                    ILabelService labelService = labelServiceFactory.Create(shipment.ShipmentTypeCode);
                    Debug.Assert(Transaction.Current == null, "No transaction should exist at this point.");

                    labelData = labelService.Create(shipment);

                    return new GetLabelResult(result, labelData, shipment, clone, fieldsToRestore);
                }
                catch (ShippingException ex)
                {
                    lastException = ex;
                }
                catch (Exception ex) when (
                        ex is InsureShipException ||
                        ex is ShipWorksLicenseException ||
                        ex is TangoException ||
                        ex is TemplateTokenException)
                {
                    lastException = new ShippingException(ex.Message, ex);
                }
            }

            return new GetLabelResult(result, lastException);
        }

        /// <summary>
        /// Ensure that the shipment type has not been restricted
        /// </summary>
        private static void EnsureShipmentTypesAreAllowed(ShipmentTypeCode shipmentType, ILicenseService licenseService)
        {
            EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.ProcessShipment, shipmentType);

            if (restrictionLevel == EditionRestrictionLevel.Forbidden)
            {
                throw new ShippingException($"ShipWorks can no longer process {EnumHelper.GetDescription(shipmentType)} shipments. Please try using USPS.");
            }
        }
    }
}
