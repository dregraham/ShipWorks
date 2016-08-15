using System.IO;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Converts a stream to a string
        /// </summary>
        /// <exception cref="IOException"></exception>
        public static string ConvertToString(this Stream stream)
        {
            MethodConditions.EnsureArgumentIsNotNull(stream);

            stream.Position = 0;
            using (StreamReader streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}