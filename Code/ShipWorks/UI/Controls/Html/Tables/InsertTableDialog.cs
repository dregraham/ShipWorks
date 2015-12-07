using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using Interapptive.Shared;
using ShipWorks.UI.Controls.Colors;

namespace ShipWorks.UI.Controls.Html.Tables
{
	/// <summary>
	/// Window for entering values for creating a new table
	/// </summary>
	class InsertTableDialog : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelRows;
        private System.Windows.Forms.Label labelColumns;
        private ShipWorks.UI.Controls.NumericTextBox rows;
        private System.Windows.Forms.Label labelWidth;
        private ShipWorks.UI.Controls.NumericTextBox width;
        private System.Windows.Forms.ComboBox widthType;
        private System.Windows.Forms.Label labelBorder;
        private ShipWorks.UI.Controls.NumericTextBox borderWidth;
        private System.Windows.Forms.ComboBox borderStyle;
        private System.Windows.Forms.Label labelSpacing;
        private ShipWorks.UI.Controls.NumericTextBox cellSpacing;
        private ShipWorks.UI.Controls.NumericTextBox cellPadding;
        private System.Windows.Forms.Label labelPadding;
        private System.Windows.Forms.GroupBox groupDimensions;
        private System.Windows.Forms.GroupBox groupBorders;
        private System.Windows.Forms.CheckBox borderCollapse;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private ShipWorks.UI.Controls.NumericTextBox columns;
        private ShipWorks.UI.Controls.Colors.ColorComboBox borderColor;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [NDependIgnoreLongMethod]
        private void InitializeComponent()
		{
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelRows = new System.Windows.Forms.Label();
            this.labelColumns = new System.Windows.Forms.Label();
            this.rows = new ShipWorks.UI.Controls.NumericTextBox();
            this.columns = new ShipWorks.UI.Controls.NumericTextBox();
            this.labelWidth = new System.Windows.Forms.Label();
            this.width = new ShipWorks.UI.Controls.NumericTextBox();
            this.widthType = new System.Windows.Forms.ComboBox();
            this.labelBorder = new System.Windows.Forms.Label();
            this.borderWidth = new ShipWorks.UI.Controls.NumericTextBox();
            this.borderStyle = new System.Windows.Forms.ComboBox();
            this.labelSpacing = new System.Windows.Forms.Label();
            this.cellSpacing = new ShipWorks.UI.Controls.NumericTextBox();
            this.cellPadding = new ShipWorks.UI.Controls.NumericTextBox();
            this.labelPadding = new System.Windows.Forms.Label();
            this.groupDimensions = new System.Windows.Forms.GroupBox();
            this.groupBorders = new System.Windows.Forms.GroupBox();
            this.borderCollapse = new System.Windows.Forms.CheckBox();
            this.borderColor = new ShipWorks.UI.Controls.Colors.ColorComboBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider();
            this.groupDimensions.SuspendLayout();
            this.groupBorders.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(130, 264);
            this.ok.Name = "ok";
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(214, 264);
            this.cancel.Name = "cancel";
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            // 
            // labelRows
            // 
            this.labelRows.Location = new System.Drawing.Point(22, 24);
            this.labelRows.Name = "labelRows";
            this.labelRows.Size = new System.Drawing.Size(38, 16);
            this.labelRows.TabIndex = 2;
            this.labelRows.Text = "Rows:";
            // 
            // labelColumns
            // 
            this.labelColumns.Location = new System.Drawing.Point(6, 50);
            this.labelColumns.Name = "labelColumns";
            this.labelColumns.Size = new System.Drawing.Size(66, 18);
            this.labelColumns.TabIndex = 3;
            this.labelColumns.Text = "Columns:";
            // 
            // rows
            // 
            this.rows.Location = new System.Drawing.Point(66, 24);
            this.rows.Name = "rows";
            this.rows.Size = new System.Drawing.Size(60, 21);
            this.rows.TabIndex = 4;
            this.rows.Text = "2";
            // 
            // columns
            // 
            this.columns.Location = new System.Drawing.Point(66, 50);
            this.columns.Name = "columns";
            this.columns.Size = new System.Drawing.Size(60, 21);
            this.columns.TabIndex = 5;
            this.columns.Text = "3";
            // 
            // labelWidth
            // 
            this.labelWidth.Location = new System.Drawing.Point(20, 78);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(42, 16);
            this.labelWidth.TabIndex = 6;
            this.labelWidth.Text = "Width:";
            // 
            // width
            // 
            this.width.Location = new System.Drawing.Point(66, 76);
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(60, 21);
            this.width.TabIndex = 7;
            this.width.Text = "100";
            // 
            // widthType
            // 
            this.widthType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.widthType.Items.AddRange(new object[] {
                                                           "Percent",
                                                           "Pixels"});
            this.widthType.Location = new System.Drawing.Point(130, 76);
            this.widthType.Name = "widthType";
            this.widthType.Size = new System.Drawing.Size(88, 21);
            this.widthType.TabIndex = 8;
            // 
            // labelBorder
            // 
            this.labelBorder.Location = new System.Drawing.Point(16, 24);
            this.labelBorder.Name = "labelBorder";
            this.labelBorder.Size = new System.Drawing.Size(46, 16);
            this.labelBorder.TabIndex = 9;
            this.labelBorder.Text = "Border:";
            // 
            // borderWidth
            // 
            this.borderWidth.Location = new System.Drawing.Point(66, 22);
            this.borderWidth.Name = "borderWidth";
            this.borderWidth.Size = new System.Drawing.Size(60, 21);
            this.borderWidth.TabIndex = 10;
            this.borderWidth.Text = "1";
            // 
            // borderStyle
            // 
            this.borderStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.borderStyle.Items.AddRange(new object[] {
                                                             "solid",
                                                             "dotted",
                                                             "dashed",
                                                             "double",
                                                             "groove",
                                                             "ridge",
                                                             "inset",
                                                             "outset",
                                                             "none"});
            this.borderStyle.Location = new System.Drawing.Point(128, 22);
            this.borderStyle.MaxDropDownItems = 20;
            this.borderStyle.Name = "borderStyle";
            this.borderStyle.Size = new System.Drawing.Size(88, 21);
            this.borderStyle.TabIndex = 11;
            // 
            // labelSpacing
            // 
            this.labelSpacing.Location = new System.Drawing.Point(16, 50);
            this.labelSpacing.Name = "labelSpacing";
            this.labelSpacing.Size = new System.Drawing.Size(84, 20);
            this.labelSpacing.TabIndex = 13;
            this.labelSpacing.Text = "Cell Spacing: ";
            // 
            // cellSpacing
            // 
            this.cellSpacing.Location = new System.Drawing.Point(92, 48);
            this.cellSpacing.Name = "cellSpacing";
            this.cellSpacing.Size = new System.Drawing.Size(60, 21);
            this.cellSpacing.TabIndex = 14;
            this.cellSpacing.Text = "0";
            // 
            // cellPadding
            // 
            this.cellPadding.Location = new System.Drawing.Point(92, 74);
            this.cellPadding.Name = "cellPadding";
            this.cellPadding.Size = new System.Drawing.Size(60, 21);
            this.cellPadding.TabIndex = 16;
            this.cellPadding.Text = "0";
            // 
            // labelPadding
            // 
            this.labelPadding.Location = new System.Drawing.Point(16, 76);
            this.labelPadding.Name = "labelPadding";
            this.labelPadding.Size = new System.Drawing.Size(84, 20);
            this.labelPadding.TabIndex = 15;
            this.labelPadding.Text = "Cell Padding:";
            // 
            // groupDimensions
            // 
            this.groupDimensions.Controls.Add(this.rows);
            this.groupDimensions.Controls.Add(this.columns);
            this.groupDimensions.Controls.Add(this.width);
            this.groupDimensions.Controls.Add(this.widthType);
            this.groupDimensions.Controls.Add(this.labelWidth);
            this.groupDimensions.Controls.Add(this.labelRows);
            this.groupDimensions.Controls.Add(this.labelColumns);
            this.groupDimensions.Location = new System.Drawing.Point(10, 8);
            this.groupDimensions.Name = "groupDimensions";
            this.groupDimensions.Size = new System.Drawing.Size(280, 110);
            this.groupDimensions.TabIndex = 17;
            this.groupDimensions.TabStop = false;
            this.groupDimensions.Text = "Dimensions";
            // 
            // groupBorders
            // 
            this.groupBorders.Controls.Add(this.borderCollapse);
            this.groupBorders.Controls.Add(this.borderColor);
            this.groupBorders.Controls.Add(this.borderWidth);
            this.groupBorders.Controls.Add(this.borderStyle);
            this.groupBorders.Controls.Add(this.labelBorder);
            this.groupBorders.Controls.Add(this.cellPadding);
            this.groupBorders.Controls.Add(this.labelPadding);
            this.groupBorders.Controls.Add(this.cellSpacing);
            this.groupBorders.Controls.Add(this.labelSpacing);
            this.groupBorders.Location = new System.Drawing.Point(10, 126);
            this.groupBorders.Name = "groupBorders";
            this.groupBorders.Size = new System.Drawing.Size(280, 132);
            this.groupBorders.TabIndex = 18;
            this.groupBorders.TabStop = false;
            this.groupBorders.Text = "Borders and Spacing";
            // 
            // borderCollapse
            // 
            this.borderCollapse.Checked = true;
            this.borderCollapse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.borderCollapse.Location = new System.Drawing.Point(18, 100);
            this.borderCollapse.Name = "borderCollapse";
            this.borderCollapse.Size = new System.Drawing.Size(124, 24);
            this.borderCollapse.TabIndex = 18;
            this.borderCollapse.Text = "Collapse borders";
            // 
            // borderColor
            // 
            this.borderColor.Color = System.Drawing.Color.Black;
            this.borderColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.borderColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.borderColor.Location = new System.Drawing.Point(218, 22);
            this.borderColor.Name = "borderColor";
            this.borderColor.Size = new System.Drawing.Size(52, 22);
            this.borderColor.TabIndex = 17;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // InsertTableDialog
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(302, 294);
            this.Controls.Add(this.groupBorders);
            this.Controls.Add(this.groupDimensions);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InsertTableDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Insert Table";
            this.Load += new System.EventHandler(this.OnLoad);
            this.groupDimensions.ResumeLayout(false);
            this.groupBorders.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        /// <summary>
        /// Constructor
        /// </summary>
		public InsertTableDialog()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, System.EventArgs e)
        {
            widthType.SelectedIndex = 0;
            borderStyle.SelectedIndex = 0;
        }

        /// <summary>
        /// Get the HTML tha represents the table the user selected
        /// </summary>
        public string TableHtml
        {
            get
            {
                int rowCount = 1;
                int colCount = 1;
                int tableWidth = 100;

                int border = 1;
                int spacing = 0;
                int padding = 0;

                try
                {
                    rowCount = Convert.ToInt32(rows.Text);
                    colCount = Convert.ToInt32(columns.Text);
                    tableWidth = Convert.ToInt32(width.Text);
                    border = Convert.ToInt32(borderWidth.Text);
                    spacing = Convert.ToInt32(cellSpacing.Text);
                    padding = Convert.ToInt32(cellPadding.Text);
                }
                catch (FormatException)
                {

                }

                StringBuilder tableHtml = new StringBuilder();

                // Open table
                tableHtml.AppendFormat("<table cellSpacing={0} cellPadding={1} style=\"border: {2}px {3} {4}; border-collapse: {5}; width: {6}{7}\" >",
                    spacing,
                    padding,
                    border,
                    (string) borderStyle.SelectedItem,
                    ColorTranslator.ToHtml(borderColor.Color),
                    borderCollapse.Checked ? "collapse" : "separate",
                    tableWidth,
                    widthType.SelectedIndex == 0 ? "%" : "px");

                // Add in each row
                for (int row = 0; row < rowCount; row++)
                {
                    tableHtml.Append("<tr>");

                    // Add in each column
                    for (int col = 0; col < colCount; col++)
                    {
                        tableHtml.AppendFormat("<td style=\"border: {0}px {1} {2};\" >&nbsp;</td>",
                            border,
                            (string) borderStyle.SelectedItem,
                            ColorTranslator.ToHtml(borderColor.Color));
                    }

                    tableHtml.Append("</tr>");
                }

                // Close the table
                tableHtml.Append("</table>");

                return tableHtml.ToString();
            }
        }

        /// <summary>
        /// User wants to use the current settings
        /// </summary>
        private void OnOK(object sender, System.EventArgs e)
        {
            int rowCount = 0;
            int colCount = 0;

            // Clear old errors
            errorProvider.SetError(rows, null);
            errorProvider.SetError(columns, null);

            try
            {
                rowCount = Convert.ToInt32(rows.Text);
                colCount = Convert.ToInt32(columns.Text);
            }
            catch (FormatException)
            {

            }

            if (rowCount == 0)
            {
                errorProvider.SetError(rows, "There must be at least one row.");
                return;
            }

            if (colCount == 0)
            {
                errorProvider.SetError(columns, "There must be at least one column.");
                return;
            }

            DialogResult = DialogResult.OK;
        }
	}
}
