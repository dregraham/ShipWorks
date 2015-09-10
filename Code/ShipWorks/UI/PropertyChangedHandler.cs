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
        private readonly Func<PropertyChangingEventHandler> getPropertyChanging;
        private readonly object source;

        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyChangedHandler(object source, Func<PropertyChangedEventHandler> getPropertyChanged) : this(source, getPropertyChanged, () => null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyChangedHandler(object source, Func<PropertyChangedEventHandler> getPropertyChanged, Func<PropertyChangingEventHandler> getPropertyChanging)
        {
            this.source = source;
            this.getPropertyChanged = getPropertyChanged;
            this.getPropertyChanging = getPropertyChanging;
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

            OnPropertyChanging(name);
            field = value;
            OnPropertyChanged(name);

            return true;
        }

        /// <summary>
        /// Raise the property changed event
        /// </summary>
        protected virtual void OnPropertyChanging(string propertyName)
        {
            getPropertyChanging()?.Invoke(source, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Raise the property changed event
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            getPropertyChanged()?.Invoke(source, new PropertyChangedEventArgs(propertyName));
        }
    }
}