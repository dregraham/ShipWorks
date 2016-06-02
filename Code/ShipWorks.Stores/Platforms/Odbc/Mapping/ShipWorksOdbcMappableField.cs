using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// The ShipWorks half of an OdbcFieldMapEntry
    /// </summary>
    public class ShipWorksOdbcMappableField : IOdbcMappableField
	{
	    private readonly EntityField2 field;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcMappableField"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="displayName">The display name.</param>
        [JsonConstructor]
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
	        this.field = field;
            ContainingObjectName = field?.ContainingObjectName;
            Name = field?.Name;
	        DisplayName = displayName;
            IsRequired = isRequired;
	    }

        /// <summary>
        /// The name of the object that contains this field
        /// </summary>
        public string ContainingObjectName { get; set; }

        /// <summary>
        /// The name of the field
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The fields value
        /// </summary>
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
            try
            {
                return Convert.ChangeType(value, field.DataType);
            }
            catch (Exception ex)
            {
                throw new ShipWorksOdbcException($"Unable to convert {value} to {field.DataType} for {GetQualifiedName()}.", ex);
            }
        }
    }
}
