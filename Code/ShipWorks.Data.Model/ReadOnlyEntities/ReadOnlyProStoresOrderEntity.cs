﻿///////////////////////////////////////////////////////////////
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
    /// Read-only representation of the entity 'ProStoresOrder'. <br/><br/>
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
            
            
            
            ProStoresOrderSearch = source.ProStoresOrderSearch?.Select(x => x.AsReadOnly(objectMap)).OfType<IProStoresOrderSearchEntity>().ToReadOnly() ??
                Enumerable.Empty<IProStoresOrderSearchEntity>();

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
        
        
        
        public IEnumerable<IProStoresOrderSearchEntity> ProStoresOrderSearch { get; }
        
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
        public IProStoresOrderEntity AsReadOnlyProStoresOrder() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IProStoresOrderEntity AsReadOnlyProStoresOrder(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProStoresOrderData(IProStoresOrderEntity source);
    }
}
