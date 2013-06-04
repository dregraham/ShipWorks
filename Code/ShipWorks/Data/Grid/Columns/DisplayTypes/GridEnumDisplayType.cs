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

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Column display type that can show enum descriptions or images
    /// </summary>
    public class GridEnumDisplayType<T> : GridColumnDisplayType where T: struct
    {
        EnumSortMethod sortMethod;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GridEnumDisplayType(EnumSortMethod sortMethod)
        {
            this.sortMethod = sortMethod;
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
            if (value == null)
            {
                return null;
            }

            return EnumHelper.GetImage((Enum) value);
        }
    }
}
