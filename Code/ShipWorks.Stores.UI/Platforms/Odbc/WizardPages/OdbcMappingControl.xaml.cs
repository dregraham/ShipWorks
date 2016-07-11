﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    /// <summary>
    /// Interaction logic for OdbcMappingControl.xaml
    /// </summary>
    public partial class OdbcMappingControl
    {
        public OdbcMappingControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// When a new map is selected, reset the mapping grid scrollbar to the top
        /// </summary>
        private void SelectedFieldMapChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            MappingGridScrollbar.ScrollToTop();
        }
    }
}
