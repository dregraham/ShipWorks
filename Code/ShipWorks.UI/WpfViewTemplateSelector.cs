using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.UI
{
    /// <summary>
    /// Template selector for a wizard step
    /// </summary>
    public class WpfViewTemplateSelector : DataTemplateSelector
    {
        static readonly DataTemplate emptyTemplate = new DataTemplate();
        readonly Dictionary<Type, DataTemplate> templates = new Dictionary<Type, DataTemplate>();

        /// <summary>
        /// Select a template for a given item
        /// </summary>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return emptyTemplate;
            }

            DataTemplate template = null;
            if (templates.TryGetValue(item.GetType(), out template))
            {
                return template;
            }

            Type itemType = item.GetType();
            template = CreateTemplate(itemType);

            templates.Add(itemType, template);

            return template;
        }

        /// <summary>
        /// Create a template for the type specified
        /// </summary>
        private DataTemplate CreateTemplate(Type itemType)
        {
            var viewAttribute = Attribute.GetCustomAttribute(itemType, typeof(WpfViewAttribute)) as WpfViewAttribute;
            if (viewAttribute == null)
            {
                throw new InvalidOperationException($"A view must be specified for {itemType.Name}");
            }

            return new DataTemplate
            {
                VisualTree = new FrameworkElementFactory(viewAttribute.ViewType)
            };
        }
    }
}
