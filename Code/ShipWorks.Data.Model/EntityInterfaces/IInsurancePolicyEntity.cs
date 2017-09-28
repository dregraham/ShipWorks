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
    /// Entity interface which represents the entity 'InsurancePolicy'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IInsurancePolicyEntity
    {
        
        /// <summary> The ShipmentID property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShipmentID { get; }
        /// <summary> The InsureShipStoreName property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."InsureShipStoreName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 75<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String InsureShipStoreName { get; }
        /// <summary> The CreatedWithApi property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."CreatedWithApi"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean CreatedWithApi { get; }
        /// <summary> The ItemName property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."ItemName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ItemName { get; }
        /// <summary> The Description property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Description { get; }
        /// <summary> The ClaimType property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."ClaimType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ClaimType { get; }
        /// <summary> The DamageValue property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."DamageValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Decimal> DamageValue { get; }
        /// <summary> The SubmissionDate property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."SubmissionDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> SubmissionDate { get; }
        /// <summary> The ClaimID property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."ClaimID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ClaimID { get; }
        /// <summary> The EmailAddress property of the Entity InsurancePolicy<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InsurancePolicy"."EmailAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String EmailAddress { get; }
        
        IShipmentEntity Shipment { get; }
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IInsurancePolicyEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IInsurancePolicyEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'InsurancePolicy'. <br/><br/>
    /// 
    /// </summary>
    public partial class InsurancePolicyEntity : IInsurancePolicyEntity
    {
        IShipmentEntity IInsurancePolicyEntity.Shipment => Shipment;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IInsurancePolicyEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IInsurancePolicyEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IInsurancePolicyEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyInsurancePolicyEntity(this, objectMap);
        }

        
    }
}
