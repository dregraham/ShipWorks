using System;

namespace ShipWorks.UI
{
    /// <summary>
    /// Define a view that Wpf will use for the given class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WpfViewAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewType"></param>
        public WpfViewAttribute(Type viewType)
        {
            ViewType = viewType;
        }

        /// <summary>
        /// Type that will be used for the view
        /// </summary>
        public Type ViewType { get; }
    }
}
