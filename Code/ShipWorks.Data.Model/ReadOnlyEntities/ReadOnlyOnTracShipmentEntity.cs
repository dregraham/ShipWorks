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
    /// Entity interface which represents the entity 'OnTracShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOnTracShipmentEntity : IOnTracShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOnTracShipmentEntity(IOnTracShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            OnTracAccountID = source.OnTracAccountID;
            Service = source.Service;
            IsCod = source.IsCod;
            CodType = source.CodType;
            CodAmount = source.CodAmount;
            SaturdayDelivery = source.SaturdayDelivery;
            SignatureRequired = source.SignatureRequired;
            PackagingType = source.PackagingType;
            Instructions = source.Instructions;
            DimsProfileID = source.DimsProfileID;
            DimsLength = source.DimsLength;
            DimsWidth = source.DimsWidth;
            DimsHeight = source.DimsHeight;
            DimsWeight = source.DimsWeight;
            DimsAddWeight = source.DimsAddWeight;
            Reference1 = source.Reference1;
            Reference2 = source.Reference2;
            InsuranceValue = source.InsuranceValue;
            InsurancePennyOne = source.InsurancePennyOne;
            DeclaredValue = source.DeclaredValue;
            RequestedLabelFormat = source.RequestedLabelFormat;
            
            Shipment = source.Shipment?.AsReadOnly(objectMap);
            
            

            CopyCustomOnTracShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The OnTracAccountID property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."OnTracAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OnTracAccountID { get; }
        /// <summary> The Service property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Service { get; }
        /// <summary> The IsCod property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."IsCod"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsCod { get; }
        /// <summary> The CodType property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."CodType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CodType { get; }
        /// <summary> The CodAmount property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."CodAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal CodAmount { get; }
        /// <summary> The SaturdayDelivery property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean SaturdayDelivery { get; }
        /// <summary> The SignatureRequired property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."SignatureRequired"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean SignatureRequired { get; }
        /// <summary> The PackagingType property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PackagingType { get; }
        /// <summary> The Instructions property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."Instructions"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Instructions { get; }
        /// <summary> The DimsProfileID property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DimsAddWeight { get; }
        /// <summary> The Reference1 property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."Reference1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Reference1 { get; }
        /// <summary> The Reference2 property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."Reference2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Reference2 { get; }
        /// <summary> The InsuranceValue property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal InsuranceValue { get; }
        /// <summary> The InsurancePennyOne property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."InsurancePennyOne"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean InsurancePennyOne { get; }
        /// <summary> The DeclaredValue property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."DeclaredValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal DeclaredValue { get; }
        /// <summary> The RequestedLabelFormat property of the Entity OnTracShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        
        public IShipmentEntity Shipment { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOnTracShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOnTracShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOnTracShipmentData(IOnTracShipmentEntity source);
    }
}
