﻿///////////////////////////////////////////////////////////////
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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'TemplateComputerSettings'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ITemplateComputerSettingsEntity
    {
        
        /// <summary> The TemplateComputerSettingsID property of the Entity TemplateComputerSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateComputerSettings"."TemplateComputerSettingsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 TemplateComputerSettingsID { get; }
        /// <summary> The TemplateID property of the Entity TemplateComputerSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateComputerSettings"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 TemplateID { get; }
        /// <summary> The ComputerID property of the Entity TemplateComputerSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateComputerSettings"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ComputerID { get; }
        /// <summary> The PrinterName property of the Entity TemplateComputerSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateComputerSettings"."PrinterName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PrinterName { get; }
        /// <summary> The PaperSource property of the Entity TemplateComputerSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateComputerSettings"."PaperSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PaperSource { get; }
        
        
        ITemplateEntity Template { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ITemplateComputerSettingsEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ITemplateComputerSettingsEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'TemplateComputerSettings'. <br/><br/>
    /// 
    /// </summary>
    public partial class TemplateComputerSettingsEntity : ITemplateComputerSettingsEntity
    {
        
        ITemplateEntity ITemplateComputerSettingsEntity.Template => Template;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateComputerSettingsEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual ITemplateComputerSettingsEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ITemplateComputerSettingsEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyTemplateComputerSettingsEntity(this, objectMap);
        }
    }
}
