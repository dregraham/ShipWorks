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
    /// Read-only representation of the entity 'DhlExpressPackage'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyDhlExpressPackageEntity : IDhlExpressPackageEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyDhlExpressPackageEntity(IDhlExpressPackageEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            DhlExpressPackageID = source.DhlExpressPackageID;
            ShipmentID = source.ShipmentID;
            Weight = source.Weight;
            DimsProfileID = source.DimsProfileID;
            DimsLength = source.DimsLength;
            DimsWidth = source.DimsWidth;
            DimsHeight = source.DimsHeight;
            DimsAddWeight = source.DimsAddWeight;
            DimsWeight = source.DimsWeight;
            DeclaredValue = source.DeclaredValue;
            TrackingNumber = source.TrackingNumber;
            
            
            DhlExpressShipment = (IDhlExpressShipmentEntity) source.DhlExpressShipment?.AsReadOnly(objectMap);
            

            CopyCustomDhlExpressPackageData(source);
        }

        
        /// <summary> The DhlExpressPackageID property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DhlExpressPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 DhlExpressPackageID { get; }
        /// <summary> The ShipmentID property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The Weight property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Weight { get; }
        /// <summary> The DimsProfileID property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsHeight { get; }
        /// <summary> The DimsAddWeight property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DimsAddWeight { get; }
        /// <summary> The DimsWeight property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWeight { get; }
        /// <summary> The DeclaredValue property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DeclaredValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal DeclaredValue { get; }
        /// <summary> The TrackingNumber property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."TrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TrackingNumber { get; }
        
        
        public IDhlExpressShipmentEntity DhlExpressShipment { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlExpressPackageEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlExpressPackageEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomDhlExpressPackageData(IDhlExpressPackageEntity source);
    }
}
