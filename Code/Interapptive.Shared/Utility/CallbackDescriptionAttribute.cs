using System;
using System.ComponentModel;
using System.Reflection;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Allows for dynamic creation of enumeration description based the return of a class member.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false)]    
    public sealed class CallbackDescriptionAttribute : DescriptionAttribute
    {
        private readonly string className;
        private readonly string memberName;

        private PropertyInfo property;
        private MethodInfo method;
        private object instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackDescriptionAttribute"/> class.
        /// </summary>
        public CallbackDescriptionAttribute(string className, string memberName)
        {
            this.className = className;
            this.memberName = memberName;
        }

        /// <summary>
        /// Gets the name of the class being reflected on.
        /// </summary>
        public string ClassName
        {
            get { return className; }
        }

        /// <summary>
        /// Gets the name of the member being invoked.
        /// </summary>
        public string MemberName
        {
            get { return memberName; }
        }

        /// <summary>
        /// Gets the description stored in this attribute.
        /// </summary>
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
            Assembly assembly = Assembly.Load("ShipWorks.Core");
            Type type = assembly.GetType(className);

            // Create an instance of the type.
            instance = Activator.CreateInstance(type);

            // Get the property defined by memberName and determine if it exists.
            property = type.GetProperty(memberName);
            if (property == null)
            {
                // If the property doesn't exist, it must be a method.
                method = type.GetMethod(memberName);
            }
        }
    }
}
