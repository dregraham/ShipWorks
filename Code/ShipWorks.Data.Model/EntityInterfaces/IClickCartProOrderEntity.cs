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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'ClickCartProOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IClickCartProOrderEntity: IOrderEntity
    {
        
        /// <summary> The ClickCartProOrderID property of the Entity ClickCartProOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ClickCartProOrder"."ClickCartProOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ClickCartProOrderID { get; }
        
        
        
        IEnumerable<IClickCartProOrderSearchEntity> ClickCartProOrderSearch { get; }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IClickCartProOrderEntity AsReadOnlyClickCartProOrder();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IClickCartProOrderEntity AsReadOnlyClickCartProOrder(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ClickCartProOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class ClickCartProOrderEntity : IClickCartProOrderEntity
    {
        
        
        IEnumerable<IClickCartProOrderSearchEntity> IClickCartProOrderEntity.ClickCartProOrderSearch => ClickCartProOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IClickCartProOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyClickCartProOrderEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IClickCartProOrderEntity AsReadOnlyClickCartProOrder() =>
            (IClickCartProOrderEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IClickCartProOrderEntity AsReadOnlyClickCartProOrder(IDictionary<object, object> objectMap) =>
            (IClickCartProOrderEntity) AsReadOnly(objectMap);
        
    }
}
