using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Drawing;
using ShipWorks.Properties;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Templates.Controls
{
    /// <summary>
    /// Wrapper for nodes in the 
    /// </summary>
    public class TemplateTreeNode
    {
        // One and only one of these will be non-null
        TemplateFolderEntity folder;
        TemplateEntity template;

        // The single instance of the node that represents the root of the tree
        static TemplateTreeNode rootNode = new TemplateTreeNode();
        
        /// <summary>
        /// Used for creating the root node
        /// </summary>
        protected TemplateTreeNode()
        {

        }

        /// <summary>
        /// Folder node constructor
        /// </summary>
        public TemplateTreeNode(TemplateFolderEntity folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            this.folder = folder;
        }

        /// <summary>
        /// Template node constructor
        /// </summary>
        public TemplateTreeNode(TemplateEntity template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            this.template = template;
        }

        /// <summary>
        /// The node to be used as the root of the top-level folders.
        /// </summary>
        public static TemplateTreeNode RootNode
        {
            get { return rootNode; }
        }

        /// <summary>
        /// Indicates if the row represents a folder.  If false, it represents a template.
        /// </summary>
        public bool IsFolder
        {
            get { return IsRoot || folder != null; }
        }

        /// <summary>
        /// Indicates if the row represents the root of the folder structure.  This is not a real row in the database, but used as an in-memory
        /// placeholder parent for top-level folders.
        /// </summary>
        public bool IsRoot
        {
            get { return folder == null && template == null; }
        }

        /// <summary>
        /// Indicates if this node is a template snippet, or a folder that can contain snippest.
        /// </summary>
        public bool IsSnippetNode
        {
            get { return (IsFolder && Folder.IsSnippetsOrDescendantFolder) || (!IsFolder && Template.IsSnippet); }
        }

        /// <summary>
        /// If IsFolder is true, the TemplateFolder represented by the row. Otherwise null.
        /// </summary>
        public TemplateFolderEntity Folder
        {
            get { return folder; }
        }

        /// <summary>
        /// If IsFolder is false, the Template represented by the row.  Otherwise null.
        /// </summary>
        public TemplateEntity Template
        {
            get { return template; }
        }
        
        /// <summary>
        /// Either the template or folder entity
        /// </summary>
        public EntityBase2 Entity
        {
            get { return IsFolder ? (EntityBase2) Folder : Template; }
        }

        /// <summary>
        /// The primary key of the underlying object
        /// </summary>
        public long ID
        {
            get 
            {
                if (IsRoot)
                {
                    return -1;
                }

                return IsFolder ? Folder.TemplateFolderID : Template.TemplateID; 
            }
        }

        /// <summary>
        /// The name of the underlying object
        /// </summary>
        public string Name
        {
            get
            {
                if (IsRoot)
                {
                    return "Template Folders";
                }

                return IsFolder ? Folder.Name : Template.Name;
            }
            set
            {
                if (IsRoot)
                {
                    throw new InvalidOperationException("Cannot change the name of the root folder.");
                }

                if (IsFolder)
                {
                    Folder.Name = value;
                }
                else
                {
                    Template.Name = value;
                }
            }
        }

        /// <summary>
        /// The image to represent the node in the template tree
        /// </summary>
        public Image Image
        {
            get
            {
                if (IsRoot)
                {
                    return Resources.text_tree;
                }

                if (IsFolder)
                {
                    if (Folder.IsSnippetsFolder)
                    {
                        return Resources.template_snippet_folder;
                    }

                    if (Folder.IsSystemFolder)
                    {
                        return Resources.template_system_folder;
                    }

                    return Resources.folderclosed;
                }
                else
                {
                    return TemplateHelper.GetTemplateImage(Template);
                }
            }
        }

        /// <summary>
        /// The folder that contains this folder or template
        /// </summary>
        public TemplateFolderEntity ParentFolder
        {
            get
            {
                if (IsRoot)
                {
                    return null;
                }

                return IsFolder ? Folder.ParentFolder : Template.ParentFolder;
            }
        }

        /// <summary>
        /// The ID of the folder that contains this folder or template
        /// </summary>
        public long? ParentFolderID
        {
            get
            {
                if (IsRoot)
                {
                    return null;
                }

                return IsFolder ? Folder.ParentFolderID : Template.ParentFolderID;
            }
            set
            {
                if (IsRoot)
                {
                    throw new InvalidOperationException("Cannot change the ParentFolderID of the root.");
                }

                if (IsFolder)
                {
                    Folder.ParentFolderID = value;
                }
                else
                {
                    if (value == null)
                    {
                        throw new InvalidOperationException("Cannot set a template to have a null parent folder.");
                    }

                    Template.ParentFolderID = value.Value;
                }
            }
        }
    }
}
