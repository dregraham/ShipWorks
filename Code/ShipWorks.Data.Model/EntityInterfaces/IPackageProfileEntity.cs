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
    /// Entity interface which represents the entity 'PackageProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IPackageProfileEntity
    {
        
        /// <summary> The PackageProfileID property of the Entity PackageProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PackageProfile"."PackageProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 PackageProfileID { get; }
        /// <summary> The ShippingProfileID property of the Entity PackageProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PackageProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The Weight property of the Entity PackageProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PackageProfile"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> Weight { get; }
        /// <summary> The DimsProfileID property of the Entity PackageProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PackageProfile"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity PackageProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PackageProfile"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity PackageProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PackageProfile"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity PackageProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PackageProfile"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity PackageProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PackageProfile"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity PackageProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PackageProfile"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DimsAddWeight { get; }
        
        
        IShippingProfileEntity ShippingProfile { get; }
        
        IEnumerable<IFedExProfilePackageEntity> FedExProfilePackage { get; }
        IEnumerable<IUpsProfilePackageEntity> UpsProfilePackage { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPackageProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPackageProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'PackageProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class PackageProfileEntity : IPackageProfileEntity
    {
        
        IShippingProfileEntity IPackageProfileEntity.ShippingProfile => ShippingProfile;
        
        IEnumerable<IFedExProfilePackageEntity> IPackageProfileEntity.FedExProfilePackage => FedExProfilePackage;
        IEnumerable<IUpsProfilePackageEntity> IPackageProfileEntity.UpsProfilePackage => UpsProfilePackage;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPackageProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IPackageProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IPackageProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyPackageProfileEntity(this, objectMap);
        }

        
    }
}
