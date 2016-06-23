using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.FactoryClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// The ShipWorks half of an OdbcFieldMapEntry
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class ShipWorksOdbcMappableField : IShipWorksOdbcMappableField
    {
        private readonly Dictionary<Type, object> typeDefaultValues;

        /// <summary>
        /// All the public constructors go through this constructor
        /// </summary>
        private ShipWorksOdbcMappableField(string containingObjectName, string name, string displayName, bool isRequired)
        {
            ContainingObjectName = containingObjectName;
            Name = name;
            DisplayName = displayName;
            IsRequired = isRequired;
            Field = GetField();
            typeDefaultValues = GetTypeDefaultValues();
        }

        /// <summary>
        /// Used for deserialization
        /// </summary>
        [JsonConstructor]
        public ShipWorksOdbcMappableField(string containingObjectName, string name, string displayName)
            : this(containingObjectName, name, displayName, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcMappableField"/> class.
        /// </summary>
        public ShipWorksOdbcMappableField(EntityField2 field, OdbcOrderFieldDescription fieldDescription)
            : this(field.ContainingObjectName, field.Name, EnumHelper.GetDescription(fieldDescription), false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcMappableField"/> class.
        /// </summary>
        public ShipWorksOdbcMappableField(EntityField2 field, OdbcOrderFieldDescription fieldDescription, bool isRequired)
            : this(field.ContainingObjectName, field.Name, EnumHelper.GetDescription(fieldDescription), isRequired)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcMappableField"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="displayName">The display name.</param>
        public ShipWorksOdbcMappableField(EntityField2 field, string displayName) 
            : this(field.ContainingObjectName, field.Name, displayName, false)
        {
        }

        /// <summary>
        /// Gets the ShipWorks Field
        /// </summary>
        [JsonIgnore]
        public IEntityField2 Field { get; }

        /// <summary>
        /// The name of the object that contains this field
        /// </summary>
        public string ContainingObjectName { get; }

        /// <summary>
        /// The name of the field
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The fields value
        /// </summary>
        [JsonIgnore]
        public object Value { get; private set; }

        /// <summary>
        /// The fields display name
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Is the field required to be mapped.
        /// </summary>
        [JsonIgnore]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets the qualified name for the field - table.column
        /// </summary>
        [Obfuscation(Exclude = false)]
        public string GetQualifiedName()
        {
            return $"{ContainingObjectName}.{Name}";
        }

        /// <summary>
        /// Set the Value to the given value
        /// </summary>
        [Obfuscation(Exclude = false)]
        public void LoadValue(object value)
        {
            Value = ChangeType(value);
        }

        /// <summary>
        /// Convert the given object to the supplied type
        /// </summary>
        [Obfuscation(Exclude = true)]
        private object ChangeType(object value)
        {

            // Don't write to readonly or primary key fields. They shouldn't be mapped.
            if (IsFieldMappable())
            {
                throw new ShipWorksOdbcException($"Invalid Map. '{GetQualifiedName()}' should never be mapped.");
            }

            // If value is null and we have a default value for the destination type, return the default value.
            if (UseDefaultValue(value))
            {
                return typeDefaultValues[Field.DataType];
            }

            // If the value is null (and the type wasn't mapped) or the value is already the expected type, return it.
            if (value == null || value.GetType() == Field.DataType)
            {
                return value;
            }

            try
            {
                // parse decimal info with number styles to ensure we can handle currency and thousands
                if (Field.DataType == typeof(decimal))
                {
                    return ConvertDecimal(value);
                }

                return Convert.ChangeType(value, Field.DataType);
            }
            catch (Exception ex)
            {
                throw new ShipWorksOdbcException($"Unable to convert '{value}' to {Field.DataType} for {GetQualifiedName()}.", ex);
            }
        }

        /// <summary>
        /// True if value is null, the field doesn't allow nulls, and the type has a default value.
        /// </summary>
        /// <remarks>
        /// All potential types should be mapped. If not, the debug assert will let us know during development...
        /// </remarks>
        private bool UseDefaultValue(object value)
        {
            Debug.Assert(typeDefaultValues.ContainsKey(Field.DataType),
                $"No default value for {Field.Name} because the type is {Field.DataType}");

            return value == null && !Field.IsNullable && typeDefaultValues.ContainsKey(Field.DataType);
        }

        /// <summary>
        /// Is this a valid field to map?
        /// </summary>
        /// <returns></returns>
        private bool IsFieldMappable()
        {
            return Field.IsPrimaryKey || Field.IsReadOnly;
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        private IEntityField2 GetField()
        {
            EntityType entityType;

            try
            {
                entityType = EntityTypeProvider.GetEntityType(ContainingObjectName);
            }
            catch (ArgumentException ex)
            {
                throw new ShipWorksOdbcException($"Invalid entity '{ContainingObjectName}' specified in Map.", ex);
            }

            IEntity2 entity = GeneralEntityFactory.Create(entityType);
            IEntityField2 calculatedField = entity?.Fields[Name];

            if (calculatedField == null)
            {
                throw new Exception($"Invalid ShipWorks Field {GetQualifiedName()} specified in Map.");
            }

            return calculatedField;
        }

        private static Dictionary<Type, object> GetTypeDefaultValues()
        {
            return new Dictionary<Type, object>
            {
                {typeof(string), string.Empty},
                {typeof(double), 0D},
                {typeof(float), 0F},
                {typeof(decimal), 0M},
                {typeof(int), 0},
                {typeof(long), 0L},
                {typeof(short), 0},
                {typeof(bool), false},
                {typeof(DateTime), null} // We don't want to override dates with any default, so keep it null.
            };
        }

        /// <summary>
        /// Converts the given object to a decimal
        /// </summary>
        [Obfuscation(Exclude = true)]
        private decimal ConvertDecimal(object value)
        {
            return decimal.Parse(value.ToString(),
                NumberStyles.AllowLeadingSign |
                NumberStyles.AllowLeadingWhite |
                NumberStyles.AllowTrailingWhite |
                NumberStyles.AllowCurrencySymbol |
                NumberStyles.AllowDecimalPoint |
                NumberStyles.AllowThousands,
                new CultureInfo("en-US"));
        }
    }
}
