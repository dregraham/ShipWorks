using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Interapptive.Shared.Usb
{
    /// <summary>
    /// Loaded attributes of a device
    /// </summary>
    public class HidDeviceAttributes
    {
        string productHexID;
        int productID;
        string vendorHexID;
        int vendorID;
        int version;

        /// <summary>
        /// Constructor
        /// </summary>
        public HidDeviceAttributes(NativeUsb.HidDAttributes attributes)
        {
            vendorID = attributes.VendorID;
            productID = attributes.ProductID;
            version = attributes.VersionNumber;

            vendorHexID = "0x" + BitConverter.ToString(BitConverter.GetBytes(vendorID).Reverse().ToArray()).Replace("-", "");
            productHexID = "0x" + BitConverter.ToString(BitConverter.GetBytes(productID).Reverse().ToArray()).Replace("-", "");
        }

        public string ProductHexID
        {
            get
            {
                return productHexID;
            }
        }

        public int ProductID
        {
            get
            {
                return productID;
            }
        }

        public string VendorHexID
        {
            get
            {
                return vendorHexID;
            }
        }

        public int VendorID
        {
            get
            {
                return vendorID;
            }
        }

        public int Version
        {
            get
            {
                return version;
            }
        }
    }


}
