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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'TemplateFolder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ITemplateFolderEntity
    {
        
        /// <summary> The TemplateFolderID property of the Entity TemplateFolder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateFolder"."TemplateFolderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 TemplateFolderID { get; }
        /// <summary> The RowVersion property of the Entity TemplateFolder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateFolder"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ParentFolderID property of the Entity TemplateFolder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateFolder"."ParentFolderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ParentFolderID { get; }
        /// <summary> The Name property of the Entity TemplateFolder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateFolder"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        
        
        ITemplateFolderEntity ParentFolder { get; }
        
        IEnumerable<ITemplateEntity> Templates { get; }
        IEnumerable<ITemplateFolderEntity> ChildFolders { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ITemplateFolderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ITemplateFolderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'TemplateFolder'. <br/><br/>
    /// 
    /// </summary>
    public partial class TemplateFolderEntity : ITemplateFolderEntity
    {
        
        ITemplateFolderEntity ITemplateFolderEntity.ParentFolder => ParentFolder;
        
        IEnumerable<ITemplateEntity> ITemplateFolderEntity.Templates => Templates;
        IEnumerable<ITemplateFolderEntity> ITemplateFolderEntity.ChildFolders => ChildFolders;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateFolderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual ITemplateFolderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ITemplateFolderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyTemplateFolderEntity(this, objectMap);
        }

        
    }
}
