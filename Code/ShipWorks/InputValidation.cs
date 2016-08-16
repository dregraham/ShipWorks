﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ShipWorks
{
    public static class InputValidation<T>
       where T : IDataErrorInfo
    {
        private static readonly Dictionary<string, KeyValuePair<Func<T, object>, ValidationAttribute[]>> allValidators;

        /// <summary>
        /// Validate a single column in the source
        /// </summary>
        /// <remarks>
        /// Usually called from IErrorDataInfo.this[]</remarks>
        /// <param name="source">Instance to validate</param>
        /// <param name="columnName">Name of column to validate</param>
        /// <returns>Error messages separated by newline or string.Empty if no errors</returns>
        public static string Validate(T source, string columnName)
        {
            KeyValuePair<Func<T, object>, ValidationAttribute[]> validators;

            if (allValidators.TryGetValue(columnName, out validators))
            {
                object value = validators.Key(source);
                List<string> errors;

                try
                {
                    errors = validators.Value.Where(v => !v.IsValid(value)).Select(v => v.ErrorMessage ?? "").ToList();
                }
                catch (Exception ex)
                {
                    errors = new List<string>()
                    {
                        ex.Message
                    };
                }

                return string.Join(Environment.NewLine, errors);
            }

            return string.Empty;
        }

        /// <summary>
        /// Validate all columns in the source
        /// </summary>
        /// <param name="source">Instance to validate</param>
        /// <returns>List of all error messages. Empty list if no errors</returns>
        public static ICollection<string> Validate(T source)
        {
            List<string> messages = new List<string>();

            foreach (var validators in allValidators.Values)
            {
                object value = validators.Key(source);
                messages.AddRange(validators.Value.Where(v => !v.IsValid(value)).Select(v => v.ErrorMessage ?? ""));
            }

            return messages;
        }

        /// <summary>
        /// Get all validation attributes on a property
        /// </summary>
        /// <param name="property"></param>
        private static ValidationAttribute[] GetValidations(PropertyInfo property)
        {
            return (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), true);
        }

        /// <summary>
        /// Create a lambda to receive a property value
        /// </summary>
        /// <param name="property"></param>
        private static Func<T, object> CreateValueGetter(PropertyInfo property)
        {
            ParameterExpression instance = Expression.Parameter(typeof(T), "i");
            UnaryExpression cast = Expression.TypeAs(Expression.Property(instance, property), typeof(object));

            return (Func<T, object>)Expression.Lambda(cast, instance).Compile();
        }

        /// <summary>
        /// InputValidation
        /// </summary>
        static InputValidation()
        {
            allValidators = new Dictionary<string, KeyValuePair<Func<T, object>, ValidationAttribute[]>>();

            foreach (var property in typeof(T).GetProperties())
            {
                ValidationAttribute[] validations = GetValidations(property);

                if (validations.Length > 0)
                {
                    allValidators.Add(property.Name, new KeyValuePair<Func<T, object>, ValidationAttribute[]>(CreateValueGetter(property), validations));
                }
            }
        }
    }
}
