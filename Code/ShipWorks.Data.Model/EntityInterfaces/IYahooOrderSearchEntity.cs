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
    /// Entity interface which represents the entity 'YahooOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IYahooOrderSearchEntity
    {
        
        /// <summary> The YahooOrderSearchID property of the Entity YahooOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooOrderSearch"."YahooOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 YahooOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity YahooOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The YahooOrderID property of the Entity YahooOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooOrderSearch"."YahooOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String YahooOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity YahooOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        IYahooOrderEntity YahooOrder { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IYahooOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IYahooOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'YahooOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class YahooOrderSearchEntity : IYahooOrderSearchEntity
    {
        
        IYahooOrderEntity IYahooOrderSearchEntity.YahooOrder => YahooOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IYahooOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IYahooOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IYahooOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyYahooOrderSearchEntity(this, objectMap);
        }
    }
}
