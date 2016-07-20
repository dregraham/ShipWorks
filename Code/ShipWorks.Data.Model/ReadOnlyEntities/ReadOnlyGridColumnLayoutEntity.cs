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
    /// Entity interface which represents the entity 'GridColumnLayout'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyGridColumnLayoutEntity : IGridColumnLayoutEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyGridColumnLayoutEntity(IGridColumnLayoutEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            GridColumnLayoutID = source.GridColumnLayoutID;
            DefinitionSet = source.DefinitionSet;
            DefaultSortColumnGuid = source.DefaultSortColumnGuid;
            DefaultSortOrder = source.DefaultSortOrder;
            LastSortColumnGuid = source.LastSortColumnGuid;
            LastSortOrder = source.LastSortOrder;
            DetailViewSettings = source.DetailViewSettings;
            
            
            
            GridColumnPositions = source.GridColumnPositions?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IGridColumnPositionEntity>();

            CopyCustomGridColumnLayoutData(source);
        }

        
        /// <summary> The GridColumnLayoutID property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."GridColumnLayoutID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 GridColumnLayoutID { get; }
        /// <summary> The DefinitionSet property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."DefinitionSet"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DefinitionSet { get; }
        /// <summary> The DefaultSortColumnGuid property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."DefaultSortColumnGuid"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Guid DefaultSortColumnGuid { get; }
        /// <summary> The DefaultSortOrder property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."DefaultSortOrder"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DefaultSortOrder { get; }
        /// <summary> The LastSortColumnGuid property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."LastSortColumnGuid"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Guid LastSortColumnGuid { get; }
        /// <summary> The LastSortOrder property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."LastSortOrder"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 LastSortOrder { get; }
        /// <summary> The DetailViewSettings property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."DetailViewSettings"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String DetailViewSettings { get; }
        
        
        
        public IEnumerable<IGridColumnPositionEntity> GridColumnPositions { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IGridColumnLayoutEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IGridColumnLayoutEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomGridColumnLayoutData(IGridColumnLayoutEntity source);
    }
}
