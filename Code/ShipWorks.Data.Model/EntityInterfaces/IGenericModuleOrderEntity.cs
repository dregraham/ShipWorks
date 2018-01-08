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
    /// Entity interface which represents the entity 'GenericModuleOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IGenericModuleOrderEntity: IOrderEntity
    {
        
        /// <summary> The AmazonOrderID property of the Entity GenericModuleOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleOrder"."AmazonOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AmazonOrderID { get; }
        /// <summary> The IsFBA property of the Entity GenericModuleOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleOrder"."IsFBA"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsFBA { get; }
        /// <summary> The IsPrime property of the Entity GenericModuleOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleOrder"."IsPrime"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 IsPrime { get; }
        /// <summary> The IsSameDay property of the Entity GenericModuleOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleOrder"."IsSameDay"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsSameDay { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGenericModuleOrderEntity AsReadOnlyGenericModuleOrder();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGenericModuleOrderEntity AsReadOnlyGenericModuleOrder(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'GenericModuleOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class GenericModuleOrderEntity : IGenericModuleOrderEntity
    {
        
        

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
                return (IGenericModuleOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyGenericModuleOrderEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IGenericModuleOrderEntity AsReadOnlyGenericModuleOrder() =>
            (IGenericModuleOrderEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IGenericModuleOrderEntity AsReadOnlyGenericModuleOrder(IDictionary<object, object> objectMap) =>
            (IGenericModuleOrderEntity) AsReadOnly(objectMap);
        
    }
}
