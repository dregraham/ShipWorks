using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Submitter for posting files and\or variables to a web URL.
    /// </summary>
    public class HttpFilePostRequestSubmitter : HttpVariableRequestSubmitter
    {
        HttpFileCollection files = new HttpFileCollection();

        string boundary = "sw" + DateTime.Now.Ticks.ToString("x");

        /// <summary>
        /// Constructor
        /// </summary>
        public HttpFilePostRequestSubmitter()
        {
            ContentType = "multipart/form-data; boundary=" + boundary;
        }

        /// <summary>
        /// The files to be posted
        /// </summary>
        public HttpFileCollection Files
        {
            get { return files; }
        }

        /// <summary>
        /// Get the content to be posted
        /// </summary>
        public override byte[] GetPostContent()
        {
            using (MemoryStream contentStream = new MemoryStream())
            {
                // Write the variables
                WriteVariables(contentStream);

                // Write the files
                WriteFiles(contentStream);

                // Write the footer
                string footer = "--" + boundary + "--\r\n";
                byte[] footerBytes = Encoding.UTF8.GetBytes(footer);
                contentStream.Write(footerBytes, 0, footerBytes.Length);

                // Return the content result
                return contentStream.ToArray();
            }
        }

        /// <summary>
        /// Write all the variables to the stream using the given boundary
        /// </summary>
        private void WriteVariables(Stream contentStream)
        {
            string formDataTemplate = 
                "--" + boundary + "\r\n" +
                "Content-Disposition: form-data; name=\"{0}\";\r\n" +
                "\r\n" +
                "{1}\r\n";

            foreach (HttpVariable variable in Variables)
            {
                string formVariable = string.Format(formDataTemplate, variable.Name, variable.Value);
                byte[] formVariableBytes = Encoding.UTF8.GetBytes(formVariable);

                // Write the variable to the stream
                contentStream.Write(formVariableBytes, 0, formVariableBytes.Length);
            }
        }

        /// <summary>
        /// Write files to the stream
        /// </summary>
        private void WriteFiles(Stream contentStream)
        {
            // Header for each file
            string headerTemplate =
                "--" + boundary + "\r\n" +
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                "Content-Type: application/octet-stream\r\n" +
                "\r\n";

            // Footer for each file
            string footer = "\r\n";
            byte[] footerBytes = Encoding.UTF8.GetBytes(footer);

            // Write each file content
            foreach (HttpFile file in Files)
            {
                string header = string.Format(headerTemplate, file.VariableName, file.FileName);
                byte[] headerBytes = Encoding.UTF8.GetBytes(header);

                // Write the file header
                contentStream.Write(headerBytes, 0, headerBytes.Length);

                using (Stream fileStream = file.OpenStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;

                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        contentStream.Write(buffer, 0, bytesRead);
                    }
                }

                // Write the final newline marker
                contentStream.Write(footerBytes, 0, footerBytes.Length);
            }
        }
    }
}
