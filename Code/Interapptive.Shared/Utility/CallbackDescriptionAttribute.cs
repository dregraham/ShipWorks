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
        private readonly string memberName;

        private PropertyInfo property;
        private MethodInfo method;
        private object instance;

        public CallbackDescriptionAttribute(string className, string memberName)
        {
            this.className = className;
            this.memberName = memberName;
        }

        /// <summary>
        /// Gets the description stored in this attribute.
        /// </summary>
        /// <returns>The description stored in this attribute.</returns>
        public override string Description
        {
            get
            {
                if (instance == null)
                {
                    // Instance not previously reflected.
                    ReflectObjectAndMember();
                }
                
                string descriptionToReturn;

                if (property!=null)
                {
                    descriptionToReturn = (string)property.GetValue(instance, null);
                }
                else
                {
                    descriptionToReturn = (string)method.Invoke(instance, null);
                }

                return descriptionToReturn;
            }
        }

        /// <summary>
        /// Reflects the object and member.
        /// </summary>
        private void ReflectObjectAndMember()
        {
            // Get the type defined by className.
            Type type = Assembly.GetEntryAssembly().GetType(className);

            // Create an instance of the type.
            instance = Activator.CreateInstance(type);

            // Get the property defined by methodName and determine if it exists.
            property = type.GetProperty(memberName);
            if (property == null)
            {
                // If the property called methodname doesn't exist, it must be a method.
                method = type.GetMethod(memberName);
            }
        }
    }
}
