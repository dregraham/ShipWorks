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
    /// Read-only representation of the entity 'IParcelPackage'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyIParcelPackageEntity : IIParcelPackageEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyIParcelPackageEntity(IIParcelPackageEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            IParcelPackageID = source.IParcelPackageID;
            ShipmentID = source.ShipmentID;
            Weight = source.Weight;
            DimsProfileID = source.DimsProfileID;
            DimsLength = source.DimsLength;
            DimsWidth = source.DimsWidth;
            DimsHeight = source.DimsHeight;
            DimsAddWeight = source.DimsAddWeight;
            DimsWeight = source.DimsWeight;
            Insurance = source.Insurance;
            InsuranceValue = source.InsuranceValue;
            InsurancePennyOne = source.InsurancePennyOne;
            DeclaredValue = source.DeclaredValue;
            TrackingNumber = source.TrackingNumber;
            ParcelNumber = source.ParcelNumber;
            SkuAndQuantities = source.SkuAndQuantities;
            
            
            IParcelShipment = source.IParcelShipment?.AsReadOnly(objectMap);
            

            CopyCustomIParcelPackageData(source);
        }

        
        /// <summary> The IParcelPackageID property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."iParcelPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 IParcelPackageID { get; }
        /// <summary> The ShipmentID property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The Weight property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Weight { get; }
        /// <summary> The DimsProfileID property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DimsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DimsProfileID { get; }
        /// <summary> The DimsLength property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DimsLength"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsLength { get; }
        /// <summary> The DimsWidth property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DimsWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWidth { get; }
        /// <summary> The DimsHeight property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DimsHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsHeight { get; }
        /// <summary> The DimsAddWeight property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DimsAddWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DimsAddWeight { get; }
        /// <summary> The DimsWeight property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DimsWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double DimsWeight { get; }
        /// <summary> The Insurance property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Insurance { get; }
        /// <summary> The InsuranceValue property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."InsuranceValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal InsuranceValue { get; }
        /// <summary> The InsurancePennyOne property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."InsurancePennyOne"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean InsurancePennyOne { get; }
        /// <summary> The DeclaredValue property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."DeclaredValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal DeclaredValue { get; }
        /// <summary> The TrackingNumber property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."TrackingNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TrackingNumber { get; }
        /// <summary> The ParcelNumber property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."ParcelNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ParcelNumber { get; }
        /// <summary> The SkuAndQuantities property of the Entity IParcelPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelPackage"."SkuAndQuantities"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SkuAndQuantities { get; }
        
        
        public IIParcelShipmentEntity IParcelShipment { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelPackageEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelPackageEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomIParcelPackageData(IIParcelPackageEntity source);
    }
}
