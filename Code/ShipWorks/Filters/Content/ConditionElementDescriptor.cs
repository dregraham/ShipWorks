using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using ShipWorks.Filters.Content.Conditions;
using System.Diagnostics;
using ShipWorks.Stores;
using System.Reflection;
using ShipWorks.Stores.Platforms;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// Represents metadata about a condition type that can be instantiated and used by the user.
    /// </summary>
    public class ConditionElementDescriptor
    {
        Type type;
        string displayName;
        string identifier;

        // The list of store types the element is valid for.  If the list is empty, its assumed valid for all.
        List<StoreTypeCode> storeTypesCodes = new List<StoreTypeCode>();

        MethodInfo applicableTest;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionElementDescriptor(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (!Attribute.IsDefined(type, typeof(ConditionElementAttribute)))
            {
                throw new InvalidOperationException("Cannot create ConditionElementDescriptor instance for class without ConditionElementAttribute.");
            }

            if (!type.IsClass)
            {
                throw new InvalidOperationException("Cannot create ConditionElementDescriptor instance for non-class.");
            }

            if (!type.IsSubclassOf(typeof(ConditionElement)))
            {
                throw new InvalidOperationException("Cannot create ConditionElementDescriptor instance for class not derived from ConditionElement.");
            }

            #if DEBUG

            bool validNamespace = 
                type.Namespace.StartsWith("ShipWorks.Filters.Content.Conditions") ||
                type.Namespace.EndsWith("CoreExtensions.Filters") ||
                type.Namespace.EndsWith("CoreExtensions.Filters.Orders");

            Debug.Assert(validNamespace,
                @"When obfuscated, only types in the above namespaces will work.  This is due to the xr option we use with demeanor.  If a type truly
                  should go in another namespace, then ensure it will work under obfuscation, and adjust this assert.");

            #endif

            this.type = type;

            // Load the attribute
            ConditionElementAttribute attribute = (ConditionElementAttribute) Attribute.GetCustomAttribute(type, typeof(ConditionElementAttribute));
            this.displayName = attribute.DisplayName;
            this.identifier = attribute.Identifier;
            
            // Determine the valid store types
            foreach (ConditionStoreTypeAttribute storeAttribute in Attribute.GetCustomAttributes(type, typeof(ConditionStoreTypeAttribute)))
            {
                storeTypesCodes.Add(storeAttribute.StoreType);
            }

            // Determine if there should be a function to test for applicability
            if (!string.IsNullOrEmpty(attribute.ApplicableTest))
            {
                applicableTest = type.GetMethod(attribute.ApplicableTest, BindingFlags.Public | BindingFlags.Static);
                if (applicableTest == null)
                {
                    throw new InvalidOperationException(string.Format("A public static method {0} could not be found on condition {1}", attribute.ApplicableTest, identifier));
                }

                if (applicableTest.ReturnType != typeof(bool))
                {
                    throw new InvalidOperationException("The applicable test must return a boolean.");
                }

                if (applicableTest.GetParameters().Length != 1 || applicableTest.GetParameters()[0].ParameterType != typeof(List<StoreType>))
                {
                    throw new InvalidOperationException("The applicable test must take a single parameter of type List<StoreType>");
                }
            }
        }

        /// <summary>
        /// String representation of the object.
        /// </summary>
        public override string ToString()
        {
            return DisplayName;
        }

        /// <summary>
        /// The user-visible name of the condition.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }

        /// <summary>
        /// An identifier that uniquely identifies the condition element.  Once applies, this identifier cannot be changed,
        /// as it is used in a key in the serialization process.
        /// </summary>
        public string Identifier
        {
            get
            {
                return identifier;
            }
        }

        /// <summary>
        /// The CLR type of the condition.
        /// </summary>
        public Type SystemType
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// The list of store types the condition applies to.  If empty, its assumed to be all.
        /// </summary>
        public List<StoreTypeCode> StoreTypes
        {
            get { return storeTypesCodes.ToList(); }
        }

        /// <summary>
        /// Determines if the condition is applicable given the current list of store types
        /// </summary>
        public bool IsApplicable(List<StoreType> storeTypes)
        {
            if (storeTypesCodes.Count > 0 && storeTypes.Select(t => t.TypeCode).Intersect(storeTypesCodes).Count() == 0)
            {
                return false;
            }

            if (applicableTest != null)
            {
                if (! (bool) applicableTest.Invoke(null, new object[] { storeTypes } ))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Create a new instance of the ConditionElement for this type.
        /// </summary>
        public ConditionElement CreateInstance()
        {
            return (ConditionElement) SystemType.GetConstructor(Type.EmptyTypes).Invoke(null);
        }
    }
}
