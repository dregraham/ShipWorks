﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.GenericFile.WizardPages
{
    /// <summary>
    /// Wizard page for setting up the XML import
    /// </summary>
    public partial class GenericStoreXmlSetupPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreXmlSetupPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the 
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            e.Skip = GetStore<GenericFileStoreEntity>().FileFormat != (int) GenericFileFormat.Xml;
        }

        /// <summary>
        /// Stepping next from the control
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!xmlFormatControl.IsVerified)
            {
                MessageHelper.ShowInformation(this, "You must verify ShipWorks can load one of your XML files to continue.");
                e.NextPage = this;
                return;
            }

            if (!xmlFormatControl.SaveToEntity(GetStore<GenericFileStoreEntity>()))
            {
                e.NextPage = this;
                return;
            }
        }
    }
}
