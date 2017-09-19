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
    /// Entity interface which represents the entity 'ClickCartProOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IClickCartProOrderSearchEntity
    {
        
        /// <summary> The ClickCartProOrderSearchID property of the Entity ClickCartProOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ClickCartProOrderSearch"."ClickCartProOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ClickCartProOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity ClickCartProOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ClickCartProOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The ClickCartProOrderID property of the Entity ClickCartProOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ClickCartProOrderSearch"."ClickCartProOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ClickCartProOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity ClickCartProOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ClickCartProOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        IClickCartProOrderEntity ClickCartProOrder { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IClickCartProOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IClickCartProOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ClickCartProOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class ClickCartProOrderSearchEntity : IClickCartProOrderSearchEntity
    {
        
        IClickCartProOrderEntity IClickCartProOrderSearchEntity.ClickCartProOrder => ClickCartProOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IClickCartProOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IClickCartProOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IClickCartProOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyClickCartProOrderSearchEntity(this, objectMap);
        }
    }
}
