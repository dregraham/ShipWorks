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
    /// Entity interface which represents the entity 'UpsRateSurcharge'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsRateSurchargeEntity
    {
        
        /// <summary> The UpsRateSurchargeID property of the Entity UpsRateSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateSurcharge"."UpsRateSurchargeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 UpsRateSurchargeID { get; }
        /// <summary> The UpsRateTableID property of the Entity UpsRateSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateSurcharge"."UpsRateTableID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UpsRateTableID { get; }
        /// <summary> The SurchargeType property of the Entity UpsRateSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateSurcharge"."SurchargeType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 SurchargeType { get; }
        /// <summary> The Amount property of the Entity UpsRateSurcharge<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRateSurcharge"."Amount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Amount { get; }
        
        
        IUpsRateTableEntity UpsRateTable { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsRateSurchargeEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsRateSurchargeEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsRateSurcharge'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsRateSurchargeEntity : IUpsRateSurchargeEntity
    {
        
        IUpsRateTableEntity IUpsRateSurchargeEntity.UpsRateTable => UpsRateTable;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsRateSurchargeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUpsRateSurchargeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsRateSurchargeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsRateSurchargeEntity(this, objectMap);
        }
    }
}
