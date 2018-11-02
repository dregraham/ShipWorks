using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using ShipWorks.OrderLookup;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Row definition that can be hidden based on field layout information
    /// </summary>
    public class FieldLayoutRowDefinition : RowDefinition
    {
        [Obfuscation(Exclude = true)]
        public static readonly DependencyProperty DefaultHeightProperty =
            DependencyProperty.Register("DefaultHeight", typeof(GridLength), typeof(FieldLayoutRowDefinition),
                new PropertyMetadata(GridLength.Auto, OnDefaultHeightChanged));

        [Obfuscation(Exclude = true)]
        public static readonly DependencyProperty FieldIDProperty =
            DependencyProperty.Register("FieldID", typeof(SectionLayoutFieldIDs?), typeof(FieldLayoutRowDefinition),
                new PropertyMetadata(null, OnFieldIDChanged));

        /// <summary>
        /// Default height
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public GridLength DefaultHeight
        {
            get { return (GridLength) GetValue(DefaultHeightProperty); }
            set { SetValue(DefaultHeightProperty, value); }
        }

        /// <summary>
        /// Field ID
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public SectionLayoutFieldIDs? FieldID
        {
            get { return (SectionLayoutFieldIDs?) GetValue(FieldIDProperty); }
            set { SetValue(FieldIDProperty, value); }
        }

        /// <summary>
        /// Handle control initialization
        /// </summary>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            DataContextChanged += OnDataContextChanged;
        }

        /// <summary>
        /// Handle when the default height has changed
        /// </summary>
        private static void OnDefaultHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            UpdateHeight(d);

        /// <summary>
        /// Handle when the field ID has changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnFieldIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            UpdateHeight(d);

        /// <summary>
        /// Handle when the data context has changed
        /// </summary>
        private static void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) =>
            UpdateHeight(sender);

        /// <summary>
        /// Update the height of the row
        /// </summary>
        private static void UpdateHeight(object sender)
        {
            if (sender is FieldLayoutRowDefinition rowDefinition)
            {
                if (rowDefinition.DataContext is IOrderLookupFieldLayoutProviderHost fieldRepositoryProvider &&
                    rowDefinition.FieldID.HasValue)
                {
                    var fieldVisible = fieldRepositoryProvider.FieldLayoutProvider
                        .Fetch()
                        .SelectMany(x => x.SectionFields)
                        .Where(x => x.Id.Equals(rowDefinition.FieldID.Value))
                        .Any(x => x.Selected);

                    rowDefinition.Height = fieldVisible ? rowDefinition.DefaultHeight : new GridLength(0);
                }
                else
                {
                    rowDefinition.Height = rowDefinition.DefaultHeight;
                }
            }
        }
    }
}
