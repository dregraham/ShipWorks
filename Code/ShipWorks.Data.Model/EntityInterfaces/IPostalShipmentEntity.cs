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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'PostalShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IPostalShipmentEntity
    {
        
        /// <summary> The ShipmentID property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The Service property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Service { get; }
        /// <summary> The Confirmation property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."Confirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Confirmation { get; }
        /// <summary> The PackagingType property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PackagingType { get; }
        /// <summary> The DimsProfileID property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DimsAddWeight { get; }
        /// <summary> The NonRectangular property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."NonRectangular"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean NonRectangular { get; }
        /// <summary> The NonMachinable property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean NonMachinable { get; }
        /// <summary> The CustomsContentType property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."CustomsContentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 CustomsContentType { get; }
        /// <summary> The CustomsContentDescription property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."CustomsContentDescription"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CustomsContentDescription { get; }
        /// <summary> The InsuranceValue property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal InsuranceValue { get; }
        /// <summary> The ExpressSignatureWaiver property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."ExpressSignatureWaiver"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ExpressSignatureWaiver { get; }
        /// <summary> The SortType property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."SortType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 SortType { get; }
        /// <summary> The EntryFacility property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."EntryFacility"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 EntryFacility { get; }
        /// <summary> The Memo1 property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."Memo1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Memo1 { get; }
        /// <summary> The Memo2 property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."Memo2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Memo2 { get; }
        /// <summary> The Memo3 property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."Memo3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Memo3 { get; }
        /// <summary> The NoPostage property of the Entity PostalShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalShipment"."NoPostage"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean NoPostage { get; }
        
        IEndiciaShipmentEntity Endicia { get; }
        IShipmentEntity Shipment { get; }
        IUspsShipmentEntity Usps { get; }
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPostalShipmentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPostalShipmentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'PostalShipment'. <br/><br/>
    /// 
    /// </summary>
    public partial class PostalShipmentEntity : IPostalShipmentEntity
    {
        IEndiciaShipmentEntity IPostalShipmentEntity.Endicia => Endicia;
        IShipmentEntity IPostalShipmentEntity.Shipment => Shipment;
        IUspsShipmentEntity IPostalShipmentEntity.Usps => Usps;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPostalShipmentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IPostalShipmentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IPostalShipmentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyPostalShipmentEntity(this, objectMap);
        }
    }
}
