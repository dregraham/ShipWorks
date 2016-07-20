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
    /// Entity interface which represents the entity 'Order'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOrderEntity : IOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOrderEntity(IOrderEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            OrderID = source.OrderID;
            RowVersion = source.RowVersion;
            StoreID = source.StoreID;
            CustomerID = source.CustomerID;
            OrderNumber = source.OrderNumber;
            OrderNumberComplete = source.OrderNumberComplete;
            OrderDate = source.OrderDate;
            OrderTotal = source.OrderTotal;
            LocalStatus = source.LocalStatus;
            IsManual = source.IsManual;
            OnlineLastModified = source.OnlineLastModified;
            OnlineCustomerID = source.OnlineCustomerID;
            OnlineStatus = source.OnlineStatus;
            OnlineStatusCode = source.OnlineStatusCode;
            RequestedShipping = source.RequestedShipping;
            BillFirstName = source.BillFirstName;
            BillMiddleName = source.BillMiddleName;
            BillLastName = source.BillLastName;
            BillCompany = source.BillCompany;
            BillStreet1 = source.BillStreet1;
            BillStreet2 = source.BillStreet2;
            BillStreet3 = source.BillStreet3;
            BillCity = source.BillCity;
            BillStateProvCode = source.BillStateProvCode;
            BillPostalCode = source.BillPostalCode;
            BillCountryCode = source.BillCountryCode;
            BillPhone = source.BillPhone;
            BillFax = source.BillFax;
            BillEmail = source.BillEmail;
            BillWebsite = source.BillWebsite;
            BillAddressValidationSuggestionCount = source.BillAddressValidationSuggestionCount;
            BillAddressValidationStatus = source.BillAddressValidationStatus;
            BillAddressValidationError = source.BillAddressValidationError;
            BillResidentialStatus = source.BillResidentialStatus;
            BillPOBox = source.BillPOBox;
            BillUSTerritory = source.BillUSTerritory;
            BillMilitaryAddress = source.BillMilitaryAddress;
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
            ShipFax = source.ShipFax;
            ShipEmail = source.ShipEmail;
            ShipWebsite = source.ShipWebsite;
            ShipAddressValidationSuggestionCount = source.ShipAddressValidationSuggestionCount;
            ShipAddressValidationStatus = source.ShipAddressValidationStatus;
            ShipAddressValidationError = source.ShipAddressValidationError;
            ShipResidentialStatus = source.ShipResidentialStatus;
            ShipPOBox = source.ShipPOBox;
            ShipUSTerritory = source.ShipUSTerritory;
            ShipMilitaryAddress = source.ShipMilitaryAddress;
            RollupItemCount = source.RollupItemCount;
            RollupItemName = source.RollupItemName;
            RollupItemCode = source.RollupItemCode;
            RollupItemSKU = source.RollupItemSKU;
            RollupItemLocation = source.RollupItemLocation;
            RollupItemQuantity = source.RollupItemQuantity;
            RollupItemTotalWeight = source.RollupItemTotalWeight;
            RollupNoteCount = source.RollupNoteCount;
            BillNameParseStatus = source.BillNameParseStatus;
            BillUnparsedName = source.BillUnparsedName;
            ShipNameParseStatus = source.ShipNameParseStatus;
            ShipUnparsedName = source.ShipUnparsedName;
            ShipSenseHashKey = source.ShipSenseHashKey;
            ShipSenseRecognitionStatus = source.ShipSenseRecognitionStatus;
            ShipAddressType = source.ShipAddressType;
            
            
            Customer = source.Customer?.AsReadOnly(objectMap);
            Store = source.Store?.AsReadOnly(objectMap);
            
            Notes = source.Notes?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<INoteEntity>();
            OrderCharges = source.OrderCharges?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IOrderChargeEntity>();
            OrderItems = source.OrderItems?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IOrderItemEntity>();
            OrderPaymentDetails = source.OrderPaymentDetails?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IOrderPaymentDetailEntity>();
            Shipments = source.Shipments?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IShipmentEntity>();
            ValidatedAddress = source.ValidatedAddress?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IValidatedAddressEntity>();

            CopyCustomOrderData(source);
        }

        
        /// <summary> The OrderID property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The RowVersion property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The StoreID property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 StoreID { get; }
        /// <summary> The CustomerID property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."CustomerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 CustomerID { get; }
        /// <summary> The OrderNumber property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderNumber { get; }
        /// <summary> The OrderNumberComplete property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OrderNumberComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OrderNumberComplete { get; }
        /// <summary> The OrderDate property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OrderDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime OrderDate { get; }
        /// <summary> The OrderTotal property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OrderTotal"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal OrderTotal { get; }
        /// <summary> The LocalStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."LocalStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LocalStatus { get; }
        /// <summary> The IsManual property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."IsManual"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsManual { get; }
        /// <summary> The OnlineLastModified property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OnlineLastModified"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime OnlineLastModified { get; }
        /// <summary> The OnlineCustomerID property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OnlineCustomerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Variant, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.Object OnlineCustomerID { get; }
        /// <summary> The OnlineStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OnlineStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OnlineStatus { get; }
        /// <summary> The OnlineStatusCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."OnlineStatusCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): Variant, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.Object OnlineStatusCode { get; }
        /// <summary> The RequestedShipping property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RequestedShipping"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String RequestedShipping { get; }
        /// <summary> The BillFirstName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillFirstName { get; }
        /// <summary> The BillMiddleName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillMiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillMiddleName { get; }
        /// <summary> The BillLastName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillLastName { get; }
        /// <summary> The BillCompany property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillCompany { get; }
        /// <summary> The BillStreet1 property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillStreet1 { get; }
        /// <summary> The BillStreet2 property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillStreet2 { get; }
        /// <summary> The BillStreet3 property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillStreet3 { get; }
        /// <summary> The BillCity property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillCity { get; }
        /// <summary> The BillStateProvCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillStateProvCode { get; }
        /// <summary> The BillPostalCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillPostalCode { get; }
        /// <summary> The BillCountryCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillCountryCode { get; }
        /// <summary> The BillPhone property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillPhone { get; }
        /// <summary> The BillFax property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillFax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillFax { get; }
        /// <summary> The BillEmail property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillEmail { get; }
        /// <summary> The BillWebsite property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillWebsite"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillWebsite { get; }
        /// <summary> The BillAddressValidationSuggestionCount property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillAddressValidationSuggestionCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 BillAddressValidationSuggestionCount { get; }
        /// <summary> The BillAddressValidationStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillAddressValidationStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 BillAddressValidationStatus { get; }
        /// <summary> The BillAddressValidationError property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillAddressValidationError"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillAddressValidationError { get; }
        /// <summary> The BillResidentialStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillResidentialStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 BillResidentialStatus { get; }
        /// <summary> The BillPOBox property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillPOBox"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 BillPOBox { get; }
        /// <summary> The BillUSTerritory property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillUSTerritory"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 BillUSTerritory { get; }
        /// <summary> The BillMilitaryAddress property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillMilitaryAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 BillMilitaryAddress { get; }
        /// <summary> The ShipFirstName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipFirstName { get; }
        /// <summary> The ShipMiddleName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipMiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipMiddleName { get; }
        /// <summary> The ShipLastName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipLastName { get; }
        /// <summary> The ShipCompany property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipCompany { get; }
        /// <summary> The ShipStreet1 property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipStreet1 { get; }
        /// <summary> The ShipStreet2 property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipStreet2 { get; }
        /// <summary> The ShipStreet3 property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipStreet3 { get; }
        /// <summary> The ShipCity property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipCity { get; }
        /// <summary> The ShipStateProvCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipStateProvCode { get; }
        /// <summary> The ShipPostalCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipPostalCode { get; }
        /// <summary> The ShipCountryCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipCountryCode { get; }
        /// <summary> The ShipPhone property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipPhone { get; }
        /// <summary> The ShipFax property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipFax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipFax { get; }
        /// <summary> The ShipEmail property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipEmail { get; }
        /// <summary> The ShipWebsite property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipWebsite"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipWebsite { get; }
        /// <summary> The ShipAddressValidationSuggestionCount property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipAddressValidationSuggestionCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipAddressValidationSuggestionCount { get; }
        /// <summary> The ShipAddressValidationStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipAddressValidationStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipAddressValidationStatus { get; }
        /// <summary> The ShipAddressValidationError property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipAddressValidationError"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipAddressValidationError { get; }
        /// <summary> The ShipResidentialStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipResidentialStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipResidentialStatus { get; }
        /// <summary> The ShipPOBox property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipPOBox"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipPOBox { get; }
        /// <summary> The ShipUSTerritory property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipUSTerritory"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipUSTerritory { get; }
        /// <summary> The ShipMilitaryAddress property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipMilitaryAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipMilitaryAddress { get; }
        /// <summary> The RollupItemCount property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RollupItemCount { get; }
        /// <summary> The RollupItemName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String RollupItemName { get; }
        /// <summary> The RollupItemCode property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String RollupItemCode { get; }
        /// <summary> The RollupItemSKU property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemSKU"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String RollupItemSKU { get; }
        /// <summary> The RollupItemLocation property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemLocation"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String RollupItemLocation { get; }
        /// <summary> The RollupItemQuantity property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemQuantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> RollupItemQuantity { get; }
        /// <summary> The RollupItemTotalWeight property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupItemTotalWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double RollupItemTotalWeight { get; }
        /// <summary> The RollupNoteCount property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."RollupNoteCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RollupNoteCount { get; }
        /// <summary> The BillNameParseStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillNameParseStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 BillNameParseStatus { get; }
        /// <summary> The BillUnparsedName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."BillUnparsedName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillUnparsedName { get; }
        /// <summary> The ShipNameParseStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipNameParseStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipNameParseStatus { get; }
        /// <summary> The ShipUnparsedName property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipUnparsedName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipUnparsedName { get; }
        /// <summary> The ShipSenseHashKey property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipSenseHashKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipSenseHashKey { get; }
        /// <summary> The ShipSenseRecognitionStatus property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipSenseRecognitionStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipSenseRecognitionStatus { get; }
        /// <summary> The ShipAddressType property of the Entity Order<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Order"."ShipAddressType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipAddressType { get; }
        
        
        public ICustomerEntity Customer { get; }
        
        public IStoreEntity Store { get; }
        
        
        public IEnumerable<INoteEntity> Notes { get; }
        
        public IEnumerable<IOrderChargeEntity> OrderCharges { get; }
        
        public IEnumerable<IOrderItemEntity> OrderItems { get; }
        
        public IEnumerable<IOrderPaymentDetailEntity> OrderPaymentDetails { get; }
        
        public IEnumerable<IShipmentEntity> Shipments { get; }
        
        public IEnumerable<IValidatedAddressEntity> ValidatedAddress { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOrderData(IOrderEntity source);
    }
}
