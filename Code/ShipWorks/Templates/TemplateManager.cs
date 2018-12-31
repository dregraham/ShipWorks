using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(TemplateManager));

        private static bool needCheckForChanges = false;

        private static TableSynchronizer<TemplateFolderEntity> folderSynchronizer;
        private static TableSynchronizer<TemplateEntity> templateSynchronizer;
        private static TemplateTree templateTree;

        private static List<ObjectLabelEntity> deletedTemplateLabels;
        
        private const string PickListsFolderName = "Pick Lists";
        private const string ReportsFolderName = "Reports";

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

        /// <summary>
        /// Fetch all of the pick list templates from the pick list folder. If the folder does not exist, return
        /// templates from the reports folder
        /// </summary>
        public static IEnumerable<TemplateEntity> FetchPickListTemplates()
        {
            IEnumerable<TemplateEntity> pickListTemplates = Tree.AllTemplates.Where(t => t.ParentFolder.Name == PickListsFolderName);
            if (pickListTemplates.None())
            {
                pickListTemplates = Tree.AllTemplates.Where(t => t.ParentFolder.Name == ReportsFolderName);
            }

            return pickListTemplates;
        }

        /// <summary>
        /// Fetch the default pick list template or null if none found
        /// </summary>
        public static TemplateEntity FetchDefaultPickListTemplate()
        {
            long? pickListTemplateId = ConfigurationData.FetchReadOnly().DefaultPickListTemplateID;

            return pickListTemplateId.HasValue ? FetchTemplateByID(pickListTemplateId.Value) : null;
        }

        /// <summary>
        /// Fetch the first template with a TemplateID that matches the given id. Returns null if none found.
        /// </summary>
        private static TemplateEntity FetchTemplateByID(long pickListTemplateId) =>
            Tree.AllTemplates.FirstOrDefault(t => t.TemplateID == pickListTemplateId);
    }
}
