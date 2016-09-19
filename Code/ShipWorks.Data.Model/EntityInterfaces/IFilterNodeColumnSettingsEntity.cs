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
    /// Entity interface which represents the entity 'FilterNodeColumnSettings'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IFilterNodeColumnSettingsEntity
    {
        
        /// <summary> The FilterNodeColumnSettingsID property of the Entity FilterNodeColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeColumnSettings"."FilterNodeColumnSettingsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 FilterNodeColumnSettingsID { get; }
        /// <summary> The UserID property of the Entity FilterNodeColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeColumnSettings"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> UserID { get; }
        /// <summary> The FilterNodeID property of the Entity FilterNodeColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeColumnSettings"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterNodeID { get; }
        /// <summary> The Inherit property of the Entity FilterNodeColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeColumnSettings"."Inherit"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Inherit { get; }
        /// <summary> The GridColumnLayoutID property of the Entity FilterNodeColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeColumnSettings"."GridColumnLayoutID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 GridColumnLayoutID { get; }
        
        
        IFilterNodeEntity FilterNode { get; }
        IUserEntity User { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterNodeColumnSettingsEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterNodeColumnSettingsEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'FilterNodeColumnSettings'. <br/><br/>
    /// 
    /// </summary>
    public partial class FilterNodeColumnSettingsEntity : IFilterNodeColumnSettingsEntity
    {
        
        IFilterNodeEntity IFilterNodeColumnSettingsEntity.FilterNode => FilterNode;
        IUserEntity IFilterNodeColumnSettingsEntity.User => User;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterNodeColumnSettingsEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IFilterNodeColumnSettingsEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IFilterNodeColumnSettingsEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyFilterNodeColumnSettingsEntity(this, objectMap);
        }
    }
}
