using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Interapptive.Shared.Utility
{
    public class CallbackDescriptionAttribute:DescriptionAttribute
    {
        private readonly string className;
        private readonly string methodName;

        public CallbackDescriptionAttribute(string className, string methodName)
        {
            this.className = className;
            this.methodName = methodName;
        }

        /// <summary>
        /// Gets the description stored in this attribute.
        /// </summary>
        /// <returns>The description stored in this attribute.</returns>
        public override string Description
        {
            get
            {
                string descriptionToReturn;

                // Get the type defined by className.
                Type type = Assembly.GetEntryAssembly().GetType(className);

                // Create an instance of the type.
                object instance = Activator.CreateInstance(type);

                // Get the property defined by methodName and determine if it exists.
                PropertyInfo property = type.GetProperty(methodName);
                if (property != null)
                {
                    descriptionToReturn = (string)property.GetValue(instance, null);
                }
                else
                {
                    // If the property called methodname doesn't exist, it must be a method.
                    MethodInfo method = type.GetMethod(methodName);

                    descriptionToReturn = (string)method.Invoke(instance, null);
                }

                return descriptionToReturn;
            }
        }
    }
}
