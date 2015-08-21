﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ShipWorks.Shipping.Carriers.Amazon;

namespace ShipWorks.UI.Controls.MultiValueBinders
{
    /// <summary>
    /// Class to help bind UI controls with a list of items.
    /// </summary>
    /// <typeparam name="TDataSource">The list of items on which to bind.</typeparam>
    /// <typeparam name="TProperty">The type of the property on which will be bound.</typeparam>
    public class GenericMultiValueBinder<TDataSource, TProperty> : INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private TProperty propertyValue;
        private bool isMultiValued;
        private readonly IEnumerable<TDataSource> dataSource;
        private readonly Func<TDataSource, TProperty> selectFunc;
        private readonly Action<TDataSource, TProperty> updateFunc;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSource">Generic list of items on which to bind.</param>
        /// <param name="selectFunc">Function that returns a property value of TDataSource.  This is used to determine if the dataSource has distinct values. </param>
        /// <param name="updateFunc">Action that updates each of the items in dataSource.</param>
        public GenericMultiValueBinder(IEnumerable<TDataSource> dataSource, Func<TDataSource, TProperty> selectFunc, Action<TDataSource, TProperty> updateFunc)
        {
            handler = new PropertyChangedHandler(PropertyChanged);
            this.dataSource = dataSource;
            this.selectFunc = selectFunc;
            this.updateFunc = updateFunc;

            // Determine if the dataSource is multivalued.
            IsMultiValued = DistinctPropertyValues.Count() > 1;

            // If it's not multi valued, set the Text property
            if (DistinctPropertyValues.Any() && !IsMultiValued)
            {
                PropertyValue = DistinctPropertyValues.First();
            }
        }

        /// <summary>
        /// The text value to display.
        /// </summary>
        public TProperty PropertyValue
        {
            get
            {
                return propertyValue;
            }
            set
            {
                handler.Set(() => PropertyValue, ref propertyValue, value);
                IsMultiValued = false;
                Save();
            }
        }

        /// <summary>
        /// Returns true if the number of distinct text values is greater than 1.
        /// </summary>
        public bool IsMultiValued
        {
            get
            {
                return isMultiValued;
            }
            private set
            {
                handler.Set(() => IsMultiValued, ref isMultiValued, value);
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
                dataSource.ToList().ForEach(s => updateFunc(s, PropertyValue));

                IsMultiValued = DistinctPropertyValues.Count() > 1;
            }
        }
    }
}
