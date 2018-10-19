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
        private static readonly DataTemplate emptyTemplate = new DataTemplate();
        private readonly Dictionary<Type, Func<DataTemplate>> templates = new Dictionary<Type, Func<DataTemplate>>();

        /// <summary>
        /// Select a template for a given item
        /// </summary>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return emptyTemplate;
            }

            Func<DataTemplate> templateCreator = null;
            if (templates.TryGetValue(item.GetType(), out templateCreator))
            {
                return templateCreator();
            }

            Type itemType = item.GetType();
            templateCreator = CreateTemplateCreator(itemType);

            templates.Add(itemType, templateCreator);

            return templateCreator();
        }

        /// <summary>
        /// Create a template for the type specified
        /// </summary>
        private Func<DataTemplate> CreateTemplateCreator(Type itemType)
        {
            var viewAttribute = Attribute.GetCustomAttribute(itemType, typeof(WpfViewAttribute)) as WpfViewAttribute;
            if (viewAttribute == null)
            {
                throw new InvalidOperationException($"A view must be specified for {itemType.Name}");
            }

            return () => new DataTemplate { VisualTree = new FrameworkElementFactory(viewAttribute.ViewType) };
        }
    }
}
