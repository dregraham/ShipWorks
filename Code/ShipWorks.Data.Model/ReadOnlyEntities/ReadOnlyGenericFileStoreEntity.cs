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
    /// Read-only representation of the entity 'GenericFileStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyGenericFileStoreEntity : ReadOnlyStoreEntity, IGenericFileStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyGenericFileStoreEntity(IGenericFileStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FileFormat = source.FileFormat;
            FileSource = source.FileSource;
            DiskFolder = source.DiskFolder;
            FtpAccountID = source.FtpAccountID;
            FtpFolder = source.FtpFolder;
            EmailAccountID = source.EmailAccountID;
            EmailIncomingFolder = source.EmailIncomingFolder;
            EmailFolderValidityID = source.EmailFolderValidityID;
            EmailFolderLastMessageID = source.EmailFolderLastMessageID;
            EmailOnlyUnread = source.EmailOnlyUnread;
            NamePatternMatch = source.NamePatternMatch;
            NamePatternSkip = source.NamePatternSkip;
            SuccessAction = source.SuccessAction;
            SuccessMoveFolder = source.SuccessMoveFolder;
            ErrorAction = source.ErrorAction;
            ErrorMoveFolder = source.ErrorMoveFolder;
            XmlXsltFileName = source.XmlXsltFileName;
            XmlXsltContent = source.XmlXsltContent;
            FlatImportMap = source.FlatImportMap;
            
            
            

            CopyCustomGenericFileStoreData(source);
        }

        
        /// <summary> The FileFormat property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."FileFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FileFormat { get; }
        /// <summary> The FileSource property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."FileSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FileSource { get; }
        /// <summary> The DiskFolder property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."DiskFolder"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 355<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DiskFolder { get; }
        /// <summary> The FtpAccountID property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."FtpAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> FtpAccountID { get; }
        /// <summary> The FtpFolder property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."FtpFolder"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 355<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FtpFolder { get; }
        /// <summary> The EmailAccountID property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."EmailAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> EmailAccountID { get; }
        /// <summary> The EmailIncomingFolder property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."EmailFolder"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EmailIncomingFolder { get; }
        /// <summary> The EmailFolderValidityID property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."EmailFolderValidityID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EmailFolderValidityID { get; }
        /// <summary> The EmailFolderLastMessageID property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."EmailFolderLastMessageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EmailFolderLastMessageID { get; }
        /// <summary> The EmailOnlyUnread property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."EmailOnlyUnread"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean EmailOnlyUnread { get; }
        /// <summary> The NamePatternMatch property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."NamePatternMatch"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String NamePatternMatch { get; }
        /// <summary> The NamePatternSkip property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."NamePatternSkip"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String NamePatternSkip { get; }
        /// <summary> The SuccessAction property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."SuccessAction"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 SuccessAction { get; }
        /// <summary> The SuccessMoveFolder property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."SuccessMoveFolder"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 355<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SuccessMoveFolder { get; }
        /// <summary> The ErrorAction property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."ErrorAction"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ErrorAction { get; }
        /// <summary> The ErrorMoveFolder property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."ErrorMoveFolder"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 355<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ErrorMoveFolder { get; }
        /// <summary> The XmlXsltFileName property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."XmlXsltFileName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 355<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String XmlXsltFileName { get; }
        /// <summary> The XmlXsltContent property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."XmlXsltContent"<br/>
        /// Table field type characteristics (type, precision, scale, length): NText, 0, 0, 1073741823<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String XmlXsltContent { get; }
        /// <summary> The FlatImportMap property of the Entity GenericFileStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericFileStore"."FlatImportMap"<br/>
        /// Table field type characteristics (type, precision, scale, length): NText, 0, 0, 1073741823<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FlatImportMap { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IGenericFileStoreEntity AsReadOnlyGenericFileStore() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IGenericFileStoreEntity AsReadOnlyGenericFileStore(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomGenericFileStoreData(IGenericFileStoreEntity source);
    }
}
