using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Globalization;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// The ShipWorks half of an OdbcFieldMapEntry
    /// </summary>
    public class ShipWorksOdbcMappableField : IShipWorksOdbcMappableField
    {
        public const string UnitCostDisplayName = "Unit Cost";
        public const string TotalCostDisplayName = "Total Cost";
        public const string UnitWeightDisplayName = "Unit Weight";
        public const string TotalWeightDisplayName = "Total Weight";
        public const string UnitPriceDisplayName = "Unit Price";
        public const string TotalPriceDisplayName = "Total Price";
        public const string QuantityDisplayName = "Quantity";
        public const string OrderDateAndTimeDisplayName = "Order Date & Time";
        public const string OrderDateDisplayName = "Order Date";
        public const string OrderTimeDisplayName = "Order Time";

        [JsonConstructor]
        public ShipWorksOdbcMappableField(string displayName)
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcMappableField"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="displayName">The display name.</param>
        public ShipWorksOdbcMappableField(EntityField2 field, string displayName) : this(field, displayName, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcMappableField"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="isRequired"></param>
        public ShipWorksOdbcMappableField(EntityField2 field, string displayName, bool isRequired)
	    {
            ContainingObjectName = field.ContainingObjectName;
            Name = field.Name;
            TypeName = field.DataType.FullName;
            DisplayName = displayName;
            IsRequired = isRequired;
	    }

        /// <summary>
        /// The type of the ShipWorks field
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string TypeName { get; set; }

        /// <summary>
        /// The name of the object that contains this field
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ContainingObjectName { get; set; }

        /// <summary>
        /// The name of the field
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Name { get; set; }

        /// <summary>
        /// The fields value
        /// </summary>
        [JsonIgnore]
        public object Value { get; private set; }

        /// <summary>
        /// The fields display name
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DisplayName { get; }

        /// <summary>
        /// Is the field required to be mapped.
        /// </summary>
        [JsonIgnore]
        [Obfuscation(Exclude = true)]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets the qualified name for the field - table.column
        /// </summary>
        public string GetQualifiedName()
        {
            return $"{ContainingObjectName}.{Name}";
        }

        /// <summary>
        /// Set the Value to the given value
        /// </summary>
        public void LoadValue(object value)
        {
            Value = ChangeType(value);
        }

        /// <summary>
        /// Convert the given object to the supplied type
        /// </summary>
        private object ChangeType(object value)
        {
            Type destinationType = Type.GetType(TypeName);

            if (value == null || destinationType == null || value.GetType() == destinationType.GetType())
            {
                return Value;
            }

            try
            {
                // parse decimal info with number styles to ensure we can handle currency and thousands
                if (destinationType == typeof(decimal))
                {
                    return ConvertDecimal(value);
                }

                return Convert.ChangeType(value, destinationType);
            }
            catch (Exception ex)
            {
                throw new ShipWorksOdbcException($"Unable to convert {value} to {destinationType} for {GetQualifiedName()}.", ex);
            }
        }

        /// <summary>
        /// Converts the given object to a decimal
        /// </summary>
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
