using System;
using System.IO;
using System.Xml.Serialization;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// String type extension methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks whether the string value is numeric
        /// </summary>
        public static bool IsNumeric(this string input)
        {
            long value;
            return long.TryParse(input, out value);
        }

        /// <summary>
        /// Returns null if the input is an empty string
        /// </summary>
        public static string NullIfEmpty(this string input) =>
            input == string.Empty ? null : input;

        /// <summary>
        /// Try deserializing JSON to an object
        /// </summary>
        public static bool TryParseJson<T>(this string input, out T result)
        {
            if (input.IsNullOrWhiteSpace())
            {
                result = default(T);
                return false;
            }

            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(input, settings);
            return success && result != null;
        }

        /// <summary>
        /// Try deserializing XML to an object
        /// </summary>
        public static bool TryParseXml<T>(this string objectData, out T result)
        {
            try
            {
                result = SerializationUtility.DeserializeFromXml<T>(objectData);
                return true;
            }
            catch (InvalidOperationException)
            {
                result = default(T);
                return false;
            }
        }

        /// <summary>
        /// Validate that the string is within the min and max allowed values
        /// </summary>
        /// <param name="input">the string to test</param>
        /// <param name="maxLength">the max length</param>
        /// <param name="minLength">the min length</param>
        /// <param name="errorMessage">the error to display</param>
        public static Result ValidateLength(this string input, int? maxLength, int? minLength, string errorMessage = "")
        {
            if (input?.Length > maxLength || input?.Length < minLength)
            {
                return Result.FromError(errorMessage);
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Capatalize forst letter of string. Return input if null or empty
        /// </summary>
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            return input[0].ToString().ToUpper() + input.Substring(1);
        }
    }
}