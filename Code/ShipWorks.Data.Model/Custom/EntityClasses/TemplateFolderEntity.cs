using System;
using System.Data;
using System.Text;
using ShipWorks.Templates;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial-class extension of LLBLGen class
    /// </summary>
    public partial class TemplateFolderEntity : IEquatable<TemplateFolderEntity>
    {
        object templateTree;
        bool readOnly = false;

        /// <summary>
        /// Get the full path and the name combined.  Does not end in a backslash.
        /// </summary>
        public string FullName
        {
            get { return string.Format(@"{0}{1}", FolderPath, Name); }
        }

        /// <summary>
        /// Gets the full path to the folder the folder is contained in.  Ends end the trailing backslash.
        /// </summary>
        public string FolderPath
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                TemplateFolderEntity folder = this.ParentFolder;
                while (folder != null)
                {
                    sb.Insert(0, string.Format(@"{0}\", folder.Name));

                    folder = folder.ParentFolder;
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Indicates if the folder is a builtin system folder that should not be editable by the user. This includes the System folder and the System\Snippets folder.
        /// </summary>
        public bool IsBuiltin
        {
            get { return IsSystemFolder || IsSnippetsFolder; }
        }

        /// <summary>
        /// Indicates if the folder is the single instance of the root "System" folder.
        /// </summary>
        public bool IsSystemFolder
        {
            get { return TemplateFolderID == TemplateBuiltinFolders.SystemFolderID; }
        }

        /// <summary>
        /// Indicates if this folder represents the Snippets folder
        /// </summary>
        public bool IsSnippetsFolder
        {
            get { return TemplateFolderID == TemplateBuiltinFolders.SnippetsFolderID; }
        }

        /// <summary>
        /// Indicates if this folder is the snippets folder, or a child folder of the snippets folder
        /// </summary>
        public bool IsSnippetsOrDescendantFolder
        {
            get
            {
                TemplateFolderEntity folder = this;
                while (folder != null)
                {
                    if (folder.IsSnippetsFolder)
                    {
                        return true;
                    }

                    folder = folder.ParentFolder;
                }

                return false;
            }
        }

        /// <summary>
        /// The TemplateTree (if any) this TemplateFolderEntity instance belongs to.
        /// </summary>
        public object TemplateTree
        {
            get { return templateTree; }
            set { templateTree = value; }
        }

        /// <summary>
        /// Indicates if the folder is ReadOnly
        /// </summary>
        public bool IsReadOnly
        {
            get { return readOnly; }
        }

        /// <summary>
        /// Makes the folder throw ReadOnlyException if editing is attempted.
        /// </summary>
        public void MakeReadOnly()
        {
            readOnly = true;
        }

        /// <summary>
        /// A field value is changing
        /// </summary>
        protected override void OnFieldValueChanged(object originalValue, int field)
        {
            if (readOnly)
            {
                throw new ReadOnlyException("The folder instance has been marked as ReadOnly.");
            }

            base.OnFieldValueChanged(originalValue, field);
        }

        /// <summary>
        /// Implement IEquality equals to ensure reference equality semantics.
        /// </summary>
        /// <remarks>
        /// We have to do this because when searching lists for templates and folders we want to do it based on
        /// reference existance, and by default the LLBLGen equality operaters do a check based on primary key values.
        /// </remarks>
        public bool Equals(TemplateFolderEntity other)
        {
            return object.ReferenceEquals(this, other);
        }
    }
}
