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
    /// Entity interface which represents the entity 'LemonStandOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ILemonStandOrderEntity: IOrderEntity
    {
        
        /// <summary> The LemonStandOrderID property of the Entity LemonStandOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrder"."LemonStandOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LemonStandOrderID { get; }
        
        
        
        IEnumerable<ILemonStandOrderSearchEntity> LemonStandOrderSearch { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new ILemonStandOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new ILemonStandOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'LemonStandOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class LemonStandOrderEntity : ILemonStandOrderEntity
    {
        
        
        IEnumerable<ILemonStandOrderSearchEntity> ILemonStandOrderEntity.LemonStandOrderSearch => LemonStandOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ILemonStandOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new ILemonStandOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ILemonStandOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyLemonStandOrderEntity(this, objectMap);
        }
    }
}
