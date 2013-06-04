using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators.Editors
{
    /// <summary>
    /// Editor for the grid rollup decorator settings
    /// </summary>
    public partial class GridRollupDecoratorEditor : GridColumnDecoratorEditor
    {
        GridRollupDecorator decorator;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridRollupDecoratorEditor(GridRollupDecorator decorator)
        {
            InitializeComponent();

            this.decorator = decorator;

            // Remove the UI for Mulitiple Identical if the column does not support it
            if (decorator.RollupStrategy == GridRollupStrategy.SingleChildOrNull)
            {
                multipleIdentical.Visible = false;
                labelMultipleIdentical.Visible = false;
                infotipMultipleIdentical.Visible = false;

                zeroItems.Top = multipleIdentical.Top;
                labelZeroItems.Top = labelMultipleIdentical.Top;
            }

            Height = zeroItems.Bottom + 10;

            multipleItems.Text = decorator.MultipleVariedFormat;
            multipleIdentical.Text = decorator.MultipleIdenticalFormat;
            zeroItems.Text = decorator.ZeroFormat;

            multipleItems.TextChanged += new EventHandler(OnChangeFormat);
            multipleIdentical.TextChanged += new EventHandler(OnChangeFormat);
            zeroItems.TextChanged += new EventHandler(OnChangeFormat);
        }

        /// <summary>
        /// The user has changed the format
        /// </summary>
        void OnChangeFormat(object sender, EventArgs e)
        {
            decorator.MultipleVariedFormat = multipleItems.Text;
            decorator.MultipleIdenticalFormat = multipleIdentical.Text;
            decorator.ZeroFormat = zeroItems.Text;
        }
    }
}
