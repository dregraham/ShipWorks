﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;

namespace ShipWorks.Stores.Platforms.Amazon.DTO
{
    /// <summary>
    /// DTO for Amazon Platform Item Gift Notes
    /// </summary>
    public class GiftNote
    { 
        public string ASIN { get; set; }
        public string SKU { get; set; }
        public string OrderItemId { get; set; }
        public decimal Fee { get; set; }
        public string GiftWrapLevel { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Create a formatted GiftNote from an OrderSourceNote
        /// </summary>
        public static GiftNote FromOrderSourceNote(OrderSourceNote orderSourceNote)
        {
            var itemNote = new GiftNote();
            // Amazon orders are in a format like:
            // "OrderItemId:123\nASIN:ASSDFSDFA\n..."
            // If it isn't in that format, return the message as text
            if(orderSourceNote.Text.Contains($"{'\n'}OrderItemId:"))
            {
                var messageParts = orderSourceNote.Text.Split('\n');
                itemNote.ASIN = ParseNoteTextProperty("ASIN:", messageParts);
                itemNote.SKU = ParseNoteTextProperty("SKU:", messageParts);
                itemNote.OrderItemId = ParseNoteTextProperty("OrderItemId:", messageParts);
                itemNote.GiftWrapLevel = ParseNoteTextProperty("Gift Wrap Level:", messageParts);
                var fee = ParseNoteTextProperty("Fee:", messageParts);
                if (fee.HasValue() && decimal.TryParse(fee, out decimal parsedFee))
                {
                    itemNote.Fee = parsedFee;
                }

                itemNote.Message = ParseNoteMessageProperty(messageParts);
            }
            else
            {
                itemNote.Message = orderSourceNote.Text;
            }

            return itemNote;
        }

        /// <summary>
        /// Parse the given property from the note text
        /// </summary>
        private static string ParseNoteTextProperty(string propertyName, string[] messageParts)
        {
            var property = messageParts.FirstOrDefault((p) => p.StartsWith(propertyName));
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
            var remaining = messageParts.SkipWhile(p => !p.StartsWith("Message:")).ToArray();

            if (remaining.Any())
            {
                //Remove the "Message:" bit
                remaining[0] = remaining[0].Substring(8);
                return String.Join("\n", remaining).Trim();
            }

            return string.Empty;
        }
    }
}
