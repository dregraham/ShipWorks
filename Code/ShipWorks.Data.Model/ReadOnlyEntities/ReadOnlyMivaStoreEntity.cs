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
    /// Read-only representation of the entity 'MivaStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyMivaStoreEntity : ReadOnlyGenericModuleStoreEntity, IMivaStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyMivaStoreEntity(IMivaStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            EncryptionPassphrase = source.EncryptionPassphrase;
            LiveManualOrderNumbers = source.LiveManualOrderNumbers;
            SebenzaCheckoutDataEnabled = source.SebenzaCheckoutDataEnabled;
            OnlineUpdateStrategy = source.OnlineUpdateStrategy;
            OnlineUpdateStatusChangeEmail = source.OnlineUpdateStatusChangeEmail;
            
            
            

            CopyCustomMivaStoreData(source);
        }

        
        /// <summary> The EncryptionPassphrase property of the Entity MivaStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaStore"."EncryptionPassphrase"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EncryptionPassphrase { get; }
        /// <summary> The LiveManualOrderNumbers property of the Entity MivaStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaStore"."LiveManualOrderNumbers"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean LiveManualOrderNumbers { get; }
        /// <summary> The SebenzaCheckoutDataEnabled property of the Entity MivaStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaStore"."SebenzaCheckoutDataEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean SebenzaCheckoutDataEnabled { get; }
        /// <summary> The OnlineUpdateStrategy property of the Entity MivaStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaStore"."OnlineUpdateStrategy"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OnlineUpdateStrategy { get; }
        /// <summary> The OnlineUpdateStatusChangeEmail property of the Entity MivaStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaStore"."OnlineUpdateStatusChangeEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean OnlineUpdateStatusChangeEmail { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IMivaStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IMivaStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomMivaStoreData(IMivaStoreEntity source);
    }
}
