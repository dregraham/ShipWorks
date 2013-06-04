using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.UI.Controls;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Base class for user controls for editing shipping profiles
    /// </summary>
    public partial class ShippingProfileControlBase : ShippingProfileControlCore
    {
        ShippingProfileEntity profile;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The profile that has been loaded
        /// </summary>
        protected ShippingProfileEntity Profile
        {
            get
            {
                return profile;
            }
        }

        /// <summary>
        /// Load the given profile
        /// </summary>
        public virtual void LoadProfile(ShippingProfileEntity profile)
        {
            this.profile = profile;

            base.AllowChangeCheckState = !profile.ShipmentTypePrimary;
        }

        /// <summary>
        /// Overridden by derived class to save the profile information to the loaded entity
        /// </summary>
        public virtual void SaveToEntity()
        {
            SaveMappingsToEntities();
        }

        /// <summary>
        /// Cancel any changes that have been made
        /// </summary>
        public virtual void CancelChanges()
        {

        }
    }
}
