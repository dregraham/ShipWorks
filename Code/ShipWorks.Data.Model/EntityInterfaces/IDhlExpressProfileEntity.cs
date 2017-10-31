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
    /// Entity interface which represents the entity 'DhlExpressProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IDhlExpressProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The ShipEngineAccountID property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."ShipEngineAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ShipEngineAccountID { get; }
        /// <summary> The Service property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Service { get; }
        /// <summary> The DeliveryDutyPaid property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."DeliveryDutyPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DeliveryDutyPaid { get; }
        /// <summary> The NonMachinable property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> NonMachinable { get; }
        /// <summary> The SaturdayDelivery property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> SaturdayDelivery { get; }
        /// <summary> The Contents property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."Contents"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> Contents { get; }
        /// <summary> The NonDelivery property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."NonDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> NonDelivery { get; }
        
        IShippingProfileEntity ShippingProfile { get; }
        
        
        IEnumerable<IDhlExpressProfilePackageEntity> Packages { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDhlExpressProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDhlExpressProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'DhlExpressProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class DhlExpressProfileEntity : IDhlExpressProfileEntity
    {
        IShippingProfileEntity IDhlExpressProfileEntity.ShippingProfile => ShippingProfile;
        
        
        IEnumerable<IDhlExpressProfilePackageEntity> IDhlExpressProfileEntity.Packages => Packages;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlExpressProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IDhlExpressProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IDhlExpressProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyDhlExpressProfileEntity(this, objectMap);
        }

        
    }
}
