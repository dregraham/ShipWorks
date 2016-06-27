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
        private readonly IEntityField2 field;

        /// <summary>
        /// Used for deserialization
        /// </summary>
        [JsonConstructor]
        public ShipWorksOdbcMappableField(string containingObjectName, string name, string displayName)
            : this(GetField(containingObjectName, name), displayName, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcMappableField"/> class.
        /// </summary>
        public ShipWorksOdbcMappableField(IEntityField2 field, OdbcOrderFieldDescription fieldDescription)
            : this(field, EnumHelper.GetDescription(fieldDescription), false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcMappableField"/> class.
        /// </summary>
        public ShipWorksOdbcMappableField(IEntityField2 field, OdbcOrderFieldDescription fieldDescription, bool isRequired)
            : this(field, EnumHelper.GetDescription(fieldDescription), isRequired)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcMappableField"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="displayName">The display name.</param>
        public ShipWorksOdbcMappableField(IEntityField2 field, string displayName) 
            : this(field, displayName, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcMappableField"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="isRequired">if set to <c>true</c> [is required].</param>
        private ShipWorksOdbcMappableField(IEntityField2 field, string displayName, bool isRequired)
        {
            this.field = field;
            DisplayName = displayName;
            IsRequired = isRequired;
            typeDefaultValues = GetTypeDefaultValues();
        }

        /// <summary>
        /// The name of the object that contains this field
        /// </summary>
        public string ContainingObjectName => field.ContainingObjectName;

        /// <summary>
        /// The name of the field
        /// </summary>
        public string Name => field.Name;

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
        [JsonIgnore]
        public string QualifiedName => $"{ContainingObjectName}.{Name}";

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
        [Obfuscation(Exclude = false)]
        private object ChangeType(object value)
        {

            // Don't write to readonly or primary key fields. They shouldn't be mapped.
            if (IsFieldMappable())
            {
                throw new ShipWorksOdbcException($"Invalid Map. '{QualifiedName}' should never be mapped.");
            }

            // If value is null and we have a default value for the destination type, return the default value.
            if (UseDefaultValue(value))
            {
                return typeDefaultValues[field.DataType];
            }

            // If the value is null (and there wasn't a default value for the type) 
            // or the value is already the expected type, return it.
            if (value == null || value.GetType() == field.DataType)
            {
                return value;
            }

            try
            {
                // parse decimal info with number styles to ensure we can handle currency and thousands
                if (field.DataType == typeof(decimal))
                {
                    return ConvertDecimal(value);
                }

                return Convert.ChangeType(value, field.DataType);
            }
            catch (Exception ex)
            {
                throw new ShipWorksOdbcException($"Unable to convert '{value}' to {field.DataType} for {QualifiedName}.", ex);
            }
        }

        /// <summary>
        /// True if value is null, the field doesn't allow nulls, and the type has a default value.
        /// </summary>
        /// <remarks>
        /// All potential types should be mapped. If not, the debug assert will let us know during development...
        /// </remarks>
        [Obfuscation(Exclude = false)]
        private bool UseDefaultValue(object value)
        {
            Debug.Assert(typeDefaultValues.ContainsKey(field.DataType),
                $"No default value for {field.Name} because the type is {field.DataType}");

            return value == null && !field.IsNullable && typeDefaultValues.ContainsKey(field.DataType);
        }

        /// <summary>
        /// Is this a valid field to map?
        /// </summary>
        /// <returns></returns>
        [Obfuscation(Exclude = false)]
        private bool IsFieldMappable()
        {
            return field.IsPrimaryKey || field.IsReadOnly;
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        [Obfuscation(Exclude = false)]
        private static IEntityField2 GetField(string containingObjectName, string fieldName)
        {
            EntityType entityType;

            try
            {
                entityType = EntityTypeProvider.GetEntityType(containingObjectName);
            }
            catch (ArgumentException ex)
            {
                throw new ShipWorksOdbcException($"Invalid entity '{containingObjectName}' specified in Map.", ex);
            }

            IEntity2 entity = GeneralEntityFactory.Create(entityType);
            IEntityField2 calculatedField = entity?.Fields[fieldName];

            if (calculatedField == null)
            {
                throw new Exception($"Invalid ShipWorks Field {containingObjectName}.{fieldName} specified in Map.");
            }

            return calculatedField;
        }

        [Obfuscation(Exclude = false)]
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
        [Obfuscation(Exclude = false)]
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
