using System;
using System.Data;
using System.Text;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial-class extension of LLBLGen class
    /// </summary>
    public partial class TemplateEntity : IEquatable<TemplateEntity>
    {
        object templateTree;
        bool readOnly = false;

        /// <summary>
        /// Get the full path and the name combined
        /// </summary>
        public string FullName
        {
            get { return string.Format(@"{0}{1}", FolderPath, Name); }
        }

        /// <summary>
        /// Gets the full path to the folder the template is contained in.  Ends end the trailing backslash.
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
        /// Indicates if this template is a Snippet.  A template is a Snippet simply if it is in the System\Snippets folder.
        /// </summary>
        public bool IsSnippet
        {
            get
            {
                return ParentFolder != null && ParentFolder.IsSnippetsOrDescendantFolder;
            }
        }

        /// <summary>
        /// The TemplateTree (if any) this TemplateEntity instance belongs to.
        /// </summary>
        public object TemplateTree
        {
            get { return templateTree; }
            set { templateTree = value; }
        }

        /// <summary>
        /// Indicates if the TemplateEntity is ReadOnly
        /// </summary>
        public bool IsReadOnly
        {
            get { return readOnly; }
        }

        /// <summary>
        /// Makes the TemplateEntity throw ReadOnlyException if editing is attempted.
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
                throw new ReadOnlyException("The template instance has been marked as ReadOnly.");
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
        public bool Equals(TemplateEntity other)
        {
            return object.ReferenceEquals(this, other);
        }

        /// <summary>
        /// Debugging ease
        /// </summary>
        public override string ToString()
        {
            return string.Format("Template ({0})", FullName);
        }
    }
}
