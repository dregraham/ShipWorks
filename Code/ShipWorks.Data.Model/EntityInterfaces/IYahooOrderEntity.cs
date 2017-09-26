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
    /// Entity interface which represents the entity 'YahooOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IYahooOrderEntity: IOrderEntity
    {
        
        /// <summary> The YahooOrderID property of the Entity YahooOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooOrder"."YahooOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String YahooOrderID { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IYahooOrderEntity AsReadOnlyYahooOrder();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IYahooOrderEntity AsReadOnlyYahooOrder(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'YahooOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class YahooOrderEntity : IYahooOrderEntity
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
                return (IYahooOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyYahooOrderEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IYahooOrderEntity AsReadOnlyYahooOrder() =>
            (IYahooOrderEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IYahooOrderEntity AsReadOnlyYahooOrder(IDictionary<object, object> objectMap) =>
            (IYahooOrderEntity) AsReadOnly(objectMap);
        
    }
}
