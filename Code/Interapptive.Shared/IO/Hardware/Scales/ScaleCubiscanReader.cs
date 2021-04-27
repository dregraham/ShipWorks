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
        private readonly ILog log;
        private static readonly string measureRequestString = $"{Convert.ToChar(2)}M{Convert.ToChar(3)}{Convert.ToChar(13)}{Convert.ToChar(10)}";

        /// <summary>
        /// Constructor
        /// </summary>
        public ScaleCubiscanReader(ICubiscanConfigurationManager cubiscanConfigurationManager)
        {
            log = LogManager.GetLogger(typeof(ScaleCubiscanReader));
        }

        /// <summary>
        /// Requests dimensions from a Cubiscan and returns the result
        /// </summary>
        /// <param name="config">Current cubiscan configuration</param>
        public ScaleReadResult ReadScale(CubiscanConfiguration config)
        {
            try
            {
                if (!config.IsConfigured)
                {
                    return ScaleReadResult.NotFound("No cubiscan device configured.");
                }
                
                var result = Functional.RetryAsync(() => GetScaleReadResult(config), 3, _ => true).Result;
                return ReadCubiScan(result.Value);
            }
            catch(Exception ex)
            {
                log.Warn($"Unable to read weight from Cubiscan device.", ex);
                return ScaleReadResult.ReadError("Unable to read weight from Cubiscan device.", ScaleType.Cubiscan);
            }
        }

        /// <summary>
        /// Parses the message from Cubiscan
        /// </summary>
        private ScaleReadResult ReadCubiScan(string value)
        {
            if ((value?.Length ?? 0) < 42)
            {
                return ScaleReadResult.ReadError($"Invalid response from Cubiscan device. (Response: {value})", ScaleType.Cubiscan);
            }

            if (!double.TryParse(value.Substring(35, 6), out double weight) ||
                !double.TryParse(value.Substring(12, 5), out double length) ||
                !double.TryParse(value.Substring(19, 5), out double width) ||
                !double.TryParse(value.Substring(26, 5), out double height))
            {
                return ScaleReadResult.ReadError($"Could not parse dimension from Cubiscan device. (Response: {value})", ScaleType.Cubiscan);
            }

            return ScaleReadResult.Success(weight, length, width, height, ScaleType.Cubiscan);
        }

        /// <summary>
        /// Opens a port, sends a measure request, and returns the result
        /// </summary>
        /// <param name="cubiscanConfiguration"></param>
        /// <returns>Raw result from cubiscan</returns>
        private async Task<GenericResult<string>> GetScaleReadResult(CubiscanConfiguration cubiscanConfiguration)
        {
            using (TcpClient client = new TcpClient())
            {
                // todo: get this value from the configuration when we get to that story
                await client.ConnectAsync(cubiscanConfiguration.IpAddress, cubiscanConfiguration.Port).ConfigureAwait(false);

                NetworkStream networkStream = client.GetStream();
                string response;

                using (StreamWriter writer = new StreamWriter(networkStream))
                using (StreamReader reader = new StreamReader(networkStream))
                {
                    writer.AutoFlush = true;

                    await writer.WriteLineAsync(measureRequestString).ConfigureAwait(false);
                    response = await reader.ReadLineAsync().ConfigureAwait(false);
                }
                client.Close();
                return GenericResult.FromSuccess<string>(response);
            }
        }
    }
}
