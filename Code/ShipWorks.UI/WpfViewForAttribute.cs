using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using ShipWorks.ApplicationCore;

namespace ShipWorks.UI
{
    /// <summary>
    /// Define the view model for which the given view applies
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WpfViewForAttribute : Attribute
    {
        private static Lazy<IDictionary<Type, Type>> viewLookup = new Lazy<IDictionary<Type, Type>>(() => CreateViewLookup());

        /// <summary>
        /// Constructor
        /// </summary>
        public WpfViewForAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        /// <summary>
        /// Type for which the view will be displayed
        /// </summary>
        public Type ViewModelType { get; }

        /// <summary>
        /// Find a view for the given view model
        /// </summary>
        public static Type FindViewFor(Type itemType) =>
            viewLookup.Value.TryGetValue(itemType, out Type viewType) ? viewType : null;

        /// <summary>
        /// Create the view lookup
        /// </summary>
        private static IDictionary<Type, Type> CreateViewLookup() =>
            IoC.AllAssemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(Control).IsAssignableFrom(x))
                .Select(x => (View: x, ViewModel: (Attribute.GetCustomAttribute(x, typeof(WpfViewForAttribute)) as WpfViewForAttribute)?.ViewModelType))
                .Where(x => x.ViewModel != null)
                .ToDictionary(x => x.ViewModel, x => x.View);
    }
}
