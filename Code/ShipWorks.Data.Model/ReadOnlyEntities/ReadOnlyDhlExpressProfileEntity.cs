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
    /// Read-only representation of the entity 'DhlExpressProfile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyDhlExpressProfileEntity : IDhlExpressProfileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyDhlExpressProfileEntity(IDhlExpressProfileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingProfileID = source.ShippingProfileID;
            DhlExpressAccountID = source.DhlExpressAccountID;
            Service = source.Service;
            DeliveryDutyPaid = source.DeliveryDutyPaid;
            NonMachinable = source.NonMachinable;
            SaturdayDelivery = source.SaturdayDelivery;
            
            ShippingProfile = (IShippingProfileEntity) source.ShippingProfile?.AsReadOnly(objectMap);
            
            
            Packages = source.Packages?.Select(x => x.AsReadOnly(objectMap)).OfType<IDhlExpressProfilePackageEntity>().ToReadOnly() ??
                Enumerable.Empty<IDhlExpressProfilePackageEntity>();

            CopyCustomDhlExpressProfileData(source);
        }

        
        /// <summary> The ShippingProfileID property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The DhlExpressAccountID property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."DhlExpressAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> DhlExpressAccountID { get; }
        /// <summary> The Service property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> Service { get; }
        /// <summary> The DeliveryDutyPaid property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."DeliveryDutyPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> DeliveryDutyPaid { get; }
        /// <summary> The NonMachinable property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> NonMachinable { get; }
        /// <summary> The SaturdayDelivery property of the Entity DhlExpressProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressProfile"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> SaturdayDelivery { get; }
        
        public IShippingProfileEntity ShippingProfile { get; }
        
        
        
        public IEnumerable<IDhlExpressProfilePackageEntity> Packages { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlExpressProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlExpressProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomDhlExpressProfileData(IDhlExpressProfileEntity source);
    }
}
