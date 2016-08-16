using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using log4net;
using System.Text.RegularExpressions;
using System.IO;

namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// Utility class for reading the weight from an external scale hooked up via serial
    /// </summary>
    class ScaleSerialPortReader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ScaleSerialPortReader));

        // Once we find the port and the brand, we save them
        string lockedPort = "";
        ScaleBrand lockedBrand = (ScaleBrand) (-1);
        int lockedBaud = 0;
        Parity lockedParity = (Parity) (-1);

        // The different scale brands we support
        enum ScaleBrand
        {
            EndiciaSalter,
            Fairbanks,
            Toledo,
        }

        // The order in which to try scales
        static List<ScaleBrand> scaleOrder = new List<ScaleBrand>();

        /// <summary>
        /// Static constructor
        /// </summary>
        static ScaleSerialPortReader()
        {
            scaleOrder.Add(ScaleBrand.EndiciaSalter);
            scaleOrder.Add(ScaleBrand.Fairbanks);
            scaleOrder.Add(ScaleBrand.Toledo);
        }

        /// <summary>
        /// Read the weight from the scale
        /// </summary>
        public ScaleReadResult ReadScale()
        {
            // If we have already locked in a port and a brand, don't search at all
            if (lockedPort.Length != 0)
            {
                // Try to read from the scale brand on the given port
                ScaleReadResult result = ReadScale(lockedPort, lockedBrand);

                if (result.Status != ScaleReadStatus.Success)
                {
                    // Didn't find something on the locked port, clear it
                    lockedPort = "";
                }

                return result;
            }

            foreach (string portName in SerialPort.GetPortNames())
            {
                // Try to read each scale in the given order
                foreach (ScaleBrand brand in scaleOrder)
                {
                    // Try to read from the scale brand on the given port
                    ScaleReadResult result = ReadScale(portName, brand);

                    if (result.Status == ScaleReadStatus.Success)
                    {
                        lockedPort = portName;
                        lockedBrand = brand;

                        return result;
                    }
                }
            }

            // If we get here, we did not find a valid scale from which to read the weight
            return ScaleReadResult.NotFound("Could not find a compatible scale, the scale is in motion, or the scale is being used by another application.");
        }

        /// <summary>
        /// Try to read from the given scale brand on the specified port
        /// </summary>
        private ScaleReadResult ReadScale(string portName, ScaleBrand brand)
        {
            // Read the weight using the format of the appropriate brand
            switch (brand)
            {
                case ScaleBrand.EndiciaSalter:
                    {
                        return ReadEndiciaSalter(portName);
                    }

                case ScaleBrand.Fairbanks:
                    {
                        return ReadFairbanks(portName);
                    }

                case ScaleBrand.Toledo:
                    {
                        return ReadToledo(portName);
                    }
            }

            throw new InvalidOperationException("Invalid scale brand.");
        }

        /// <summary>
        /// Read the weight from the scale
        /// </summary>
        private ScaleReadResult ReadEndiciaSalter(string portName)
        {
            try
            {
                using (SerialPort port = OpenPort(portName, 2400, Parity.None, 8, StopBits.One))
                {
                    if (port == null)
                    {
                        return ScaleReadResult.NotFound("Could not open port to read scale.");
                    }

                    string result = port.ReadTo("\r");

                    if (result.Length < 5)
                    {
                        return ScaleReadResult.NotFound("Data invalid for Salter scale.");
                    }

                    // We are now always assuming its in lb\oz, as the Endicia2200 scale and the Salter
                    // scale are almost totally similar, but use different symbols to indicate lb\oz vs. grams.
                    bool poundsOz = true;

                    // Get the last 5 digits
                    result = result.Substring(result.Length - 5, 5);

                    // Pounds\Oz
                    if (poundsOz)
                    {
                        double pounds;
                        double ouncesWhole;
                        double ouncesDecimal;

                        if (!double.TryParse(result.Substring(0, 2), out pounds))
                        {
                            return ScaleReadResult.ReadError("Unknown data read from Endicia scale.");
                        }

                        if (!double.TryParse(result.Substring(2, 2), out ouncesWhole))
                        {
                            return ScaleReadResult.ReadError("Unknown data read from Endicia scale.");
                        }

                        if (!double.TryParse(result.Substring(4, 1), out ouncesDecimal))
                        {
                            return ScaleReadResult.ReadError("Unknown data read from Endicia scale.");
                        }

                        // Extract ounces
                        double ounces = ouncesWhole + ouncesDecimal / 10.0;

                        // Parse the weight part of the result
                        return ScaleReadResult.Success(pounds + ounces / 16.0);
                    }
                    else
                    {
                        double grams = Convert.ToDouble(result);

                        return ScaleReadResult.Success(grams * 2.20462262 / 1000.0);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                return ScaleReadResult.ReadError(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ScaleReadResult.ReadError(ex.Message);
            }
            catch (TimeoutException ex)
            {
                return ScaleReadResult.NotFound(ex.Message);
            }
            catch (IOException ex)
            {
                return ScaleReadResult.NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Read the weight from the scale
        /// </summary>
        private ScaleReadResult ReadFairbanks(string portName)
        {
            try
            {
                using (SerialPort port = OpenPort(portName, 9600, Parity.Odd, 7, StopBits.One))
                {
                    if (port == null)
                    {
                        return ScaleReadResult.NotFound("Could not open port to read scale.");
                    }

                    port.Write("\r");
                    string result = port.ReadLine();

                    // Strip non alpha-numerics
                    string stripped = Regex.Match(result, "[. 0-9a-zA-Z]+").Value;

                    // Parse the weight part of the result
                    double weight;
                    if (stripped.Length < 7 || !double.TryParse(stripped.Substring(0, 7), out weight))
                    {
                        return ScaleReadResult.ReadError("Unknown data read from Fairbanks scale.");
                    }

                    // If its in kilograms, do the conversion
                    if (result.IndexOf("kg") != -1)
                    {
                        weight *= 2.20462262;
                    }

                    return ScaleReadResult.Success(weight);
                }
            }
            catch (InvalidOperationException ex)
            {
                return ScaleReadResult.ReadError(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ScaleReadResult.ReadError(ex.Message);
            }
            catch (TimeoutException ex)
            {
                return ScaleReadResult.NotFound(ex.Message);
            }
            catch (IOException ex)
            {
                return ScaleReadResult.NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Read the weight from the scale
        /// </summary>
        private ScaleReadResult ReadToledo(string portName)
        {
            List<Parity> parityTries = new List<Parity> {Parity.Even, Parity.None};
            List<int> buadTries = new List<int> {9600, 4800, 2400, 1200};

            if (lockedPort.Length != 0)
            {
                parityTries.Clear();
                parityTries.Add(lockedParity);

                buadTries.Clear();
                buadTries.Add(lockedBaud);
            }

            // The Toledo can be configure to any several baud or parities
            foreach (int buadTry in buadTries)
            {
                foreach (Parity parityTry in parityTries)
                {
                    try
                    {
                        using (SerialPort port = OpenPort(portName, buadTry, parityTry, 7, StopBits.One))
                        {
                            if (port != null)
                            {
                                port.Write("W");
                                string result = port.ReadTo("\r");

                                if (result != null && result.Length >= 6)
                                {
                                    // Only use the last 6 characters
                                    result = result.Substring(result.Length - 6, 6);

                                    double weight;
                                    if (double.TryParse(result, out weight))
                                    {
                                        lockedBaud = buadTry;
                                        lockedParity = parityTry;

                                        return ScaleReadResult.Success(weight);
                                    }
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException)
                    {

                    }
                    catch (UnauthorizedAccessException)
                    {

                    }
                    catch (TimeoutException)
                    {

                    }
                    catch (IOException)
                    {

                    }
                }
            }

            return ScaleReadResult.NotFound("Could not locate a MT serial port scale.");
        }

        /// <summary>
        /// Open a port with the given name and settings
        /// </summary>
        private SerialPort OpenPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            try
            {
                SerialPort port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);

                port.ReadTimeout = 250;
                port.Open();

                return port;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }
    }
}
