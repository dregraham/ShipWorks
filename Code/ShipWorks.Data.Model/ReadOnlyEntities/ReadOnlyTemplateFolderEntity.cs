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
    /// Entity interface which represents the entity 'TemplateFolder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyTemplateFolderEntity : ITemplateFolderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyTemplateFolderEntity(ITemplateFolderEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            TemplateFolderID = source.TemplateFolderID;
            RowVersion = source.RowVersion;
            ParentFolderID = source.ParentFolderID;
            Name = source.Name;
            
            
            ParentFolder = source.ParentFolder?.AsReadOnly(objectMap);
            
            Templates = source.Templates?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<ITemplateEntity>();
            ChildFolders = source.ChildFolders?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<ITemplateFolderEntity>();

            CopyCustomTemplateFolderData(source);
        }

        
        /// <summary> The TemplateFolderID property of the Entity TemplateFolder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateFolder"."TemplateFolderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 TemplateFolderID { get; }
        /// <summary> The RowVersion property of the Entity TemplateFolder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateFolder"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ParentFolderID property of the Entity TemplateFolder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateFolder"."ParentFolderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ParentFolderID { get; }
        /// <summary> The Name property of the Entity TemplateFolder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateFolder"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Name { get; }
        
        
        public ITemplateFolderEntity ParentFolder { get; }
        
        
        public IEnumerable<ITemplateEntity> Templates { get; }
        
        public IEnumerable<ITemplateFolderEntity> ChildFolders { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateFolderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateFolderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomTemplateFolderData(ITemplateFolderEntity source);
    }
}
