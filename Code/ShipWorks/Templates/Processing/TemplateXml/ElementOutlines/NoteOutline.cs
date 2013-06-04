using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using log4net;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Content;
using ShipWorks.Users;
using ShipWorks.Data.Model;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for a 'Note' element
    /// </summary>
    public class NoteOutline : ElementOutline
    {
        static readonly ILog log = LogManager.GetLogger(typeof(NoteOutline));

        long noteID;
        NoteEntity note;

        /// <summary>
        /// Constructor
        /// </summary>
        public NoteOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("ID", () => noteID);

            AddElement("Source", () => EnumHelper.GetDescription((NoteSource) Note.Source));
            AddElement("Visibility", () => EnumHelper.GetDescription((NoteVisibility) Note.Visibility));
            AddElement("Text", () => Note.Text);
            AddElement("LastEdited", () => Note.Edited);

            AddElement("User", new UserOutline(context), () => Note.UserID == null ? (UserEntity) null : UserManager.GetUser(Note.UserID.Value));
        }

        /// <summary>
        /// Create a clone of the outline, bound to the given data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new NoteOutline(Context) { noteID = (long) data };
        }

        /// <summary>
        /// The NoteEntity represented by the bound outline
        /// </summary>
        private NoteEntity Note
        {
            get
            {
                if (note == null)
                {
                    note = (NoteEntity) DataProvider.GetEntity(noteID);
                    if (note == null)
                    {
                        log.WarnFormat("Note {0} was deleted and cannot be processed by template.", noteID);
                        throw new TemplateProcessException("A note has been deleted.");
                    }
                }

                return note;
            }
        }

        /// <summary>
        /// Utility function to create notes in the legacy 2x format
        /// </summary>
        public static ElementOutline CreateLegacy2xNotesOutline(TemplateTranslationContext context, Func<long> idProvider)
        {
            ElementOutline outline = new ElementOutline(context);
            outline.AddAttributeLegacy2x();
            outline.AddTextContent(() =>
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (NoteEntity note in DataProvider.GetRelatedEntities(idProvider(), EntityType.NoteEntity).Cast<NoteEntity>())
                    {
                        if (sb.Length > 0)
                        {
                            sb.AppendFormat("\r\n\r\n");
                        }

                        sb.Append(note.Text);
                    }

                    return sb.ToString();
                });

            return outline;
        }
    }
}
