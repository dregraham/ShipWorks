using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Usb;

namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// Class for reading weights from a USB scale
    /// </summary>
    class ScaleUsbReader
    {
        readonly ScaleReadResult scaleNotAttached = ScaleReadResult.NotFound("Could not find a USB scale attached to the computer.");

        // 2 and 4 are stable, 3 is moving
        readonly IDictionary<int, ScaleReadResult> errorResults = new Dictionary<int, ScaleReadResult>
        {
            { 1, ScaleReadResult.ReadError("An internal scale fault has occurred.") },
            { 5, ScaleReadResult.ReadError("Weight measured less than zero.") },
            { 6, ScaleReadResult.ReadError("Weight exceeds scale capacity.") },
            { 7, ScaleReadResult.ReadError("The scale must be calibrated.") },
            { 8, ScaleReadResult.ReadError("The scale must be zeroed.") }
        };

        readonly IDictionary<int, Func<double, double>> conversions = new Dictionary<int, Func<double, double>>
        {
            { 2, unitValue => unitValue * kilogramsToPounds / gramsToKilograms }, // Grams
            { 3, unitValue => unitValue* kilogramsToPounds }, // Kilograms
            { 11, unitValue => unitValue / ouncesToPounds }, // Ounces
            { 12, unitValue => unitValue} // Pounds
        };

        HidDevice usbDevice;

        // The USB ID for scales
        const int ScaleUsagePage = 0x8D;
        const double kilogramsToPounds = 2.20462262;
        const double gramsToKilograms = 1000;
        const double ouncesToPounds = 16;

        /// <summary>
        /// Read the weight from the scale
        /// </summary>
        public ScaleReadResult ReadScale()
        {
            if (usbDevice == null)
            {
                usbDevice = LocateUsbDevice();
            }

            return usbDevice != null ? ReadFromDevice() : scaleNotAttached;
        }

        /// <summary>
        /// Locate a USB scale device
        /// </summary>
        private HidDevice LocateUsbDevice()
        {
            return HidDeviceManager.FindDevices()
                .Where(device => device.Capabilities.UsagePage == ScaleUsagePage)
                .FirstOrDefault();
        }

        /// <summary>
        /// Read the scale value from the loaded device
        /// </summary>
        private ScaleReadResult ReadFromDevice()
        {
            try
            {
                byte[] data = usbDevice.ReadInputReport();

                // Ensure its a scale usage report
                if (data.Length != 6 || data[0] != 3)
                {
                    return ScaleReadResult.NotFound($"Unexpected data from USB scale: {data[0]}.");
                }

                // Per USB POS spec (pos1_02.pdf)
                int scaleStatus = data[1];
                int weightUnit = data[2];
                int dataScaling = data[3];
                int weightLsb = data[4];
                int weightMsb = data[5];

                if (errorResults.ContainsKey(scaleStatus))
                {
                    return errorResults[scaleStatus];
                }

                // The scaling factor is 2's compliment of the dataScaling value
                double scalingFactor = (dataScaling & 0x7F) - (dataScaling & 0x80);

                // Calculate the actual value
                double unitValue = Math.Pow(10, scalingFactor) * (Convert.ToDouble(weightLsb) + (Convert.ToDouble(weightMsb) * 256.0));

                // Now we need to convert based on units.  These come from the USB POS spec.  The spec does not explicitly state these numbers
                // but lists them in the correct numerical order.
                return conversions.ContainsKey(weightUnit) ?
                    ScaleReadResult.Success(conversions[weightUnit](unitValue)) :
                    ScaleReadResult.ReadError($"Invalid unit reported by scale ({weightUnit}).");
            }
            catch (HidException ex)
            {
                usbDevice = null;

                return ScaleReadResult.NotFound(ex.Message);
            }
        }
    }
}
