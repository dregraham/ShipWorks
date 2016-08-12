///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'YahooProduct'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyYahooProductEntity : IYahooProductEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyYahooProductEntity(IYahooProductEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            StoreID = source.StoreID;
            YahooProductID = source.YahooProductID;
            Weight = source.Weight;
            
            
            

            CopyCustomYahooProductData(source);
        }

        
        /// <summary> The StoreID property of the Entity YahooProduct<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooProduct"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 StoreID { get; }
        /// <summary> The YahooProductID property of the Entity YahooProduct<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooProduct"."YahooProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.String YahooProductID { get; }
        /// <summary> The Weight property of the Entity YahooProduct<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooProduct"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Weight { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IYahooProductEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IYahooProductEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomYahooProductData(IYahooProductEntity source);
    }
}
