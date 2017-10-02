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
    /// Entity interface which represents the entity 'IParcelPackage'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IIParcelPackageEntity
    {
        
        /// <summary> The IParcelPackageID property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."iParcelPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 IParcelPackageID { get; }
        /// <summary> The ShipmentID property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The Weight property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Weight { get; }
        /// <summary> The DimsProfileID property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsHeight { get; }
        /// <summary> The DimsAddWeight property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DimsAddWeight { get; }
        /// <summary> The DimsWeight property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double DimsWeight { get; }
        /// <summary> The Insurance property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Insurance { get; }
        /// <summary> The InsuranceValue property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal InsuranceValue { get; }
        /// <summary> The InsurancePennyOne property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."InsurancePennyOne"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean InsurancePennyOne { get; }
        /// <summary> The DeclaredValue property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DeclaredValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal DeclaredValue { get; }
        /// <summary> The TrackingNumber property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."TrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TrackingNumber { get; }
        /// <summary> The ParcelNumber property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."ParcelNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ParcelNumber { get; }
        /// <summary> The SkuAndQuantities property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."SkuAndQuantities"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SkuAndQuantities { get; }
        
        
        IIParcelShipmentEntity IParcelShipment { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IIParcelPackageEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IIParcelPackageEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'IParcelPackage'. <br/><br/>
    /// 
    /// </summary>
    public partial class IParcelPackageEntity : IIParcelPackageEntity
    {
        
        IIParcelShipmentEntity IIParcelPackageEntity.IParcelShipment => IParcelShipment;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelPackageEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IIParcelPackageEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IIParcelPackageEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyIParcelPackageEntity(this, objectMap);
        }

        
    }
}
