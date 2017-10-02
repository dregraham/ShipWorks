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
    /// Read-only representation of the entity 'IParcelProfilePackage'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyIParcelProfilePackageEntity : IIParcelProfilePackageEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyIParcelProfilePackageEntity(IIParcelProfilePackageEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            IParcelProfilePackageID = source.IParcelProfilePackageID;
            ShippingProfileID = source.ShippingProfileID;
            Weight = source.Weight;
            DimsProfileID = source.DimsProfileID;
            DimsLength = source.DimsLength;
            DimsWidth = source.DimsWidth;
            DimsHeight = source.DimsHeight;
            DimsWeight = source.DimsWeight;
            DimsAddWeight = source.DimsAddWeight;
            
            
            IParcelProfile = (IIParcelProfileEntity) source.IParcelProfile?.AsReadOnly(objectMap);
            

            CopyCustomIParcelProfilePackageData(source);
        }

        
        /// <summary> The IParcelProfilePackageID property of the Entity IParcelProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfilePackage"."iParcelProfilePackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 IParcelProfilePackageID { get; }
        /// <summary> The ShippingProfileID property of the Entity IParcelProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfilePackage"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The Weight property of the Entity IParcelProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfilePackage"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> Weight { get; }
        /// <summary> The DimsProfileID property of the Entity IParcelProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfilePackage"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity IParcelProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfilePackage"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity IParcelProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfilePackage"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity IParcelProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfilePackage"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DimsHeight { get; }
        /// <summary> The DimsWeight property of the Entity IParcelProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfilePackage"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DimsWeight { get; }
        /// <summary> The DimsAddWeight property of the Entity IParcelProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelProfilePackage"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> DimsAddWeight { get; }
        
        
        public IIParcelProfileEntity IParcelProfile { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelProfilePackageEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelProfilePackageEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomIParcelProfilePackageData(IIParcelProfilePackageEntity source);
    }
}
