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
    /// Entity interface which represents the entity 'AsendiaProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAsendiaProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity AsendiaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The AsendiaAccountID property of the Entity AsendiaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaProfile"."AsendiaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> AsendiaAccountID { get; }
        /// <summary> The Service property of the Entity AsendiaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<Interapptive.Shared.Enums.AsendiaServiceType> Service { get; }
        /// <summary> The NonMachinable property of the Entity AsendiaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaProfile"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> NonMachinable { get; }
        /// <summary> The Contents property of the Entity AsendiaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaProfile"."Contents"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Contents { get; }
        /// <summary> The NonDelivery property of the Entity AsendiaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaProfile"."NonDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> NonDelivery { get; }
        
        IShippingProfileEntity ShippingProfile { get; }
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAsendiaProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAsendiaProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AsendiaProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class AsendiaProfileEntity : IAsendiaProfileEntity
    {
        IShippingProfileEntity IAsendiaProfileEntity.ShippingProfile => ShippingProfile;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAsendiaProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IAsendiaProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAsendiaProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAsendiaProfileEntity(this, objectMap);
        }

        
    }
}
