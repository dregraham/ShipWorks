using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ShipWorks.Core.UI;
using System.Reflection;

namespace ShipWorks.UI.Controls.MultiValueBinders
{
    public interface IMultiValue<T> : INotifyPropertyChanged
    {
        [Obfuscation(Exclude = true)]
        T PropertyValue
        {
            get; set;
        }

        /// <summary>
        /// Returns true if the number of distinct text values is greater than 1.
        /// </summary>
        [Obfuscation(Exclude = true)]
        bool IsMultiValued
        {
            get;
        }

        /// <summary>
        /// If the dataSource is not multi valued, saves Text to each of the items and updates IsMultiValued appropriately.
        /// </summary>
        void Save();
    }

    /// <summary>
    /// Class to help bind UI controls with a list of items.
    /// </summary>
    /// <typeparam name="TDataSource">The list of items on which to bind.</typeparam>
    /// <typeparam name="TProperty">The type of the property on which will be bound.</typeparam>
    public class GenericMultiValueBinder<TDataSource, TProperty> : IMultiValue<TProperty>
    {
        private PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private TProperty propertyValue;
        private bool isMultiValued;
        private readonly IEnumerable<TDataSource> dataSource;
        private readonly Func<TDataSource, TProperty> selectFunc;
        private readonly Action<TDataSource, TProperty> updateFunc;
        private readonly string propertyName;
        private readonly Func<TDataSource, bool> readOnly;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSource">Generic list of items on which to bind.</param>
        /// <param name="selectFunc">Function that returns a property value of TDataSource.  This is used to determine if the dataSource has distinct values. </param>
        /// <param name="updateFunc">Action that updates each of the items in dataSource.</param>
        public GenericMultiValueBinder(IEnumerable<TDataSource> dataSource, string propertyName, Func<TDataSource, TProperty> selectFunc, Action<TDataSource, TProperty> updateFunc, Func<TDataSource, bool> readOnly)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            this.dataSource = dataSource;
            this.selectFunc = selectFunc;
            this.updateFunc = updateFunc;
            this.propertyName = propertyName;
            this.readOnly = readOnly;

            // Determine if the dataSource is multivalued.
            IsMultiValued = DistinctPropertyValues.Count() > 1;

            // If it's not multi valued, set the Text property
            if (DistinctPropertyValues.Any() && !IsMultiValued)
            {
                propertyValue = DistinctPropertyValues.First();
            }
        }
        
        /// <summary>
        /// The text value to display.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public TProperty PropertyValue
        {
            get
            {
                return propertyValue;
            }
            set
            {
                handler.Set(propertyName, ref propertyValue, value);
                IsMultiValued = false;
                Save();
            }
        }

        /// <summary>
        /// Returns true if the number of distinct text values is greater than 1.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsMultiValued
        {
            get
            {
                return isMultiValued;
            }
            private set
            {
                handler.Set(nameof(IsMultiValued), ref isMultiValued, value);
            }
        }

        /// <summary>
        /// Helper method to get the number of distinct text values in dataSource.
        /// </summary>
        protected IEnumerable<TProperty> DistinctPropertyValues
        {
            get
            {
                if (dataSource.Any())
                {
                    return dataSource.Select(s => selectFunc(s)).Distinct();
                }
                else
                {
                    return Enumerable.Empty<TProperty>();
                }
            }
        }
        
        /// <summary>
        /// If the dataSource is not multi valued, saves Text to each of the items and updates IsMultiValued appropriately.
        /// </summary>
        public void Save()
        {
            if (!IsMultiValued)
            {
                dataSource.ToList().ForEach(s =>
                    {
                        if (!readOnly(s))
                        {
                            updateFunc(s, PropertyValue);
                        }
                    }
                );

                IsMultiValued = DistinctPropertyValues.Count() > 1;
            }
        }
    }
}
