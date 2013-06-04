using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Services.Protocols;
using System.Xml;
using System.IO;
using System.Globalization;

namespace ShipWorks.Stores.Platforms.Amazon.Dime
{
    /// <summary>
    /// DIME attachment.
    /// 
    /// From http://xml.coverpages.org/draft-nielsen-dime-02.txt, a DIME record 
    /// is formatted as follows:
    ///
    ///  0                   1                   2                   3 
    ///  0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ 
    /// |         |M|M|C|       |       |                               | 
    /// | VERSION |B|E|F| TYPE_T| RESRVD|         OPTIONS_LENGTH        | 
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ 
    /// |            ID_LENGTH          |           TYPE_LENGTH         | 
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ 
    /// |                          DATA_LENGTH                          | 
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ 
    /// |                                                               / 
    /// /                     OPTIONS + PADDING                         / 
    /// /                                                               | 
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ 
    /// |                                                               / 
    /// /                          ID + PADDING                         / 
    /// /                                                               | 
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ 
    /// |                                                               / 
    /// /                        TYPE + PADDING                         / 
    /// /                                                               | 
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ 
    /// |                                                               / 
    /// /                        DATA + PADDING                         / 
    /// /                                                               | 
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+ 
    /// </summary>
    public class DimeAttachment
    {
        // DIME attachment identifier, used to reference an attachment
        string id = "";

        // Data stream containing the attachment data
        Stream attachmentStream;
        string attachmentPath = "";

        // MIME type or similar of the data being sent.
        string mediaType;

        /// <summary>
        /// Constructor
        /// </summary>
        public DimeAttachment(string id, string path, string mediaType)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to locate DIME attachment.", path);
            }

            attachmentPath = path;
            this.id = id;
            this.mediaType = mediaType;
        }

        /// <summary>
        /// Creates an attachment from a stream
        /// </summary>
        public DimeAttachment(string id, Stream attachmentStream, string mediaType)
        {
            this.attachmentStream = attachmentStream;
            this.id = id;
            this.mediaType = mediaType;
        }

        /// <summary>
        /// Outputs a DIME record for the contents of the attachment.  Note, 
        /// we do not support chunked records - attachments that are larger 
        /// than the allowed DIME record size (2^32 - 1 bytes). That's plenty
        /// large for our needs.
        /// </summary>
        internal uint WriteDimeRecord(BinaryWriter writer, bool firstRecord, bool lastRecord)
        {
            bool disposeStream = false;
            if (attachmentPath.Length > 0)
            {
                attachmentStream = new FileStream(attachmentPath, FileMode.Open);
                disposeStream = true;
            }
            try
            {

                // Write DIME Header 
                uint size = 12;

                // First 4 Bytes 
                //******************
                // MIME version 1
                byte byte1 = 1 << 3;

                // Record begin/end flags
                byte1 |= firstRecord ? (byte)0x4 : (byte)0;
                byte1 |= lastRecord ? (byte)0x2 : (byte)0;

                byte byte2 = (mediaType.ToLower(CultureInfo.InvariantCulture) == "http://schemas.xmlsoap.org/soap/envelope/" ? (byte)0x20 : (byte)0x10);

                writer.Write(byte1);
                writer.Write(byte2);
                writer.Write(Convert.ToInt16(0)); // Options Length, we have no Options

                // Second 4 Bytes
                //*****************
                // ID Length
                writer.Write(IPAddress.HostToNetworkOrder(Convert.ToInt16(id.Length)));

                // Type Length
                writer.Write(IPAddress.HostToNetworkOrder(Convert.ToInt16(mediaType.Length)));

                // Third 4 Bytes
                //*****************
                // Data Length
                writer.Write(IPAddress.HostToNetworkOrder(Convert.ToInt32(attachmentStream.Length)));

                // Forth 4 Bytes
                // Options + Padding (we have none)
                //writer.Write(Convert.ToUInt32(0));

                // Fifth 4 Bytes
                // ID + Padding
                size += WritePadded(writer, Encoding.ASCII.GetBytes(id), 4);

                // Fifth 4 Bytes
                // TYPE + Padding
                size += WritePadded(writer, Encoding.ASCII.GetBytes(mediaType), 4);

                // Sixth 4 Bytes
                // DATA + Padding
                size += WritePadded(writer, attachmentStream, 4);

                // flush the stream of current data
                writer.Flush();

                // return the size of the data written out
                return size;
            }
            finally
            {
                if (disposeStream)
                {
                    attachmentStream.Dispose();
                }
            }
        }

        /// <summary>
        /// Writes the contents of a stream to the writer, padded to the specified
        /// number of bytes.
        /// </summary>
        private uint WritePadded(BinaryWriter writer, Stream stream, int paddedBytes)
        {
            uint size = 0;

            byte[] buffer = new byte[1024];
            int count = 0;
            while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                writer.Write(buffer, 0, count);
                size += Convert.ToUInt32(count);
            }

            int padBy = 0;
            if (size % paddedBytes != 0)
            {
                padBy = paddedBytes - Convert.ToInt32(size % paddedBytes);
            }
            while (padBy > 0)
            {
                // padding with zeroes
                writer.Write((byte)0);

                size++;
                padBy--;
            }

            return size;
        }

        /// <summary>
        /// Writes a byte array to the stream, padding 
        /// </summary>
        private uint WritePadded(BinaryWriter writer, byte[] bytes, int paddedBytes)
        {
            uint size = Convert.ToUInt32(bytes.Length);

            int padBy = 0;
            if (size % paddedBytes != 0)
            {
                padBy = paddedBytes - bytes.Length % paddedBytes;
            }
            size += Convert.ToUInt32(padBy);

            // write the data
            writer.Write(bytes);

            // pad to multiples of paddedBytes
            while (padBy > 0)
            {
                // pad with zeroes
                writer.Write((byte)0);

                padBy--;
            }

            return size;
        }
    }
}
