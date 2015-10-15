using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandGrid;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using System.Drawing;
using ShipWorks.Data.Model.EntityClasses;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml;
using System.IO;
using System.Linq;
using Interapptive.Shared;
using Microsoft.XmlDiffPatch;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Windows.Forms;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators.Editors;
using System.Diagnostics;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using static System.String;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Represents the data required to specify and edit a grid column type
    /// </summary>
    public abstract class GridColumnDisplayType : SerializableObject
    {
        // The persisted formatting data
        GridColumnFormatEntity formatEntity;

        // Alignment of the column
        StringAlignment alignment = StringAlignment.Near;

        // How the preview will be generated
        GridColumnPreviewInputType previewInputType = GridColumnPreviewInputType.Value;

        // Used to get the value of the entity that we will use for display
        GridColumnValueProvider valueProvider;

        // Decorators that allow composing and adorning additional display functionality
        List<GridColumnDisplayDecorator> decorators = new List<GridColumnDisplayDecorator>();

        // Optional transformation that can be applied to the incoming entity to the entity that the value will be retrieved from.
        Func<EntityBase2, EntityBase2> entityTransform = null;

        #region Loading \ Saving

        /// <summary>
        /// Load the settings for the the given ColumnID
        /// </summary>
        public void Initialize(GridColumnFormatEntity formatEntity, GridColumnValueProvider valueProvider)
        {
            if (formatEntity == null)
            {
                throw new ArgumentNullException("formatEntity");
            }

            if (valueProvider == null)
            {
                throw new ArgumentNullException("valueProvider");
            }

            this.formatEntity = formatEntity;
            this.valueProvider = valueProvider;

            #if DEBUG

            bool validNamespace =
                this.GetType().Namespace.StartsWith("ShipWorks.Data.Grid.Columns.DisplayTypes") ||
                this.GetType().Namespace.EndsWith("CoreExtensions.Grid");

            Debug.Assert(validNamespace,
                @"When obfuscated, only types in the above namespaces will work.  This is due to the xr option we use with demeanor.  If a type truly
                      should go in another namespace, then ensure it will work under obfuscation, and adjust this assert.");

            foreach (GridColumnDisplayDecorator decorator in decorators)
            {
                Debug.Assert(decorator.GetType().Namespace == "ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators",
                    @"When obfuscated, only types in the above namespaces will work.  This is due to the xr option we use with demeanor.  If a type truly
                          should go in another namespace, then ensure it will work under obfuscation, and adjust this assert.");
            }

            #endif


            DeserializeXml(formatEntity.Settings);
        }

        /// <summary>
        /// Save the column settings
        /// </summary>
        public void SaveSettings(SqlAdapter adapter)
        {
            if (formatEntity == null)
            {
                throw new InvalidOperationException("Cannot save settings for column that has not been loaded.");
            }

            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }

            formatEntity.Settings = SerializeXml("Settings");

            adapter.SaveAndRefetch(formatEntity);
        }

        /// <summary>
        /// Cancels any in-memory changes that have happened since the last load or SaveSettings
        /// </summary>
        public void CancelChanges()
        {
            Initialize(formatEntity, valueProvider);
        }

        /// <summary>
        /// Custom XML serialization
        /// </summary>
        public override void SerializeXml(XmlTextWriter xmlWriter)
        {
            base.SerializeXml(xmlWriter);

            if (decorators.Count > 0)
            {
                xmlWriter.WriteStartElement("Decorators");

                foreach (GridColumnDisplayDecorator decorator in decorators)
                {
                    xmlWriter.WriteStartElement("Decorator");
                    xmlWriter.WriteAttributeString("identifier", decorator.Identifier);

                    decorator.SerializeXml(xmlWriter);

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Deserialize the given XML from the specified XPath
        /// </summary>
        public override void DeserializeXml(XPathNavigator xpath)
        {
            base.DeserializeXml(xpath);

            // Attempt to load settings for each decorator
            foreach (GridColumnDisplayDecorator decorator in decorators)
            {
                XPathNavigator settings = xpath.SelectSingleNode(Format("//Decorator[@identifier = '{0}']", decorator.Identifier));
                if (settings != null)
                {
                    decorator.DeserializeXml(settings);
                }
            }
        }

        #endregion

        /// <summary>
        /// The alignment of the cell text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public StringAlignment Alignment
        {
            get { return alignment; }
            set { alignment = value; }
        }

        /// <summary>
        /// Controls how the preview will be generated
        /// </summary>
        [XmlIgnore]
        protected GridColumnPreviewInputType PreviewInputType
        {
            get { return previewInputType; }
            set { previewInputType = value; }
        }

        /// <summary>
        /// Optional transformation that can be applied to the incoming entity to the entity that the value will be retrieved from.
        /// </summary>
        [XmlIgnore]
        public Func<EntityBase2, EntityBase2> EntityTransform
        {
            get { return entityTransform; }
            set { entityTransform = value; }
        }

        /// <summary>
        /// Default width of the column in pixels
        /// </summary>
        [XmlIgnore]
        public virtual int DefaultWidth
        {
            get { return 100; }
        }

        /// <summary>
        /// Create a new GridColumn based on the configured settings
        /// </summary>
        public EntityGridColumn CreateGridColumn(GridColumnDefinition definition)
        {
            EntityGridColumn column = new EntityGridColumn(definition);

            // Apply the alignment settings
            column.CellHorizontalAlignment = alignment;
            column.HeaderHorizontalAlignment = alignment;

            return column;
        }

        /// <summary>
        /// Called to format the preview of the given value.  By default calls FormatValue, but can be overridden for special processing.
        /// </summary>
        public virtual GridColumnFormattedValue FormatPreview(object exampleValue)
        {
            switch (previewInputType)
            {
                case GridColumnPreviewInputType.Entity:
                    {
                        return FormatValue((EntityBase2) exampleValue);
                    }

                case GridColumnPreviewInputType.Value:
                    {
                        return GetFormattedResult(exampleValue, null);
                    }

                case GridColumnPreviewInputType.LiteralString:
                    {
                        Debug.Assert(exampleValue as string != null);
                        GridColumnFormattedValue formattedValue = new GridColumnFormattedValue(exampleValue, null, null) { Text = (exampleValue ?? "").ToString() };

                        ApplyDecoration(formattedValue);

                        return formattedValue;
                    }
            }

            throw new InvalidOperationException("Unhandled previewMode: " + previewInputType);
        }


        /// <summary>
        /// Get the content to display based on the value, entity, and this DisplayType's configuration.
        /// </summary>
        public GridColumnFormattedValue FormatValue(EntityBase2 entity)
        {
            if (entity != null && entityTransform != null)
            {
                entity = entityTransform(entity);
            }

            object value = GetEntityValue(entity);

            return GetFormattedResult(value, entity);
        }

        /// <summary>
        /// Format the given value into the formatted result for grid display.  Entity is the entity the value came from.  May
        /// be null in a prreview scenario.
        /// </summary>
        private GridColumnFormattedValue GetFormattedResult(object value, EntityBase2 entity)
        {
            GridColumnFormattedValue formattedValue = new GridColumnFormattedValue(value, entity, valueProvider.PrimaryField);

            formattedValue.Text = GetDisplayText(value);
            formattedValue.Image = GetDisplayImage(value);
            formattedValue.ForeColor = GetDisplayForeColor(value);
            formattedValue.FontStyle = GetDisplayFontStyle(value);
            formattedValue.ToolTipText = GetToolTip(value);

            ApplyDecoration(formattedValue);

            return formattedValue;
        }

        /// <summary>
        /// Gets the tool tip.
        /// </summary>
        public virtual string GetToolTip(object value)
        {
            return Empty;
        }

        internal string GetToolTipText(EntityGridRow row, EntityGridColumn column)
        {
            return row.GetFormattedValue(column).ToolTipText;
        }

        /// <summary>
        /// Apply the decoration of all our attached decorators
        /// </summary>
        private void ApplyDecoration(GridColumnFormattedValue formattedValue)
        {
            foreach (GridColumnDisplayDecorator decorator in decorators)
            {
                decorator.ApplyDecoration(formattedValue);
            }
        }

        /// <summary>
        /// Get the value to use for formatting from the given entity
        /// </summary>
        protected virtual object GetEntityValue(EntityBase2 entity)
        {
            if (entity == null)
            {
                return null;
            }

            return valueProvider.GetValue(entity);
        }

        /// <summary>
        /// Get the display text to use for the given value and entity
        /// </summary>
        protected virtual string GetDisplayText(object value)
        {
            if (value == null)
            {
                return Empty;
            }

            return Format(GetValueFormatString(), value);
        }

        /// <summary>
        /// Get the data format string to use when formatting the value for display.
        /// </summary>
        protected virtual string GetValueFormatString()
        {
            return "{0}";
        }

        /// <summary>
        /// Get the image, if any, to display
        /// </summary>
        protected virtual Image GetDisplayImage(object value)
        {
            return null;
        }

        /// <summary>
        /// Get the foreground color to use for the text.  Return null if the default is to be used.
        /// </summary>
        protected virtual Color? GetDisplayForeColor(object value)
        {
            return null;
        }

        /// <summary>
        /// Get the font style to use for the text.  Return null if the default is to be used.
        /// </summary>
        protected virtual FontStyle? GetDisplayFontStyle(object value)
        {
            return null;
        }

        /// <summary>
        /// Create the editor the user can use to edit properties of the type
        /// </summary>
        public virtual GridColumnDisplayEditor CreateEditor()
        {
            GridColumnDisplayEditor editor = new GridColumnDisplayEditor(this);

            // Add in the UI of all the decorators
            foreach (GridColumnDisplayDecorator decorator in decorators)
            {
                GridColumnDecoratorEditor decoratorEditor = decorator.CreateEditor();
                if (decoratorEditor != null)
                {
                    decoratorEditor.Top = editor.Controls.OfType<Control>().Max(c => c.Bottom) + 2;

                    editor.Controls.Add(decoratorEditor);
                    editor.Height = decoratorEditor.Bottom + 2;
                }
            }

            return editor;
        }

        /// <summary>
        /// Create the default value provider for the display type.  This is only used if the GridColumnDefinition is created using
        /// the constructor that takes a field as an argument.  Otherwise the user can override by specifying a specific GridColumnValueProvider instance.
        /// </summary>
        public virtual GridColumnValueProvider CreateDefaultValueProvider(EntityField2 field)
        {
            return new GridColumnFieldValueProvider(field);
        }

        /// <summary>
        /// Create the default sort provider for the display type.  This is only used if the GridColumnDefinition is created using
        /// a sort of an EntityField2 object.  If a GridColumnSortProvider is specified, it overrides what the display type returns.
        /// </summary>
        public virtual GridColumnSortProvider CreateDefaultSortProvider(EntityField2 field)
        {
            return new GridColumnSortProvider(field);
        }

        /// <summary>
        /// Decorate this display type with additional functionality provided by the specified decorator.  If a decorator is already
        /// present it is wrapped and decorated by this one.  The return value is "this" to make chaining this call easy, and to make
        /// using it form the column definition factories easy.
        /// </summary>
        public GridColumnDisplayType Decorate(GridColumnDisplayDecorator decorator)
        {
            if (decorator == null)
            {
                throw new ArgumentNullException("decorator");
            }

            if (decorators.Any(d => d.Identifier == decorator.Identifier))
            {
                throw new InvalidOperationException("A decorator with the Identifier '" + decorator.Identifier + "' is already present.");
            }

            decorators.Add(decorator);

            return this;
        }

        /// <summary>
        /// The mouse is moving over a cell defined by this column for the given row.
        /// </summary>
        internal virtual void OnCellMouseMove(EntityGridRow row, EntityGridColumn column, MouseEventArgs e)
        {
            foreach (GridColumnDisplayDecorator decorator in decorators)
            {
                decorator.OnCellMouseMove(row, column, e);
            }
        }

        /// <summary>
        /// The mouse is being pressed in a cell defined by this column.  Return false to cancel the press.
        /// </summary>
        internal virtual bool OnCellMouseDown(EntityGridRow row, EntityGridColumn column, MouseEventArgs e)
        {
            foreach (GridColumnDisplayDecorator decorator in decorators)
            {
                if (!decorator.OnCellMouseDown(row, column, e))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The mouse is being released in a cell defined by this column.
        /// </summary>
        internal virtual void OnCellMouseUp(EntityGridRow row, EntityGridColumn column, MouseEventArgs e)
        {
            foreach (GridColumnDisplayDecorator decorator in decorators)
            {
                decorator.OnCellMouseUp(row, column, e);
            }
        }

        /// <summary>
        /// The mouse has left the area of the cell for the given row
        /// </summary>
        internal virtual void OnCellMouseLeave(EntityGridRow row, EntityGridColumn column)
        {
            foreach (GridColumnDisplayDecorator decorator in decorators)
            {
                decorator.OnCellMouseLeave(row, column);
            }
        }
    }
}
