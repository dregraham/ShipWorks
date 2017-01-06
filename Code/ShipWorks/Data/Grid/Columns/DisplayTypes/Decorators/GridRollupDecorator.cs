using System;
using System.Xml.Serialization;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators.Editors;
using ShipWorks.Properties;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators
{
    /// <summary>
    /// Decorator that is applied to columns whose values are a result of rolling up multiple child rows of data.
    /// </summary>
    public class GridRollupDecorator : GridColumnDisplayDecorator
    {
        EntityField2 childCountField;
        GridRollupStrategy rollupStrategy = GridRollupStrategy.SingleChildOrNull;

        string multipleVariedFormat = "Multiple (#)";
        string multipleIdenticalFormat = "* (#)";
        string zeroFormat = "";

        bool showMultiImage = false;

        /// <summary>
        /// Create a new instance of the rollup decorator
        /// </summary>
        public GridRollupDecorator(EntityField2 childCountField)
            : this(childCountField, GridRollupStrategy.SingleChildOrNull)
        {

        }

        /// <summary>
        /// Create a new instance of the rollup decorator
        /// </summary>
        public GridRollupDecorator(EntityField2 childCountField, GridRollupStrategy rollupStrategy)
        {
            this.childCountField = childCountField;
            this.rollupStrategy = rollupStrategy;

            Identifier = "Rollup";
        }

        /// <summary>
        /// The strategy used to rollup the column values.  This is necessary in order to present the appropriate formatting UI.
        /// </summary>
        [XmlIgnore]
        public GridRollupStrategy RollupStrategy
        {
            get { return rollupStrategy; }
            set { rollupStrategy = value; }
        }

        /// <summary>
        /// Format displayed when there are multiple child items with different values
        /// </summary>
        public string MultipleVariedFormat
        {
            get { return multipleVariedFormat; }
            set { multipleVariedFormat = value; }
        }

        /// <summary>
        /// Format displayed when there are multiple child items that all have the same value
        /// </summary>
        public string MultipleIdenticalFormat
        {
            get { return multipleIdenticalFormat; }
            set { multipleIdenticalFormat = value; }
        }

        /// <summary>
        /// Format displayed when there are zero child items
        /// </summary>
        public string ZeroFormat
        {
            get { return zeroFormat; }
            set { zeroFormat = value; }
        }

        /// <summary>
        /// Shows an image representing multiple different items when the image is null and count is greater than one.
        /// </summary>
        public bool ShowMultiImage
        {
            get { return showMultiImage; }
            set { showMultiImage = value; }
        }

        /// <summary>
        /// Create the editor for this decorator
        /// </summary>
        public override GridColumnDecoratorEditor CreateEditor()
        {
            return new GridRollupDecoratorEditor(this);
        }

        /// <summary>
        /// Decorate the given formatted value
        /// </summary>
        public override void ApplyDecoration(GridColumnFormattedValue formattedValue)
        {
            object countValue = null;

            // Can be null when its a preview
            if (formattedValue.Entity != null)
            {
                countValue = EntityUtility.GetFieldValue(formattedValue.Entity, childCountField);
            }
            else
            {
                countValue = 1;
            }

            // If the count is null, then that means we're probably dealing with a row for a store type that isn't for the count field type.
            // Like if we are rolling up an ebay column and this isn't an ebay store.
            if (countValue != null)
            {
                formattedValue.Text = FormatRollupText(Convert.ToInt64(countValue), formattedValue);

                if (ShowMultiImage)
                {
                    if ((int) countValue > 1 && formattedValue.Image == null)
                    {
                        formattedValue.Image = Resources.flower_greengray;
                    }
                }
            }
        }

        /// <summary>
        /// Get the rollup display based on the child count and what the normal base text would be
        /// </summary>
        protected string FormatRollupText(long childCount, GridColumnFormattedValue formattedValue)
        {
            if (childCount == 0)
            {
                return zeroFormat;
            }

            if (childCount > 1)
            {
                if (rollupStrategy == GridRollupStrategy.SameValueOrNull && formattedValue.Value != null && formattedValue.Text != null)
                {
                    // Special case... don't show a blank followed by the formatting count.
                    if (formattedValue.Text.Trim().Length == 0)
                    {
                        return "";
                    }
                    else
                    {
                        return multipleIdenticalFormat.Replace("*", formattedValue.Text).Replace("#", childCount.ToString());
                    }
                }
                else
                {
                    return multipleVariedFormat.Replace("#", childCount.ToString());
                }
            }

            return formattedValue.Text;
        }
    }
}
