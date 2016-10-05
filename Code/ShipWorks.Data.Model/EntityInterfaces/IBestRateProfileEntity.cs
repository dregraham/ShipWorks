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
    /// Entity interface which represents the entity 'BestRateProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IBestRateProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity BestRateProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The DimsProfileID property of the Entity BestRateProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateProfile"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity BestRateProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateProfile"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity BestRateProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateProfile"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity BestRateProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateProfile"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity BestRateProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateProfile"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity BestRateProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateProfile"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DimsAddWeight { get; }
        /// <summary> The Weight property of the Entity BestRateProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateProfile"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> Weight { get; }
        /// <summary> The ServiceLevel property of the Entity BestRateProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "BestRateProfile"."ServiceLevel"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ServiceLevel { get; }
        
        IShippingProfileEntity ShippingProfile { get; }
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IBestRateProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IBestRateProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'BestRateProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class BestRateProfileEntity : IBestRateProfileEntity
    {
        IShippingProfileEntity IBestRateProfileEntity.ShippingProfile => ShippingProfile;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IBestRateProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IBestRateProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IBestRateProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyBestRateProfileEntity(this, objectMap);
        }
    }
}
