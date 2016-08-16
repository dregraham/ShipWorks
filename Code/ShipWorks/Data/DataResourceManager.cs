using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Interapptive.Shared;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data
{
    /// <summary>
    /// Manages storages and retrieval of all resources saved in the database.
    /// </summary>
    public static class DataResourceManager
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DataResourceManager));

        // All the resource references that we know their metadata.
        static Dictionary<long, DataResourceReference> cachedReferences = new Dictionary<long, DataResourceReference>();

        // Used for thread sync
        static object resourceLock = new object();

        // Time-to-live for a cached resource.  It resets on each file system access.
        static TimeSpan resourceTimeToLive = TimeSpan.FromDays(14);

        // Sometimes we will want to query without getting the data fields
        static ExcludeIncludeFieldsList excludeDataFields = new ExcludeIncludeFieldsList((IList) new IEntityFieldCore[] { ResourceFields.Data, ResourceFields.Checksum });

        /// <summary>
        /// Static constructor
        /// </summary>
        static DataResourceManager()
        {
        }

        /// <summary>
        /// Provide method for background process to register thread to clean up resource cache
        /// </summary>
        public static void RegisterResourceCacheCleanup()
        {
            IdleWatcher.RegisterDatabaseIndependentWork("ResouceCacheCleanup", CleanupThread, TimeSpan.FromHours(2));
        }

        /// <summary>
        /// Initialize the class on connection to a new database
        /// </summary>
        public static void InitializeForCurrentDatabase()
        {
            cachedReferences.Clear();
        }

        /// <summary>
        /// Register the file as a resource in the database.  If already present, the existing reference is returned.
        /// </summary>
        public static DataResourceReference CreateFromFile(string filename, long consumerID)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }

            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("The resource file could not be found.", filename);
            }

            byte[] data = File.ReadAllBytes(filename);

            return InstantiateResource(data, consumerID, Path.GetFileName(filename), false);
        }

        /// <summary>
        /// Register the data as a resource in the database.  If already present, the existing reference is returned.
        /// </summary>
        public static DataResourceReference CreateFromText(string text, long consumerID)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return InstantiateResource(Encoding.UTF8.GetBytes(text), consumerID, null, true);
        }

        /// <summary>
        /// Register the data as a resource in the database.  If already present, the existing reference is returned.
        /// </summary>
        public static DataResourceReference CreateFromBytes(byte[] data, long consumerID, string label)
        {
            return InstantiateResource(data, consumerID, label, false);
        }

        /// <summary>
        /// Instantiate a new resource with the given properties
        /// </summary>
        private static DataResourceReference InstantiateResource(byte[] data, long consumerID, string label, bool compress)
        {
            string resourceFilename;
            long resourceID = EnsureResourceData(data, label, compress, out resourceFilename);

            // The key is the data itself - in other words one reference per unique type of resource data.  We can use
            // the ResourceID as a key in that case.
            string key = string.Format("#{0}", resourceID);

            // However, if a label is supplied, then we must be unique per label too.  This is so that if like one shipping label image resource is created
            // for the SAME label image, but for labels "Full" and "Barcode", each resource gets created.
            if (!string.IsNullOrEmpty(label))
            {
                key += string.Format("_{0}", label);
            }

            // Ensure this consumer\key pair references the resource
            long referenceID = ObjectReferenceManager.SetReference(consumerID, key, resourceID);

            // Create the reference object to return to the user
            DataResourceReference reference = new DataResourceReference(referenceID, resourceID, resourceFilename, label);

            lock (resourceLock)
            {
                cachedReferences[referenceID] = reference;
            }

            return reference;
        }

        /// <summary>
        /// Ensure a Resource row exists for the given data.  If 'label' contains a filename and the resource does not yet exist, the extension of the filename is used
        /// as the new resource filename extension.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static long EnsureResourceData(byte[] data, string label, bool compress, out string resourceFilename)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            log.InfoFormat("Ensuring resource ({0})", label);

            long resourceID;

            lock (resourceLock)
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    SHA256 sha = SHA256.Create();
                    byte[] checksum = sha.ComputeHash(data);

                    // See if we can find an existing resource
                    ResourceCollection resources = new ResourceCollection();
                    adapter.FetchEntityCollection(resources, new RelationPredicateBucket(ResourceFields.Checksum == (object) checksum), 1, null, null, excludeDataFields);

                    // The resource already exists, just use it
                    if (resources.Count > 0)
                    {
                        resourceID = resources[0].ResourceID;
                        resourceFilename = resources[0].Filename;

                        log.InfoFormat("Found existing resource {0}", resourceFilename);
                    }
                    else
                    {
                        // If "label" is a file path, use it's extension for our resource file extension.  Otherwise default to our own.
                        string fileExtension = (!string.IsNullOrEmpty(label) && Path.HasExtension(label)) ? Path.GetExtension(label).ToLowerInvariant() : ".swr";

                        // Find an available filename
                        while (true)
                        {
                            resourceFilename = string.Format("{0}{1}",
                                Guid.NewGuid().ToString("D").Substring(0, 8),
                                fileExtension);

                            if (ResourceCollection.GetCount(adapter, ResourceFields.Filename == resourceFilename) == 0)
                            {
                                break;
                            }
                        }

                        ResourceEntity resource = new ResourceEntity();
                        resource.Data = compress ? GZipUtility.Compress(data) : data;
                        resource.Checksum = checksum;
                        resource.Compressed = compress;
                        resource.Filename = resourceFilename;

                        // Don't refetch b\c we don't want to pull back the Data
                        adapter.SaveEntity(resource, false);

                        // Get the resource id
                        resourceID = (long) resource.GetCurrentFieldValue((int) ResourceFieldIndex.ResourceID);

                        log.InfoFormat("Created new resource {0}", resourceFilename);
                    }

                    adapter.Commit();
                }

                string filePath = Path.Combine(DataPath.CurrentResources, resourceFilename);

                try
                {
                    // Go ahead and make sure its written locally to our resource path, as if its being asked for now it will likely be used soon
                    if (!File.Exists(filePath))
                    {
                        File.WriteAllBytes(filePath, data);
                    }
                    else
                    {
                        File.SetLastAccessTimeUtc(filePath, DateTime.UtcNow);
                    }
                }
                catch (IOException ex)
                {
                    log.Warn("Couldn't update file " + filePath, ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    log.Warn(string.Format("Unable to access file {0}", filePath), ex);
                }
            }

            return resourceID;
        }

        /// <summary>
        /// Load the resource reference with the given ID.  This ensures the resource is loaded and cached on disk.
        /// If the resource is invalid or has been deleted from the database, null is returned.
        /// </summary>
        public static DataResourceReference LoadResourceReference(long referenceID)
        {
            // Get the resource information
            DataResourceReference resourceReference = GetResourceReference(referenceID);

            // If we couldn't get it, no such resource exists.
            if (resourceReference == null)
            {
                log.InfoFormat("Resource reference {0} does not exist in the database.", referenceID);
                return null;
            }

            // This ensures the local file data has been pulled into the cache
            resourceReference.GetCachedFilename();

            return resourceReference;
        }

        /// <summary>
        /// Get each resource for the consumer and ensure that all the resource data files have been loaded and cached locally
        /// </summary>
        public static List<DataResourceReference> LoadConsumerResourceReferences(long consumerID)
        {
            Stopwatch sw = Stopwatch.StartNew();

            List<DataResourceReference> resources = GetConsumerResourceReferences(consumerID);
            resources.ForEach(resource => resource.GetCachedFilename());

            log.DebugFormat("LoadConsumerResources: {0}", sw.Elapsed.TotalSeconds);

            return resources;
        }

        /// <summary>
        /// Get all the resource referenced by the consumer, but the local cached data files will not yet be loaded
        /// </summary>
        public static List<DataResourceReference> GetConsumerResourceReferences(long consumerID)
        {
            Stopwatch sw = Stopwatch.StartNew();

            List<DataResourceReference> resources = new List<DataResourceReference>();

            foreach (long referenceID in GetConsumerResourceReferenceIDs(consumerID))
            {
                // Get the resource information
                DataResourceReference resourceReference = GetResourceReference(referenceID);

                // If we couldn't get it, no such resource exists.
                if (resourceReference == null)
                {
                    log.InfoFormat("Resource reference {0} does not exist in the database.", referenceID);
                    continue;
                }

                resources.Add(resourceReference);
            }

            log.DebugFormat("GetConsumerResources: {0}", sw.Elapsed.TotalSeconds);

            return resources;
        }

        /// <summary>
        /// Get the list of resource keys used by the given consumer
        /// </summary>
        public static List<long> GetConsumerResourceReferenceIDs(long consumerID)
        {
            List<long> references = new List<long>();

            foreach (ObjectReferenceEntity reference in ObjectReferenceCollection.Fetch(SqlAdapter.Default, ObjectReferenceFields.ConsumerID == consumerID))
            {
                if (EntityUtility.GetEntityType(reference.ObjectID) == EntityType.ResourceEntity)
                {
                    references.Add(reference.ObjectReferenceID);
                }
            }

            return references;
        }

        /// <summary>
        /// Release the resource reference used by the given consumer.  The resource is not deleted from the database.  Only
        /// the ObjectReference entry is removed.
        /// </summary>
        public static void ReleaseResourceReference(long referenceID)
        {
            ObjectReferenceManager.ClearReference(referenceID);
        }

        /// <summary>
        /// Get the resource reference of the resource with the given id.  The local cached file data will not have necessarily been loaded
        /// </summary>
        public static DataResourceReference GetResourceReference(long referenceID)
        {
            lock (resourceLock)
            {
                // First see if its in the cache
                DataResourceReference reference;
                if (cachedReferences.TryGetValue(referenceID, out reference))
                {
                    return reference;
                }

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    // See if we can find the reference
                    ObjectReferenceCollection references = ObjectReferenceCollection.Fetch(adapter, ObjectReferenceFields.ObjectReferenceID == referenceID);
                    if (references.Count == 1)
                    {
                        ObjectReferenceEntity objectReference = references[0];

                        // See if we can find the resource.  The only way we wouldn't is if someone manually removed it from the DB
                        ResourceCollection resources = new ResourceCollection();
                        adapter.FetchEntityCollection(resources, new RelationPredicateBucket(ResourceFields.ResourceID == objectReference.ObjectID), 1, null, null, excludeDataFields);

                        if (resources.Count > 0)
                        {
                            ResourceEntity resource = resources[0];

                            string label = null;

                            // If the user applied a label, then we put it after the underscore in the key.  That code is in InstantiateResource
                            int marker = objectReference.ReferenceKey.IndexOf("_");
                            if (marker >= 0)
                            {
                                label = objectReference.ReferenceKey.Substring(marker + 1);
                            }

                            reference = new DataResourceReference(referenceID, resource.ResourceID, resource.Filename, label);
                            cachedReferences[referenceID] = reference;

                            return reference;
                        }
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Ensure the usage of the given resource by the specified consumer is tracked.
        /// </summary>
        private static void EnsureReference(long resourceID, long consumerID, SqlAdapter adapter)
        {
            // Ensure its a valid resource
            if (ResourceCollection.GetCount(adapter, ResourceFields.ResourceID == resourceID) == 1)
            {
                ObjectReferenceManager.SetReference(consumerID, string.Format("#{0}", resourceID), resourceID, null);
            }
        }

        /// <summary>
        /// Delete resources that are no longer being referenced
        /// </summary>
        public static void DeleteAbandonedResourceData()
        {
            using (new LoggedStopwatch(log, "Delete abandoned resources."))
            {
                // Set the timeout to unlimited.  The stored procedure will take care of it's run time.
                const int timeoutSeconds = 0;
                string scriptName = EnumHelper.GetApiValue(PurgeDatabaseType.AbandonedResources);

                try
                {
                    // we always want this call to be the deadlock victim
                    using (new SqlDeadlockPriorityScope(-5))
                    {
                        using (SqlConnection connection = SqlSession.Current.OpenConnection(timeoutSeconds))
                        {
                            try
                            {
                                using (SqlCommand command = connection.CreateCommand())
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.CommandText = scriptName;
                                    // Disable the command timeout since the scripts should take care of timing themselves out
                                    command.CommandTimeout = timeoutSeconds;
                                    command.Parameters.AddWithValue("@olderThan", DateTime.UtcNow);
                                    command.Parameters.AddWithValue("@runUntil", DateTime.UtcNow.AddMinutes(15));

                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (SqlException ex)
                            {
                                string exceptionMessage = ex.Message.ToLower();
                                if (exceptionMessage.Contains("sqllockexception") || exceptionMessage.Contains("could not acquire applock"))
                                {
                                    log.Warn(ex.Message);
                                }
                                else
                                {
                                    log.Error(string.Format("Error likely returned from stored proc {0}", scriptName), ex);
                                    throw;
                                }
                            }
                        }
                    }
                }
                catch (SqlDeadlockException ex)
                {
                    // don't let it crash, we'll just try to cleanup the next go-around
                    log.Error("Deadlock detected trying to deleted abandoned resource data.", ex);
                }
            }
        }

        /// <summary>
        /// Runs on a schedule to see if any log files are in need of deleting.
        /// </summary>
        private static void CleanupThread()
        {
            log.InfoFormat("Running resource manager cleanup...");

            try
            {
                foreach (string resourcePath in DataPath.AllResources)
                {
                    DirectoryInfo resourceRoot = new DirectoryInfo(resourcePath);
                    if (!resourceRoot.Exists)
                    {
                        continue;
                    }

                    // Look at each entry in the directory
                    foreach (FileSystemInfo fsi in resourceRoot.GetFileSystemInfos())
                    {
                        // We only look at files.  There should not be any folders.
                        FileInfo fi = fsi as FileInfo;
                        if (fi != null)
                        {
                            if (fi.LastWriteTime < DateTime.Now - resourceTimeToLive)
                            {
                                log.InfoFormat("Deleting cached resource '{0}'", fsi.Name);

                                File.Delete(fsi.FullName);
                            }

                            // If there's a bunch of stuff to delete, we don't want to peg the CPU
                            Thread.Sleep(2);
                        }

                        // Quit if we leave the idle state
                        if (!IdleWatcher.IsIdle)
                        {
                            return;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                log.Error("Failed during resource cache cleanup.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Error("Failed during resource cache cleanup.", ex);
            }
        }
    }
}
