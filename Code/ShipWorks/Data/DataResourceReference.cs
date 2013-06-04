using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ShipWorks.ApplicationCore;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Utility;
using Interapptive.Shared.IO.Zip;

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
                string alternate = Path.GetFileNameWithoutExtension(filename) + "." + extension;

                if (!File.Exists(Path.Combine(DataPath.CurrentResources, alternate)))
                {
                    File.Copy(GetCachedFilename(), Path.Combine(DataPath.CurrentResources, alternate));
                }

                return alternate;
            }
        }
    }
}
