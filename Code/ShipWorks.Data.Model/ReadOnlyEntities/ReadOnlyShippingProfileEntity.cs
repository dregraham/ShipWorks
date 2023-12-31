﻿///////////////////////////////////////////////////////////////
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
    /// Read-only representation of the entity 'ShippingProfile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShippingProfileEntity : IShippingProfileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShippingProfileEntity(IShippingProfileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingProfileID = source.ShippingProfileID;
            RowVersion = source.RowVersion;
            Name = source.Name;
            ShipmentType = source.ShipmentType;
            ShipmentTypePrimary = source.ShipmentTypePrimary;
            OriginID = source.OriginID;
            Insurance = source.Insurance;
            InsuranceInitialValueSource = source.InsuranceInitialValueSource;
            InsuranceInitialValueAmount = source.InsuranceInitialValueAmount;
            ReturnShipment = source.ReturnShipment;
            RequestedLabelFormat = source.RequestedLabelFormat;
            IncludeReturn = source.IncludeReturn;
            ApplyReturnProfile = source.ApplyReturnProfile;
            ReturnProfileID = source.ReturnProfileID;
            
            AmazonSFP = (IAmazonSFPProfileEntity) source.AmazonSFP?.AsReadOnly(objectMap);
            AmazonSWA = (IAmazonSWAProfileEntity) source.AmazonSWA?.AsReadOnly(objectMap);
            Asendia = (IAsendiaProfileEntity) source.Asendia?.AsReadOnly(objectMap);
            BestRate = (IBestRateProfileEntity) source.BestRate?.AsReadOnly(objectMap);
            DhlEcommerce = (IDhlEcommerceProfileEntity) source.DhlEcommerce?.AsReadOnly(objectMap);
            DhlExpress = (IDhlExpressProfileEntity) source.DhlExpress?.AsReadOnly(objectMap);
            FedEx = (IFedExProfileEntity) source.FedEx?.AsReadOnly(objectMap);
            IParcel = (IIParcelProfileEntity) source.IParcel?.AsReadOnly(objectMap);
            OnTrac = (IOnTracProfileEntity) source.OnTrac?.AsReadOnly(objectMap);
            Other = (IOtherProfileEntity) source.Other?.AsReadOnly(objectMap);
            Postal = (IPostalProfileEntity) source.Postal?.AsReadOnly(objectMap);
            Ups = (IUpsProfileEntity) source.Ups?.AsReadOnly(objectMap);
            
            
            Packages = source.Packages?.Select(x => x.AsReadOnly(objectMap)).OfType<IPackageProfileEntity>().ToReadOnly() ??
                Enumerable.Empty<IPackageProfileEntity>();

            CopyCustomShippingProfileData(source);
        }

        
        /// <summary> The ShippingProfileID property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The RowVersion property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The Name property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Name { get; }
        /// <summary> The ShipmentType property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<ShipWorks.Shipping.ShipmentTypeCode> ShipmentType { get; }
        /// <summary> The ShipmentTypePrimary property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."ShipmentTypePrimary"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ShipmentTypePrimary { get; }
        /// <summary> The OriginID property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."OriginID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> OriginID { get; }
        /// <summary> The Insurance property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."Insurance"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> Insurance { get; }
        /// <summary> The InsuranceInitialValueSource property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."InsuranceInitialValueSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> InsuranceInitialValueSource { get; }
        /// <summary> The InsuranceInitialValueAmount property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."InsuranceInitialValueAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> InsuranceInitialValueAmount { get; }
        /// <summary> The ReturnShipment property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."ReturnShipment"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> ReturnShipment { get; }
        /// <summary> The RequestedLabelFormat property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> RequestedLabelFormat { get; }
        /// <summary> The IncludeReturn property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."IncludeReturn"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> IncludeReturn { get; }
        /// <summary> The ApplyReturnProfile property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."ApplyReturnProfile"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> ApplyReturnProfile { get; }
        /// <summary> The ReturnProfileID property of the Entity ShippingProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProfile"."ReturnProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ReturnProfileID { get; }
        
        public IAmazonSFPProfileEntity AmazonSFP { get; }
        
        public IAmazonSWAProfileEntity AmazonSWA { get; }
        
        public IAsendiaProfileEntity Asendia { get; }
        
        public IBestRateProfileEntity BestRate { get; }
        
        public IDhlEcommerceProfileEntity DhlEcommerce { get; }
        
        public IDhlExpressProfileEntity DhlExpress { get; }
        
        public IFedExProfileEntity FedEx { get; }
        
        public IIParcelProfileEntity IParcel { get; }
        
        public IOnTracProfileEntity OnTrac { get; }
        
        public IOtherProfileEntity Other { get; }
        
        public IPostalProfileEntity Postal { get; }
        
        public IUpsProfileEntity Ups { get; }
        
        
        
        public IEnumerable<IPackageProfileEntity> Packages { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShippingProfileData(IShippingProfileEntity source);
    }
}
