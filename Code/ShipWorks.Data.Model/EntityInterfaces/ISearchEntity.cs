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
    /// Entity interface which represents the entity 'Search'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ISearchEntity
    {
        
        /// <summary> The SearchID property of the Entity Search<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Search"."SearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 SearchID { get; }
        /// <summary> The Started property of the Entity Search<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Search"."Started"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime Started { get; }
        /// <summary> The Pinged property of the Entity Search<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Search"."Pinged"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime Pinged { get; }
        /// <summary> The FilterNodeID property of the Entity Search<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Search"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterNodeID { get; }
        /// <summary> The UserID property of the Entity Search<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Search"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UserID { get; }
        /// <summary> The ComputerID property of the Entity Search<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Search"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ComputerID { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ISearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ISearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Search'. <br/><br/>
    /// 
    /// </summary>
    public partial class SearchEntity : ISearchEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ISearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual ISearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ISearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlySearchEntity(this, objectMap);
        }
    }
}
