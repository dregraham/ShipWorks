﻿namespace ShipWorks.Editions
{
    public enum ShipmentTypeRestrictionType
    {
        /// <summary>
        /// The entire shipment type is restricted/disabled from being used in ShipWorks.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// New accounts cannot be registered/created in ShipWorks.
        /// </summary>
        AccountRegistration = 1,

        /// <summary>
        /// A shipment cannot be processed.
        /// </summary>
        Processing = 2
    }
}
