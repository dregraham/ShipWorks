using System;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Shipment;
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
        public static ShipmentRequestList CreateShipmentRequestList(
            ShipmentEntity shipworksShipment,
            int onTracAccountNumber)
        {
            if (!string.IsNullOrWhiteSpace(shipworksShipment.OriginStreet2)
                || !string.IsNullOrWhiteSpace(shipworksShipment.OriginStreet3))
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

            bool isLetter = (shipworksShipment.OnTrac.PackagingType == (int) OnTracPackagingType.Letter);

            //Create request object
            var shipmentRequestList = new ShipmentRequestList
            {
                Shipments = new[]
                {
                    new ShipmentRequest
                    {
                        UID = string.Empty,
                        shipper = new Shipper
                        {
                            Name = String.IsNullOrEmpty(origin.Company) ? origin.UnparsedName : origin.Company,
                            Addr1 = StringUtility.Truncate(origin.Street1, 43),
                            City = StringUtility.Truncate(origin.City, 20),
                            State = origin.StateProvCode,
                            Zip = PersonUtility.GetZip5(origin.PostalCode),
                            Contact = StringUtility.Truncate(origin.UnparsedName, 20),
                            Phone = PersonUtility.GetPhoneDigits10(origin.Phone)
                        },
                        consignee = new Consignee
                        {
                            Name = String.IsNullOrEmpty(recipient.Company) ? recipient.UnparsedName : recipient.Company,
                            Addr1 = StringUtility.Truncate(recipient.Street1, 60),
                            Addr2 = StringUtility.Truncate(recipient.Street2, 60),
                            Addr3 = StringUtility.Truncate(recipient.Street3, 60),
                            City = StringUtility.Truncate(recipient.City, 20),
                            State = recipient.StateProvCode,
                            Zip = PersonUtility.GetZip5(recipient.PostalCode),
                            Contact = StringUtility.Truncate(recipient.UnparsedName, 20),
                            Phone = PersonUtility.GetPhoneDigits10(recipient.Phone)
                        },
                        Service = GetOnTracServiceType((OnTracServiceType) onTracShipment.Service),
                        SignatureRequired = onTracShipment.SignatureRequired,
                        Residential = shipworksShipment.ResidentialResult,
                        SaturdayDel = onTracShipment.SaturdayDelivery,
                        Declared = (double) onTracShipment.DeclaredValue,
                        COD = (double) onTracShipment.CodAmount,
                        CODType = GetOnTracCodType((OnTracCodType) onTracShipment.CodType),
                        Weight =  isLetter ? 0 : shipworksShipment.TotalWeight,
                        BillTo = onTracAccountNumber,
                        Instructions = StringUtility.Truncate(instructions, 90),
                        Reference = StringUtility.Truncate(ref1, 50),
                        Reference2 = StringUtility.Truncate(ref2, 50),
                        Reference3 = string.Empty,
                        Tracking = string.Empty,
                        DIM = new Dim
                        {
                            Length = isLetter ? 0 : onTracShipment.DimsLength,
                            Width = isLetter ? 0 : onTracShipment.DimsWidth,
                            Height = isLetter ? 0 : onTracShipment.DimsHeight
                        },
                        LabelType = GetOnTracLabelType(shipworksShipment.ActualLabelFormat),
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
        /// Given a shipworks COD type, return an OnTrac COD type defined by OnTrac XSD
        /// </summary>
        /// <param name="onTracCodType"> ShipWorks OnTrac COD type </param>
        private static codType GetOnTracCodType(OnTracCodType onTracCodType)
        {
            string apiValue = EnumHelper.GetApiValue(onTracCodType);

            return (codType) Enum.Parse(typeof(codType), apiValue);
        }

        /// <summary>
        /// If not Thermal, return gif api int enumerator. If is thermal, return requested int api thermal type enumerator.
        /// </summary>
        private static int GetOnTracLabelType(int? thermalType)
        {
            if (!thermalType.HasValue)
            {
                return 4; // GIF
            }

            switch ((ThermalLanguage) thermalType)
            {
                case ThermalLanguage.EPL:
                    return 6; //4x5 epl label
                case ThermalLanguage.ZPL:
                    return 7; //4x5 zpl label
                default:
                    throw new ArgumentOutOfRangeException("thermalType");
            }
        }
    }
}