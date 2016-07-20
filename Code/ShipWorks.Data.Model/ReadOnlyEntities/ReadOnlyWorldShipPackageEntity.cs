///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
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
    /// Entity interface which represents the entity 'WorldShipPackage'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyWorldShipPackageEntity : IWorldShipPackageEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyWorldShipPackageEntity(IWorldShipPackageEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UpsPackageID = source.UpsPackageID;
            ShipmentID = source.ShipmentID;
            PackageType = source.PackageType;
            Weight = source.Weight;
            ReferenceNumber = source.ReferenceNumber;
            ReferenceNumber2 = source.ReferenceNumber2;
            CodOption = source.CodOption;
            CodAmount = source.CodAmount;
            CodCashOnly = source.CodCashOnly;
            DeliveryConfirmation = source.DeliveryConfirmation;
            DeliveryConfirmationSignature = source.DeliveryConfirmationSignature;
            DeliveryConfirmationAdult = source.DeliveryConfirmationAdult;
            Length = source.Length;
            Width = source.Width;
            Height = source.Height;
            DeclaredValueAmount = source.DeclaredValueAmount;
            DeclaredValueOption = source.DeclaredValueOption;
            CN22GoodsType = source.CN22GoodsType;
            CN22Description = source.CN22Description;
            PostalSubClass = source.PostalSubClass;
            MIDeliveryConfirmation = source.MIDeliveryConfirmation;
            QvnOption = source.QvnOption;
            QvnFrom = source.QvnFrom;
            QvnSubjectLine = source.QvnSubjectLine;
            QvnMemo = source.QvnMemo;
            Qvn1ShipNotify = source.Qvn1ShipNotify;
            Qvn1ContactName = source.Qvn1ContactName;
            Qvn1Email = source.Qvn1Email;
            Qvn2ShipNotify = source.Qvn2ShipNotify;
            Qvn2ContactName = source.Qvn2ContactName;
            Qvn2Email = source.Qvn2Email;
            Qvn3ShipNotify = source.Qvn3ShipNotify;
            Qvn3ContactName = source.Qvn3ContactName;
            Qvn3Email = source.Qvn3Email;
            ShipperRelease = source.ShipperRelease;
            AdditionalHandlingEnabled = source.AdditionalHandlingEnabled;
            VerbalConfirmationOption = source.VerbalConfirmationOption;
            VerbalConfirmationContactName = source.VerbalConfirmationContactName;
            VerbalConfirmationTelephone = source.VerbalConfirmationTelephone;
            DryIceRegulationSet = source.DryIceRegulationSet;
            DryIceWeight = source.DryIceWeight;
            DryIceMedicalPurpose = source.DryIceMedicalPurpose;
            DryIceOption = source.DryIceOption;
            DryIceWeightUnitOfMeasure = source.DryIceWeightUnitOfMeasure;
            
            
            WorldShipShipment = source.WorldShipShipment?.AsReadOnly(objectMap);
            

            CopyCustomWorldShipPackageData(source);
        }

        
        /// <summary> The UpsPackageID property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."UpsPackageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 UpsPackageID { get; }
        /// <summary> The ShipmentID property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The PackageType property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."PackageType"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PackageType { get; }
        /// <summary> The Weight property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Weight { get; }
        /// <summary> The ReferenceNumber property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."ReferenceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReferenceNumber { get; }
        /// <summary> The ReferenceNumber2 property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."ReferenceNumber2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ReferenceNumber2 { get; }
        /// <summary> The CodOption property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."CodOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodOption { get; }
        /// <summary> The CodAmount property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."CodAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal CodAmount { get; }
        /// <summary> The CodCashOnly property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."CodCashOnly"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CodCashOnly { get; }
        /// <summary> The DeliveryConfirmation property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DeliveryConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DeliveryConfirmation { get; }
        /// <summary> The DeliveryConfirmationSignature property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DeliveryConfirmationSignature"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DeliveryConfirmationSignature { get; }
        /// <summary> The DeliveryConfirmationAdult property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DeliveryConfirmationAdult"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DeliveryConfirmationAdult { get; }
        /// <summary> The Length property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Length"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Length { get; }
        /// <summary> The Width property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Width"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Width { get; }
        /// <summary> The Height property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Height"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Height { get; }
        /// <summary> The DeclaredValueAmount property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DeclaredValueAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DeclaredValueAmount { get; }
        /// <summary> The DeclaredValueOption property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DeclaredValueOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): NChar, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String DeclaredValueOption { get; }
        /// <summary> The CN22GoodsType property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."CN22GoodsType"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CN22GoodsType { get; }
        /// <summary> The CN22Description property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."CN22Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CN22Description { get; }
        /// <summary> The PostalSubClass property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."PostalSubClass"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String PostalSubClass { get; }
        /// <summary> The MIDeliveryConfirmation property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."MIDeliveryConfirmation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String MIDeliveryConfirmation { get; }
        /// <summary> The QvnOption property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."QvnOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String QvnOption { get; }
        /// <summary> The QvnFrom property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."QvnFrom"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String QvnFrom { get; }
        /// <summary> The QvnSubjectLine property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."QvnSubjectLine"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 18<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String QvnSubjectLine { get; }
        /// <summary> The QvnMemo property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."QvnMemo"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String QvnMemo { get; }
        /// <summary> The Qvn1ShipNotify property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn1ShipNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Qvn1ShipNotify { get; }
        /// <summary> The Qvn1ContactName property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn1ContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Qvn1ContactName { get; }
        /// <summary> The Qvn1Email property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn1Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Qvn1Email { get; }
        /// <summary> The Qvn2ShipNotify property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn2ShipNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Qvn2ShipNotify { get; }
        /// <summary> The Qvn2ContactName property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn2ContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Qvn2ContactName { get; }
        /// <summary> The Qvn2Email property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn2Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Qvn2Email { get; }
        /// <summary> The Qvn3ShipNotify property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn3ShipNotify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Qvn3ShipNotify { get; }
        /// <summary> The Qvn3ContactName property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn3ContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Qvn3ContactName { get; }
        /// <summary> The Qvn3Email property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."Qvn3Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Qvn3Email { get; }
        /// <summary> The ShipperRelease property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."ShipperRelease"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ShipperRelease { get; }
        /// <summary> The AdditionalHandlingEnabled property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."AdditionalHandlingEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String AdditionalHandlingEnabled { get; }
        /// <summary> The VerbalConfirmationOption property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."VerbalConfirmationOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String VerbalConfirmationOption { get; }
        /// <summary> The VerbalConfirmationContactName property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."VerbalConfirmationContactName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String VerbalConfirmationContactName { get; }
        /// <summary> The VerbalConfirmationTelephone property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."VerbalConfirmationTelephone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String VerbalConfirmationTelephone { get; }
        /// <summary> The DryIceRegulationSet property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DryIceRegulationSet"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 5<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String DryIceRegulationSet { get; }
        /// <summary> The DryIceWeight property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DryIceWeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Double> DryIceWeight { get; }
        /// <summary> The DryIceMedicalPurpose property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DryIceMedicalPurpose"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String DryIceMedicalPurpose { get; }
        /// <summary> The DryIceOption property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DryIceOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String DryIceOption { get; }
        /// <summary> The DryIceWeightUnitOfMeasure property of the Entity WorldShipPackage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WorldShipPackage"."DryIceWeightUnitOfMeasure"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String DryIceWeightUnitOfMeasure { get; }
        
        
        public IWorldShipShipmentEntity WorldShipShipment { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWorldShipPackageEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IWorldShipPackageEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomWorldShipPackageData(IWorldShipPackageEntity source);
    }
}
