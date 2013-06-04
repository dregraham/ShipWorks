using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Usb
{
    /// <summary>
    /// Reports capabilities of an HID device
    /// </summary>
    public class HidDeviceCapabilities
    {
        NativeUsb.HidPCaps caps;

        /// <summary>
        /// Constructor
        /// </summary>
        public HidDeviceCapabilities(NativeUsb.HidPCaps caps)
        {
            this.caps = caps;
        }

        public short FeatureReportByteLength
        {
            get { return caps.FeatureReportByteLength; }
        }

        public short InputReportByteLength
        {
            get { return caps.InputReportByteLength; }
        }

        public short NumberFeatureButtonCaps
        {
            get { return caps.NumberFeatureButtonCaps; }
        }

        public short NumberFeatureDataIndices
        {
            get { return caps.NumberFeatureDataIndices; }
        }

        public short NumberFeatureValueCaps
        {
            get { return caps.NumberFeatureValueCaps; }
        }

        public short NumberInputButtonCaps
        {
            get { return caps.NumberInputButtonCaps; }
        }

        public short NumberInputDataIndices
        {
            get { return caps.NumberInputDataIndices; }
        }

        public short NumberInputValueCaps
        {
            get { return caps.NumberInputValueCaps; }
        }

        public short NumberLinkCollectionNodes
        {
            get { return caps.NumberLinkCollectionNodes; }
        }

        public short NumberOutputButtonCaps
        {
            get { return caps.NumberOutputButtonCaps; }
        }

        public short NumberOutputDataIndices
        {
            get { return caps.NumberOutputDataIndices; }
        }

        public short NumberOutputValueCaps
        {
            get { return caps.NumberOutputValueCaps; }
        }

        public short OutputReportByteLength
        {
            get { return caps.OutputReportByteLength; }
        }

        public short[] Reserved
        {
            get { return caps.Reserved; }
        }

        public short Usage
        {
            get { return caps.Usage; }
        }

        public short UsagePage
        {
            get { return caps.UsagePage; }
        }

    }
}
