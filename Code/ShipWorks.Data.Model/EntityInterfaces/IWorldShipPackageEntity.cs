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
    /// Entity interface which represents the entity 'WorldShipPackage'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IWorldShipPackageEntity
    {
        
        /// <summary> The UpsPackageID property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."UpsPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 UpsPackageID { get; }
        /// <summary> The ShipmentID property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The PackageType property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."PackageType"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PackageType { get; }
        /// <summary> The Weight property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Weight { get; }
        /// <summary> The ReferenceNumber property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."ReferenceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ReferenceNumber { get; }
        /// <summary> The ReferenceNumber2 property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."ReferenceNumber2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ReferenceNumber2 { get; }
        /// <summary> The CodOption property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."CodOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CodOption { get; }
        /// <summary> The CodAmount property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."CodAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal CodAmount { get; }
        /// <summary> The CodCashOnly property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."CodCashOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CodCashOnly { get; }
        /// <summary> The DeliveryConfirmation property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DeliveryConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DeliveryConfirmation { get; }
        /// <summary> The DeliveryConfirmationSignature property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DeliveryConfirmationSignature"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DeliveryConfirmationSignature { get; }
        /// <summary> The DeliveryConfirmationAdult property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DeliveryConfirmationAdult"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DeliveryConfirmationAdult { get; }
        /// <summary> The Length property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Length"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Length { get; }
        /// <summary> The Width property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Width"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Width { get; }
        /// <summary> The Height property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Height"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Height { get; }
        /// <summary> The DeclaredValueAmount property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DeclaredValueAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DeclaredValueAmount { get; }
        /// <summary> The DeclaredValueOption property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DeclaredValueOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): NChar, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String DeclaredValueOption { get; }
        /// <summary> The CN22GoodsType property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."CN22GoodsType"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String CN22GoodsType { get; }
        /// <summary> The CN22Description property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."CN22Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String CN22Description { get; }
        /// <summary> The PostalSubClass property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."PostalSubClass"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String PostalSubClass { get; }
        /// <summary> The MIDeliveryConfirmation property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."MIDeliveryConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String MIDeliveryConfirmation { get; }
        /// <summary> The QvnOption property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."QvnOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String QvnOption { get; }
        /// <summary> The QvnFrom property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."QvnFrom"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String QvnFrom { get; }
        /// <summary> The QvnSubjectLine property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."QvnSubjectLine"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 18<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String QvnSubjectLine { get; }
        /// <summary> The QvnMemo property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."QvnMemo"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String QvnMemo { get; }
        /// <summary> The Qvn1ShipNotify property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn1ShipNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Qvn1ShipNotify { get; }
        /// <summary> The Qvn1ContactName property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn1ContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Qvn1ContactName { get; }
        /// <summary> The Qvn1Email property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn1Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Qvn1Email { get; }
        /// <summary> The Qvn2ShipNotify property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn2ShipNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Qvn2ShipNotify { get; }
        /// <summary> The Qvn2ContactName property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn2ContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Qvn2ContactName { get; }
        /// <summary> The Qvn2Email property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn2Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Qvn2Email { get; }
        /// <summary> The Qvn3ShipNotify property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn3ShipNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Qvn3ShipNotify { get; }
        /// <summary> The Qvn3ContactName property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn3ContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Qvn3ContactName { get; }
        /// <summary> The Qvn3Email property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn3Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Qvn3Email { get; }
        /// <summary> The ShipperRelease property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."ShipperRelease"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ShipperRelease { get; }
        /// <summary> The AdditionalHandlingEnabled property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."AdditionalHandlingEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String AdditionalHandlingEnabled { get; }
        /// <summary> The VerbalConfirmationOption property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."VerbalConfirmationOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String VerbalConfirmationOption { get; }
        /// <summary> The VerbalConfirmationContactName property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."VerbalConfirmationContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String VerbalConfirmationContactName { get; }
        /// <summary> The VerbalConfirmationTelephone property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."VerbalConfirmationTelephone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String VerbalConfirmationTelephone { get; }
        /// <summary> The DryIceRegulationSet property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DryIceRegulationSet"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 5<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String DryIceRegulationSet { get; }
        /// <summary> The DryIceWeight property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DryIceWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Double> DryIceWeight { get; }
        /// <summary> The DryIceMedicalPurpose property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DryIceMedicalPurpose"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String DryIceMedicalPurpose { get; }
        /// <summary> The DryIceOption property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DryIceOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String DryIceOption { get; }
        /// <summary> The DryIceWeightUnitOfMeasure property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DryIceWeightUnitOfMeasure"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String DryIceWeightUnitOfMeasure { get; }
        
        
        IWorldShipShipmentEntity WorldShipShipment { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IWorldShipPackageEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IWorldShipPackageEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'WorldShipPackage'. <br/><br/>
    /// 
    /// </summary>
    public partial class WorldShipPackageEntity : IWorldShipPackageEntity
    {
        
        IWorldShipShipmentEntity IWorldShipPackageEntity.WorldShipShipment => WorldShipShipment;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWorldShipPackageEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IWorldShipPackageEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IWorldShipPackageEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyWorldShipPackageEntity(this, objectMap);
        }
    }
}
