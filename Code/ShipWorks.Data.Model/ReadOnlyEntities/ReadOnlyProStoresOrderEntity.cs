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
    /// Entity interface which represents the entity 'ProStoresOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyProStoresOrderEntity : ReadOnlyOrderEntity, IProStoresOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyProStoresOrderEntity(IProStoresOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ConfirmationNumber = source.ConfirmationNumber;
            AuthorizedDate = source.AuthorizedDate;
            AuthorizedBy = source.AuthorizedBy;
            
            
            

            CopyCustomProStoresOrderData(source);
        }

        
        /// <summary> The ConfirmationNumber property of the Entity ProStoresOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrder"."ConfirmationNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ConfirmationNumber { get; }
        /// <summary> The AuthorizedDate property of the Entity ProStoresOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrder"."AuthorizedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> AuthorizedDate { get; }
        /// <summary> The AuthorizedBy property of the Entity ProStoresOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrder"."AuthorizedBy"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AuthorizedBy { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IProStoresOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IProStoresOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProStoresOrderData(IProStoresOrderEntity source);
    }
}
