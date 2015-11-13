using System;
using System.Text;
using System.IO;
using ShipWorks.ApplicationCore;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Utility;
using Interapptive.Shared.IO.Zip;
using System.Drawing;
using System.Drawing.Imaging;

namespace ShipWorks.Data
{
    /// <summary>
    /// Information about a resource saved in the datbase
    /// </summary>
    public class DataResourceReference
    {
        static readonly ILog log = LogManager.GetLogger(typeof(DataResourceReference));

        long referenceID;
        long resourceID;
        string filename;
        string label;

        /// <summary>
        /// Constructor to be used for tests, etc.
        /// </summary>
        protected DataResourceReference()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataResourceReference(long referenceID, long resourceID, string filename, string label)
        {
            this.referenceID = referenceID;
            this.resourceID = resourceID;
            this.filename = filename;
            this.label = label;
        }

        /// <summary>
        /// For logging and debugging.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} ({1})", referenceID, filename);
        }

        /// <summary>
        /// The reference ID of the resource reference in the database. This is not the ResourceID itself
        /// </summary>
        public long ReferenceID
        {
            get { return referenceID; }
        }

        /// <summary>
        /// The resourceID of the resource.  Many references can refer to the same resource data.
        /// </summary>
        public long ResourceID
        {
            get { return resourceID; }
        }

        /// <summary>
        /// The Filename, without path, where the resource should be cached on disk
        /// </summary>
        public string Filename
        {
            get { return filename; }
        }

        /// <summary>
        /// A resource-type specific string indicating where it came form.
        /// </summary>
        public string Label
        {
            get { return label; }
        }

        /// <summary>
        /// Indicates if this is a placeholder resource for data that has been purged
        /// </summary>
        public bool IsPurgedPlaceholder
        {
            get
            {
                return filename.StartsWith("__purged_", StringComparison.InvariantCulture);
            }
        }

        /// <summary>
        /// Read the entire contents of the resource as a text stream.
        /// </summary>
        public string ReadAllText()
        {
            return ReadAllText(Encoding.UTF8);
        }

        /// <summary>
        /// Read the entire contents of the resource as a text stream.
        /// </summary>
        public string ReadAllText(Encoding encoding)
        {
            return File.ReadAllText(GetCachedFilename(), encoding);
        }

        /// <summary>
        /// Ensure that the resource data has been loaded from the database and cached as a local file, and the filename is returned
        /// </summary>
        public string GetCachedFilename()
        {
            string fullPath = Path.Combine(DataPath.CurrentResources, filename);

            // See if the resource file exists on disk
            if (File.Exists(fullPath))
            {
                log.InfoFormat("Resource {0} found cached at '{1}'.", referenceID, filename);

                // "Touch" it, to keep it from getting cleaned up.  Although its possible its in use right now, being read or written by another sw, so
                // we have to handle the exception for that
                try
                {
                    if (DateTime.Now - File.GetLastWriteTime(fullPath) > TimeSpan.FromDays(2))
                    {
                        File.SetLastWriteTime(fullPath, DateTime.Now);
                    }
                }
                catch (IOException ex)
                {
                    // Just let it go - if its being used, its been touched.
                    if (!ex.Message.ToUpperInvariant().Contains("being used by another process".ToUpperInvariant()))
                    {
                        throw;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // Just let it go - if its being used, its been touched.
                }
            }
            // If it does not exist on disk, we have to load it from the database and get it there.
            else
            {
                ResourceEntity resource = new ResourceEntity(resourceID);

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntity(resource);
                }

                // If we didnt fetch it, the resource is gone.
                if (resource.Fields.State != EntityState.Fetched)
                {
                    log.InfoFormat("Resource {0} has been deleted.", referenceID);

                    throw new NotFoundException(string.Format("Could not find resource: {0}", string.Format("{0} ({1})", referenceID, label)));
                }
                else if (IsPurgedPlaceholder)
                {
                    log.InfoFormat("Resource {0} is being cached to '{1}' [Purged].", referenceID, filename);
                    WritePurgedPlaceholerContent(fullPath);
                }
                else
                {
                    log.InfoFormat("Resource {0} is being cached to '{1}'.", referenceID, filename);
                    File.WriteAllBytes(fullPath, resource.Compressed ? GZipUtility.Decompress(resource.Data) : resource.Data);
                }
            }

            return fullPath;
        }

        /// <summary>
        /// Get the filename, but with the given extension.  Since shipworks writes everything with an "swr" extension, this is
        /// needed to get a version with the correct extension.
        /// </summary>
        public string GetAlternateFilename(string extension)
        {
            lock (filename)
            {
                string existing = GetCachedFilename();
                string alternate = ConstructAlternateFilename(extension);

                if (!File.Exists(alternate))
                {
                    File.Copy(existing, alternate);
                }

                return Path.GetFileName(alternate);
            }
        }

        /// <summary>
        /// Regenerates the filename for the resource with the given extension. Useful in exception handling in cases where the
        /// resource could not be loaded (due to corruption or some other reason).
        /// </summary>
        /// <param name="extension">The extension.</param>
        public void RegenerateAlternateFile(string extension)
        {
            lock (filename)
            {
                string alternate = ConstructAlternateFilename(extension);

                // We basically just want to wipe out the "friendly" file with the correct extension
                // and copy over the .swr file again.
                if (File.Exists(alternate))
                {
                    File.Delete(alternate);
                }

                File.Copy(GetCachedFilename(), alternate);
            }
        }

        /// <summary>
        /// Constructs the alternate filename for this resource.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        private string ConstructAlternateFilename(string extension)
        {
            return Path.Combine(DataPath.CurrentResources, Path.GetFileNameWithoutExtension(filename) + "." + extension);
        }

        /// <summary>
        /// Write out the placeholder content to use for purges
        /// </summary>
        private void WritePurgedPlaceholerContent(string fullPath)
        {
            switch (Filename)
            {
                case "__purged_print_html.swr":
                case "__purged_print_thermal.swr":
                case "__purged_email_plain.swr":
                case "__purged_email_html_swr":

                    // All of these are special cased in the Email viewer and print result viewer to show the content, so the file contents don't really matter
                    File.WriteAllText(fullPath, "[Purged]");

                    break;

                // PNG, GIF, or JPG shipping label image
                case "__purged_label.png":
                case "__purged_label.gif":
                case "__purged_label.jpg":

                    // Determine the format
                    ImageFormat imageFormat = ImageFormat.Png;
                    if (fullPath.EndsWith(".gif")) imageFormat = ImageFormat.Gif;
                    if (fullPath.EndsWith(".jpg")) imageFormat = ImageFormat.Jpeg;

                    using (Bitmap image = new Bitmap(500, 200))
                    {
                        using (Graphics g = Graphics.FromImage(image))
                        {
                            g.Clear(Color.White);

                            using (Font font = new Font("Tahoma", 10.25f))
                            {
                                g.DrawString("Content Removed", font, Brushes.Black, new PointF(50, 50));
                                g.DrawString("This label has been deleted by the 'Delete old data' action task.", font, Brushes.Black, new Rectangle(50, 90, 400, 100));
                            }
                        }

                        image.Save(fullPath, imageFormat);
                    }

                    break;

                // EPL
                case "__purged_label_epl.swr":

                    File.WriteAllText(fullPath,
                        "N\r\n" +
                        "ZB\r\n" +
                        "R100,100\r\n" +
                        "A0,0,0,3,1,1,N,\"Content Removed\"\r\n" +
                        "A0,80,0,3,1,1,N,\"This label has been deleted by the\"\r\n" +
                        "A0,120,0,3,1,1,N,\"'Delete old data' action task.\"\r\n" +
                        "P1\r\n");

                    break;

                // ZPL
                case "__purged_label_zpl.swr":

                    File.WriteAllText(fullPath,
                        "^XA^CFD\r\n" +
                        "^FO100,100^AQ\r\n" +
                        "^FDContent Removed^FS\r\n" +
                        "^FO100,180\r\n" +
                        "^AQ\r\n" +
                        "^FDThis label has been deleted by the 'Delete old data' action task.^FS\r\n" +
                        "^XZ");

                    break;

                default:
                    throw new InvalidOperationException(string.Format("Filename {0} is not a recognized purged placeholder.", filename));

            }
        }
    }
}
