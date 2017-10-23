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
    /// Entity interface which represents the entity 'DhlExpressPackage'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IDhlExpressPackageEntity
    {
        
        /// <summary> The DhlExpressPackageID property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DhlExpressPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 DhlExpressPackageID { get; }
        /// <summary> The ShipmentID property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The Weight property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Weight { get; }
        /// <summary> The DimsProfileID property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsHeight { get; }
        /// <summary> The DimsAddWeight property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DimsAddWeight { get; }
        /// <summary> The DimsWeight property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWeight { get; }
        /// <summary> The TrackingNumber property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."TrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TrackingNumber { get; }
        /// <summary> The Insurance property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Insurance { get; }
        /// <summary> The InsuranceValue property of the Entity DhlExpressPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressPackage"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal InsuranceValue { get; }
        
        
        IDhlExpressShipmentEntity DhlExpressShipment { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDhlExpressPackageEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDhlExpressPackageEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'DhlExpressPackage'. <br/><br/>
    /// 
    /// </summary>
    public partial class DhlExpressPackageEntity : IDhlExpressPackageEntity
    {
        
        IDhlExpressShipmentEntity IDhlExpressPackageEntity.DhlExpressShipment => DhlExpressShipment;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDhlExpressPackageEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IDhlExpressPackageEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IDhlExpressPackageEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyDhlExpressPackageEntity(this, objectMap);
        }

        
    }
}
