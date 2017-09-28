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
    /// Entity interface which represents the entity 'IParcelProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IIParcelProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The IParcelAccountID property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."iParcelAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> IParcelAccountID { get; }
        /// <summary> The Service property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Service { get; }
        /// <summary> The Reference property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."Reference"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Reference { get; }
        /// <summary> The TrackByEmail property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."TrackByEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> TrackByEmail { get; }
        /// <summary> The TrackBySMS property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."TrackBySMS"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> TrackBySMS { get; }
        /// <summary> The IsDeliveryDutyPaid property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."IsDeliveryDutyPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> IsDeliveryDutyPaid { get; }
        /// <summary> The SkuAndQuantities property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."SkuAndQuantities"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String SkuAndQuantities { get; }
        
        IShippingProfileEntity ShippingProfile { get; }
        
        
        IEnumerable<IIParcelProfilePackageEntity> Packages { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IIParcelProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IIParcelProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'IParcelProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class IParcelProfileEntity : IIParcelProfileEntity
    {
        IShippingProfileEntity IIParcelProfileEntity.ShippingProfile => ShippingProfile;
        
        
        IEnumerable<IIParcelProfilePackageEntity> IIParcelProfileEntity.Packages => Packages;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IIParcelProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IIParcelProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyIParcelProfileEntity(this, objectMap);
        }

        
    }
}
