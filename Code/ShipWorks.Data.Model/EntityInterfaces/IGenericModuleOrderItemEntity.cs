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
    /// Entity interface which represents the entity 'GenericModuleOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IGenericModuleOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The AmazonOrderItemCode property of the Entity GenericModuleOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleOrderItem"."AmazonOrderItemCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AmazonOrderItemCode { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGenericModuleOrderItemEntity AsReadOnlyGenericModuleOrderItem();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGenericModuleOrderItemEntity AsReadOnlyGenericModuleOrderItem(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'GenericModuleOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class GenericModuleOrderItemEntity : IGenericModuleOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IGenericModuleOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyGenericModuleOrderItemEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IGenericModuleOrderItemEntity AsReadOnlyGenericModuleOrderItem() =>
            (IGenericModuleOrderItemEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IGenericModuleOrderItemEntity AsReadOnlyGenericModuleOrderItem(IDictionary<object, object> objectMap) =>
            (IGenericModuleOrderItemEntity) AsReadOnly(objectMap);
        
    }
}
