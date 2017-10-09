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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'Shipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The RowVersion property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The OrderID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The ShipmentType property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipmentType { get; }
        /// <summary> The ContentWeight property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ContentWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double ContentWeight { get; }
        /// <summary> The TotalWeight property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."TotalWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double TotalWeight { get; }
        /// <summary> The Processed property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."Processed"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Processed { get; }
        /// <summary> The ProcessedDate property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ProcessedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> ProcessedDate { get; }
        /// <summary> The ProcessedUserID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ProcessedUserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ProcessedUserID { get; }
        /// <summary> The ProcessedComputerID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ProcessedComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ProcessedComputerID { get; }
        /// <summary> The ShipDate property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime ShipDate { get; }
        /// <summary> The ShipmentCost property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipmentCost"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal ShipmentCost { get; }
        /// <summary> The Voided property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."Voided"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Voided { get; }
        /// <summary> The VoidedDate property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."VoidedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> VoidedDate { get; }
        /// <summary> The VoidedUserID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."VoidedUserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> VoidedUserID { get; }
        /// <summary> The VoidedComputerID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."VoidedComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> VoidedComputerID { get; }
        /// <summary> The TrackingNumber property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."TrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TrackingNumber { get; }
        /// <summary> The CustomsGenerated property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."CustomsGenerated"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean CustomsGenerated { get; }
        /// <summary> The CustomsValue property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."CustomsValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal CustomsValue { get; }
        /// <summary> The RequestedLabelFormat property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RequestedLabelFormat { get; }
        /// <summary> The ActualLabelFormat property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ActualLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ActualLabelFormat { get; }
        /// <summary> The ShipFirstName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipFirstName { get; }
        /// <summary> The ShipMiddleName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipMiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipMiddleName { get; }
        /// <summary> The ShipLastName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipLastName { get; }
        /// <summary> The ShipCompany property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipCompany { get; }
        /// <summary> The ShipStreet1 property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipStreet1 { get; }
        /// <summary> The ShipStreet2 property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipStreet2 { get; }
        /// <summary> The ShipStreet3 property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipStreet3 { get; }
        /// <summary> The ShipCity property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipCity { get; }
        /// <summary> The ShipStateProvCode property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipStateProvCode { get; }
        /// <summary> The ShipPostalCode property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipPostalCode { get; }
        /// <summary> The ShipCountryCode property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipCountryCode { get; }
        /// <summary> The ShipPhone property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipPhone { get; }
        /// <summary> The ShipEmail property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipEmail { get; }
        /// <summary> The ShipAddressValidationSuggestionCount property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipAddressValidationSuggestionCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipAddressValidationSuggestionCount { get; }
        /// <summary> The ShipAddressValidationStatus property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipAddressValidationStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipAddressValidationStatus { get; }
        /// <summary> The ShipAddressValidationError property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipAddressValidationError"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipAddressValidationError { get; }
        /// <summary> The ShipResidentialStatus property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipResidentialStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipResidentialStatus { get; }
        /// <summary> The ShipPOBox property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipPOBox"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipPOBox { get; }
        /// <summary> The ShipUSTerritory property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipUSTerritory"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipUSTerritory { get; }
        /// <summary> The ShipMilitaryAddress property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipMilitaryAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipMilitaryAddress { get; }
        /// <summary> The ResidentialDetermination property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ResidentialDetermination"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ResidentialDetermination { get; }
        /// <summary> The ResidentialResult property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ResidentialResult"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ResidentialResult { get; }
        /// <summary> The OriginOriginID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginOriginID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginOriginID { get; }
        /// <summary> The OriginFirstName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginFirstName { get; }
        /// <summary> The OriginMiddleName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginMiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginMiddleName { get; }
        /// <summary> The OriginLastName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginLastName { get; }
        /// <summary> The OriginCompany property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginCompany { get; }
        /// <summary> The OriginStreet1 property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginStreet1 { get; }
        /// <summary> The OriginStreet2 property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginStreet2 { get; }
        /// <summary> The OriginStreet3 property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginStreet3 { get; }
        /// <summary> The OriginCity property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginCity { get; }
        /// <summary> The OriginStateProvCode property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginStateProvCode { get; }
        /// <summary> The OriginPostalCode property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginPostalCode { get; }
        /// <summary> The OriginCountryCode property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginCountryCode { get; }
        /// <summary> The OriginPhone property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginPhone { get; }
        /// <summary> The OriginFax property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginFax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginFax { get; }
        /// <summary> The OriginEmail property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginEmail { get; }
        /// <summary> The OriginWebsite property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginWebsite"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginWebsite { get; }
        /// <summary> The ReturnShipment property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ReturnShipment"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ReturnShipment { get; }
        /// <summary> The Insurance property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Insurance { get; }
        /// <summary> The InsuranceProvider property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."InsuranceProvider"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 InsuranceProvider { get; }
        /// <summary> The ShipNameParseStatus property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipNameParseStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipNameParseStatus { get; }
        /// <summary> The ShipUnparsedName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipUnparsedName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipUnparsedName { get; }
        /// <summary> The OriginNameParseStatus property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginNameParseStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 OriginNameParseStatus { get; }
        /// <summary> The OriginUnparsedName property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OriginUnparsedName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OriginUnparsedName { get; }
        /// <summary> The BestRateEvents property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."BestRateEvents"<br/>
        /// Table field type characteristics (type, precision, scale, length): TinyInt, 3, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte BestRateEvents { get; }
        /// <summary> The ShipSenseStatus property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipSenseStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipSenseStatus { get; }
        /// <summary> The ShipSenseChangeSets property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipSenseChangeSets"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipSenseChangeSets { get; }
        /// <summary> The ShipSenseEntry property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."ShipSenseEntry"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] ShipSenseEntry { get; }
        /// <summary> The OnlineShipmentID property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."OnlineShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OnlineShipmentID { get; }
        /// <summary> The BilledType property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."BilledType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 BilledType { get; }
        /// <summary> The BilledWeight property of the Entity Shipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Shipment"."BilledWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double BilledWeight { get; }
        
        IAmazonShipmentEntity Amazon { get; }
        IBestRateShipmentEntity BestRate { get; }
        IDhlExpressShipmentEntity DhlExpressShipment { get; }
        IFedExShipmentEntity FedEx { get; }
        IInsurancePolicyEntity InsurancePolicy { get; }
        IIParcelShipmentEntity IParcel { get; }
        IOnTracShipmentEntity OnTrac { get; }
        IOtherShipmentEntity Other { get; }
        IPostalShipmentEntity Postal { get; }
        IUpsShipmentEntity Ups { get; }
        
        IOrderEntity Order { get; }
        
        IEnumerable<IShipmentCustomsItemEntity> CustomsItems { get; }
        IEnumerable<IShipmentReturnItemEntity> ReturnItems { get; }
        IEnumerable<IValidatedAddressEntity> ValidatedAddress { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Shipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShipmentEntity : IShipmentEntity
    {
        IAmazonShipmentEntity IShipmentEntity.Amazon => Amazon;
        IBestRateShipmentEntity IShipmentEntity.BestRate => BestRate;
        IDhlExpressShipmentEntity IShipmentEntity.DhlExpressShipment => DhlExpressShipment;
        IFedExShipmentEntity IShipmentEntity.FedEx => FedEx;
        IInsurancePolicyEntity IShipmentEntity.InsurancePolicy => InsurancePolicy;
        IIParcelShipmentEntity IShipmentEntity.IParcel => IParcel;
        IOnTracShipmentEntity IShipmentEntity.OnTrac => OnTrac;
        IOtherShipmentEntity IShipmentEntity.Other => Other;
        IPostalShipmentEntity IShipmentEntity.Postal => Postal;
        IUpsShipmentEntity IShipmentEntity.Ups => Ups;
        
        IOrderEntity IShipmentEntity.Order => Order;
        
        IEnumerable<IShipmentCustomsItemEntity> IShipmentEntity.CustomsItems => CustomsItems;
        IEnumerable<IShipmentReturnItemEntity> IShipmentEntity.ReturnItems => ReturnItems;
        IEnumerable<IValidatedAddressEntity> IShipmentEntity.ValidatedAddress => ValidatedAddress;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShipmentEntity(this, objectMap);
        }

        
    }
}
