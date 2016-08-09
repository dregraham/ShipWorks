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
    /// Entity interface which represents the entity 'PostalProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IPostalProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The Service property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Service { get; }
        /// <summary> The Confirmation property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."Confirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Confirmation { get; }
        /// <summary> The Weight property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> Weight { get; }
        /// <summary> The PackagingType property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> PackagingType { get; }
        /// <summary> The DimsProfileID property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DimsAddWeight { get; }
        /// <summary> The NonRectangular property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."NonRectangular"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> NonRectangular { get; }
        /// <summary> The NonMachinable property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> NonMachinable { get; }
        /// <summary> The CustomsContentType property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."CustomsContentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> CustomsContentType { get; }
        /// <summary> The CustomsContentDescription property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."CustomsContentDescription"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String CustomsContentDescription { get; }
        /// <summary> The ExpressSignatureWaiver property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."ExpressSignatureWaiver"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> ExpressSignatureWaiver { get; }
        /// <summary> The SortType property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."SortType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> SortType { get; }
        /// <summary> The EntryFacility property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."EntryFacility"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> EntryFacility { get; }
        /// <summary> The Memo1 property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."Memo1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Memo1 { get; }
        /// <summary> The Memo2 property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."Memo2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Memo2 { get; }
        /// <summary> The Memo3 property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."Memo3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Memo3 { get; }
        /// <summary> The NoPostage property of the Entity PostalProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PostalProfile"."NoPostage"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> NoPostage { get; }
        
        IEndiciaProfileEntity Endicia { get; }
        IShippingProfileEntity Profile { get; }
        IUspsProfileEntity Usps { get; }
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPostalProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPostalProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'PostalProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class PostalProfileEntity : IPostalProfileEntity
    {
        IEndiciaProfileEntity IPostalProfileEntity.Endicia => Endicia;
        IShippingProfileEntity IPostalProfileEntity.Profile => Profile;
        IUspsProfileEntity IPostalProfileEntity.Usps => Usps;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPostalProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IPostalProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IPostalProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyPostalProfileEntity(this, objectMap);
        }
    }
}
