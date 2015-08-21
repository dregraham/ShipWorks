using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ShipWorks.Shipping.Carriers.Amazon;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Class responsible for handling the logic for loading and saving text data.  
    /// </summary>
    /// <typeparam name="T">The type of the datasource used.</typeparam>
    public class TextMultiValueBinder<T> : INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;

        private string text;
        private bool isMultiValued;
        private readonly IEnumerable<T> dataSource;

        private readonly Func<T, string> selectFunc;
        private readonly Action<T, string> updateFunc;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSource">Generic list of items on which to bind.</param>
        /// <param name="selectFunc">Function that returns a property value of T.  This is used to determine if the dataSource has distinct values. </param>
        /// <param name="updateFunc">Action that updates each of the items in dataSource.</param>
        public TextMultiValueBinder(IEnumerable<T> dataSource, Func<T, string> selectFunc, Action<T, string> updateFunc)
        {
            handler = new PropertyChangedHandler(PropertyChanged);
            this.dataSource = dataSource;
            this.selectFunc = selectFunc;
            this.updateFunc = updateFunc;

            // Determine if the dataSource is multivalued.
            IsMultiValued = DistinctTexts.Count() > 1;

            // If it's not multi valued, set the Text property
            if (!IsMultiValued)
            {
                Text = DistinctTexts.First();
            }
        }

        /// <summary>
        /// The text value to display.
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                handler.Set(() => Text, ref text, value);
                IsMultiValued = false;
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
        private IEnumerable<string> DistinctTexts
        {
            get
            {
                if (dataSource.Any())
                {
                    return dataSource.Select(s => selectFunc(s)).Distinct();
                }
                else
                {
                    return Enumerable.Empty<string>();
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
                dataSource.ToList().ForEach(s => updateFunc(s, Text));

                IsMultiValued = DistinctTexts.Count() > 1;
            }
        }
    }
}
