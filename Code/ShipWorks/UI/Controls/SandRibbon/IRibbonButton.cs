﻿using System;

namespace ShipWorks.UI.Controls.SandRibbon
{
    /// <summary>
    /// Interface to make testing button usage easier
    /// </summary>
    public interface IRibbonButton
    {
        /// <summary>
        /// Is the button enabled
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Activate handler
        /// </summary>
        event EventHandler Activate;

        /// <summary>
        /// Tag associated with the element
        /// </summary>
        object Tag { get; }

        /// <summary>
        /// The event name to send to telemetry
        /// </summary>
        string TelemetryEventName { get; set; }

        /// <summary>
        /// Track any telemetry
        /// </summary>
        void TrackButtonClick(string postfix);
    }
}