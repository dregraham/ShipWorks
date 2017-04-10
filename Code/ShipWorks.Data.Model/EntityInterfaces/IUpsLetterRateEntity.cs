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
    /// Entity interface which represents the entity 'UpsLetterRate'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsLetterRateEntity
    {
        
        /// <summary> The UpsLetterRateID property of the Entity UpsLetterRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLetterRate"."UpsLetterRateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 UpsLetterRateID { get; }
        /// <summary> The UpsRateTableID property of the Entity UpsLetterRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLetterRate"."UpsRateTableID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UpsRateTableID { get; }
        /// <summary> The Zone property of the Entity UpsLetterRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLetterRate"."Zone"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Zone { get; }
        /// <summary> The Service property of the Entity UpsLetterRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLetterRate"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Service { get; }
        /// <summary> The Rate property of the Entity UpsLetterRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLetterRate"."Rate"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Rate { get; }
        
        
        IUpsRateTableEntity UpsRateTable { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsLetterRateEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsLetterRateEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsLetterRate'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsLetterRateEntity : IUpsLetterRateEntity
    {
        
        IUpsRateTableEntity IUpsLetterRateEntity.UpsRateTable => UpsRateTable;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLetterRateEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUpsLetterRateEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsLetterRateEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsLetterRateEntity(this, objectMap);
        }
    }
}
