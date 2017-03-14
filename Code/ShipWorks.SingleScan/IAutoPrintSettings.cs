﻿namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Permissions for AutoPrint
    /// </summary>
    public interface IAutoPrintSettings
    {
        /// <summary>
        /// Whether or not auto print is permitted in the current state
        /// </summary>
        bool IsAutoPrintEnabled();

        /// <summary>
        /// Whether or not auto weigh is turned on
        /// </summary>
        bool IsAutoWeighEnabled();
    }
}