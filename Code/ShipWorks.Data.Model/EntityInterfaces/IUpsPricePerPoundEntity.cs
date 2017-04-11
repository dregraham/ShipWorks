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
    /// Entity interface which represents the entity 'UpsPricePerPound'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUpsPricePerPoundEntity
    {
        
        /// <summary> The UpsPricePerPoundID property of the Entity UpsPricePerPound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPricePerPound"."UpsPricePerPoundID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 UpsPricePerPoundID { get; }
        /// <summary> The UpsRateTableID property of the Entity UpsPricePerPound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPricePerPound"."UpsRateTableID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UpsRateTableID { get; }
        /// <summary> The Zone property of the Entity UpsPricePerPound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPricePerPound"."Zone"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Zone { get; }
        /// <summary> The Service property of the Entity UpsPricePerPound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPricePerPound"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Service { get; }
        /// <summary> The Rate property of the Entity UpsPricePerPound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPricePerPound"."Rate"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Rate { get; }
        
        
        IUpsRateTableEntity UpsRateTable { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsPricePerPoundEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUpsPricePerPoundEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UpsPricePerPound'. <br/><br/>
    /// 
    /// </summary>
    public partial class UpsPricePerPoundEntity : IUpsPricePerPoundEntity
    {
        
        IUpsRateTableEntity IUpsPricePerPoundEntity.UpsRateTable => UpsRateTable;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsPricePerPoundEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUpsPricePerPoundEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUpsPricePerPoundEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUpsPricePerPoundEntity(this, objectMap);
        }
    }
}
