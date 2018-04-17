using System;
using System.Globalization;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.ShipmentRequest;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Carriers.OnTrac.Net.Shipment
{
    /// <summary>
    /// Converts ShipWorks ShipmentEntity to an OnTrac DTO
    /// </summary>
    public static class OnTracDtoAdapter
    {
        /// <summary>
        /// Create a shipment object that conforms to the OnTrac XSD
        /// </summary>
        public static Schemas.ShipmentRequest.OnTracShipmentRequest CreateShipmentRequest(
            ShipmentEntity shipworksShipment,
            int onTracAccountNumber)
        {
            if (!string.IsNullOrWhiteSpace(shipworksShipment.OriginStreet2) ||
                !string.IsNullOrWhiteSpace(shipworksShipment.OriginStreet3))
            {
                throw new OnTracException("From Street Address must be one line only.");
            }

            var origin = new PersonAdapter(shipworksShipment, "Origin");
            var recipient = new PersonAdapter(shipworksShipment, "Ship");

            OnTracShipmentEntity onTracShipment = shipworksShipment.OnTrac;

            string ref1 = string.IsNullOrEmpty(onTracShipment.Reference1) ? string.Empty :
                TemplateTokenProcessor.ProcessTokens(onTracShipment.Reference1, shipworksShipment.ShipmentID);

            string ref2 = string.IsNullOrEmpty(onTracShipment.Reference2) ? string.Empty :
                TemplateTokenProcessor.ProcessTokens(onTracShipment.Reference2, shipworksShipment.ShipmentID);

            string instructions = string.IsNullOrEmpty(onTracShipment.Instructions) ? string.Empty :
                TemplateTokenProcessor.ProcessTokens(onTracShipment.Instructions, shipworksShipment.ShipmentID);

            bool isLetter = shipworksShipment.OnTrac.PackagingType == (int) OnTracPackagingType.Letter;

            //Create request object
            var shipmentRequestList = new Schemas.ShipmentRequest.OnTracShipmentRequest
            {
                Shipments = new[]
                {
                    new Schemas.ShipmentRequest.Shipment
                    {
                        UID = string.Empty,
                        shipper = new shipper
                        {
                            Name = string.IsNullOrEmpty(origin.Company) ? origin.UnparsedName : origin.Company,
                            Addr1 = origin.Street1.Truncate(43),
                            City = origin.City.Truncate(20),
                            State = origin.StateProvCode,
                            Zip = PersonUtility.GetZip5(origin.PostalCode),
                            Contact = origin.UnparsedName.Truncate(20),
                            Phone = PersonUtility.GetPhoneDigits10(origin.Phone)
                        },
                        consignee = new consignee
                        {
                            Name = string.IsNullOrEmpty(recipient.Company) ? recipient.UnparsedName : recipient.Company,
                            Addr1 = recipient.Street1.Truncate(60),
                            Addr2 = recipient.Street2.Truncate(60),
                            Addr3 = recipient.Street3.Truncate(60),
                            City = recipient.City.Truncate(20),
                            State = recipient.StateProvCode,
                            Zip = PersonUtility.GetZip5(recipient.PostalCode),
                            Contact = recipient.UnparsedName.Truncate(20),
                            Phone = PersonUtility.GetPhoneDigits10(recipient.Phone)
                        },
                        Service = GetOnTracServiceType((OnTracServiceType) onTracShipment.Service),
                        SignatureRequired = onTracShipment.SignatureRequired,
                        Residential = shipworksShipment.ResidentialResult,
                        SaturdayDel = onTracShipment.SaturdayDelivery,
                        Declared = onTracShipment.DeclaredValue,
                        COD = onTracShipment.CodAmount,
                        CODType = EnumHelper.GetApiValue((OnTracCodType) onTracShipment.CodType),
                        Weight =  (isLetter ? 0 : shipworksShipment.TotalWeight).ToString(CultureInfo.InvariantCulture),
                        BillTo = onTracAccountNumber.ToString(),
                        Instructions = instructions.Truncate(90),
                        Reference = ref1.Truncate(50),
                        Reference2 = ref2.Truncate(50),
                        Reference3 = string.Empty,
                        Tracking = string.Empty,
                        DIM = new DIM
                        {
                            Length = (isLetter ? 0 : onTracShipment.DimsLength).ToString(CultureInfo.InvariantCulture),
                            Width = (isLetter ? 0 : onTracShipment.DimsWidth).ToString(CultureInfo.InvariantCulture),
                            Height = (isLetter ? 0 : onTracShipment.DimsHeight).ToString(CultureInfo.InvariantCulture)
                        },
                        LabelType = GetOnTracLabelType(shipworksShipment.ActualLabelFormat).ToString(),
                        ShipEmail = "",
                        DelEmail = "",
                        ShipDate = shipworksShipment.ShipDate
                    }
                }
            };
            return shipmentRequestList;
        }

        /// <summary>
        /// Given a shipworks service type, return an OnTrac service type defined by OnTrac XSD
        /// </summary>
        private static string GetOnTracServiceType(OnTracServiceType onTracServiceType)
        {
            return EnumHelper.GetApiValue(onTracServiceType);
        }

        /// <summary>
        /// If not Thermal, return pdf api int enumerator. If is thermal, return requested int api thermal type enumerator.
        /// </summary>
        private static int GetOnTracLabelType(int? thermalType)
        {
            if (!thermalType.HasValue)
            {
                // PDF
                return 14;
            }

            switch ((ThermalLanguage) thermalType)
            {
                case ThermalLanguage.EPL:
                    //4x6 epl label
                    return 11; 
                case ThermalLanguage.ZPL:
                    //4x6 zpl label
                    return 9; 
                default:
                    throw new ArgumentOutOfRangeException("thermalType");
            }
        }
    }
}