///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
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
    /// Read-only representation of the entity 'Shipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShipmentEntity : IShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShipmentEntity(IShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            RowVersion = source.RowVersion;
            OrderID = source.OrderID;
            ShipmentType = source.ShipmentType;
            ContentWeight = source.ContentWeight;
            TotalWeight = source.TotalWeight;
            Processed = source.Processed;
            ProcessedDate = source.ProcessedDate;
            ProcessedUserID = source.ProcessedUserID;
            ProcessedComputerID = source.ProcessedComputerID;
            ShipDate = source.ShipDate;
            ShipmentCost = source.ShipmentCost;
            Voided = source.Voided;
            VoidedDate = source.VoidedDate;
            VoidedUserID = source.VoidedUserID;
            VoidedComputerID = source.VoidedComputerID;
            TrackingNumber = source.TrackingNumber;
            CustomsGenerated = source.CustomsGenerated;
            CustomsValue = source.CustomsValue;
            RequestedLabelFormat = source.RequestedLabelFormat;
            ActualLabelFormat = source.ActualLabelFormat;
            ShipFirstName = source.ShipFirstName;
            ShipMiddleName = source.ShipMiddleName;
            ShipLastName = source.ShipLastName;
            ShipCompany = source.ShipCompany;
            ShipStreet1 = source.ShipStreet1;
            ShipStreet2 = source.ShipStreet2;
            ShipStreet3 = source.ShipStreet3;
            ShipCity = source.ShipCity;
            ShipStateProvCode = source.ShipStateProvCode;
            ShipPostalCode = source.ShipPostalCode;
            ShipCountryCode = source.ShipCountryCode;
            ShipPhone = source.ShipPhone;
            ShipEmail = source.ShipEmail;
            ShipAddressValidationSuggestionCount = source.ShipAddressValidationSuggestionCount;
            ShipAddressValidationStatus = source.ShipAddressValidationStatus;
            ShipAddressValidationError = source.ShipAddressValidationError;
            ShipResidentialStatus = source.ShipResidentialStatus;
            ShipPOBox = source.ShipPOBox;
            ShipUSTerritory = source.ShipUSTerritory;
            ShipMilitaryAddress = source.ShipMilitaryAddress;
            ResidentialDetermination = source.ResidentialDetermination;
            ResidentialResult = source.ResidentialResult;
            OriginOriginID = source.OriginOriginID;
            OriginFirstName = source.OriginFirstName;
            OriginMiddleName = source.OriginMiddleName;
            OriginLastName = source.OriginLastName;
            OriginCompany = source.OriginCompany;
            OriginStreet1 = source.OriginStreet1;
            OriginStreet2 = source.OriginStreet2;
            OriginStreet3 = source.OriginStreet3;
            OriginCity = source.OriginCity;
            OriginStateProvCode = source.OriginStateProvCode;
            OriginPostalCode = source.OriginPostalCode;
            OriginCountryCode = source.OriginCountryCode;
            OriginPhone = source.OriginPhone;
            OriginFax = source.OriginFax;
            OriginEmail = source.OriginEmail;
            OriginWebsite = source.OriginWebsite;
            ReturnShipment = source.ReturnShipment;
            Insurance = source.Insurance;
            InsuranceProvider = source.InsuranceProvider;
            ShipNameParseStatus = source.ShipNameParseStatus;
            ShipUnparsedName = source.ShipUnparsedName;
            OriginNameParseStatus = source.OriginNameParseStatus;
            OriginUnparsedName = source.OriginUnparsedName;
            BestRateEvents = source.BestRateEvents;
            ShipSenseStatus = source.ShipSenseStatus;
            ShipSenseChangeSets = source.ShipSenseChangeSets;
            ShipSenseEntry = source.ShipSenseEntry;
            OnlineShipmentID = source.OnlineShipmentID;
            BilledType = source.BilledType;
            BilledWeight = source.BilledWeight;
            
            Amazon = (IAmazonShipmentEntity) source.Amazon?.AsReadOnly(objectMap);
            BestRate = (IBestRateShipmentEntity) source.BestRate?.AsReadOnly(objectMap);
            DhlExpress = (IDhlExpressShipmentEntity) source.DhlExpress?.AsReadOnly(objectMap);
            FedEx = (IFedExShipmentEntity) source.FedEx?.AsReadOnly(objectMap);
            InsurancePolicy = (IInsurancePolicyEntity) source.InsurancePolicy?.AsReadOnly(objectMap);
            IParcel = (IIParcelShipmentEntity) source.IParcel?.AsReadOnly(objectMap);
            OnTrac = (IOnTracShipmentEntity) source.OnTrac?.AsReadOnly(objectMap);
            Other = (IOtherShipmentEntity) source.Other?.AsReadOnly(objectMap);
            Postal = (IPostalShipmentEntity) source.Postal?.AsReadOnly(objectMap);
            Ups = (IUpsShipmentEntity) source.Ups?.AsReadOnly(objectMap);
            
            Order = (IOrderEntity) source.Order?.AsReadOnly(objectMap);
            
            CustomsItems = source.CustomsItems?.Select(x => x.AsReadOnly(objectMap)).OfType<IShipmentCustomsItemEntity>().ToReadOnly() ??
                Enumerable.Empty<IShipmentCustomsItemEntity>();
            ReturnItems = source.ReturnItems?.Select(x => x.AsReadOnly(objectMap)).OfType<IShipmentReturnItemEntity>().ToReadOnly() ??
                Enumerable.Empty<IShipmentReturnItemEntity>();
            ValidatedAddress = source.ValidatedAddress?.Select(x => x.AsReadOnly(objectMap)).OfType<IValidatedAddressEntity>().ToReadOnly() ??
                Enumerable.Empty<IValidatedAddressEntity>();

            CopyCustomShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The RowVersion property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The OrderID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The ShipmentType property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipmentType { get; }
        /// <summary> The ContentWeight property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ContentWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double ContentWeight { get; }
        /// <summary> The TotalWeight property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."TotalWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double TotalWeight { get; }
        /// <summary> The Processed property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."Processed"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Processed { get; }
        /// <summary> The ProcessedDate property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ProcessedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> ProcessedDate { get; }
        /// <summary> The ProcessedUserID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ProcessedUserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ProcessedUserID { get; }
        /// <summary> The ProcessedComputerID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ProcessedComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ProcessedComputerID { get; }
        /// <summary> The ShipDate property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime ShipDate { get; }
        /// <summary> The ShipmentCost property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipmentCost"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal ShipmentCost { get; }
        /// <summary> The Voided property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."Voided"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Voided { get; }
        /// <summary> The VoidedDate property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."VoidedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> VoidedDate { get; }
        /// <summary> The VoidedUserID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."VoidedUserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> VoidedUserID { get; }
        /// <summary> The VoidedComputerID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."VoidedComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> VoidedComputerID { get; }
        /// <summary> The TrackingNumber property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."TrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TrackingNumber { get; }
        /// <summary> The CustomsGenerated property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."CustomsGenerated"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CustomsGenerated { get; }
        /// <summary> The CustomsValue property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."CustomsValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal CustomsValue { get; }
        /// <summary> The RequestedLabelFormat property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        /// <summary> The ActualLabelFormat property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ActualLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ActualLabelFormat { get; }
        /// <summary> The ShipFirstName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipFirstName { get; }
        /// <summary> The ShipMiddleName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipMiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipMiddleName { get; }
        /// <summary> The ShipLastName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipLastName { get; }
        /// <summary> The ShipCompany property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipCompany { get; }
        /// <summary> The ShipStreet1 property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipStreet1 { get; }
        /// <summary> The ShipStreet2 property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipStreet2 { get; }
        /// <summary> The ShipStreet3 property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipStreet3 { get; }
        /// <summary> The ShipCity property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipCity { get; }
        /// <summary> The ShipStateProvCode property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipStateProvCode { get; }
        /// <summary> The ShipPostalCode property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipPostalCode { get; }
        /// <summary> The ShipCountryCode property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipCountryCode { get; }
        /// <summary> The ShipPhone property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipPhone { get; }
        /// <summary> The ShipEmail property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipEmail { get; }
        /// <summary> The ShipAddressValidationSuggestionCount property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipAddressValidationSuggestionCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipAddressValidationSuggestionCount { get; }
        /// <summary> The ShipAddressValidationStatus property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipAddressValidationStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipAddressValidationStatus { get; }
        /// <summary> The ShipAddressValidationError property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipAddressValidationError"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipAddressValidationError { get; }
        /// <summary> The ShipResidentialStatus property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipResidentialStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipResidentialStatus { get; }
        /// <summary> The ShipPOBox property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipPOBox"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipPOBox { get; }
        /// <summary> The ShipUSTerritory property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipUSTerritory"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipUSTerritory { get; }
        /// <summary> The ShipMilitaryAddress property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipMilitaryAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipMilitaryAddress { get; }
        /// <summary> The ResidentialDetermination property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ResidentialDetermination"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ResidentialDetermination { get; }
        /// <summary> The ResidentialResult property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ResidentialResult"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ResidentialResult { get; }
        /// <summary> The OriginOriginID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginOriginID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginOriginID { get; }
        /// <summary> The OriginFirstName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginFirstName { get; }
        /// <summary> The OriginMiddleName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginMiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginMiddleName { get; }
        /// <summary> The OriginLastName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginLastName { get; }
        /// <summary> The OriginCompany property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginCompany { get; }
        /// <summary> The OriginStreet1 property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginStreet1 { get; }
        /// <summary> The OriginStreet2 property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginStreet2 { get; }
        /// <summary> The OriginStreet3 property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginStreet3 { get; }
        /// <summary> The OriginCity property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginCity { get; }
        /// <summary> The OriginStateProvCode property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginStateProvCode { get; }
        /// <summary> The OriginPostalCode property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginPostalCode { get; }
        /// <summary> The OriginCountryCode property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginCountryCode { get; }
        /// <summary> The OriginPhone property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginPhone { get; }
        /// <summary> The OriginFax property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginFax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginFax { get; }
        /// <summary> The OriginEmail property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginEmail { get; }
        /// <summary> The OriginWebsite property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginWebsite"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginWebsite { get; }
        /// <summary> The ReturnShipment property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ReturnShipment"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ReturnShipment { get; }
        /// <summary> The Insurance property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Insurance { get; }
        /// <summary> The InsuranceProvider property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."InsuranceProvider"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 InsuranceProvider { get; }
        /// <summary> The ShipNameParseStatus property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipNameParseStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipNameParseStatus { get; }
        /// <summary> The ShipUnparsedName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipUnparsedName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipUnparsedName { get; }
        /// <summary> The OriginNameParseStatus property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginNameParseStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OriginNameParseStatus { get; }
        /// <summary> The OriginUnparsedName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginUnparsedName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OriginUnparsedName { get; }
        /// <summary> The BestRateEvents property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."BestRateEvents"<br/>
        /// Table field type characteristics (type, precision, scale, length): TinyInt, 3, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte BestRateEvents { get; }
        /// <summary> The ShipSenseStatus property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipSenseStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipSenseStatus { get; }
        /// <summary> The ShipSenseChangeSets property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipSenseChangeSets"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipSenseChangeSets { get; }
        /// <summary> The ShipSenseEntry property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipSenseEntry"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] ShipSenseEntry { get; }
        /// <summary> The OnlineShipmentID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OnlineShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OnlineShipmentID { get; }
        /// <summary> The BilledType property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."BilledType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 BilledType { get; }
        /// <summary> The BilledWeight property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."BilledWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double BilledWeight { get; }
        
        public IAmazonShipmentEntity Amazon { get; }
        
        public IBestRateShipmentEntity BestRate { get; }
        
        public IDhlExpressShipmentEntity DhlExpress { get; }
        
        public IFedExShipmentEntity FedEx { get; }
        
        public IInsurancePolicyEntity InsurancePolicy { get; }
        
        public IIParcelShipmentEntity IParcel { get; }
        
        public IOnTracShipmentEntity OnTrac { get; }
        
        public IOtherShipmentEntity Other { get; }
        
        public IPostalShipmentEntity Postal { get; }
        
        public IUpsShipmentEntity Ups { get; }
        
        
        public IOrderEntity Order { get; }
        
        
        public IEnumerable<IShipmentCustomsItemEntity> CustomsItems { get; }
        
        public IEnumerable<IShipmentReturnItemEntity> ReturnItems { get; }
        
        public IEnumerable<IValidatedAddressEntity> ValidatedAddress { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShipmentData(IShipmentEntity source);
    }
}
