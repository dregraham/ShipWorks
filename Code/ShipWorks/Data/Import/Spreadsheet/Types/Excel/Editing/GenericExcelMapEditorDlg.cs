﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Import.Spreadsheet;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Excel.Editing
{
    /// <summary>
    /// Window for editing an existing excel map
    /// </summary>
    public partial class GenericExcelMapEditorDlg : Form
    {
        GenericExcelMap map;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericExcelMapEditorDlg(GenericExcelMap map)
        {
            InitializeComponent();

            this.map = map;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            WindowStateSaver.Manage(this, WindowStateSaverOptions.SizeOnly);

            mapEditor.LoadMap(map);
        }

        /// <summary>
        /// OK'ing the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (mapEditor.SaveToMap())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
