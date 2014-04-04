using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Services.Protocols;
using System.Xml;
using System.IO;

namespace ShipWorks.Stores.Platforms.Amazon.Dime
{
    /// <summary>
    /// Stream that wraps the underlying stream's data in a DIME attachment and
    /// appends other user-specified attachments.
    /// </summary>
    public class DimeInjectionStream : MemoryStream
    {
        // original web request stream 
        Stream originalStream;

        // collection of attachments to add to the request
        List<DimeAttachment> attachments;

        /// <summary>
        /// Constructor
        /// </summary>
        public DimeInjectionStream(Stream originalStream, List<DimeAttachment> attachments)
        {
            this.originalStream = originalStream;

            // make a copy of the attachment list since we'll be modifying it
            this.attachments = new List<DimeAttachment>(attachments);
        }

        /// <summary>
        /// Close the original stream, but inject the DIME attachments before doing so.
        /// </summary>
        public override void Close()
        {
            // create a new attachment from the soap request
            Position = 0;
            attachments.Insert(0, new DimeAttachment(Guid.NewGuid().ToString(), this, "http://schemas.xmlsoap.org/soap/envelope/"));

            // write our contents to the original stream, then close the original stream
            using (BinaryWriter writer = new BinaryWriter(originalStream))
            {
                for (int i = 0; i < attachments.Count; i++)
                {
                    attachments[i].WriteDimeRecord(writer, i == 0, i == (attachments.Count - 1));
                }
            }

            originalStream.Close();
        }
    }

}
