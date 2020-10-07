﻿using System.Reflection;

namespace ShipWorks.Warehouse.Configuration.DTO.ShippingSettings
{
    /// <summary>
    /// DTO for importing OnTrac account data from the hub
    /// </summary>
    [Obfuscation]
    public class OnTracAccountConfiguration
    {
        /// <summary>
        /// The OnTrac account number
        /// </summary>
        public int AccountNumber { get; set; }

        /// <summary>
        /// The Ontrac password
        /// </summary>
        public string password { get; set; }
    }
}