using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using Interapptive.Shared.Utility;
using System.Drawing;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Properties;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.SortProviders;
using static System.String;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Column display type that can show enum descriptions or images
    /// </summary>
    public class GridEnumDisplayType<T> : GridColumnDisplayType where T: struct
    {
        EnumSortMethod sortMethod;
        bool showIcon = true;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GridEnumDisplayType(EnumSortMethod sortMethod)
        {
            this.sortMethod = sortMethod;
        }

        /// <summary>
        /// Create the editor to use
        /// </summary>
        public override GridColumnDisplayEditor CreateEditor()
        {
            // Only show our specific editor (which allows for image on\off) if there are any images present for this enum
            if (EnumHelper.GetEnumList<T>().Any(e => e.Image != null))
            {
                return new GridEnumDisplayEditor<T>(this);
            }

            return base.CreateEditor();
        }

        /// <summary>
        /// Create the default sort provider to use
        /// </summary>
        public override GridColumnSortProvider CreateDefaultSortProvider(EntityField2 field)
        {
            if (sortMethod == EnumSortMethod.Description)
            {
                return new GridColumnEnumDescriptionSortProvider<T>(field);
            }
            else
            {
                return base.CreateDefaultSortProvider(field);
            }
        }

        /// <summary>
        /// Convert the value into an enum type
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            object value = base.GetEntityValue(entity);

            return ConvertToEnum(value);
        }

        /// <summary>
        /// Convert the given value to an enum type
        /// </summary>
        private object ConvertToEnum(object value)
        {
            object enumValue = null;

            if (value != null)
            {
                List<T> enumValues = Enum.GetValues(typeof(T)).Cast<T>().Where(e => (int) (object) e == (int) value).ToList();

                if (enumValues.Count == 1)
                {
                    enumValue = enumValues[0];
                }
            }

            return enumValue;
        }

        /// <summary>
        /// Indicates if the icon representing the store type should be displayed
        /// </summary>
        public bool ShowIcon
        {
            get { return showIcon; }
            set { showIcon = value; }
        }

        /// <summary>
        /// Get the text to display for the given value
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            if (value == null)
            {
                return "";
            }

            return EnumHelper.GetDescription((Enum) value);
        }

        /// <summary>
        /// Get the image to use that is associated with the enum value
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            if (value == null || !showIcon)
            {
                return null;
            }

            return EnumHelper.GetImage((Enum) value);
        }

        /// <summary>
        /// Gets the tool tip.
        /// </summary>
        public override string GetToolTip(object value)
        {
            if (value == null)
            {
                return Empty;
            }

            var details = EnumHelper.GetDetails((Enum) value);
            return IsNullOrWhiteSpace(details) ? base.GetToolTip(value) : details;
        }
    }
}
