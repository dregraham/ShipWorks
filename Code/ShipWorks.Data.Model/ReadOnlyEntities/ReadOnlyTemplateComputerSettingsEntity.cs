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
    /// Read-only representation of the entity 'TemplateComputerSettings'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyTemplateComputerSettingsEntity : ITemplateComputerSettingsEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyTemplateComputerSettingsEntity(ITemplateComputerSettingsEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            TemplateComputerSettingsID = source.TemplateComputerSettingsID;
            TemplateID = source.TemplateID;
            ComputerID = source.ComputerID;
            PrinterName = source.PrinterName;
            PaperSource = source.PaperSource;
            
            
            Template = (ITemplateEntity) source.Template?.AsReadOnly(objectMap);
            

            CopyCustomTemplateComputerSettingsData(source);
        }

        
        /// <summary> The TemplateComputerSettingsID property of the Entity TemplateComputerSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateComputerSettings"."TemplateComputerSettingsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 TemplateComputerSettingsID { get; }
        /// <summary> The TemplateID property of the Entity TemplateComputerSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateComputerSettings"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 TemplateID { get; }
        /// <summary> The ComputerID property of the Entity TemplateComputerSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateComputerSettings"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ComputerID { get; }
        /// <summary> The PrinterName property of the Entity TemplateComputerSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateComputerSettings"."PrinterName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PrinterName { get; }
        /// <summary> The PaperSource property of the Entity TemplateComputerSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateComputerSettings"."PaperSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PaperSource { get; }
        
        
        public ITemplateEntity Template { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateComputerSettingsEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateComputerSettingsEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomTemplateComputerSettingsData(ITemplateComputerSettingsEntity source);
    }
}
