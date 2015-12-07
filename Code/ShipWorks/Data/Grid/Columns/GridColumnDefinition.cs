using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Stores.Platforms;
using Divelements.SandGrid;
using System.Linq;
using Interapptive.Shared;
using ShipWorks.Stores;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Defines the properties of a grid column
    /// </summary>
    public sealed class GridColumnDefinition
    {
        Guid columnGuid;
        string headerText;

        GridColumnValueProvider displayValueProvider;
        GridColumnSortProvider sortProvider;

        object exampleValue;

        bool defaultVisible;
        int pixelWidth = 0;

        ColumnAutoSizeMode autoSizeMode = ColumnAutoSizeMode.None;
        bool autoWrap = false;

        // If not null controls the only store type this column is displayed for
        StoreTypeCode? storeTypeCode = null;

        // If not null, restricts when the definition is applicable, based on stores
        GridColumnApplicableTest applicableTest;

        // The display type that controls who this column looks
        GridColumnDisplayType displayType;

        /// <summary>
        /// Core constructor of all common properties
        /// </summary>
        private GridColumnDefinition(
            string columnGuid,
            bool defaultVisible,
            GridColumnDisplayType displayType,
            string headerText,
            object exampleValue)
        {
            this.columnGuid = new Guid(columnGuid);
            this.defaultVisible = defaultVisible;
            this.displayType = displayType;
            this.headerText = headerText;
            this.exampleValue = exampleValue;
        }

        /// <summary>
        /// Constructor. Most basic. Not visible. 
        /// </summary>
        public GridColumnDefinition(
            string columnGuid,
            GridColumnDisplayType displayType,
            string headerText, 
            object exampleValue, 
            EntityField2 displayField) :
            this(
                columnGuid, 
                false, 
                displayType, 
                headerText, 
                exampleValue, 
                displayField)
        {

        }

        /// <summary>
        /// Constructor.  Not visible.  Adds different sortField from displayField.
        /// </summary>
        [NDependIgnoreTooManyParams]
        public GridColumnDefinition(
            string columnGuid, 
            GridColumnDisplayType displayType, 
            string headerText, 
            object exampleValue,
            EntityField2 displayField, 
            EntityField2 sortField) :
            this(
                columnGuid, 
                false,
                displayType, 
                headerText, 
                exampleValue,
                displayField,
                sortField)
        {

        }

        /// <summary>
        /// Constructor.  Not visible. Explicitly provide the value providers.
        /// </summary>
        [NDependIgnoreTooManyParams]
        public GridColumnDefinition(
            string columnGuid,
            GridColumnDisplayType displayType,
            string headerText,
            object exampleValue,
            GridColumnValueProvider displayValueProvider,
            GridColumnSortProvider sortProvider) :
            this(
                columnGuid,
                false,
                displayType,
                headerText,
                exampleValue,
                displayValueProvider,
                sortProvider)
        {

        }

        /// <summary>
        /// Constructor. Most basic.  Specify visibility.  
        /// </summary>
        [NDependIgnoreTooManyParams]
        public GridColumnDefinition(
            string columnGuid,
            bool defaultVisible,
            GridColumnDisplayType displayType,
            string headerText,
            object exampleValue,
            EntityField2 displayField) :
            this(
                columnGuid,
                defaultVisible,
                displayType,
                headerText,
                exampleValue,
                displayField,
                displayField)
        {

        }

        /// <summary>
        /// Constructor. Specify visibility.  Adds different sortField from displayField.
        /// </summary>
        [NDependIgnoreTooManyParams]
        public GridColumnDefinition(
            string columnGuid, 
            bool defaultVisible,
            GridColumnDisplayType displayType, 
            string headerText, 
            object exampleValue,
            EntityField2 displayField, 
            EntityField2 sortField) :
            this(
                columnGuid, 
                defaultVisible, 
                displayType,
                headerText, 
                exampleValue)
        {
            // If the sort field is null, we just use the display field
            if ((object) sortField == null)
            {
                sortField = displayField;
            }

            displayValueProvider = displayType.CreateDefaultValueProvider(displayField);
            sortProvider = displayType.CreateDefaultSortProvider(sortField);
        }

        /// <summary>
        /// Constructor.  Specify visibility and explicitly provide the value providers.
        /// </summary>
        [NDependIgnoreTooManyParams]
        public GridColumnDefinition(
            string columnGuid,
            bool defaultVisible,
            GridColumnDisplayType displayType,
            string headerText,
            object exampleValue,
            GridColumnValueProvider displayValueProvider,
            GridColumnSortProvider sortProvider) :
            this(
                columnGuid,
                defaultVisible,
                displayType,
                headerText,
                exampleValue)
        {
            this.displayValueProvider = displayValueProvider;
            this.sortProvider = sortProvider;
        }

        /// <summary>
        /// Convenience setter for setting the EntityTransform of both the DisplayType and SortProvider at the same time.
        /// </summary>
        public Func<EntityBase2, EntityBase2> EntityTransform
        {
            set
            {
                displayType.EntityTransform = value;
                sortProvider.EntityTransform = value;
            }
        }

        /// <summary>
        /// Create a new grid column based on this definition
        /// </summary>
        public EntityGridColumn CreateGridColumn()
        {
            EntityGridColumn column = DisplayType.CreateGridColumn(this);

            return column;
        }

        /// <summary>
        /// The ShipWorks assigned guid of the column
        /// </summary>
        public Guid ColumnGuid
        {
            get { return columnGuid; }
        }

        /// <summary>
        /// Provides the value to use for display
        /// </summary>
        public GridColumnValueProvider DisplayValueProvider
        {
            get { return displayValueProvider; }
        }

        /// <summary>
        /// Provides the value to use for sorting
        /// </summary>
        public GridColumnSortProvider SortProvider
        {
            get { return sortProvider; }
        }
        
        /// <summary>
        /// The text to display in the column header
        /// </summary>
        public string HeaderText
        {
            get { return headerText; }
        }

        /// <summary>
        /// The column display type to use
        /// </summary>
        public GridColumnDisplayType DisplayType
        {
            get { return displayType; }
        }

        /// <summary>
        /// An example of the a value this column may display. Use for displayType previewing.
        /// </summary>
        public object ExampleValue
        {
            get { return exampleValue; }
        }

        /// <summary>
        /// Controls if the column is visible by default
        /// </summary>
        public bool DefaultVisible
        {
            get { return defaultVisible; }
        }

        /// <summary>
        /// The width of the column by default
        /// </summary>
        public int DefaultWidth
        {
            get 
            {
                if (pixelWidth > 0)
                {
                    return pixelWidth;
                }

                return displayType.DefaultWidth;
            }
            set
            {
                pixelWidth = value;
            }
        }

        /// <summary>
        /// The column autosizing mode.
        /// </summary>
        public ColumnAutoSizeMode AutoSizeMode
        {
            get { return autoSizeMode; }
            set { autoSizeMode = value; }
        }

        /// <summary>
        /// If true, the text in the column is automatically wrapped, and the row made a height to accomodate.
        /// </summary>
        public bool AutoWrap
        {
            get { return autoWrap; }
            set { autoWrap = value; }
        }

        /// <summary>
        /// If not null, controls the only StoreTypeCode that this column will display for.
        /// </summary>
        public StoreTypeCode? StoreTypeCode
        {
            get { return storeTypeCode; }
            set { storeTypeCode = value; }
        }

        /// <summary>
        /// Indicates if the name of the store type should be displayed in the definition header text
        /// </summary>
        public bool ShowStoreTypeInHeader
        {
            get
            {
                return StoreTypeCode != null && StoreManager.GetUniqueStoreTypes().Count > 1;
            }
        }

        /// <summary>
        /// Test to be applies to see if the definition is valid for the current set of stores.
        /// </summary>
        public GridColumnApplicableTest ApplicableTest
        {
            get { return applicableTest; }
            set { applicableTest = value; }
        }

        /// <summary>
        /// Determines if the definition is applicable given the current set of stores
        /// </summary>
        public bool IsApplicable(object contextData)
        {
            IList<StoreType> storeTypes = StoreManager.GetUniqueStoreTypes();

            if (storeTypeCode != null && !storeTypes.Select(t => t.TypeCode).Contains(storeTypeCode.Value))
            {
                return false;
            }

            if (applicableTest != null)
            {
                if (!applicableTest(contextData))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// When definitions are reused by multiple definition sets, they must still have unique Guids.  So this changes the guid of
        /// this definition so its unique within it's derived set.
        /// </summary>
        public void MarkAsDerived()
        {
            // Remove the first 6 chars
            string old = columnGuid.ToString("N").Remove(0, 6);

            // Has to be a reproducable replacement (we can't just use Guid.NewGuid), b\c this value has to be the same every time.
            columnGuid = new Guid(old + "A1B2C3");
        }
    }
}
