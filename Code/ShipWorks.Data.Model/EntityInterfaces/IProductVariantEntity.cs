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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'ProductVariant'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IProductVariantEntity
    {
        
        /// <summary> The ProductVariantID property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."ProductVariantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ProductVariantID { get; }
        /// <summary> The ProductID property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."ProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ProductID { get; }
        /// <summary> The CreatedDate property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime CreatedDate { get; }
        /// <summary> The Name property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        /// <summary> The IsActive property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."IsActive"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsActive { get; }
        /// <summary> The UPC property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."UPC"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String UPC { get; }
        /// <summary> The ASIN property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."ASIN"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ASIN { get; }
        /// <summary> The ISBN property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."ISBN"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ISBN { get; }
        /// <summary> The Weight property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 29, 9, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Decimal> Weight { get; }
        /// <summary> The Length property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."Length"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Decimal> Length { get; }
        /// <summary> The Width property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."Width"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Decimal> Width { get; }
        /// <summary> The Height property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."Height"<br/>
        /// Table field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Decimal> Height { get; }
        /// <summary> The ImageUrl property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."ImageUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ImageUrl { get; }
        /// <summary> The BinLocation property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."BinLocation"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String BinLocation { get; }
        /// <summary> The HarmonizedCode property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."HarmonizedCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String HarmonizedCode { get; }
        /// <summary> The DeclaredValue property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."DeclaredValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Decimal> DeclaredValue { get; }
        /// <summary> The CountryOfOrigin property of the Entity ProductVariant<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariant"."CountryOfOrigin"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String CountryOfOrigin { get; }
        
        
        IProductEntity Product { get; }
        
        IEnumerable<IProductBundleEntity> IncludedInBundles { get; }
        IEnumerable<IProductVariantAliasEntity> Aliases { get; }
        IEnumerable<IProductVariantAttributeEntity> VariantAttributes { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductVariantEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductVariantEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ProductVariant'. <br/><br/>
    /// 
    /// </summary>
    public partial class ProductVariantEntity : IProductVariantEntity
    {
        
        IProductEntity IProductVariantEntity.Product => Product;
        
        IEnumerable<IProductBundleEntity> IProductVariantEntity.IncludedInBundles => IncludedInBundles;
        IEnumerable<IProductVariantAliasEntity> IProductVariantEntity.Aliases => Aliases;
        IEnumerable<IProductVariantAttributeEntity> IProductVariantEntity.VariantAttributes => VariantAttributes;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IProductVariantEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IProductVariantEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyProductVariantEntity(this, objectMap);
        }

        
    }
}
