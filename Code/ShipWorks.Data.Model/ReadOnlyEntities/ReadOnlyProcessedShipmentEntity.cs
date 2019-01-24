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
    /// Read-only representation of the entity 'ProcessedShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyProcessedShipmentEntity : IProcessedShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyProcessedShipmentEntity(IProcessedShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            ShipmentType = source.ShipmentType;
            ShipDate = source.ShipDate;
            Insurance = source.Insurance;
            InsuranceProvider = source.InsuranceProvider;
            ProcessedDate = source.ProcessedDate;
            ProcessedUserID = source.ProcessedUserID;
            ProcessedComputerID = source.ProcessedComputerID;
            Voided = source.Voided;
            VoidedDate = source.VoidedDate;
            VoidedUserID = source.VoidedUserID;
            VoidedComputerID = source.VoidedComputerID;
            TotalWeight = source.TotalWeight;
            TrackingNumber = source.TrackingNumber;
            ShipmentCost = source.ShipmentCost;
            ShipSenseStatus = source.ShipSenseStatus;
            ShipAddressValidationStatus = source.ShipAddressValidationStatus;
            ShipResidentialStatus = source.ShipResidentialStatus;
            ShipPOBox = source.ShipPOBox;
            ShipMilitaryAddress = source.ShipMilitaryAddress;
            RequestedLabelFormat = source.RequestedLabelFormat;
            ActualLabelFormat = source.ActualLabelFormat;
            OrderID = source.OrderID;
            OrderNumberComplete = source.OrderNumberComplete;
            Service = source.Service;
            ShipUSTerritory = source.ShipUSTerritory;
            ProcessedWithUiMode = source.ProcessedWithUiMode;
            CombineSplitStatus = source.CombineSplitStatus;
            
            
            

            CopyCustomProcessedShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipmentID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The ShipmentType property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipmentType"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public ShipWorks.Shipping.ShipmentTypeCode ShipmentType { get; }
        /// <summary> The ShipDate property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipDate"<br/>
        /// View field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime ShipDate { get; }
        /// <summary> The Insurance property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."Insurance"<br/>
        /// View field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Insurance { get; }
        /// <summary> The InsuranceProvider property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."InsuranceProvider"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 InsuranceProvider { get; }
        /// <summary> The ProcessedDate property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ProcessedDate"<br/>
        /// View field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> ProcessedDate { get; }
        /// <summary> The ProcessedUserID property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ProcessedUserID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ProcessedUserID { get; }
        /// <summary> The ProcessedComputerID property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ProcessedComputerID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ProcessedComputerID { get; }
        /// <summary> The Voided property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."Voided"<br/>
        /// View field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Voided { get; }
        /// <summary> The VoidedDate property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."VoidedDate"<br/>
        /// View field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> VoidedDate { get; }
        /// <summary> The VoidedUserID property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."VoidedUserID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> VoidedUserID { get; }
        /// <summary> The VoidedComputerID property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."VoidedComputerID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> VoidedComputerID { get; }
        /// <summary> The TotalWeight property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."TotalWeight"<br/>
        /// View field type characteristics (type, precision, scale, length): Decimal, 29, 9, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal TotalWeight { get; }
        /// <summary> The TrackingNumber property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."TrackingNumber"<br/>
        /// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TrackingNumber { get; }
        /// <summary> The ShipmentCost property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipmentCost"<br/>
        /// View field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal ShipmentCost { get; }
        /// <summary> The ShipSenseStatus property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipSenseStatus"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipSenseStatus { get; }
        /// <summary> The ShipAddressValidationStatus property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipAddressValidationStatus"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipAddressValidationStatus { get; }
        /// <summary> The ShipResidentialStatus property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipResidentialStatus"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipResidentialStatus { get; }
        /// <summary> The ShipPOBox property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipPOBox"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipPOBox { get; }
        /// <summary> The ShipMilitaryAddress property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipMilitaryAddress"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipMilitaryAddress { get; }
        /// <summary> The RequestedLabelFormat property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."RequestedLabelFormat"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        /// <summary> The ActualLabelFormat property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ActualLabelFormat"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ActualLabelFormat { get; }
        /// <summary> The OrderID property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."OrderID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The OrderNumberComplete property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."OrderNumberComplete"<br/>
        /// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OrderNumberComplete { get; }
        /// <summary> The Service property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."Service"<br/>
        /// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 101<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Service { get; }
        /// <summary> The ShipUSTerritory property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ShipUSTerritory"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipUSTerritory { get; }
        /// <summary> The ProcessedWithUiMode property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."ProcessedWithUiMode"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ProcessedWithUiMode { get; }
        /// <summary> The CombineSplitStatus property of the Entity ProcessedShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProcessedShipmentsView"."CombineSplitStatus"<br/>
        /// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CombineSplitStatus { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProcessedShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProcessedShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProcessedShipmentData(IProcessedShipmentEntity source);
    }
}
