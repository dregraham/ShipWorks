using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using log4net;

namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// Class to request and retrieve dimensions from a Cubiscan 110
    /// </summary>
    public class ScaleCubiscanReader
    {
        private ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScaleCubiscanReader()
        {
            log = LogManager.GetLogger(typeof(ScaleCubiscanReader));
        }  

        /// <summary>
        /// Requests dimensions from a Cubiscan and returns the result
        /// </summary>
        public ScaleReadResult ReadScale()
        {
            var result = GetScaleReadResult().Result;
            if (result.Success)
            {
                return ReadCubiScan(result.Value);
            }
            else
            {
                log.Warn($"Unable to read weight from cubiscan scale: {result.Exception.Message}", result.Exception);
                return ScaleReadResult.ReadError("Unable to read weight from cubiscan scale.", ScaleType.Cubiscan);
            }
        }

        /// <summary>
        /// Parses the message from Cubiscan
        /// </summary>
        private ScaleReadResult ReadCubiScan(string value)
        {
            if ((value?.Length ?? 0) < 35)
            {
                return ScaleReadResult.ReadError($"Invalid response from Cubiscan. (Response: {value})", ScaleType.Cubiscan);
            }

            double weight = double.Parse(value.Substring(35,6));
            double length = double.Parse(value.Substring(12,5));
            double width = double.Parse(value.Substring(19,5));
            double height = double.Parse(value.Substring(26,5));

            return ScaleReadResult.Success(weight, length, width, height, ScaleType.Cubiscan);
        }

        /// <summary>
        /// Opens a port, sends a measure request, and returns the result
        /// </summary>
        /// <returns>Raw result from cubiscan</returns>
        private async Task<GenericResult<string>> GetScaleReadResult()
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    // todo: get this value from the configuration when we get to that story
                    await client.ConnectAsync("127.0.0.1", 1050);

                    NetworkStream networkStream = client.GetStream();
                    string response;

                    using (StreamWriter writer = new StreamWriter(networkStream))
                    using (StreamReader reader = new StreamReader(networkStream))
                    {
                        writer.AutoFlush = true;

                        string measurement = Convert.ToChar(2) + "M" + Convert.ToChar(3) + Convert.ToChar(13) + Convert.ToChar(10);
                        await writer.WriteLineAsync(measurement);
                        response = await reader.ReadLineAsync();
                    }
                    client.Close();
                    return GenericResult.FromSuccess<string>(response);
                }                
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<string>(ex.Message);
            }
        }
    }
}
