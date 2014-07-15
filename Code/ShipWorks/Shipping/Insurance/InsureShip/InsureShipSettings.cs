using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Class to get InsureShip settings.
    /// </summary>
    public class InsureShipSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether [use test server] based on a registry setting.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use test server]; otherwise, <c>false</c>.
        /// </value>
        public bool UseTestServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("InsureShipTestServer", false);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("InsureShipTestServer", value);
            }
        }

        /// <summary>
        /// InsureShip Distributor ID
        /// </summary>
        public string DistributorID
        {
            get
            {
                return UseTestServer ? "D00002" : "D00050";
            }
        }

        /// <summary>
        /// InsureShip Username
        /// </summary>
        public string Username
        {
            get
            {
                return UseTestServer ? SecureText.Decrypt("7fA9js5H7h4=", "InsureShip7458") : SecureText.Decrypt("xYGNUSctosMN3kr2vZw1cg==", "InsureShip7458");
            }
        }

        /// <summary>
        /// InsureShip Password
        /// </summary>
        public string Password
        {
            get
            {
                return UseTestServer ? SecureText.Decrypt("xreVCItexMlyOYE+edlqEg==", "InsureShip7458") : SecureText.Decrypt("byJ2OXi5odKK0PVy7VPB6zKGm6aI++SX", "InsureShip7458");
            }
        }

        /// <summary>
        /// InsureShip Url
        /// </summary>
        public Uri Url
        {
            get
            {
                return UseTestServer ? new Uri("https://int.insureship.com/api/") : new Uri("https://api2.insureship.com/api/");
            }
        }
    }
}
