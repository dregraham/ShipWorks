﻿///////////////////////////////////////////////////////////////
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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'YahooOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IYahooOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The YahooProductID property of the Entity YahooOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooOrderItem"."YahooProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String YahooProductID { get; }
        /// <summary> The Url property of the Entity YahooOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooOrderItem"."Url"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Url { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IYahooOrderItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IYahooOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'YahooOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class YahooOrderItemEntity : IYahooOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IYahooOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IYahooOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IYahooOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyYahooOrderItemEntity(this, objectMap);
        }
    }
}