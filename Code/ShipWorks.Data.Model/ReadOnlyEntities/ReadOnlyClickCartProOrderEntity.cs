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
    /// Read-only representation of the entity 'ClickCartProOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyClickCartProOrderEntity : ReadOnlyOrderEntity, IClickCartProOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyClickCartProOrderEntity(IClickCartProOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ClickCartProOrderID = source.ClickCartProOrderID;
            
            
            
            ClickCartProOrderSearch = source.ClickCartProOrderSearch?.Select(x => x.AsReadOnly(objectMap)).OfType<IClickCartProOrderSearchEntity>().ToReadOnly() ??
                Enumerable.Empty<IClickCartProOrderSearchEntity>();

            CopyCustomClickCartProOrderData(source);
        }

        
        /// <summary> The ClickCartProOrderID property of the Entity ClickCartProOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ClickCartProOrder"."ClickCartProOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ClickCartProOrderID { get; }
        
        
        
        public IEnumerable<IClickCartProOrderSearchEntity> ClickCartProOrderSearch { get; }
        
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
        public IClickCartProOrderEntity AsReadOnlyClickCartProOrder() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IClickCartProOrderEntity AsReadOnlyClickCartProOrder(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomClickCartProOrderData(IClickCartProOrderEntity source);
    }
}
