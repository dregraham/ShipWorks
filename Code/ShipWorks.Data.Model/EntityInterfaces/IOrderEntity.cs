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
    /// Entity interface which represents the entity 'Order'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOrderEntity
    {
        
        /// <summary> The OrderID property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The RowVersion property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The StoreID property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 StoreID { get; }
        /// <summary> The CustomerID property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."CustomerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 CustomerID { get; }
        /// <summary> The OrderNumber property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderNumber { get; }
        /// <summary> The OrderNumberComplete property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OrderNumberComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OrderNumberComplete { get; }
        /// <summary> The OrderDate property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OrderDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime OrderDate { get; }
        /// <summary> The OrderTotal property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OrderTotal"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal OrderTotal { get; }
        /// <summary> The LocalStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."LocalStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LocalStatus { get; }
        /// <summary> The IsManual property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."IsManual"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsManual { get; }
        /// <summary> The OnlineLastModified property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OnlineLastModified"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime2, 7, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime OnlineLastModified { get; }
        /// <summary> The OnlineCustomerID property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OnlineCustomerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Variant, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.Object OnlineCustomerID { get; }
        /// <summary> The OnlineStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OnlineStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OnlineStatus { get; }
        /// <summary> The OnlineStatusCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OnlineStatusCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Variant, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.Object OnlineStatusCode { get; }
        /// <summary> The RequestedShipping property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RequestedShipping"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String RequestedShipping { get; }
        /// <summary> The BillFirstName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillFirstName { get; }
        /// <summary> The BillMiddleName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillMiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillMiddleName { get; }
        /// <summary> The BillLastName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillLastName { get; }
        /// <summary> The BillCompany property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillCompany { get; }
        /// <summary> The BillStreet1 property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillStreet1 { get; }
        /// <summary> The BillStreet2 property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillStreet2 { get; }
        /// <summary> The BillStreet3 property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillStreet3 { get; }
        /// <summary> The BillCity property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillCity { get; }
        /// <summary> The BillStateProvCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillStateProvCode { get; }
        /// <summary> The BillPostalCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillPostalCode { get; }
        /// <summary> The BillCountryCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillCountryCode { get; }
        /// <summary> The BillPhone property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillPhone { get; }
        /// <summary> The BillFax property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillFax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillFax { get; }
        /// <summary> The BillEmail property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillEmail { get; }
        /// <summary> The BillWebsite property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillWebsite"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillWebsite { get; }
        /// <summary> The BillAddressValidationSuggestionCount property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillAddressValidationSuggestionCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 BillAddressValidationSuggestionCount { get; }
        /// <summary> The BillAddressValidationStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillAddressValidationStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 BillAddressValidationStatus { get; }
        /// <summary> The BillAddressValidationError property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillAddressValidationError"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillAddressValidationError { get; }
        /// <summary> The BillResidentialStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillResidentialStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 BillResidentialStatus { get; }
        /// <summary> The BillPOBox property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillPOBox"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 BillPOBox { get; }
        /// <summary> The BillUSTerritory property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillUSTerritory"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 BillUSTerritory { get; }
        /// <summary> The BillMilitaryAddress property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillMilitaryAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 BillMilitaryAddress { get; }
        /// <summary> The ShipFirstName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipFirstName { get; }
        /// <summary> The ShipMiddleName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipMiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipMiddleName { get; }
        /// <summary> The ShipLastName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipLastName { get; }
        /// <summary> The ShipCompany property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipCompany { get; }
        /// <summary> The ShipStreet1 property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipStreet1 { get; }
        /// <summary> The ShipStreet2 property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipStreet2 { get; }
        /// <summary> The ShipStreet3 property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipStreet3 { get; }
        /// <summary> The ShipCity property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipCity { get; }
        /// <summary> The ShipStateProvCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipStateProvCode { get; }
        /// <summary> The ShipPostalCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipPostalCode { get; }
        /// <summary> The ShipCountryCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipCountryCode { get; }
        /// <summary> The ShipPhone property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipPhone { get; }
        /// <summary> The ShipFax property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipFax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipFax { get; }
        /// <summary> The ShipEmail property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipEmail { get; }
        /// <summary> The ShipWebsite property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipWebsite"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipWebsite { get; }
        /// <summary> The ShipAddressValidationSuggestionCount property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipAddressValidationSuggestionCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipAddressValidationSuggestionCount { get; }
        /// <summary> The ShipAddressValidationStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipAddressValidationStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipAddressValidationStatus { get; }
        /// <summary> The ShipAddressValidationError property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipAddressValidationError"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipAddressValidationError { get; }
        /// <summary> The ShipResidentialStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipResidentialStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipResidentialStatus { get; }
        /// <summary> The ShipPOBox property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipPOBox"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipPOBox { get; }
        /// <summary> The ShipUSTerritory property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipUSTerritory"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipUSTerritory { get; }
        /// <summary> The ShipMilitaryAddress property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipMilitaryAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipMilitaryAddress { get; }
        /// <summary> The RollupItemCount property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RollupItemCount { get; }
        /// <summary> The RollupItemName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String RollupItemName { get; }
        /// <summary> The RollupItemCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String RollupItemCode { get; }
        /// <summary> The RollupItemSKU property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemSKU"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String RollupItemSKU { get; }
        /// <summary> The RollupItemLocation property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemLocation"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String RollupItemLocation { get; }
        /// <summary> The RollupItemQuantity property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemQuantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> RollupItemQuantity { get; }
        /// <summary> The RollupItemTotalWeight property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemTotalWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double RollupItemTotalWeight { get; }
        /// <summary> The RollupNoteCount property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupNoteCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 RollupNoteCount { get; }
        /// <summary> The BillNameParseStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillNameParseStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 BillNameParseStatus { get; }
        /// <summary> The BillUnparsedName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillUnparsedName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BillUnparsedName { get; }
        /// <summary> The ShipNameParseStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipNameParseStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipNameParseStatus { get; }
        /// <summary> The ShipUnparsedName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipUnparsedName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipUnparsedName { get; }
        /// <summary> The ShipSenseHashKey property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipSenseHashKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShipSenseHashKey { get; }
        /// <summary> The ShipSenseRecognitionStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipSenseRecognitionStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipSenseRecognitionStatus { get; }
        /// <summary> The ShipAddressType property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipAddressType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipAddressType { get; }
        /// <summary> The CombineSplitStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."CombineSplitStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        Interapptive.Shared.Enums.CombineSplitStatusType CombineSplitStatus { get; }
        
        
        ICustomerEntity Customer { get; }
        IStoreEntity Store { get; }
        
        IEnumerable<INoteEntity> Notes { get; }
        IEnumerable<IOrderChargeEntity> OrderCharges { get; }
        IEnumerable<IOrderItemEntity> OrderItems { get; }
        IEnumerable<IOrderPaymentDetailEntity> OrderPaymentDetails { get; }
        IEnumerable<IOrderSearchEntity> OrderSearch { get; }
        IEnumerable<IShipmentEntity> Shipments { get; }
        IEnumerable<IValidatedAddressEntity> ValidatedAddress { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Order'. <br/><br/>
    /// 
    /// </summary>
    public partial class OrderEntity : IOrderEntity
    {
        
        ICustomerEntity IOrderEntity.Customer => Customer;
        IStoreEntity IOrderEntity.Store => Store;
        
        IEnumerable<INoteEntity> IOrderEntity.Notes => Notes;
        IEnumerable<IOrderChargeEntity> IOrderEntity.OrderCharges => OrderCharges;
        IEnumerable<IOrderItemEntity> IOrderEntity.OrderItems => OrderItems;
        IEnumerable<IOrderPaymentDetailEntity> IOrderEntity.OrderPaymentDetails => OrderPaymentDetails;
        IEnumerable<IOrderSearchEntity> IOrderEntity.OrderSearch => OrderSearch;
        IEnumerable<IShipmentEntity> IOrderEntity.Shipments => Shipments;
        IEnumerable<IValidatedAddressEntity> IOrderEntity.ValidatedAddress => ValidatedAddress;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOrderEntity(this, objectMap);
        }
    }
}
