using System.Collections.Generic;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Gateway and cache for templates
    /// </summary>
    public static class TemplateManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateManager));

        static bool needCheckForChanges = false;

        static TableSynchronizer<TemplateFolderEntity> folderSynchronizer;
        static TableSynchronizer<TemplateEntity> templateSynchronizer;
        static TemplateTree templateTree;

        static List<ObjectLabelEntity> deletedTemplateLabels;

        /// <summary>
        /// Initialize TemplateManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            folderSynchronizer = new TableSynchronizer<TemplateFolderEntity>();
            templateSynchronizer = new TableSynchronizer<TemplateEntity>();

            deletedTemplateLabels = null;
            templateTree = null;

            InternalCheckForChanges();

            // Go ahead and pre-compile all the templates now
            foreach (TemplateEntity template in templateTree.AllTemplates)
            {
                templateTree.XslCache.FromTemplate(template);
            }
        }

        /// <summary>
        /// Mark that the next time the tree is requested the db will need to be checked for changes
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            lock (templateSynchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        private static void InternalCheckForChanges()
        {
            lock (templateSynchronizer)
            {
                bool changes = false;

                if (folderSynchronizer.Synchronize())
                {
                    changes = true;
                }

                if (templateSynchronizer.Synchronize())
                {
                    changes = true;
                }

                if (changes || templateTree == null)
                {
                    // Create a new TemplateTree based on the updated template and folder collections.  The Tree will just be ReadOnly copy - so it will be thread safe.
                    templateTree = TemplateTree.CreateFrom(
                        folderSynchronizer.EntityCollection,
                        templateSynchronizer.EntityCollection,
                        (templateTree != null) ? templateTree.XslCache : null);

                    // If there are changes, we'll have to reload the deleted templates next time
                    deletedTemplateLabels = null;
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// The global default live ReadOnly TemplateTree
        /// </summary>
        public static TemplateTree Tree
        {
            get
            {
                lock (templateSynchronizer)
                {
                    if (needCheckForChanges)
                    {
                        InternalCheckForChanges();
                    }

                    return templateTree;
                }
            }
        }

        /// <summary>
        /// Get the labels for all templates that have been deleted
        /// </summary>
        public static IList<ObjectLabelEntity> DeletedTemplateLabels
        {
            get
            {
                if (deletedTemplateLabels == null)
                {
                    deletedTemplateLabels = new List<ObjectLabelEntity>();

                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        deletedTemplateLabels.AddRange(ObjectLabelCollection.Fetch(adapter,
                            ObjectLabelFields.IsDeleted == true &
                            ObjectLabelFields.ObjectType == EntityUtility.GetEntitySeed(EntityType.TemplateEntity)));
                    }
                }

                return deletedTemplateLabels;
            }
        }
    }
}
