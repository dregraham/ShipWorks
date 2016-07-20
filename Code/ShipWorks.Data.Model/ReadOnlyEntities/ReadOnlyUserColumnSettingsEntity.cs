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
    /// Entity interface which represents the entity 'UserColumnSettings'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUserColumnSettingsEntity : IUserColumnSettingsEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUserColumnSettingsEntity(IUserColumnSettingsEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UserColumnSettingsID = source.UserColumnSettingsID;
            SettingsKey = source.SettingsKey;
            UserID = source.UserID;
            InitialSortType = source.InitialSortType;
            GridColumnLayoutID = source.GridColumnLayoutID;
            
            
            

            CopyCustomUserColumnSettingsData(source);
        }

        
        /// <summary> The UserColumnSettingsID property of the Entity UserColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserColumnSettings"."UserColumnSettingsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 UserColumnSettingsID { get; }
        /// <summary> The SettingsKey property of the Entity UserColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserColumnSettings"."SettingsKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Guid SettingsKey { get; }
        /// <summary> The UserID property of the Entity UserColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserColumnSettings"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UserID { get; }
        /// <summary> The InitialSortType property of the Entity UserColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserColumnSettings"."InitialSortType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 InitialSortType { get; }
        /// <summary> The GridColumnLayoutID property of the Entity UserColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserColumnSettings"."GridColumnLayoutID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 GridColumnLayoutID { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUserColumnSettingsEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUserColumnSettingsEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUserColumnSettingsData(IUserColumnSettingsEntity source);
    }
}
