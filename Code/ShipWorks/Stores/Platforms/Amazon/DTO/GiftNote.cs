using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;

namespace ShipWorks.Stores.Platforms.Amazon.DTO
{
    public class GiftNote
    {
        public OrderSourceNoteType Type { get; set; }
        public string ASIN { get; set; }
        public string SKU { get; set; }
        public string OrderItemId { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Create a formatted GiftNote from an OrderSourceNote
        /// </summary>
        public static GiftNote FromOrderSourceNote(OrderSourceNote orderSourceNote)
        {
            var itemNote = new GiftNote();
            var messageParts = orderSourceNote.Text.Split('\n');
            itemNote.ASIN = ParseNoteTextProperty("ASIN", messageParts);
            itemNote.SKU = ParseNoteTextProperty("SKU", messageParts);
            itemNote.OrderItemId = ParseNoteTextProperty("OrderItemId", messageParts);
            itemNote.Message = ParseNoteMessageProperty(messageParts);

            return itemNote;
        }

        /// <summary>
        /// Parse the given property from the note text
        /// </summary>
        private static string ParseNoteTextProperty(string propertyName, string[] messageParts)
        {
            var property = messageParts.FirstOrDefault((p) => p.Contains(propertyName));
            if (property.HasValue())
            {
                return property.Split(':')[1].Trim();
            }

            return null;
        }

        /// <summary>
        /// Parse out the message property from the note text
        /// </summary>
        private static string ParseNoteMessageProperty(string[] messageParts)
        {
            //The different properties provided in the OrderSourceNote text are delimited by a \n
            //However, The message can also contain \n so in order to keep the full message we assume
            //the message is the last property and take everything after the "Message:" part of the string
            var remaining = messageParts.SkipWhile(p => !p.StartsWith("Message")).ToArray();

            //Remove the "Message:" bit
            remaining[0] = remaining[0].Substring(8);
            return String.Join("\n", remaining).Trim();
        }
    }
}
