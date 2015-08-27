using System;
using System.ComponentModel;

namespace ShipWorks.Core.UI
{
    /// <summary>
    /// Help manage INotifyPropertyChanged classes
    /// </summary>
    public class PropertyChangedHandler
    {
        private readonly Func<PropertyChangedEventHandler> getPropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyChangedHandler(Func<PropertyChangedEventHandler> getPropertyChanged)
        {
            this.getPropertyChanged = getPropertyChanged;
        }

        /// <summary>
        /// Set the value of a field for a property
        /// </summary>
        public bool Set<T>(string name, ref T field, T value)
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(name);

            return true;
        }

        /// <summary>
        /// Raise the property changed event
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            getPropertyChanged()?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}