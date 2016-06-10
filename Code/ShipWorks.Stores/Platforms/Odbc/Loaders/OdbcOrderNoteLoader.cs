using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Loaders
{
    /// <summary>
    /// Loads note into OdbcOrder
    /// </summary>
    public class OdbcOrderNoteLoader : IOdbcOrderDetailLoader
    {
        private readonly IOrderNote note;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcOrderNoteLoader"/> class.
        /// </summary>
        public OdbcOrderNoteLoader(IOrderNote note)
        {
            this.note = note;
        }

        /// <summary>
        /// Loads the order details into the given entity
        /// </summary>
        public void Load(IOdbcFieldMap map, OrderEntity order)
        {
            IEnumerable<IOdbcFieldMapEntry> mappedNoteFields =
               map.FindEntriesBy(NoteFields.Text, false).ToList();

            foreach (IOdbcFieldMapEntry mappedNote in mappedNoteFields)
            {
                string displayName = mappedNote.ShipWorksField.DisplayName;
                NoteVisibility visibility = displayName.ToLowerInvariant().Contains("public")
                    ? NoteVisibility.Public
                    : NoteVisibility.Internal;

                note.Add(order, (string)mappedNote.ShipWorksField.Value, order.OrderDate, visibility);
            }
        }
    }
}
