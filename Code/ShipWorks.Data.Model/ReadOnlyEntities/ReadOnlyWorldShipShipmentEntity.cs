///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'WorldShipShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyWorldShipShipmentEntity : IWorldShipShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyWorldShipShipmentEntity(IWorldShipShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            OrderNumber = source.OrderNumber;
            FromCompanyOrName = source.FromCompanyOrName;
            FromAttention = source.FromAttention;
            FromAddress1 = source.FromAddress1;
            FromAddress2 = source.FromAddress2;
            FromAddress3 = source.FromAddress3;
            FromCountryCode = source.FromCountryCode;
            FromPostalCode = source.FromPostalCode;
            FromCity = source.FromCity;
            FromStateProvCode = source.FromStateProvCode;
            FromTelephone = source.FromTelephone;
            FromEmail = source.FromEmail;
            FromAccountNumber = source.FromAccountNumber;
            ToCustomerID = source.ToCustomerID;
            ToCompanyOrName = source.ToCompanyOrName;
            ToAttention = source.ToAttention;
            ToAddress1 = source.ToAddress1;
            ToAddress2 = source.ToAddress2;
            ToAddress3 = source.ToAddress3;
            ToCountryCode = source.ToCountryCode;
            ToPostalCode = source.ToPostalCode;
            ToCity = source.ToCity;
            ToStateProvCode = source.ToStateProvCode;
            ToTelephone = source.ToTelephone;
            ToEmail = source.ToEmail;
            ToAccountNumber = source.ToAccountNumber;
            ToResidential = source.ToResidential;
            ServiceType = source.ServiceType;
            BillTransportationTo = source.BillTransportationTo;
            SaturdayDelivery = source.SaturdayDelivery;
            QvnOption = source.QvnOption;
            QvnFrom = source.QvnFrom;
            QvnSubjectLine = source.QvnSubjectLine;
            QvnMemo = source.QvnMemo;
            Qvn1ShipNotify = source.Qvn1ShipNotify;
            Qvn1DeliveryNotify = source.Qvn1DeliveryNotify;
            Qvn1ExceptionNotify = source.Qvn1ExceptionNotify;
            Qvn1ContactName = source.Qvn1ContactName;
            Qvn1Email = source.Qvn1Email;
            Qvn2ShipNotify = source.Qvn2ShipNotify;
            Qvn2DeliveryNotify = source.Qvn2DeliveryNotify;
            Qvn2ExceptionNotify = source.Qvn2ExceptionNotify;
            Qvn2ContactName = source.Qvn2ContactName;
            Qvn2Email = source.Qvn2Email;
            Qvn3ShipNotify = source.Qvn3ShipNotify;
            Qvn3DeliveryNotify = source.Qvn3DeliveryNotify;
            Qvn3ExceptionNotify = source.Qvn3ExceptionNotify;
            Qvn3ContactName = source.Qvn3ContactName;
            Qvn3Email = source.Qvn3Email;
            CustomsDescriptionOfGoods = source.CustomsDescriptionOfGoods;
            CustomsDocumentsOnly = source.CustomsDocumentsOnly;
            ShipperNumber = source.ShipperNumber;
            PackageCount = source.PackageCount;
            DeliveryConfirmation = source.DeliveryConfirmation;
            DeliveryConfirmationAdult = source.DeliveryConfirmationAdult;
            InvoiceTermsOfSale = source.InvoiceTermsOfSale;
            InvoiceReasonForExport = source.InvoiceReasonForExport;
            InvoiceComments = source.InvoiceComments;
            InvoiceCurrencyCode = source.InvoiceCurrencyCode;
            InvoiceChargesFreight = source.InvoiceChargesFreight;
            InvoiceChargesInsurance = source.InvoiceChargesInsurance;
            InvoiceChargesOther = source.InvoiceChargesOther;
            ShipmentProcessedOnComputerID = source.ShipmentProcessedOnComputerID;
            UspsEndorsement = source.UspsEndorsement;
            CarbonNeutral = source.CarbonNeutral;
            
            
            
            Goods = source.Goods?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IWorldShipGoodsEntity>();
            Packages = source.Packages?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IWorldShipPackageEntity>();
            WorldShipProcessed = source.WorldShipProcessed?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IWorldShipProcessedEntity>();

            CopyCustomWorldShipShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The OrderNumber property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OrderNumber { get; }
        /// <summary> The FromCompanyOrName property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromCompanyOrName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromCompanyOrName { get; }
        /// <summary> The FromAttention property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromAttention"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromAttention { get; }
        /// <summary> The FromAddress1 property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromAddress1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromAddress1 { get; }
        /// <summary> The FromAddress2 property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromAddress2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromAddress2 { get; }
        /// <summary> The FromAddress3 property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromAddress3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromAddress3 { get; }
        /// <summary> The FromCountryCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromCountryCode { get; }
        /// <summary> The FromPostalCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromPostalCode { get; }
        /// <summary> The FromCity property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromCity { get; }
        /// <summary> The FromStateProvCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromStateProvCode { get; }
        /// <summary> The FromTelephone property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromTelephone"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromTelephone { get; }
        /// <summary> The FromEmail property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromEmail { get; }
        /// <summary> The FromAccountNumber property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."FromAccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromAccountNumber { get; }
        /// <summary> The ToCustomerID property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToCustomerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToCustomerID { get; }
        /// <summary> The ToCompanyOrName property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToCompanyOrName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToCompanyOrName { get; }
        /// <summary> The ToAttention property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToAttention"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToAttention { get; }
        /// <summary> The ToAddress1 property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToAddress1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToAddress1 { get; }
        /// <summary> The ToAddress2 property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToAddress2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToAddress2 { get; }
        /// <summary> The ToAddress3 property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToAddress3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToAddress3 { get; }
        /// <summary> The ToCountryCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToCountryCode { get; }
        /// <summary> The ToPostalCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToPostalCode { get; }
        /// <summary> The ToCity property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToCity { get; }
        /// <summary> The ToStateProvCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToStateProvCode { get; }
        /// <summary> The ToTelephone property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToTelephone"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToTelephone { get; }
        /// <summary> The ToEmail property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToEmail { get; }
        /// <summary> The ToAccountNumber property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToAccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToAccountNumber { get; }
        /// <summary> The ToResidential property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ToResidential"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToResidential { get; }
        /// <summary> The ServiceType property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ServiceType"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ServiceType { get; }
        /// <summary> The BillTransportationTo property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."BillTransportationTo"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillTransportationTo { get; }
        /// <summary> The SaturdayDelivery property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SaturdayDelivery { get; }
        /// <summary> The QvnOption property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."QvnOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String QvnOption { get; }
        /// <summary> The QvnFrom property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."QvnFrom"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String QvnFrom { get; }
        /// <summary> The QvnSubjectLine property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."QvnSubjectLine"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 18<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String QvnSubjectLine { get; }
        /// <summary> The QvnMemo property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."QvnMemo"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String QvnMemo { get; }
        /// <summary> The Qvn1ShipNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn1ShipNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn1ShipNotify { get; }
        /// <summary> The Qvn1DeliveryNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn1DeliveryNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn1DeliveryNotify { get; }
        /// <summary> The Qvn1ExceptionNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn1ExceptionNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn1ExceptionNotify { get; }
        /// <summary> The Qvn1ContactName property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn1ContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn1ContactName { get; }
        /// <summary> The Qvn1Email property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn1Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn1Email { get; }
        /// <summary> The Qvn2ShipNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn2ShipNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn2ShipNotify { get; }
        /// <summary> The Qvn2DeliveryNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn2DeliveryNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn2DeliveryNotify { get; }
        /// <summary> The Qvn2ExceptionNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn2ExceptionNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn2ExceptionNotify { get; }
        /// <summary> The Qvn2ContactName property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn2ContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn2ContactName { get; }
        /// <summary> The Qvn2Email property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn2Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn2Email { get; }
        /// <summary> The Qvn3ShipNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn3ShipNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn3ShipNotify { get; }
        /// <summary> The Qvn3DeliveryNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn3DeliveryNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn3DeliveryNotify { get; }
        /// <summary> The Qvn3ExceptionNotify property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn3ExceptionNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn3ExceptionNotify { get; }
        /// <summary> The Qvn3ContactName property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn3ContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn3ContactName { get; }
        /// <summary> The Qvn3Email property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."Qvn3Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Qvn3Email { get; }
        /// <summary> The CustomsDescriptionOfGoods property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."CustomsDescriptionOfGoods"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CustomsDescriptionOfGoods { get; }
        /// <summary> The CustomsDocumentsOnly property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."CustomsDocumentsOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CustomsDocumentsOnly { get; }
        /// <summary> The ShipperNumber property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ShipperNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipperNumber { get; }
        /// <summary> The PackageCount property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."PackageCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PackageCount { get; }
        /// <summary> The DeliveryConfirmation property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."DeliveryConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DeliveryConfirmation { get; }
        /// <summary> The DeliveryConfirmationAdult property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."DeliveryConfirmationAdult"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DeliveryConfirmationAdult { get; }
        /// <summary> The InvoiceTermsOfSale property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceTermsOfSale"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String InvoiceTermsOfSale { get; }
        /// <summary> The InvoiceReasonForExport property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceReasonForExport"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String InvoiceReasonForExport { get; }
        /// <summary> The InvoiceComments property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceComments"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 200<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String InvoiceComments { get; }
        /// <summary> The InvoiceCurrencyCode property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceCurrencyCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String InvoiceCurrencyCode { get; }
        /// <summary> The InvoiceChargesFreight property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceChargesFreight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> InvoiceChargesFreight { get; }
        /// <summary> The InvoiceChargesInsurance property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceChargesInsurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> InvoiceChargesInsurance { get; }
        /// <summary> The InvoiceChargesOther property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."InvoiceChargesOther"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> InvoiceChargesOther { get; }
        /// <summary> The ShipmentProcessedOnComputerID property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."ShipmentProcessedOnComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ShipmentProcessedOnComputerID { get; }
        /// <summary> The UspsEndorsement property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."UspsEndorsement"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String UspsEndorsement { get; }
        /// <summary> The CarbonNeutral property of the Entity WorldShipShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipShipment"."CarbonNeutral"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CarbonNeutral { get; }
        
        
        
        public IEnumerable<IWorldShipGoodsEntity> Goods { get; }
        
        public IEnumerable<IWorldShipPackageEntity> Packages { get; }
        
        public IEnumerable<IWorldShipProcessedEntity> WorldShipProcessed { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWorldShipShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWorldShipShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomWorldShipShipmentData(IWorldShipShipmentEntity source);
    }
}
