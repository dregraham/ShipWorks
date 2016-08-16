using System;
using ShipWorks.Core.UI.SandRibbon;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.UI.ShippingRibbon
{
    /// <summary>
    /// Make the apply profile dropdown appear as simply a button
    /// </summary>
    internal class ApplyProfileButtonWrapper : IRibbonButton, IDisposable
    {
        readonly RibbonButton actualApplyProfileButton;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="actualApplyProfileButton"></param>
        public ApplyProfileButtonWrapper(RibbonButton actualApplyProfileButton)
        {
            this.actualApplyProfileButton = actualApplyProfileButton;
        }

        /// <summary>
        /// Apply a specific profile
        /// </summary>
        public void ApplyProfile(ShippingProfileEntity profile)
        {
            actualApplyProfileButton.Tag = profile;
            Activate?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Is the button enabled
        /// </summary>
        public bool Enabled
        {
            get { return actualApplyProfileButton.Enabled; }
            set { actualApplyProfileButton.Enabled = value; }
        }

        /// <summary>
        /// Tag associated with the element
        /// </summary>
        public object Tag
        {
            get { return actualApplyProfileButton.Tag; }
        }

        /// <summary>
        /// Activate handler
        /// </summary>
        public event EventHandler Activate;

        /// <summary>
        /// Dispose of the actual control
        /// </summary>
        public void Dispose()
        {
            actualApplyProfileButton.Dispose();
        }
    }
}