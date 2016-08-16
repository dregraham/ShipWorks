using System;
using System.Windows;

namespace ShipWorks.UI.Controls.TypeBasedTemplateSelector
{
    /// <summary>
    /// Provides a link between a value and a <see cref="DataTemplate"/>
    /// for the <see cref="DynamicTemplateSelector"/>
    /// </summary>
    /// <remarks>
    /// In this case, our value is a <see cref="System.Type"/> which we are attempting to match
    /// to a <see cref="DataTemplate"/>
    /// </remarks>
    public class Template : DependencyObject
    {
        /// <summary>
        /// Provides the value used to match this <see cref="DataTemplate"/> to an item
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Type), typeof(Template));

        /// <summary>
        /// Provides the <see cref="DataTemplate"/> used to render items matching the <see cref="Value"/>
        /// </summary>
        public static readonly DependencyProperty DataTemplateProperty =
           DependencyProperty.Register("DataTemplate", typeof(DataTemplate), typeof(Template));

        /// <summary>
        /// Gets or Sets the value used to match this <see cref="DataTemplate"/> to an item
        /// </summary>
        public Type Value
        {
            get { return (Type) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or Sets the <see cref="DataTemplate"/> used to render items matching the <see cref="Value"/>
        /// </summary>
        public DataTemplate DataTemplate
        {
            get { return (DataTemplate) GetValue(DataTemplateProperty); }
            set { SetValue(DataTemplateProperty, value); }
        }
    }
}