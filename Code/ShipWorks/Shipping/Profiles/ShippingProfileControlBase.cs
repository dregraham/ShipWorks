using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

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
        protected ShippingProfileEntity Profile => profile;

        /// <summary>
        /// Load the given profile
        /// </summary>
        public virtual void LoadProfile(ShippingProfileEntity profile)
        {
            this.profile = profile;

            AllowChangeCheckState = !profile.ShipmentTypePrimary;
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
        
        /// <summary>
        /// Resize all the GroupBoxes in the specified container so they are all uniform
        /// </summary>
        /// <remarks>We're doing this here instead of in the designer because the inheritance of the profile controls
        /// makes getting the sizeing right difficult</remarks>
        protected static void ResizeGroupBoxes(Control container)
        {
            foreach (GroupBox groupBox in container.Controls.OfType<GroupBox>())
            {
                groupBox.Location = new Point(9, groupBox.Location.Y);
                groupBox.Size = new Size(container.Size.Width - 18, groupBox.Size.Height);
            }
        }
    }
}
