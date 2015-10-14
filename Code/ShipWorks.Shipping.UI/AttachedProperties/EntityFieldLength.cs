using ShipWorks.Data.Utility;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShipWorks.Shipping.UI.AttachedProperties
{
    /// <summary>
    /// Attached property that will set the max length of a textbox based on the entity length
    /// </summary>
    public static class EntityFieldLength
    {
        private static readonly EntityFieldLengthProvider entityFieldLengthProvider = new EntityFieldLengthProvider();

        public static readonly DependencyProperty FieldNameProperty = DependencyProperty.RegisterAttached("FieldName", typeof(EntityFieldLengthSource),
                typeof(EntityFieldLength), new PropertyMetadata(EntityFieldLengthSource.None, OnNameChanged));

        /// <summary>
        /// Set the max length of the textbox based on the entity type
        /// </summary>
        private static void OnNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int length = EntityFieldLengthProvider.GetMaxLength((EntityFieldLengthSource)e.NewValue);

            SetTextMaxLength(d as TextBox, length);

            ComboBox comboBox = d as ComboBox;
            if (comboBox != null)
            {
                comboBox.Loaded -= SetComboBoxMaxLength;
                comboBox.Loaded += SetComboBoxMaxLength;
            }    
        }

        /// <summary>
        /// Set the length of the textbox in an editable combo box
        /// </summary>
        private static void SetComboBoxMaxLength(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null)
            {
                return;
            }

            int length = EntityFieldLengthProvider.GetMaxLength(GetFieldName(comboBox));
            SetTextMaxLength(FindChild(comboBox, "PART_EditableTextBox") as TextBox, length);
        }

        /// <summary>
        /// Get the current value of the property
        /// </summary>
        public static EntityFieldLengthSource GetFieldName(DependencyObject d) => (EntityFieldLengthSource)d.GetValue(FieldNameProperty);

        /// <summary>
        /// Set the current value of the property
        /// </summary>
        public static void SetFieldName(DependencyObject d, EntityFieldLengthSource value) => d.SetValue(FieldNameProperty, value);

        /// <summary>
        /// Set the max length of the specified textbox
        /// </summary>
        private static void SetTextMaxLength(TextBox textBox, int length)
        {
            if (textBox != null)
            {
                textBox.SetValue(TextBox.MaxLengthProperty, length);
            }
        }

        /// <summary>
        /// Looks for a child control within a parent by name
        /// </summary>
        public static DependencyObject FindChild(DependencyObject parent, string name)
        {
            // confirm parent and name are valid.
            if (parent == null || string.IsNullOrEmpty(name))
            {
                return null;
            }

            if (parent is FrameworkElement && (parent as FrameworkElement).Name == name)
            {
                return parent;
            }

            DependencyObject result = null;

            if (parent is FrameworkElement)
            {
                (parent as FrameworkElement).ApplyTemplate();
            }

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                result = FindChild(child, name);
                if (result != null)
                {
                    break;
                }
            }

            return result;
        }
    }
}
