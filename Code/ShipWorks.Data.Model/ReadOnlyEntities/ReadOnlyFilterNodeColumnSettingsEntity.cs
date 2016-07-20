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
    /// Entity interface which represents the entity 'FilterNodeColumnSettings'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFilterNodeColumnSettingsEntity : IFilterNodeColumnSettingsEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFilterNodeColumnSettingsEntity(IFilterNodeColumnSettingsEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FilterNodeColumnSettingsID = source.FilterNodeColumnSettingsID;
            UserID = source.UserID;
            FilterNodeID = source.FilterNodeID;
            Inherit = source.Inherit;
            GridColumnLayoutID = source.GridColumnLayoutID;
            
            
            FilterNode = source.FilterNode?.AsReadOnly(objectMap);
            User = source.User?.AsReadOnly(objectMap);
            

            CopyCustomFilterNodeColumnSettingsData(source);
        }

        
        /// <summary> The FilterNodeColumnSettingsID property of the Entity FilterNodeColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeColumnSettings"."FilterNodeColumnSettingsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 FilterNodeColumnSettingsID { get; }
        /// <summary> The UserID property of the Entity FilterNodeColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeColumnSettings"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> UserID { get; }
        /// <summary> The FilterNodeID property of the Entity FilterNodeColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeColumnSettings"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterNodeID { get; }
        /// <summary> The Inherit property of the Entity FilterNodeColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeColumnSettings"."Inherit"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Inherit { get; }
        /// <summary> The GridColumnLayoutID property of the Entity FilterNodeColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeColumnSettings"."GridColumnLayoutID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 GridColumnLayoutID { get; }
        
        
        public IFilterNodeEntity FilterNode { get; }
        
        public IUserEntity User { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterNodeColumnSettingsEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterNodeColumnSettingsEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFilterNodeColumnSettingsData(IFilterNodeColumnSettingsEntity source);
    }
}
