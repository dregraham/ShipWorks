﻿using System;
using System.Threading.Tasks;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Wrapper class for InsuranceUtility
    /// </summary>
    public class InsuranceUtilityWrapper : IInsuranceUtility
    {
        private IShippingSettings shippingSettings;
        private readonly IAsyncMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceUtilityWrapper(IShippingSettings shippingSettings, IAsyncMessageHelper messageHelper)
        {
            this.shippingSettings = shippingSettings;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Get the cost of insurance for the given shipment given the specified declared value
        /// </summary>
        public InsuranceCost GetInsuranceCost(ShipmentEntity shipment, decimal declaredValue) => InsuranceUtility.GetInsuranceCost(shipment, declaredValue);

        /// <summary>
        /// Show the InsurancePennyOneDlg and save values if necessary
        /// </summary>
        public void ShowInsurancePennyOneDlg(ShipmentTypeCode shipmentTypeCode)
        {
            using (InsurancePennyOneDlg dlg = new InsurancePennyOneDlg(ShippingManager.GetCarrierName(shipmentTypeCode), true))
            {
                dlg.ShowDialog();

                if (dlg.PennyOne)
                {
                    ShippingSettingsEntity settings = shippingSettings.Fetch();

                    if (shipmentTypeCode == ShipmentTypeCode.FedEx)
                    {
                        settings.FedExInsurancePennyOne = true;
                    }
                    else if (shipmentTypeCode == ShipmentTypeCode.UpsOnLineTools ||
                             shipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
                    {
                        settings.UpsInsurancePennyOne = true;
                    }
                    else if (shipmentTypeCode == ShipmentTypeCode.OnTrac)
                    {
                        settings.OnTracInsurancePennyOne = true;
                    }
                    else if (shipmentTypeCode == ShipmentTypeCode.iParcel)
                    {
                        settings.IParcelInsurancePennyOne = true;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("linkSavings.Tag", shipmentTypeCode, @"Invalid ShipmentTypeCode for PennyOne Insurance");
                    }

                    shippingSettings.Save(settings);
                }
            }
        }

        /// <summary>
        /// Show the InsuranceBenefitsDlg and save values if necessary
        /// </summary>
        public void ShowInsuranceBenefitsDlg(ShipmentEntity shipment, InsuranceCost insuranceCost)
        {
            using (InsuranceBenefitsDlg dlg = new InsuranceBenefitsDlg(insuranceCost, shipment.InsuranceProvider != (int) InsuranceProvider.ShipWorks))
            {
                dlg.ShowDialog();

                if (dlg.ShipWorksInsuranceEnabled)
                {
                    ShippingSettingsEntity settings = shippingSettings.Fetch();

                    ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode) shipment.ShipmentType;

                    if (shipmentTypeCode == ShipmentTypeCode.FedEx)
                    {
                        settings.FedExInsuranceProvider = (int) InsuranceProvider.ShipWorks;
                    }
                    else if (shipmentTypeCode == ShipmentTypeCode.UpsOnLineTools ||
                             shipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
                    {
                        settings.UpsInsuranceProvider = (int) InsuranceProvider.ShipWorks;
                    }
                    else if (shipmentTypeCode == ShipmentTypeCode.OnTrac)
                    {
                        settings.OnTracInsuranceProvider = (int) InsuranceProvider.ShipWorks;
                    }
                    else if (shipmentTypeCode == ShipmentTypeCode.iParcel)
                    {
                        settings.IParcelInsuranceProvider = (int) InsuranceProvider.ShipWorks;
                    }
                    else if (shipment.ShipmentType == (int) ShipmentTypeCode.Endicia)
                    {
                        settings.EndiciaInsuranceProvider = (int) InsuranceProvider.ShipWorks;
                    }
                    else if (shipment.ShipmentType == (int) ShipmentTypeCode.DhlEcommerce)
                    {
                        settings.DhlEcommerceInsuranceProvider = (int) InsuranceProvider.ShipWorks;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid ShipmentType unhandled in savings link: " + shipment.ShipmentType);
                    }

                    shippingSettings.Save(settings);
                }
            }
        }

        /// <summary>
        /// Validate the given shipment
        /// </summary>
        public Task ValidateShipment(ShipmentEntity shipment) => InsuranceUtility.ValidateShipment(shipment, messageHelper);

        /// <summary>
        /// Get the default insurance value to use based on the shipment contents
        /// </summary>
        public decimal GetInsuranceValue(ShipmentEntity shipment) => InsuranceUtility.GetInsuranceValue(shipment);

        /// <summary>
        /// Determines how much the shipment was insured for.
        /// </summary>
        public decimal GetInsuredValue(ShipmentEntity shipment) => InsuranceUtility.GetInsuredValue(shipment);
    }
}
