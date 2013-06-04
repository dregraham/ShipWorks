using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Represents a file to be uploaded via HTTP post
    /// </summary>
    public class HttpFile
    {
        // The variable name to be posted
        string name;

        // For local disk files
        string localPath;

        // For ASP.NET posted files
        HttpPostedFile postedFile;

        // For in-memory strings
        string textContentFileName;
        string textContent;

        /// <summary>
        /// Create a new instance from a file on disk
        /// </summary>
        public HttpFile(string name, string path)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name cannot be null or empty", "name");
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path cannot be null or empty", "path");
            }

            this.name = name;
            this.localPath = path;
        }

        /// <summary>
        /// Create a new instance from the given string
        /// </summary>
        public HttpFile(string name, string fileName, string content)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name cannot be null or empty", "name");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("name cannot be null or empty", "fileName");
            }

            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            this.name = name;
            this.textContentFileName = fileName;
            this.textContent = content;
        }

        /// <summary>
        /// Create a new instance from the given ASP.NET posted file
        /// </summary>
        public HttpFile(string name, HttpPostedFile postedFile)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name cannot be null or empty", "name");
            }

            if (postedFile == null)
            {
                throw new ArgumentNullException("postedFile");
            }

            this.name = name;
            this.postedFile = postedFile;
        }

        /// <summary>
        /// The name to use as the variable name for the posted file data
        /// </summary>
        public string VariableName
        {
            get { return name; }
        }

        /// <summary>
        /// The name to use as the Filename for the posted file data
        /// </summary>
        public string FileName
        {
            get 
            {
                if (postedFile != null)
                {
                    return postedFile.FileName;
                }

                if (!string.IsNullOrEmpty(textContentFileName))
                {
                    return textContentFileName;
                }

                return Path.GetFileName(localPath); 
            }
        }

        /// <summary>
        /// Opens a new stream for reading the file content
        /// </summary>
        public Stream OpenStream()
        {
            if (postedFile != null)
            {
                return postedFile.InputStream;
            }

            if (!string.IsNullOrEmpty(textContentFileName))
            {
                return new MemoryStream(Encoding.UTF8.GetBytes(textContent));
            }

            return new FileStream(localPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
    }
}
