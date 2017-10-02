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
    /// Read-only representation of the entity 'ShippingSettings'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShippingSettingsEntity : IShippingSettingsEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShippingSettingsEntity(IShippingSettingsEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingSettingsID = source.ShippingSettingsID;
            InternalActivated = source.InternalActivated;
            InternalConfigured = source.InternalConfigured;
            InternalExcluded = source.InternalExcluded;
            DefaultType = source.DefaultType;
            BlankPhoneOption = source.BlankPhoneOption;
            BlankPhoneNumber = source.BlankPhoneNumber;
            InsurancePolicy = source.InsurancePolicy;
            InsuranceLastAgreed = source.InsuranceLastAgreed;
            FedExUsername = source.FedExUsername;
            FedExPassword = source.FedExPassword;
            FedExMaskAccount = source.FedExMaskAccount;
            FedExThermalDocTab = source.FedExThermalDocTab;
            FedExThermalDocTabType = source.FedExThermalDocTabType;
            FedExInsuranceProvider = source.FedExInsuranceProvider;
            FedExInsurancePennyOne = source.FedExInsurancePennyOne;
            UpsAccessKey = source.UpsAccessKey;
            UpsInsuranceProvider = source.UpsInsuranceProvider;
            UpsInsurancePennyOne = source.UpsInsurancePennyOne;
            EndiciaCustomsCertify = source.EndiciaCustomsCertify;
            EndiciaCustomsSigner = source.EndiciaCustomsSigner;
            EndiciaThermalDocTab = source.EndiciaThermalDocTab;
            EndiciaThermalDocTabType = source.EndiciaThermalDocTabType;
            EndiciaAutomaticExpress1 = source.EndiciaAutomaticExpress1;
            EndiciaAutomaticExpress1Account = source.EndiciaAutomaticExpress1Account;
            EndiciaInsuranceProvider = source.EndiciaInsuranceProvider;
            WorldShipLaunch = source.WorldShipLaunch;
            UspsAutomaticExpress1 = source.UspsAutomaticExpress1;
            UspsAutomaticExpress1Account = source.UspsAutomaticExpress1Account;
            UspsInsuranceProvider = source.UspsInsuranceProvider;
            Express1EndiciaCustomsCertify = source.Express1EndiciaCustomsCertify;
            Express1EndiciaCustomsSigner = source.Express1EndiciaCustomsSigner;
            Express1EndiciaThermalDocTab = source.Express1EndiciaThermalDocTab;
            Express1EndiciaThermalDocTabType = source.Express1EndiciaThermalDocTabType;
            Express1EndiciaSingleSource = source.Express1EndiciaSingleSource;
            OnTracInsuranceProvider = source.OnTracInsuranceProvider;
            OnTracInsurancePennyOne = source.OnTracInsurancePennyOne;
            IParcelInsuranceProvider = source.IParcelInsuranceProvider;
            IParcelInsurancePennyOne = source.IParcelInsurancePennyOne;
            Express1UspsSingleSource = source.Express1UspsSingleSource;
            UpsMailInnovationsEnabled = source.UpsMailInnovationsEnabled;
            WorldShipMailInnovationsEnabled = source.WorldShipMailInnovationsEnabled;
            InternalBestRateExcludedShipmentTypes = source.InternalBestRateExcludedShipmentTypes;
            ShipSenseEnabled = source.ShipSenseEnabled;
            ShipSenseUniquenessXml = source.ShipSenseUniquenessXml;
            ShipSenseProcessedShipmentID = source.ShipSenseProcessedShipmentID;
            ShipSenseEndShipmentID = source.ShipSenseEndShipmentID;
            AutoCreateShipments = source.AutoCreateShipments;
            FedExFimsEnabled = source.FedExFimsEnabled;
            FedExFimsUsername = source.FedExFimsUsername;
            FedExFimsPassword = source.FedExFimsPassword;
            ShipmentEditLimit = source.ShipmentEditLimit;
            ShipmentsLoaderEnsureFiltersLoadedTimeout = source.ShipmentsLoaderEnsureFiltersLoadedTimeout;
            ShipEngineApiKey = source.ShipEngineApiKey;
            
            
            

            CopyCustomShippingSettingsData(source);
        }

        
        /// <summary> The ShippingSettingsID property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."ShippingSettingsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Boolean ShippingSettingsID { get; }
        /// <summary> The InternalActivated property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."Activated"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 45<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InternalActivated { get; }
        /// <summary> The InternalConfigured property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."Configured"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 45<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InternalConfigured { get; }
        /// <summary> The InternalExcluded property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."Excluded"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 45<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InternalExcluded { get; }
        /// <summary> The DefaultType property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."DefaultType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DefaultType { get; }
        /// <summary> The BlankPhoneOption property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."BlankPhoneOption"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 BlankPhoneOption { get; }
        /// <summary> The BlankPhoneNumber property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."BlankPhoneNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BlankPhoneNumber { get; }
        /// <summary> The InsurancePolicy property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."InsurancePolicy"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 40<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InsurancePolicy { get; }
        /// <summary> The InsuranceLastAgreed property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."InsuranceLastAgreed"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> InsuranceLastAgreed { get; }
        /// <summary> The FedExUsername property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."FedExUsername"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String FedExUsername { get; }
        /// <summary> The FedExPassword property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."FedExPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String FedExPassword { get; }
        /// <summary> The FedExMaskAccount property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."FedExMaskAccount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean FedExMaskAccount { get; }
        /// <summary> The FedExThermalDocTab property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."FedExThermalDocTab"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean FedExThermalDocTab { get; }
        /// <summary> The FedExThermalDocTabType property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."FedExThermalDocTabType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FedExThermalDocTabType { get; }
        /// <summary> The FedExInsuranceProvider property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."FedExInsuranceProvider"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FedExInsuranceProvider { get; }
        /// <summary> The FedExInsurancePennyOne property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."FedExInsurancePennyOne"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean FedExInsurancePennyOne { get; }
        /// <summary> The UpsAccessKey property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."UpsAccessKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String UpsAccessKey { get; }
        /// <summary> The UpsInsuranceProvider property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."UpsInsuranceProvider"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 UpsInsuranceProvider { get; }
        /// <summary> The UpsInsurancePennyOne property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."UpsInsurancePennyOne"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean UpsInsurancePennyOne { get; }
        /// <summary> The EndiciaCustomsCertify property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."EndiciaCustomsCertify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean EndiciaCustomsCertify { get; }
        /// <summary> The EndiciaCustomsSigner property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."EndiciaCustomsSigner"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EndiciaCustomsSigner { get; }
        /// <summary> The EndiciaThermalDocTab property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."EndiciaThermalDocTab"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean EndiciaThermalDocTab { get; }
        /// <summary> The EndiciaThermalDocTabType property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."EndiciaThermalDocTabType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EndiciaThermalDocTabType { get; }
        /// <summary> The EndiciaAutomaticExpress1 property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."EndiciaAutomaticExpress1"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean EndiciaAutomaticExpress1 { get; }
        /// <summary> The EndiciaAutomaticExpress1Account property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."EndiciaAutomaticExpress1Account"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EndiciaAutomaticExpress1Account { get; }
        /// <summary> The EndiciaInsuranceProvider property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."EndiciaInsuranceProvider"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 EndiciaInsuranceProvider { get; }
        /// <summary> The WorldShipLaunch property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."WorldShipLaunch"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean WorldShipLaunch { get; }
        /// <summary> The UspsAutomaticExpress1 property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."UspsAutomaticExpress1"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean UspsAutomaticExpress1 { get; }
        /// <summary> The UspsAutomaticExpress1Account property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."UspsAutomaticExpress1Account"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UspsAutomaticExpress1Account { get; }
        /// <summary> The UspsInsuranceProvider property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."UspsInsuranceProvider"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 UspsInsuranceProvider { get; }
        /// <summary> The Express1EndiciaCustomsCertify property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."Express1EndiciaCustomsCertify"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Express1EndiciaCustomsCertify { get; }
        /// <summary> The Express1EndiciaCustomsSigner property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."Express1EndiciaCustomsSigner"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Express1EndiciaCustomsSigner { get; }
        /// <summary> The Express1EndiciaThermalDocTab property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."Express1EndiciaThermalDocTab"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Express1EndiciaThermalDocTab { get; }
        /// <summary> The Express1EndiciaThermalDocTabType property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."Express1EndiciaThermalDocTabType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Express1EndiciaThermalDocTabType { get; }
        /// <summary> The Express1EndiciaSingleSource property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."Express1EndiciaSingleSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Express1EndiciaSingleSource { get; }
        /// <summary> The OnTracInsuranceProvider property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."OnTracInsuranceProvider"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OnTracInsuranceProvider { get; }
        /// <summary> The OnTracInsurancePennyOne property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."OnTracInsurancePennyOne"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean OnTracInsurancePennyOne { get; }
        /// <summary> The IParcelInsuranceProvider property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."iParcelInsuranceProvider"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 IParcelInsuranceProvider { get; }
        /// <summary> The IParcelInsurancePennyOne property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."iParcelInsurancePennyOne"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IParcelInsurancePennyOne { get; }
        /// <summary> The Express1UspsSingleSource property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."Express1UspsSingleSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Express1UspsSingleSource { get; }
        /// <summary> The UpsMailInnovationsEnabled property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."UpsMailInnovationsEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean UpsMailInnovationsEnabled { get; }
        /// <summary> The WorldShipMailInnovationsEnabled property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."WorldShipMailInnovationsEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean WorldShipMailInnovationsEnabled { get; }
        /// <summary> The InternalBestRateExcludedShipmentTypes property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."BestRateExcludedShipmentTypes"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InternalBestRateExcludedShipmentTypes { get; }
        /// <summary> The ShipSenseEnabled property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."ShipSenseEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ShipSenseEnabled { get; }
        /// <summary> The ShipSenseUniquenessXml property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."ShipSenseUniquenessXml"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipSenseUniquenessXml { get; }
        /// <summary> The ShipSenseProcessedShipmentID property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."ShipSenseProcessedShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShipSenseProcessedShipmentID { get; }
        /// <summary> The ShipSenseEndShipmentID property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."ShipSenseEndShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShipSenseEndShipmentID { get; }
        /// <summary> The AutoCreateShipments property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."AutoCreateShipments"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean AutoCreateShipments { get; }
        /// <summary> The FedExFimsEnabled property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."FedExFimsEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean FedExFimsEnabled { get; }
        /// <summary> The FedExFimsUsername property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."FedExFimsUsername"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FedExFimsUsername { get; }
        /// <summary> The FedExFimsPassword property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."FedExFimsPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FedExFimsPassword { get; }
        /// <summary> The ShipmentEditLimit property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."ShipmentEditLimit"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipmentEditLimit { get; }
        /// <summary> The ShipmentsLoaderEnsureFiltersLoadedTimeout property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."ShipmentsLoaderEnsureFiltersLoadedTimeout"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipmentsLoaderEnsureFiltersLoadedTimeout { get; }
        /// <summary> The ShipEngineApiKey property of the Entity ShippingSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingSettings"."ShipEngineApiKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipEngineApiKey { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingSettingsEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingSettingsEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShippingSettingsData(IShippingSettingsEntity source);
    }
}
