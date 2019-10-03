using System;
using Interapptive.Shared.Metrics;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Controls.SandRibbon;

namespace ShipWorks.Shipping.UI.ShippingRibbon
{
    /// <summary>
    /// Make the apply profile dropdown appear as simply a button
    /// </summary>
    internal class ApplyProfileButtonWrapper : IRibbonButton, IDisposable
    {
        private readonly RibbonButton actualApplyProfileButton;

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
        public void ApplyProfile(IShippingProfile profile)
        {
            TrackButtonClick(profile.ShippingProfileEntity.Name);
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
        /// The event name to send to telemetry
        /// </summary>
        public string TelemetryEventName
        {
            get => actualApplyProfileButton.TelemetryEventName;
            set { }
        }

        /// <summary>
        /// Track any telemetry
        /// </summary>
        public void TrackButtonClick(string postfix)
        {
            actualApplyProfileButton.TrackButtonClick(postfix);
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