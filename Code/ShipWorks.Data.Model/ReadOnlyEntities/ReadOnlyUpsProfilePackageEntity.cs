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
    /// Read-only representation of the entity 'UpsProfilePackage'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUpsProfilePackageEntity : ReadOnlyPackageProfileEntity, IUpsProfilePackageEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUpsProfilePackageEntity(IUpsProfilePackageEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            PackagingType = source.PackagingType;
            AdditionalHandlingEnabled = source.AdditionalHandlingEnabled;
            VerbalConfirmationEnabled = source.VerbalConfirmationEnabled;
            VerbalConfirmationName = source.VerbalConfirmationName;
            VerbalConfirmationPhone = source.VerbalConfirmationPhone;
            VerbalConfirmationPhoneExtension = source.VerbalConfirmationPhoneExtension;
            DryIceEnabled = source.DryIceEnabled;
            DryIceRegulationSet = source.DryIceRegulationSet;
            DryIceWeight = source.DryIceWeight;
            DryIceIsForMedicalUse = source.DryIceIsForMedicalUse;
            
            
            UpsProfile = (IUpsProfileEntity) source.UpsProfile?.AsReadOnly(objectMap);
            

            CopyCustomUpsProfilePackageData(source);
        }

        
        /// <summary> The PackagingType property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> PackagingType { get; }
        /// <summary> The AdditionalHandlingEnabled property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."AdditionalHandlingEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> AdditionalHandlingEnabled { get; }
        /// <summary> The VerbalConfirmationEnabled property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."VerbalConfirmationEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> VerbalConfirmationEnabled { get; }
        /// <summary> The VerbalConfirmationName property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."VerbalConfirmationName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String VerbalConfirmationName { get; }
        /// <summary> The VerbalConfirmationPhone property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."VerbalConfirmationPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String VerbalConfirmationPhone { get; }
        /// <summary> The VerbalConfirmationPhoneExtension property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."VerbalConfirmationPhoneExtension"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 4<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String VerbalConfirmationPhoneExtension { get; }
        /// <summary> The DryIceEnabled property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DryIceEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> DryIceEnabled { get; }
        /// <summary> The DryIceRegulationSet property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DryIceRegulationSet"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> DryIceRegulationSet { get; }
        /// <summary> The DryIceWeight property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DryIceWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DryIceWeight { get; }
        /// <summary> The DryIceIsForMedicalUse property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DryIceIsForMedicalUse"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> DryIceIsForMedicalUse { get; }
        
        
        public IUpsProfileEntity UpsProfile { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IPackageProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IPackageProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IUpsProfilePackageEntity AsReadOnlyUpsProfilePackage() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IUpsProfilePackageEntity AsReadOnlyUpsProfilePackage(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUpsProfilePackageData(IUpsProfilePackageEntity source);
    }
}
