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
    /// Entity interface which represents the entity 'InsurancePolicy'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyInsurancePolicyEntity : IInsurancePolicyEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyInsurancePolicyEntity(IInsurancePolicyEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            InsureShipStoreName = source.InsureShipStoreName;
            CreatedWithApi = source.CreatedWithApi;
            ItemName = source.ItemName;
            Description = source.Description;
            ClaimType = source.ClaimType;
            DamageValue = source.DamageValue;
            SubmissionDate = source.SubmissionDate;
            ClaimID = source.ClaimID;
            EmailAddress = source.EmailAddress;
            
            Shipment = source.Shipment?.AsReadOnly(objectMap);
            
            

            CopyCustomInsurancePolicyData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The InsureShipStoreName property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."InsureShipStoreName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 75<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InsureShipStoreName { get; }
        /// <summary> The CreatedWithApi property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."CreatedWithApi"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CreatedWithApi { get; }
        /// <summary> The ItemName property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."ItemName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ItemName { get; }
        /// <summary> The Description property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The ClaimType property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."ClaimType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ClaimType { get; }
        /// <summary> The DamageValue property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."DamageValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> DamageValue { get; }
        /// <summary> The SubmissionDate property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."SubmissionDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> SubmissionDate { get; }
        /// <summary> The ClaimID property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."ClaimID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ClaimID { get; }
        /// <summary> The EmailAddress property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."EmailAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String EmailAddress { get; }
        
        public IShipmentEntity Shipment { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IInsurancePolicyEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IInsurancePolicyEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomInsurancePolicyData(IInsurancePolicyEntity source);
    }
}
