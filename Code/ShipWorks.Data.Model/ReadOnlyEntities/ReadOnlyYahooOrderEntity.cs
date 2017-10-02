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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'YahooOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyYahooOrderEntity : ReadOnlyOrderEntity, IYahooOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyYahooOrderEntity(IYahooOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            YahooOrderID = source.YahooOrderID;
            
            
            
            YahooOrderSearch = source.YahooOrderSearch?.Select(x => x.AsReadOnly(objectMap)).OfType<IYahooOrderSearchEntity>().ToReadOnly() ??
                Enumerable.Empty<IYahooOrderSearchEntity>();

            CopyCustomYahooOrderData(source);
        }

        
        /// <summary> The YahooOrderID property of the Entity YahooOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooOrder"."YahooOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String YahooOrderID { get; }
        
        
        
        public IEnumerable<IYahooOrderSearchEntity> YahooOrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IYahooOrderEntity AsReadOnlyYahooOrder() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IYahooOrderEntity AsReadOnlyYahooOrder(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomYahooOrderData(IYahooOrderEntity source);
    }
}
