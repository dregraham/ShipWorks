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
    /// Read-only representation of the entity 'Note'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyNoteEntity : INoteEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyNoteEntity(INoteEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            NoteID = source.NoteID;
            RowVersion = source.RowVersion;
            EntityID = source.EntityID;
            UserID = source.UserID;
            Edited = source.Edited;
            Text = source.Text;
            Source = source.Source;
            Visibility = source.Visibility;
            
            
            Order = source.Order?.AsReadOnly(objectMap);
            

            CopyCustomNoteData(source);
        }

        
        /// <summary> The NoteID property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."NoteID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 NoteID { get; }
        /// <summary> The RowVersion property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The EntityID property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EntityID { get; }
        /// <summary> The UserID property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> UserID { get; }
        /// <summary> The Edited property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."Edited"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime Edited { get; }
        /// <summary> The Text property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."Text"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Text { get; }
        /// <summary> The Source property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."Source"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Source { get; }
        /// <summary> The Visibility property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."Visibility"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Visibility { get; }
        
        
        public IOrderEntity Order { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual INoteEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual INoteEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomNoteData(INoteEntity source);
    }
}
