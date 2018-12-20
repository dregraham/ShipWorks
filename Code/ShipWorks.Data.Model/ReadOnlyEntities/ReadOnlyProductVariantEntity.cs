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
    /// Read-only representation of the entity 'ProductVariant'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyProductVariantEntity : IProductVariantEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyProductVariantEntity(IProductVariantEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ProductVariantID = source.ProductVariantID;
            ProductID = source.ProductID;
            CreatedDate = source.CreatedDate;
            Name = source.Name;
            IsActive = source.IsActive;
            UPC = source.UPC;
            ASIN = source.ASIN;
            ISBN = source.ISBN;
            Weight = source.Weight;
            Length = source.Length;
            Width = source.Width;
            Height = source.Height;
            ImageUrl = source.ImageUrl;
            BinLocation = source.BinLocation;
            HarmonizedCode = source.HarmonizedCode;
            DeclaredValue = source.DeclaredValue;
            CountryOfOrigin = source.CountryOfOrigin;
            
            
            Product = (IProductEntity) source.Product?.AsReadOnly(objectMap);
            
            IncludedInBundles = source.IncludedInBundles?.Select(x => x.AsReadOnly(objectMap)).OfType<IProductBundleEntity>().ToReadOnly() ??
                Enumerable.Empty<IProductBundleEntity>();
            Aliases = source.Aliases?.Select(x => x.AsReadOnly(objectMap)).OfType<IProductVariantAliasEntity>().ToReadOnly() ??
                Enumerable.Empty<IProductVariantAliasEntity>();
            AttributeValues = source.AttributeValues?.Select(x => x.AsReadOnly(objectMap)).OfType<IProductVariantAttributeValueEntity>().ToReadOnly() ??
                Enumerable.Empty<IProductVariantAttributeValueEntity>();

            CopyCustomProductVariantData(source);
        }

        
        /// <summary> The ProductVariantID property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."ProductVariantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ProductVariantID { get; }
        /// <summary> The ProductID property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."ProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ProductID { get; }
        /// <summary> The CreatedDate property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime CreatedDate { get; }
        /// <summary> The Name property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Name { get; }
        /// <summary> The IsActive property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."IsActive"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsActive { get; }
        /// <summary> The UPC property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."UPC"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String UPC { get; }
        /// <summary> The ASIN property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."ASIN"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ASIN { get; }
        /// <summary> The ISBN property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."ISBN"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ISBN { get; }
        /// <summary> The Weight property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 29, 9, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> Weight { get; }
        /// <summary> The Length property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."Length"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> Length { get; }
        /// <summary> The Width property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."Width"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> Width { get; }
        /// <summary> The Height property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."Height"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> Height { get; }
        /// <summary> The ImageUrl property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."ImageUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ImageUrl { get; }
        /// <summary> The BinLocation property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."BinLocation"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String BinLocation { get; }
        /// <summary> The HarmonizedCode property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."HarmonizedCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String HarmonizedCode { get; }
        /// <summary> The DeclaredValue property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."DeclaredValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> DeclaredValue { get; }
        /// <summary> The CountryOfOrigin property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."CountryOfOrigin"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CountryOfOrigin { get; }
        
        
        public IProductEntity Product { get; }
        
        
        public IEnumerable<IProductBundleEntity> IncludedInBundles { get; }
        
        public IEnumerable<IProductVariantAliasEntity> Aliases { get; }
        
        public IEnumerable<IProductVariantAttributeValueEntity> AttributeValues { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProductVariantData(IProductVariantEntity source);
    }
}
