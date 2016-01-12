using System.IO;
using System.Text;
using log4net;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources
{
    /// <summary>
    /// Base representation of a GenericFile that can be used by downloaders
    /// </summary>
    public abstract class GenericFileInstance
    {
        ILog log = LogManager.GetLogger(typeof(GenericFileInstance));

        /// <summary>
        /// The name of the file, as it makes sense for the originating source.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Opens the file stream
        /// </summary>
        public abstract Stream OpenStream();

        /// <summary>
        /// Reads the entire contents of the file as text.  Defaults to UTF-8 encoding
        /// </summary>
        public string ReadAllText()
        {
            return ReadAllText(Encoding.UTF8, true);
        }

        /// <summary>
        /// Reads the entire contents of the file as text
        /// </summary>
        public string ReadAllText(Encoding encoding, bool detectEncodingFromByteOrderMarks)
        {
            using (Stream stream = OpenStream())
            {
                using (StreamReader reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Read the entire contents of the file as the file's raw bytes
        /// </summary>
        public byte[] ReadAllBytes()
        {
            using (Stream stream = OpenStream())
            {
                byte[] bytes = new byte[stream.Length];

                int bytesRead = stream.Read(bytes, 0, (int) stream.Length);
                if (bytesRead != stream.Length)
                {
                    log.Warn($"Bytes read ({bytesRead}) is less than the reported length ({stream.Length})");
                }

                return bytes;
            }
        }
    }
}
