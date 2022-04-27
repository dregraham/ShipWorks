using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility class for working with enumerations.
    /// </summary>
    public static class EnumHelper
    {
        // Cache for reflection performance reasons
        static ConcurrentDictionary<Type, Dictionary<int, EnumMetadata>> enumMetadataCache = new ConcurrentDictionary<Type, Dictionary<int, EnumMetadata>>();
        static ConcurrentDictionary<Type, List<object>> enumEntryCache = new ConcurrentDictionary<Type, List<object>>();

        class EnumMetadata
        {
            public DescriptionAttribute DescriptionAttribute { get; set; }
            public DetailsAttribute DetailsAttribute { get; set; }
            public Image Image { get; set; }
            public WpfImageSourceAttribute WpfImageSourceAttribute { get; set; }
            public InternationalServiceAttribute InternationalServiceAttribute { get; set;}
            public bool Deprecated { get; set; }
            public int? SortOrder { get; set; }
            public string ApiValue { get; set; }
            public bool Hidden { get; internal set; }
            public int RawValue { get; internal set; }
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
        public static void BindComboBox<T>(ComboBox comboBox, Func<T, bool> includer) where T : struct
        {
            comboBox.DataSource = null;
            comboBox.DisplayMember = "Key";
            comboBox.ValueMember = "Value";
            comboBox.DataSource = GetEnumList(includer);
        }

        /// <summary>
        /// Gets a List of KeyValuePair where the key is the Description, and the Enum is the Value.
        /// </summary>
        public static IEnumerable<EnumEntry<T>> GetEnumList<T>() where T : struct
        {
            return GetEnumList<T>(null);
        }

        /// <summary>
        /// Gets a List of KeyValuePair where the key is the Description, and the Enum is the Value.
        /// </summary>
        public static IEnumerable<EnumEntry<T>> GetEnumList<T>(Func<T, bool> includer) where T : struct
        {
            IEnumerable<EnumEntry<T>> list = enumEntryCache.ContainsKey(typeof(T)) ?
                enumEntryCache[typeof(T)].OfType<EnumEntry<T>>().ToList() :
                BuildEnumList<T>();

            return new ReadOnlyCollection<EnumEntry<T>>(list
                .Where(x => includer == null || includer(x.Value))
                .ToList());
        }

        /// <summary>
        /// Get an Enum value by ApiValue
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

            if (TryGetEnumByApiValue<T>(apiValue, out var value) && value.HasValue)
            {
                return value.Value;
            }

            // If we didn't find out, throw
            throw new InvalidOperationException($"No matching Enum was found for Enum type '{typeof(T)}', apiValue '{apiValue}'.");
        }
        
        
        /// <summary>
        /// Trys to get the Enum by ApiValue
        /// </summary>
        /// <typeparam name="T">The type of Enum for which to find by ApiValue</typeparam>
        /// <param name="apiValue">The text on which to query</param>
        /// <param name="value">
        /// The first T enum whose ApiValue matches the provided apiValue
        /// If no T enum is found or apiValue is null, value will be null
        /// </param>
        /// <returns>True if apiValue is found. Otherwise null</returns>
        public static bool TryGetEnumByApiValue<T>(string apiValue, out T? value) where T : struct
        {
            if (string.IsNullOrWhiteSpace(apiValue))
            {
                value = null;
                return false;
            }

            // Iterate through each enum value and see if ApiValue matches
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                EnumMetadata metadata = GetEnumMetadata((Enum) (object) enumValue);
                if (!string.IsNullOrWhiteSpace(metadata.ApiValue) && metadata.ApiValue.ToLowerInvariant() == apiValue.ToLowerInvariant() && !metadata.Deprecated)
                {
                    value = enumValue;
                    return true;
                }
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Build the enum list that will be cached
        /// </summary>
        private static List<EnumEntry<T>> BuildEnumList<T>() where T : struct
        {
            List<EnumEntry<T>> list = Enum.GetValues(typeof(T))
                .OfType<T>()
                .Select(x => new
                {
                    Value = x,
                    Metadata = GetEnumMetadata((Enum) (object) x)
                })
                .Where(x => !x.Metadata.Deprecated && !x.Metadata.Hidden)
                .OrderBy(x => GetSortOrder(x.Value))
                .Select(x => new EnumEntry<T>(x.Value, x.Metadata.DescriptionAttribute))
                .ToList();

            enumEntryCache[typeof(T)] = list.Cast<object>().ToList();

            return list;
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
            DescriptionAttribute description = GetEnumMetadata(value).DescriptionAttribute;

            if (description == null)
            {
                throw new NullReferenceException($"No description attribute set for {value}");
            }

            return description.Description;
        }

        /// <summary>
        /// Get the international service attribute of the given enumerated value.
        /// </summary>
        public static InternationalServiceAttribute GetInternationalServiceAttribute(Enum value)
        {
            InternationalServiceAttribute internationalService = GetEnumMetadata(value).InternationalServiceAttribute;
            if (internationalService == null)
            {
                return new InternationalServiceAttribute(null, false);
            }

            return internationalService;
        }

        /// <summary>
        /// Gets the details of the given enumerated value
        /// </summary>
        public static string GetDetails(Enum value)
        {
            // The grid blindly grabs details for all enum fields. If we where to throw here,
            // we would either have to catch the error or have a second property that checks to see
            // if the details exists. Seeing that the first requires an error to control program flow
            // and the second would require using reflection twice (once to check existence and once to
            // get the value) I felt it best to return an empty string if there is no details attribute.

            return GetEnumMetadata(value).DetailsAttribute?.Details ?? string.Empty;
        }

        /// <summary>
        /// Gets whether or not the enum value has been marked as deprecated in ShipWorks
        /// </summary>
        public static bool GetDeprecated(Enum value)
        {
            return GetEnumMetadata(value).Deprecated;
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
        /// Get the WPF Image source for the given enum value
        /// </summary>
        public static string GetWpfImageSource(Enum value)
        {
            return GetEnumMetadata(value).WpfImageSourceAttribute?.ImageSource ?? string.Empty;
        }

        /// <summary>
        /// Returns the API enum value based on another enum's ApiValue
        /// </summary>
        public static TApiValue? GetApiValue<TApiValue>(Enum shipmentRoleType) where TApiValue : struct
        {
            return TryParseEnum<TApiValue>(GetApiValue(shipmentRoleType));
        }

        /// <summary>
        /// Given a string value, try to parse the enum.
        /// </summary>
        public static T? TryParseEnum<T>(string value) where T : struct
        {
            return TryParseEnum<T>(value, false);
        }

        /// <summary>
        /// Given a string value, try to parse the enum.
        /// </summary>
        public static T? TryParseEnum<T>(string value, bool ignoreCase) where T : struct
        {
            T result;
            return Enum.TryParse<T>(value, ignoreCase, out result) ? result : default(T?);
        }

        /// <summary>
        /// Get the metadata for the given enum value
        /// </summary>
        private static EnumMetadata GetEnumMetadata(Enum value)
        {
            Type enumType = value.GetType();

            var typeCache = enumMetadataCache.GetOrAdd(enumType, BuildEnumMetadata);

            EnumMetadata result;
            if (!typeCache.TryGetValue(Convert.ToInt32(value), out result))
            {
                throw new NotFoundException(string.Format("Value '{0}' is not valid for enum '{1}'.", value, enumType));
            }

            return result;
        }

        /// <summary>
        /// Build the enum metadata
        /// </summary>
        private static Dictionary<int, EnumMetadata> BuildEnumMetadata(Type enumType)
        {
            ValidateObfuscation(enumType);

            return enumType.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(BuildFieldMetadata)
                .GroupBy(x => x.RawValue)
                .ToDictionary(x => x.Key, x => x.Last());
        }

        /// <summary>
        /// Build metadata for an entry of an enum
        /// </summary>
        private static EnumMetadata BuildFieldMetadata(FieldInfo fieldInfo)
        {
            return new EnumMetadata
            {
                RawValue = Convert.ToInt32(fieldInfo.GetRawConstantValue()),
                DescriptionAttribute = (DescriptionAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)),
                DetailsAttribute = (DetailsAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(DetailsAttribute)),
                WpfImageSourceAttribute = (WpfImageSourceAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(WpfImageSourceAttribute)),
                Hidden = Attribute.GetCustomAttribute(fieldInfo, typeof(HiddenAttribute)) != null,
                Image = fieldInfo.GetCustomAttribute<ImageResourceAttribute>()?.ResourceImage,
                SortOrder = fieldInfo.GetCustomAttribute<SortOrderAttribute>()?.Position,
                Deprecated = fieldInfo.GetCustomAttribute<DeprecatedAttribute>() != null,
                ApiValue = fieldInfo.GetCustomAttribute<ApiValueAttribute>()?.ApiValue,
                InternationalServiceAttribute = (InternationalServiceAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(InternationalServiceAttribute))
            };
        }

        /// <summary>
        /// Validate that obfuscation attributes are correct
        /// </summary>
        private static void ValidateObfuscation(Type enumType)
        {
            ObfuscationAttribute obfuscate = (ObfuscationAttribute) Attribute.GetCustomAttribute(enumType, typeof(ObfuscationAttribute));

            if (obfuscate == null)
            {
                throw new InvalidOperationException("Cannot use EnumHelper on Enum not marked with ObfuscationAttribute.");
            }

            if (obfuscate.StripAfterObfuscation)
            {
                throw new InvalidOperationException("Enums using EnumHelper must set StripAfterObfuscation=false on ObfuscationAttribute");
            }
        }
    }
}
