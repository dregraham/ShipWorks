using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace ShipWorks.Core.UI
{
    /// <summary>
    /// Help manage INotifyPropertyChanged classes
    /// </summary>
    public class PropertyChangedHandler : IObservable<string>
    {
        private readonly Func<PropertyChangedEventHandler> getPropertyChanged;
        private readonly Func<PropertyChangingEventHandler> getPropertyChanging;
        private readonly object source;
        private readonly object lockObject = new object();
        private readonly IObservable<string> eventStream;
        private IObserver<string> observer;

        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyChangedHandler(object source, Func<PropertyChangedEventHandler> getPropertyChanged) : 
            this(source, getPropertyChanged, () => null)
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

            eventStream = Observable.Create<string>(x =>
            {
                observer = x;
                return Disposable.Create(() => observer = null);
            }).Publish().RefCount();
        }

        /// <summary>
        /// Set the value of a field for a property
        /// </summary>
        public bool Set<T>(string name, ref T field, T value)
        {
            return Set(name, ref field, value, false);
        }

        /// <summary>
        /// Set the value of a field for a property
        /// </summary>
        public bool Set<T>(string name, ref T field, T value, bool forceRaisePropertyChanged)
        {
            lock (lockObject)
            {
                if (!forceRaisePropertyChanged && Equals(field, value))
                {
                    return false;
                }

            	RaisePropertyChanging(name);
                field = value;
            }

            RaisePropertyChanged(name);

            return true;
        }

        /// <summary>
        /// Set the value of a field for a property
        /// </summary>
        public bool Set<T>(string name, Action<T> assignmentMethod, T getCurrentValue, T newValue)
        {
            return Set(name, assignmentMethod, getCurrentValue, newValue, false);
        }

        /// <summary>
        /// Set the value of a field for a property
        /// </summary>
        public bool Set<T>(string name, Action<T> assignmentMethod, T getCurrentValue, T newValue, bool forceRaisePropertyChanged)
        {
            lock (lockObject)
            {
                if (!forceRaisePropertyChanged && Equals(getCurrentValue, newValue))
                {
                    return false;
                }

                RaisePropertyChanging(name);
                assignmentMethod(newValue);
            }

            RaisePropertyChanged(name);

            return true;
        }

        /// <summary>
        /// Raise the changed event for the given property
        /// </summary>
        /// <remarks>This is public so that the event can be raised manually.</remarks>
        public virtual void RaisePropertyChanged(string propertyName)
        {
            observer?.OnNext(propertyName);
            getPropertyChanged()?.Invoke(source, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raise the property changed event
        /// </summary>
        protected virtual void RaisePropertyChanging(string propertyName) =>
            getPropertyChanging()?.Invoke(source, new PropertyChangingEventArgs(propertyName));

        /// <summary>
        /// Subscribe to the property changed stream
        /// </summary>
        public IDisposable Subscribe(IObserver<string> observer) => eventStream.Subscribe(observer);
    }
}