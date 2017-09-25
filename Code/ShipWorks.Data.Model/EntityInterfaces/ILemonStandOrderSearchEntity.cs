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
    /// Entity interface which represents the entity 'LemonStandOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ILemonStandOrderSearchEntity
    {
        
        /// <summary> The LemonStandOrderSearchID property of the Entity LemonStandOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderSearch"."LemonStandOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 LemonStandOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity LemonStandOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The LemonStandOrderID property of the Entity LemonStandOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderSearch"."LemonStandOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LemonStandOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity LemonStandOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        ILemonStandOrderEntity LemonStandOrder { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ILemonStandOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ILemonStandOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'LemonStandOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class LemonStandOrderSearchEntity : ILemonStandOrderSearchEntity
    {
        
        ILemonStandOrderEntity ILemonStandOrderSearchEntity.LemonStandOrder => LemonStandOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ILemonStandOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual ILemonStandOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ILemonStandOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyLemonStandOrderSearchEntity(this, objectMap);
        }
    }
}
