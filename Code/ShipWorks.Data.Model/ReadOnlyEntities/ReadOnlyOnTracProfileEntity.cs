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
    /// Read-only representation of the entity 'OnTracProfile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOnTracProfileEntity : IOnTracProfileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOnTracProfileEntity(IOnTracProfileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingProfileID = source.ShippingProfileID;
            OnTracAccountID = source.OnTracAccountID;
            ResidentialDetermination = source.ResidentialDetermination;
            Service = source.Service;
            SaturdayDelivery = source.SaturdayDelivery;
            SignatureRequired = source.SignatureRequired;
            PackagingType = source.PackagingType;
            Reference1 = source.Reference1;
            Reference2 = source.Reference2;
            Instructions = source.Instructions;
            
            ShippingProfile = (IShippingProfileEntity) source.ShippingProfile?.AsReadOnly(objectMap);
            
            

            CopyCustomOnTracProfileData(source);
        }

        
        /// <summary> The ShippingProfileID property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The OnTracAccountID property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."OnTracAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> OnTracAccountID { get; }
        /// <summary> The ResidentialDetermination property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."ResidentialDetermination"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ResidentialDetermination { get; }
        /// <summary> The Service property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> Service { get; }
        /// <summary> The SaturdayDelivery property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."SaturdayDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> SaturdayDelivery { get; }
        /// <summary> The SignatureRequired property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."SignatureRequired"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> SignatureRequired { get; }
        /// <summary> The PackagingType property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."PackagingType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> PackagingType { get; }
        /// <summary> The Reference1 property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."Reference1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Reference1 { get; }
        /// <summary> The Reference2 property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."Reference2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Reference2 { get; }
        /// <summary> The Instructions property of the Entity OnTracProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OnTracProfile"."Instructions"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Instructions { get; }
        
        public IShippingProfileEntity ShippingProfile { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOnTracProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOnTracProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOnTracProfileData(IOnTracProfileEntity source);
    }
}
