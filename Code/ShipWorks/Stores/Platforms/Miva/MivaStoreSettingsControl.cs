﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Miva
{
    /// <summary>
    /// Settings control for miva stores
    /// </summary>
    public partial class MivaStoreSettingsControl : StoreSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MivaStoreSettingsControl()
        {
            InitializeComponent();

            // load the encoding combo with values
            EnumHelper.BindComboBox<GenericStoreResponseEncoding>(encodingComboBox);
        }

        /// <summary>
        /// Load the store settings
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            MivaStoreEntity mivaStore = store as MivaStoreEntity;
            if (mivaStore == null)
            {
                throw new ArgumentException("A non-Miva store entity was supplied to MivaStoreSettingsControl.LoadStore", "store");
            }

            orderStatusControl.LoadStore(mivaStore);
            sebenzaOptions.LoadStore(mivaStore);

            GenericModuleStoreEntity genericStore = store as GenericModuleStoreEntity;
            if (genericStore != null)
            {
                encodingComboBox.SelectedValue = (GenericStoreResponseEncoding) genericStore.ModuleResponseEncoding;
            }
        }

        /// <summary>
        /// Save the settings in the control to the store
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            MivaStoreEntity mivaStore = store as MivaStoreEntity;
            if (mivaStore == null)
            {
                throw new ArgumentException("A non-Miva store entity was supplied to MivaStoreSettingsControl.SaveToEntity", "store");
            }

            bool baseValue = sebenzaOptions.SaveToEntity(mivaStore);

            if (baseValue)
            {
                baseValue = orderStatusControl.SaveToEntity(mivaStore);
            }

            if (baseValue)
            {
                if (encodingComboBox.SelectedValue != null)
                {
                    ((GenericModuleStoreEntity) store).ModuleResponseEncoding = (int) encodingComboBox.SelectedValue;
                }
            }

            return baseValue;
        }
    }
}