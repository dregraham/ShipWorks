using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Helper class to allow the designer to load abstract base class for UserControls
    /// http://www.platinumbay.com/blogs/dotneticated/archive/2008/01/05/designing-windows-forms-with-abstract-inheritance.aspx
    /// </summary>
    public class UserControlTypeDescriptionProvider : TypeDescriptionProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserControlTypeDescriptionProvider() : base(TypeDescriptor.GetProvider(typeof(UserControl)))
        {
        }

        /// <summary>
        /// Gets the UserControl type
        /// </summary>
        public override Type GetReflectionType(Type objectType, object instance)
        {
            return typeof(UserControl);
        }

        /// <summary>
        /// Creates an instance of the user control
        /// </summary>
        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            objectType = typeof(UserControl);
            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }
}
