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
    /// Read-only representation of the entity 'UpsLocalRateSurcharge'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUpsLocalRateSurchargeEntity : IUpsLocalRateSurchargeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUpsLocalRateSurchargeEntity(IUpsLocalRateSurchargeEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UpsLocalRateSurchargeID = source.UpsLocalRateSurchargeID;
            UpsRateTableID = source.UpsRateTableID;
            SurchargeType = source.SurchargeType;
            Value = source.Value;
            
            
            UpsRateTable = source.UpsRateTable?.AsReadOnly(objectMap);
            

            CopyCustomUpsLocalRateSurchargeData(source);
        }

        
        /// <summary> The UpsLocalRateSurchargeID property of the Entity UpsLocalRateSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateSurcharge"."UpsRateSurchargeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 UpsLocalRateSurchargeID { get; }
        /// <summary> The UpsRateTableID property of the Entity UpsLocalRateSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateSurcharge"."UpsRateTableID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UpsRateTableID { get; }
        /// <summary> The SurchargeType property of the Entity UpsLocalRateSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateSurcharge"."SurchargeType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 SurchargeType { get; }
        /// <summary> The Value property of the Entity UpsLocalRateSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateSurcharge"."Amount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Value { get; }
        
        
        public IUpsRateTableEntity UpsRateTable { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLocalRateSurchargeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLocalRateSurchargeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUpsLocalRateSurchargeData(IUpsLocalRateSurchargeEntity source);
    }
}
