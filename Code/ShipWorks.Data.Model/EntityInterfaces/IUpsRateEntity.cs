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
    /// Entity interface which represents the entity 'UpsRate'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsRateEntity
    {
        
        /// <summary> The UpsRateID property of the Entity UpsRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRate"."UpsRateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 UpsRateID { get; }
        /// <summary> The UpsRateTableID property of the Entity UpsRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRate"."UpsRateTableID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UpsRateTableID { get; }
        /// <summary> The Zone property of the Entity UpsRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRate"."Zone"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Zone { get; }
        /// <summary> The WeightInPounds property of the Entity UpsRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRate"."WeightInPounds"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 WeightInPounds { get; }
        /// <summary> The Service property of the Entity UpsRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRate"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Service { get; }
        /// <summary> The Rate property of the Entity UpsRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsRate"."Rate"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Rate { get; }
        
        
        IUpsRateTableEntity UpsRateTable { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsRateEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsRateEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsRate'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsRateEntity : IUpsRateEntity
    {
        
        IUpsRateTableEntity IUpsRateEntity.UpsRateTable => UpsRateTable;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsRateEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUpsRateEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsRateEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsRateEntity(this, objectMap);
        }
    }
}
