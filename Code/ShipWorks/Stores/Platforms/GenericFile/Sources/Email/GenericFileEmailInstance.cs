using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebex.Mail;
using System.IO;
using log4net;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.Email
{
    /// <summary>
    /// Implementation of the GenericFileInstance for email
    /// </summary>
    public class GenericFileEmailInstance : GenericFileInstance
    {
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileEmailInstance));

        string rebexID;
        MailMessage message;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileEmailInstance(string rebexID, MailMessage message)
        {
            this.rebexID = rebexID;
            this.message = message;
        }

        /// <summary>
        /// The unique 'Rebex' ID of the message, which is a Base64 of concatenated folder validity ID and message unique ID
        /// </summary>
        public string RebexID
        {
            get { return rebexID; }
        }

        /// <summary>
        /// The MailMessage represented by this instance
        /// </summary>
        public MailMessage MailMessage
        {
            get { return message; }
        }

        /// <summary>
        /// The 'Name' of the message - in email's case, the subject
        /// </summary>
        public override string Name
        {
            get
            {
                return message.Subject;
            }
        }

        /// <summary>
        /// Read the generic file content of the message
        /// </summary>
        public override Stream OpenStream()
        {
            log.InfoFormat("Attachments {0}: {1}", message.Attachments.Count, string.Join(", ", message.Attachments.Select(a => a.DisplayName)));

            // Find the first attachment. If there are multiple, use non-inline (actual) attachments first
            Attachment attachment = message.Attachments.OrderBy(a => !a.ContentDisposition.Inline ? 0 : 1).FirstOrDefault();

            // If we found that, use it.
            if (attachment != null)
            {
                return attachment.GetContentStream();
            }

            // Next check alternate views
            AlternateView altView = message.AlternateViews.FirstOrDefault(v => IsTextualMediaType(v.MediaType));

            // If we found it, use it.
            if (altView != null)
            {
                return altView.GetContentStream();
            }

            throw new GenericFileFormatException(string.Format("Email message '{0}' does not contain any readable text content.", message.Subject));
        }

        /// <summary>
        /// Indicates if the media type of the attachment is text based
        /// </summary>
        private static bool IsTextualMediaType(string mediaType)
        {
            string[] supportedTypes = 
                {
                    "text/plain",
                    "text/xml",
                    "application/xml",
                };

            return supportedTypes.Any(type => string.Compare(type, mediaType, StringComparison.OrdinalIgnoreCase) == 0);
        }
    }
}
