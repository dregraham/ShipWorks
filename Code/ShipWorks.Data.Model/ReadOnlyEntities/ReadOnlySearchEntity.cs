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
    /// Read-only representation of the entity 'Search'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlySearchEntity : ISearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlySearchEntity(ISearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            SearchID = source.SearchID;
            Started = source.Started;
            Pinged = source.Pinged;
            FilterNodeID = source.FilterNodeID;
            UserID = source.UserID;
            ComputerID = source.ComputerID;
            
            
            

            CopyCustomSearchData(source);
        }

        
        /// <summary> The SearchID property of the Entity Search<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Search"."SearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 SearchID { get; }
        /// <summary> The Started property of the Entity Search<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Search"."Started"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime Started { get; }
        /// <summary> The Pinged property of the Entity Search<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Search"."Pinged"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime Pinged { get; }
        /// <summary> The FilterNodeID property of the Entity Search<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Search"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterNodeID { get; }
        /// <summary> The UserID property of the Entity Search<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Search"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UserID { get; }
        /// <summary> The ComputerID property of the Entity Search<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Search"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ComputerID { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ISearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ISearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomSearchData(ISearchEntity source);
    }
}
