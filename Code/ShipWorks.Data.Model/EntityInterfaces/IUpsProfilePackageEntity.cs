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
    /// Entity interface which represents the entity 'UpsProfilePackage'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsProfilePackageEntity: IPackageProfileEntity
    {
        
        /// <summary> The PackagingType property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> PackagingType { get; }
        /// <summary> The AdditionalHandlingEnabled property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."AdditionalHandlingEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> AdditionalHandlingEnabled { get; }
        /// <summary> The VerbalConfirmationEnabled property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."VerbalConfirmationEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> VerbalConfirmationEnabled { get; }
        /// <summary> The VerbalConfirmationName property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."VerbalConfirmationName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String VerbalConfirmationName { get; }
        /// <summary> The VerbalConfirmationPhone property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."VerbalConfirmationPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String VerbalConfirmationPhone { get; }
        /// <summary> The VerbalConfirmationPhoneExtension property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."VerbalConfirmationPhoneExtension"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 4<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String VerbalConfirmationPhoneExtension { get; }
        /// <summary> The DryIceEnabled property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DryIceEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DryIceEnabled { get; }
        /// <summary> The DryIceRegulationSet property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DryIceRegulationSet"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> DryIceRegulationSet { get; }
        /// <summary> The DryIceWeight property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DryIceWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DryIceWeight { get; }
        /// <summary> The DryIceIsForMedicalUse property of the Entity UpsProfilePackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsProfilePackage"."DryIceIsForMedicalUse"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> DryIceIsForMedicalUse { get; }
        
        
        IUpsProfileEntity UpsProfile { get; }
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsProfilePackageEntity AsReadOnlyUpsProfilePackage();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsProfilePackageEntity AsReadOnlyUpsProfilePackage(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsProfilePackage'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsProfilePackageEntity : IUpsProfilePackageEntity
    {
        
        IUpsProfileEntity IUpsProfilePackageEntity.UpsProfile => UpsProfile;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IPackageProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IPackageProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsProfilePackageEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsProfilePackageEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IUpsProfilePackageEntity AsReadOnlyUpsProfilePackage() =>
            (IUpsProfilePackageEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IUpsProfilePackageEntity AsReadOnlyUpsProfilePackage(IDictionary<object, object> objectMap) =>
            (IUpsProfilePackageEntity) AsReadOnly(objectMap);
        
    }
}
