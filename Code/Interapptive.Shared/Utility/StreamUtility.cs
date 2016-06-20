using System.IO;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility class for working with streams
    /// </summary>
    public static class StreamUtility
    {
        /// <summary>
        /// Writes the contents of the stream to the specified file name
        /// </summary>
        public static void WriteToFile(Stream stream, string fileName)
        {
            // Create the stream it will be output to
            using (FileStream outputFile = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                // Copy the input stream to the output file
                byte[] byteBuffer = new byte[4096];
                int numBytes;
                while ((numBytes = stream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                {
                    outputFile.Write(byteBuffer, 0, numBytes);
                }
            }
        }
    }
}
