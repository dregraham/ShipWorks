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
    /// Read-only representation of the entity 'IParcelProfile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyIParcelProfileEntity : IIParcelProfileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyIParcelProfileEntity(IIParcelProfileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingProfileID = source.ShippingProfileID;
            IParcelAccountID = source.IParcelAccountID;
            Service = source.Service;
            Reference = source.Reference;
            TrackByEmail = source.TrackByEmail;
            TrackBySMS = source.TrackBySMS;
            IsDeliveryDutyPaid = source.IsDeliveryDutyPaid;
            SkuAndQuantities = source.SkuAndQuantities;
            
            ShippingProfile = source.ShippingProfile?.AsReadOnly(objectMap);
            
            
            Packages = source.Packages?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IIParcelProfilePackageEntity>();

            CopyCustomIParcelProfileData(source);
        }

        
        /// <summary> The ShippingProfileID property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The IParcelAccountID property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."iParcelAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> IParcelAccountID { get; }
        /// <summary> The Service property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> Service { get; }
        /// <summary> The Reference property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."Reference"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Reference { get; }
        /// <summary> The TrackByEmail property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."TrackByEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> TrackByEmail { get; }
        /// <summary> The TrackBySMS property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."TrackBySMS"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> TrackBySMS { get; }
        /// <summary> The IsDeliveryDutyPaid property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."IsDeliveryDutyPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> IsDeliveryDutyPaid { get; }
        /// <summary> The SkuAndQuantities property of the Entity IParcelProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfile"."SkuAndQuantities"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String SkuAndQuantities { get; }
        
        public IShippingProfileEntity ShippingProfile { get; }
        
        
        
        public IEnumerable<IIParcelProfilePackageEntity> Packages { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomIParcelProfileData(IIParcelProfileEntity source);
    }
}
