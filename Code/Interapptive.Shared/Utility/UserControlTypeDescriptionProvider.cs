using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Helper class to allow the designer to load abstract base class for UserControls
    /// http://www.platinumbay.com/blogs/dotneticated/archive/2008/01/05/designing-windows-forms-with-abstract-inheritance.aspx
    /// </summary>
    public class UserControlTypeDescriptionProvider : TypeDescriptionProvider
    {
        public UserControlTypeDescriptionProvider() : base(TypeDescriptor.GetProvider(typeof(UserControl)))
        {
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            return typeof(UserControl);
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            objectType = typeof(UserControl);
            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }
}
