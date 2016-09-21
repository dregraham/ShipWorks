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
    /// Read-only representation of the entity 'AmazonASIN'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAmazonASINEntity : IAmazonASINEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAmazonASINEntity(IAmazonASINEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            StoreID = source.StoreID;
            SKU = source.SKU;
            AmazonASIN = source.AmazonASIN;
            
            
            

            CopyCustomAmazonASINData(source);
        }

        
        /// <summary> The StoreID property of the Entity AmazonASIN<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonASIN"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 StoreID { get; }
        /// <summary> The SKU property of the Entity AmazonASIN<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonASIN"."SKU"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.String SKU { get; }
        /// <summary> The AmazonASIN property of the Entity AmazonASIN<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonASIN"."AmazonASIN"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AmazonASIN { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonASINEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonASINEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAmazonASINData(IAmazonASINEntity source);
    }
}
