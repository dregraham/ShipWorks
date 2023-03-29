using ShipWorks.Stores.Platforms.ShipEngine.Apollo;

namespace ShipWorks.Stores.Platforms.ShipEngine
{
    public class PlatformHelper
    {
        /// <summary>
        /// Get the note preface based on note type
        /// </summary>
        public static string GetNotePreface(OrderSourceNoteType noteType)
        {
            switch (noteType)
            {
                case OrderSourceNoteType.GiftMessage:
                    return "Gift Message: ";
                case OrderSourceNoteType.NotesToBuyer:
                    return "To Buyer: ";
                case OrderSourceNoteType.NotesFromBuyer:
                    return "From Buyer: ";
                case OrderSourceNoteType.InternalNotes:
                    return "Internal: ";
                default:
                    return string.Empty;
            }
        }
    }
}
