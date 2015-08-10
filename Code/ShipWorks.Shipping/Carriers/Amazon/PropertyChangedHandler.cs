using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Help manage INotifyPropertyChanged classes
    /// </summary>
    internal class PropertyChangedHandler
    {
        private readonly PropertyChangedEventHandler onPropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyChangedHandler(PropertyChangedEventHandler onPropertyChanged)
        {
            this.onPropertyChanged = onPropertyChanged;
        }

        /// <summary>
        /// Set the value of a field for a property
        /// </summary>
        public void Set<T>(Expression<Func<T>> func, ref T field, T value)
        {
            if (Equals(field, value))
            {
                return;
            }

            field = value;
            OnPropertyChanged(ObjectUtility.Nameof(func));
        }

        /// <summary>
        /// Raise the property changed event
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = onPropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}