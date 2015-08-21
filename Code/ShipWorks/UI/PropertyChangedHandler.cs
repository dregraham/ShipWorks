using System.ComponentModel;

namespace ShipWorks.Core.UI
{
    /// <summary>
    /// Help manage INotifyPropertyChanged classes
    /// </summary>
    public class PropertyChangedHandler
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
        public void Set<T>(string name, ref T field, T value)
        {
            if (Equals(field, value))
            {
                return;
            }

            field = value;
            OnPropertyChanged(name);
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