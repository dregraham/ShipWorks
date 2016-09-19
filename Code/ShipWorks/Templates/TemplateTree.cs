using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Interapptive.Shared.Data;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Loads and provides the relational tree of template and folder entities.  The object is not threadsafe, and does not return copies of the entities in the tree but the same
    /// entities each time.
    /// </summary>
    public sealed class TemplateTree
    {
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateTree));

        List<TemplateFolderEntity> rootFolders;

        // The "virtual" template System\Snippets that doesn't really exist but can be included as a shortcut to including all templates.
        TemplateEntity snippetsImportTemplate = null;
        List<string> snippetsImportList = null;

        // The cache of TemplateXsl objects for the tree
        TemplateXslCache xslCache;

        // Indicates if this tree is readonly
        bool readOnly = false;

        /// <summary>
        /// Creates a new ReadOnly TemplateTree based on the given templates and folders.  The collections passed in are copied.
        /// </summary>
        public static TemplateTree CreateFrom(IEnumerable<TemplateFolderEntity> folders, IEnumerable<TemplateEntity> templates)
        {
            return CreateFrom(folders, templates, null);
        }

        /// <summary>
        /// Creates a new ReadOnly TemplateTree based on the given templates and folders.  The collections passed in are copied.  The TemplateXsl objects from the given xslCache
        /// are used to prepopulate the cache.
        /// </summary>
        public static TemplateTree CreateFrom(IEnumerable<TemplateFolderEntity> folders, IEnumerable<TemplateEntity> templates, TemplateXslCache xslCache)
        {
            TemplateTree tree = new TemplateTree(folders, templates, xslCache);
            tree.MakeReadOnly();

            return tree;
        }

        /// <summary>
        /// Make a deep clone of this TemplateTree
        /// </summary>
        public TemplateTree CreateEditableClone()
        {
            TemplateTree tree = new TemplateTree(AllFolders, AllTemplates, xslCache);

            // To make it be editable we just don't call MakeReadOnly
            return tree;
        }

        /// <summary>
        /// Use one of the static CreateFrom methods.
        /// </summary>
        private TemplateTree(IEnumerable<TemplateFolderEntity> folders, IEnumerable<TemplateEntity> templates, TemplateXslCache xslCache)
        {
            // Create a new cache - but based on the one given
            this.xslCache = new TemplateXslCache(this, xslCache);

            BuildTree(folders, templates);

            Sort();
        }

        /// <summary>
        /// Make all the templates and folders in the tree ReadOnly to be sure we don't accidentally edit a tree that wasn't supposed to be.  This is how we deal with thread safety.
        /// </summary>
        private void MakeReadOnly()
        {
            foreach (TemplateEntity template in AllTemplates)
            {
                template.MakeReadOnly();
            }

            foreach (TemplateFolderEntity folder in AllFolders)
            {
                folder.MakeReadOnly();
            }

            readOnly = true;
        }

        /// <summary>
        /// The most up-to-date list of template folders
        /// </summary>
        public IList<TemplateFolderEntity> RootFolders
        {
            get
            {
                if (!readOnly)
                {
                    Sort();
                }

                return rootFolders.Where(f => f.Fields.State != EntityState.Deleted).OrderBy(f => f.Name).ToList();
            }
        }

        /// <summary>
        /// Return a flat list of all folders in no particular order
        /// </summary>
        public IList<TemplateFolderEntity> AllFolders
        {
            get
            {
                return GetFoldersAndDescendants(rootFolders);
            }
        }

        /// <summary>
        /// Return the given list of folders - plus all the descenedant folders of each folder
        /// </summary>
        private IList<TemplateFolderEntity> GetFoldersAndDescendants(IList<TemplateFolderEntity> folders)
        {
            // Filter out the deleted ones
            folders = folders.Where(f => f.Fields.State != EntityState.Deleted).ToList();

            // Start off with the given list
            List<TemplateFolderEntity> allFolders = new List<TemplateFolderEntity>(folders);

            // Then add in all descendants
            foreach (TemplateFolderEntity folder in folders)
            {
                allFolders.AddRange(GetFoldersAndDescendants(folder.ChildFolders));
            }

            return allFolders;
        }

        /// <summary>
        /// Return a flat list of all templates in no particular order
        /// </summary>
        public IList<TemplateEntity> AllTemplates
        {
            get
            {
                return AllFolders.SelectMany(f => f.Templates).Where(f => f.Fields.State != EntityState.Deleted).ToList();
            }
        }

        /// <summary>
        /// A value that goes up everytime changes are loaded
        /// </summary>
        public long TreeVersion
        {
            get
            {
                long version = 0;

                if (AllFolders.Count > 0)
                {
                    version = AllFolders.Max(f => SqlUtility.GetTimestampValue(f.RowVersion));
                }

                if (AllTemplates.Count > 0)
                {
                    version = Math.Max(version, AllTemplates.Max(f => SqlUtility.GetTimestampValue(f.RowVersion)));
                }

                return version;
            }
        }

        /// <summary>
        /// The cache of compiled TemplateXsl associated with this tree.
        /// </summary>
        public TemplateXslCache XslCache
        {
            get { return xslCache; }
        }

        /// <summary>
        /// Find the template with the complete given folder path and name, or null if it does not exist.
        /// </summary>
        public TemplateEntity FindTemplate(string fullName)
        {
            if (fullName == @"System\Snippets")
            {
                var currentSnippets = AllTemplates.Where(t => t.IsSnippet).Select(t => t.FullName);

                if (snippetsImportTemplate != null)
                {
                    // If the list of snippets has changed, regenerate the fake template
                    if (!snippetsImportList.SequenceEqual(currentSnippets))
                    {
                        snippetsImportTemplate = null;
                    }
                }

                if (snippetsImportTemplate == null)
                {
                    snippetsImportTemplate = CreateSnippetsImportTemplate();
                    snippetsImportList = currentSnippets.ToList();
                }

                return snippetsImportTemplate;
            }

            var results = AllTemplates.Where(t => t.FullName == fullName).ToList();

            if (results.Count == 0)
            {
                return null;
            }

            if (results.Count > 1)
            {
                return null;
            }

            return results[0];
        }

        /// <summary>
        /// Get the template with the given ID, or null if it does not exist
        /// </summary>
        public TemplateEntity GetTemplate(long templateID)
        {
            return AllTemplates.FirstOrDefault(t => t.TemplateID == templateID);
        }

        /// <summary>
        /// Get the folder with the given ID, or null if it does not exist
        /// </summary>
        public TemplateFolderEntity GetFolder(long folderID)
        {
            return AllFolders.FirstOrDefault(f => f.TemplateFolderID == folderID);
        }

        /// <summary>
        /// Add the given folder as a root folder managed by the tree.  Only meant to be called by the TemplateEditingService
        /// </summary>
        internal void IncludeRootFolder(TemplateFolderEntity folder)
        {
            if (readOnly)
            {
                throw new ReadOnlyException();
            }

            if (!rootFolders.Contains(folder))
            {
                rootFolders.Add(folder);
            }
        }

        /// <summary>
        /// Update the references between templates and folders to ensure the tree is properly constructed.
        /// </summary>
        private void BuildTree(IEnumerable<TemplateFolderEntity> sourceFolders, IEnumerable<TemplateEntity> sourceTemplates)
        {
            // We need to create clones of our collections that do not have any references.  We need to make sure not to preserve references - and would end up making an entire
            // clone of the tree for each template and each folder!
            List<TemplateFolderEntity> folders = EntityUtility.CloneEntityCollection(sourceFolders, false);
            List<TemplateEntity> templates = EntityUtility.CloneEntityCollection(sourceTemplates, false);

            snippetsImportTemplate = null;
            snippetsImportList = null; ;

            // Go through each folder
            foreach (TemplateFolderEntity folder in folders)
            {
                Debug.Assert(!folder.IsDirty);

                // Set the parent reference
                folder.ParentFolder = folders.SingleOrDefault(f => f.TemplateFolderID == folder.ParentFolderID);
                folder.TemplateTree = this;

                Debug.Assert(!folder.IsDirty, "We should not be making anything dirty by fixing up these references.");
            }

            // Go through each template
            foreach (TemplateEntity template in templates)
            {
                Debug.Assert(!template.IsDirty);

                // Set the parent reference
                template.ParentFolder = folders.SingleOrDefault(f => f.TemplateFolderID == template.ParentFolderID);
                template.TemplateTree = this;

                Debug.Assert(!template.IsDirty, "We should not be making anything dirty by fixing up these references.");
            }

            this.rootFolders = folders.Where(f => f.ParentFolderID == null).ToList();
        }

        /// <summary>
        /// Sort all folders alphabetically
        /// </summary>
        private void Sort()
        {
            // Sort the templates and folders within each folder
            foreach (TemplateFolderEntity folder in AllFolders)
            {
                folder.ChildFolders.Sort((int) TemplateFolderFieldIndex.Name, ListSortDirection.Ascending);
                folder.Templates.Sort((int) TemplateFieldIndex.Name, ListSortDirection.Ascending);
            }
        }

        /// <summary>
        /// Create the virtual "System\Snippets" template that can be included as a shortcut to including all templates
        /// </summary>
        private TemplateEntity CreateSnippetsImportTemplate()
        {
            TemplateFolderEntity folder = new TemplateFolderEntity();
            folder.TemplateFolderID = -1;
            folder.Name = "System";
            folder.IsNew = false;
            folder.IsDirty = false;
            folder.TemplateTree = this;

            TemplateEntity template = new TemplateEntity();
            template.TemplateID = -1;
            template.ParentFolder = folder;
            template.Name = "Snippets";
            template.Xsl = CreateSnippetsImportXsl();
            template.IsNew = false;
            template.IsDirty = false;
            template.TemplateTree = this;

            return template;
        }

        /// <summary>
        /// Create the XSL content for importing
        /// </summary>
        private string CreateSnippetsImportXsl()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" >");

            foreach (TemplateEntity template in AllTemplates.Where(t => t.IsSnippet))
            {
                sb.AppendFormat("<xsl:import href=\"{0}\" />", template.FullName);
            }

            sb.Append("</xsl:stylesheet>");

            log.InfoFormat("Virtual 'System\\Snippets' regenerating.");

            return sb.ToString();
        }
    }
}
