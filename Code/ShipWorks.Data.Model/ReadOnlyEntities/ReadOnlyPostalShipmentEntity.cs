﻿///////////////////////////////////////////////////////////////
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
    /// Read-only representation of the entity 'PostalShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyPostalShipmentEntity : IPostalShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyPostalShipmentEntity(IPostalShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            Service = source.Service;
            Confirmation = source.Confirmation;
            PackagingType = source.PackagingType;
            DimsProfileID = source.DimsProfileID;
            DimsLength = source.DimsLength;
            DimsWidth = source.DimsWidth;
            DimsHeight = source.DimsHeight;
            DimsWeight = source.DimsWeight;
            DimsAddWeight = source.DimsAddWeight;
            NonRectangular = source.NonRectangular;
            NonMachinable = source.NonMachinable;
            CustomsContentType = source.CustomsContentType;
            CustomsContentDescription = source.CustomsContentDescription;
            InsuranceValue = source.InsuranceValue;
            ExpressSignatureWaiver = source.ExpressSignatureWaiver;
            SortType = source.SortType;
            EntryFacility = source.EntryFacility;
            Memo1 = source.Memo1;
            Memo2 = source.Memo2;
            Memo3 = source.Memo3;
            NoPostage = source.NoPostage;
            Insurance = source.Insurance;
            CustomsRecipientTin = source.CustomsRecipientTin;
            
            Endicia = (IEndiciaShipmentEntity) source.Endicia?.AsReadOnly(objectMap);
            Shipment = (IShipmentEntity) source.Shipment?.AsReadOnly(objectMap);
            Usps = (IUspsShipmentEntity) source.Usps?.AsReadOnly(objectMap);
            
            

            CopyCustomPostalShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The Service property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Service { get; }
        /// <summary> The Confirmation property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."Confirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Confirmation { get; }
        /// <summary> The PackagingType property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PackagingType { get; }
        /// <summary> The DimsProfileID property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DimsAddWeight { get; }
        /// <summary> The NonRectangular property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."NonRectangular"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean NonRectangular { get; }
        /// <summary> The NonMachinable property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean NonMachinable { get; }
        /// <summary> The CustomsContentType property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."CustomsContentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 CustomsContentType { get; }
        /// <summary> The CustomsContentDescription property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."CustomsContentDescription"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CustomsContentDescription { get; }
        /// <summary> The InsuranceValue property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal InsuranceValue { get; }
        /// <summary> The ExpressSignatureWaiver property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."ExpressSignatureWaiver"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ExpressSignatureWaiver { get; }
        /// <summary> The SortType property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."SortType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 SortType { get; }
        /// <summary> The EntryFacility property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."EntryFacility"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EntryFacility { get; }
        /// <summary> The Memo1 property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."Memo1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Memo1 { get; }
        /// <summary> The Memo2 property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."Memo2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Memo2 { get; }
        /// <summary> The Memo3 property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."Memo3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Memo3 { get; }
        /// <summary> The NoPostage property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."NoPostage"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean NoPostage { get; }
        /// <summary> The Insurance property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Insurance { get; }
        /// <summary> The CustomsRecipientTin property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."CustomsRecipientTin"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 24<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CustomsRecipientTin { get; }
        
        public IEndiciaShipmentEntity Endicia { get; }
        
        public IShipmentEntity Shipment { get; }
        
        public IUspsShipmentEntity Usps { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPostalShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPostalShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomPostalShipmentData(IPostalShipmentEntity source);
    }
}
