using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility class for working with enumerations.
    /// </summary>
    public static class EnumHelper
    {
        // Cache for reflection performance reasons
        static Dictionary<Type, Dictionary<int, EnumMetadata>> enumMetadataCache = new Dictionary<Type, Dictionary<int, EnumMetadata>>();

        class EnumMetadata
        {
            public DescriptionAttribute DescriptionAttribute { get; set; }
            public Image Image { get; set; }
            public bool Deprecated { get; set; }
            public bool Hidden { get; set; }
            public int? SortOrder { get; set; }
            public string ApiValue { get; set; }
        }

        /// <summary>
        /// Bind the ComboBox to the specified Enum type.
        /// </summary>
        public static void BindComboBox<T>(ComboBox comboBox) where T : struct
        {
            BindComboBox<T>(comboBox, null);
        }

        /// <summary>
        /// Bind the ComboBox to the specified Enum type.
        /// </summary>
        public static void BindComboBox<T>(ComboBox comboBox, Func<T, bool> includer) where T: struct
        {
            BindComboBox<T>(comboBox, includer, false);
        }

        /// <summary>
        /// Bind the ComboBox to the specified Enum type.
        /// </summary>
        public static void BindComboBox<T>(ComboBox comboBox, Func<T, bool> includer, bool includeHidden) where T : struct
        {
            comboBox.DataSource = null;
            comboBox.DisplayMember = "Key";
            comboBox.ValueMember = "Value";
            comboBox.DataSource = GetEnumList<T>(includer, includeHidden);
        }

        /// <summary>
        /// Gets a List of KeyValuePair where the key is the Description, and the Enum is the Value.
        /// </summary>
        public static EnumList<T> GetEnumList<T>() where T : struct
        {
            return GetEnumList<T>(null);
        }

        /// <summary>
        /// Gets a List of KeyValuePair where the key is the Description, and the Enum is the Value.
        /// </summary>
        public static EnumList<T> GetEnumList<T>(Func<T, bool> includer) where T : struct
        {
            return GetEnumList(includer, false);
        }

        /// <summary>
        /// Gets a List of KeyValuePair where the key is the Description, and the Enum is the Value.
        /// </summary>
        public static EnumList<T> GetEnumList<T>(Func<T, bool> includer, bool includeHidden) where T : struct
        {
            EnumList<T> result = new EnumList<T>();

            // Add each enum to the list
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                if (includer != null && !includer(value))
                {
                    continue;
                }

                // Ensure we can get the metadata.  This would fail if the proper obfuscation attributes
                // are not setup.  We have to do it up front here because in WinForms binding, if the exception
                // is thrown while getting a property, it eats it.  So specifically when binding to a ComboBox
                // on an enum that was not configured properly, no exception was being seen.  When done here,
                // the exception will be thrown right when trying to bind in the first place.
                EnumMetadata metadata = GetEnumMetadata((Enum) (object) value);

                // We never show deprecated
                if (!metadata.Deprecated)
                {
                    // We only show hidden if asked
                    if (!metadata.Hidden || includeHidden)
                    {
                        result.Add(new EnumEntry<T>(value));
                    }
                }
            }

            result.Sort((left, right) => GetSortOrder(left.Value).CompareTo(GetSortOrder(right.Value)));

            return result;
        }

        /// <summary>
        /// Get an Enum value by ApiValue
        /// 
        /// </summary>
        /// <typeparam name="T">The type of Enum for which to find by ApiValue</typeparam>
        /// <param name="apiValue">The text on which to query</param>
        /// <returns>Returns the first T enum whose ApiValue matches the provided apiValue 
        /// If no T enum is found for apiValue, throw InvalidOperationException
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Throws ArgumentNullException if apiValue is null or white space  </exception>
        /// <exception cref="System.InvalidOperationException">Throws InvalidOperationException if no matching enum is found</exception>
        public static T GetEnumByApiValue<T>(string apiValue) where T : struct
        {
            if (string.IsNullOrWhiteSpace(apiValue))
            {
                throw new ArgumentNullException("apiValue");
            }

            // Iterate through each enum value and see if ApiValue matches
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                EnumMetadata metadata = GetEnumMetadata((Enum) (object) value);
                if (!string.IsNullOrWhiteSpace(metadata.ApiValue) && metadata.ApiValue.ToLowerInvariant() == apiValue.ToLowerInvariant() && !metadata.Deprecated)
                {
                    return value;
                }
            }

            // If we didn't find out, throw 
            throw new InvalidOperationException(string.Format("No matching Enum was found for Enum type '{0}', apiValue '{1}'.", typeof(T).ToString(), apiValue));
        }

        /// <summary>
        /// Get the effective sort order of the enum value relative to its other siblings
        /// </summary>
        private static int GetSortOrder(object enumValue)
        {
            var meta = GetEnumMetadata((Enum) enumValue);

            if (meta.SortOrder != null)
            {
                return meta.SortOrder.Value;
            }

            return (int) (object) enumValue;
        }

        /// <summary>
        /// Get the description of the given enumerated value.
        /// </summary>
        public static string GetDescription(Enum value)
        {
            return GetEnumMetadata(value).DescriptionAttribute.Description;
        }

        /// <summary>
        /// Gets whether or not the enum value has been marked as deprecated in ShipWorks
        /// </summary>
        public static bool GetDeprecated(Enum value)
        {
            return GetEnumMetadata(value).Deprecated;
        }

        /// <summary>
        /// Gets whether or not the enum value has been marked to be hidden from users by default
        /// </summary>
        public static bool GetHidden(Enum value)
        {
            return GetEnumMetadata(value).Hidden;
        }

        /// <summary>
        /// Gets the Image associated with the enum value, or null if no ImageResourceAttribute was applied.
        /// </summary>
        public static Image GetImage(Enum value)
        {
            return GetEnumMetadata(value).Image;
        }

        /// <summary>
        /// Get the Api Value of the given enumerated value.
        /// </summary>
        public static string GetApiValue(Enum value)
        {
            return GetEnumMetadata(value).ApiValue;
        }

        /// <summary>
        /// Get the metadata for the given enum value
        /// </summary>
        [NDependIgnoreLongMethod]
        private static EnumMetadata GetEnumMetadata(Enum value)
        {
            Type enumType = value.GetType();

            // See if its cached
            Dictionary<int, EnumMetadata> typeCache;
            if (!enumMetadataCache.TryGetValue(enumType, out typeCache))
            {
                ObfuscationAttribute obfuscate = (ObfuscationAttribute) Attribute.GetCustomAttribute(enumType, typeof(ObfuscationAttribute));
                if (obfuscate == null)
                {
                    throw new InvalidOperationException("Cannot use EnumHelper on Enum not marked with ObfuscationAttribute.");
                }

                if ((obfuscate.Feature != "PreserveLiteralValues" && obfuscate.Feature != "PreserveLiteralFields") || obfuscate.Exclude || obfuscate.StripAfterObfuscation)
                {
                    throw new InvalidOperationException("Enums using EnumHelper must set 'PreserveLiteralValues', Exclude=false, and StripAfterObfuscation=false on ObfuscationAttribute");
                }

                // If its not, we go ahead and read every value now
                typeCache = new Dictionary<int, EnumMetadata>();

                foreach (FieldInfo fieldInfo in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    EnumMetadata metadata = new EnumMetadata();

                    DescriptionAttribute attribute = (DescriptionAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
                    if (attribute == null)
                    {
                        throw new InvalidOperationException("Cannot use GetDescription on enum without DescriptionAttribute.");
                    }

                    metadata.DescriptionAttribute = attribute;

                    ImageResourceAttribute imageAttribute = (ImageResourceAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(ImageResourceAttribute));
                    if (imageAttribute != null)
                    {
                        metadata.Image = imageAttribute.ResourceImage;
                    }

                    SortOrderAttribute sortAttribute = (SortOrderAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(SortOrderAttribute));
                    if (sortAttribute != null)
                    {
                        metadata.SortOrder = sortAttribute.Position;
                    }

                    DeprecatedAttribute obsoleteAttribute = (DeprecatedAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(DeprecatedAttribute));
                    if (obsoleteAttribute != null)
                    {
                        metadata.Deprecated = true;
                    }

                    HiddenAttribute hiddenAttribute = (HiddenAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(HiddenAttribute));
                    if (hiddenAttribute != null)
                    {
                        metadata.Hidden = true;
                    }

                    ApiValueAttribute apiInfoAttribute = (ApiValueAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(ApiValueAttribute));
                    if (apiInfoAttribute != null)
                    {
                        metadata.ApiValue = apiInfoAttribute.ApiValue;
                    }

                    typeCache[Convert.ToInt32(fieldInfo.GetRawConstantValue())] = metadata;
                }

                // Cache it
                enumMetadataCache[enumType] = typeCache;
            }

            EnumMetadata result;
            if (!typeCache.TryGetValue(Convert.ToInt32(value), out result))
            {
                throw new NotFoundException(string.Format("Value '{0}' is not valid for enum '{1}'.", value, enumType));
            }

            return result;
        }
    }
}
