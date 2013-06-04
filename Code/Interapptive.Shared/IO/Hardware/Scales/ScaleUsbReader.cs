using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Usb;
using System.ComponentModel;

namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// Class for reading weights from a USB scale
    /// </summary>
    class ScaleUsbReader
    {
        HidDevice usbDevice;

        // The USB ID for scales
        const int ScaleUsagePage = 0x8D;

        /// <summary>
        /// Read the weight from the scale
        /// </summary>
        public ScaleReadResult ReadScale()
        {
            if (usbDevice == null)
            {
                LocateUsbDevice();
            }

            if (usbDevice != null)
            {
                return ReadFromDevice();
            }

            return new ScaleReadResult(ScaleReadStatus.NotFound, "Could not find a USB scale attached to the computer.");
        }

        /// <summary>
        /// Locate a USB scale device
        /// </summary>
        private void LocateUsbDevice()
        {
            foreach (HidDevice device in HidDeviceManager.FindDevices())
            {
                if (device.Capabilities.UsagePage == ScaleUsagePage)
                {
                    usbDevice = device;
                    break;
                }
            }
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
                if (data.Length == 6 && data[0] == 3)
                {
                    // Per USB POS spec (pos1_02.pdf)
                    int scaleStatus = data[1];
                    int weightUnit = data[2];
                    int dataScaling = data[3];
                    int weightLsb = data[4];
                    int weightMsb = data[5];

                    // Where does this number come from??
                    switch (scaleStatus)
                    {
                        case 1:
                            return new ScaleReadResult(ScaleReadStatus.ReadError, "An internal scale fault has occurred.");
                        
                        // Stable
                        case 2:
                            break;

                        // Moving
                        case 3:
                            break;

                        // Stable
                        case 4:
                            break;

                        // Under weight
                        case 5:
                            return new ScaleReadResult(ScaleReadStatus.ReadError, "Weight measured less than zero.");

                        // Over weight
                        case 6:
                            return new ScaleReadResult(ScaleReadStatus.ReadError, "Weight exceeds scale capacity.");

                        // Calibration required
                        case 7:
                            return new ScaleReadResult(ScaleReadStatus.ReadError, "The scale must be calibrated.");

                        // Rezero required
                        case 8:
                            return new ScaleReadResult(ScaleReadStatus.ReadError, "The scale must be zeroed.");
                    }

                    // The scaling factor is 2's compliement of the dataScaling value
                    double scalingFactor = (dataScaling & 0x7F) - (dataScaling & 0x80);

                    // Calculate the actual value
                    double unitValue = Math.Pow(10, scalingFactor) * (Convert.ToDouble(weightLsb) + (Convert.ToDouble(weightMsb) * 256.0));

                    // Now we need to convert based on units.  These come from the USB POS spec.  The spec does not explicitly state these numbers
                    // but lists them in the correct numerical order.
                    switch (weightUnit)
                    {
                        // Grams
                        case 2:
                            return new ScaleReadResult(unitValue * 2.20462262 / 1000.0);

                        // Kilograms
                        case 3:
                            return new ScaleReadResult(unitValue * 2.20462262);

                        // Ounces
                        case 11:
                            return new ScaleReadResult(unitValue / 16.0);

                        // Pounds
                        case 12:
                            return new ScaleReadResult(unitValue);
                    }

                    return new ScaleReadResult(ScaleReadStatus.ReadError, string.Format("Invalid unit reported by scale ({0}).", weightUnit));
                }
                else
                {
                    return new ScaleReadResult(ScaleReadStatus.NotFound, string.Format("Unexpected data from USB scale: {0}.", data[0]));
                }
            }
            catch (HidException ex)
            {
                usbDevice = null;

                return new ScaleReadResult(ScaleReadStatus.NotFound, ex.Message);
            }
        }
    }
}
