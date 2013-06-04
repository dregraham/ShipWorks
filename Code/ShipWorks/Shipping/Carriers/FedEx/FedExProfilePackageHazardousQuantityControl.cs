using System;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Profiles;
using System.Text.RegularExpressions;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public partial class FedExProfilePackageHazardousQuantityControl : UserControl, IShippingProfileControl
    {
        public FedExProfilePackageHazardousQuantityControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<FedExHazardousMaterialsQuantityUnits>(unit);
        }

        /// <summary>
        /// Gets/Sets the Enabled value of this control
        /// </summary>
        public bool State
        {
            get
            {
                return Enabled;
            }
            set
            {
                Enabled = value;
            }
        }

        /// <summary>
        /// Saves the control data to the entity
        /// </summary>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        /// <exception cref="System.InvalidCastException">entity should be of type FedExProfilePackageEntity</exception>
        public void SaveToEntity(EntityBase2 entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            FedExProfilePackageEntity package = entity as FedExProfilePackageEntity;

            if (package == null)
            {
                throw new InvalidCastException("entity should be of type FedExProfilePackageEntity");
            }

            package.HazardousMaterialQuanityUnits =  (int) unit.SelectedValue;
            if (string.IsNullOrWhiteSpace(quantity.Text))
            {
                package.HazardousMaterialQuantityValue = null;
            }
            else
            {
                package.HazardousMaterialQuantityValue= float.Parse(quantity.Text);
            }
        }

        /// <summary>
        /// Loads entity data to the control
        /// </summary>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        /// <exception cref="System.InvalidCastException">entity should be of type FedExProfilePackageEntity</exception>
        public void LoadFromEntity(EntityBase2 entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            FedExProfilePackageEntity package = entity as FedExProfilePackageEntity;

            if (package == null)
            {
                throw new InvalidCastException("entity should be of type FedExProfilePackageEntity");
            }

            if (package.HazardousMaterialQuantityValue.HasValue)
            {
                quantity.Text = package.HazardousMaterialQuantityValue.Value.ToString();
            }

            if (package.HazardousMaterialQuanityUnits.HasValue)
            {
                unit.SelectedValue = (FedExHazardousMaterialsQuantityUnits) package.HazardousMaterialQuanityUnits.Value;
            }

            State = package.HazardousMaterialQuantityValue.HasValue && package.HazardousMaterialQuanityUnits.HasValue;
        }
    }
}