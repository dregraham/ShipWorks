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
    /// Entity interface which represents the entity 'YahooProduct'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IYahooProductEntity
    {
        
        /// <summary> The StoreID property of the Entity YahooProduct<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooProduct"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 StoreID { get; }
        /// <summary> The YahooProductID property of the Entity YahooProduct<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooProduct"."YahooProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.String YahooProductID { get; }
        /// <summary> The Weight property of the Entity YahooProduct<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooProduct"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Weight { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IYahooProductEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IYahooProductEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'YahooProduct'. <br/><br/>
    /// 
    /// </summary>
    public partial class YahooProductEntity : IYahooProductEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IYahooProductEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IYahooProductEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IYahooProductEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyYahooProductEntity(this, objectMap);
        }

        
    }
}
