using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using System;
using System.Linq;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Handles Note related tasks
    /// </summary>
    public class OrderNote : IOrderNote
    {
        private readonly IOrderRepository orderRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderNote"/> class.
        /// </summary>
        /// <param name="orderRepository">The order repository.</param>
        public OrderNote(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// Adds the note to the order if it doesn't already exist
        /// </summary>
        public void Add(OrderEntity order, string noteText, DateTime noteDate, NoteVisibility noteVisibility)
        {
            if (string.IsNullOrWhiteSpace(noteText))
            {
                return;
            }

            string trimmedNoteText = noteText.Trim();
            
            // First see if any of the current (newly downloaded) notes match this note
            if (order.Notes.Any(n =>
                n.Text.Equals(trimmedNoteText, StringComparison.InvariantCultureIgnoreCase)
                && n.Source == (int)NoteSource.Downloaded))
            {
                return;
            }

            // If the order is not new, check the Repo. If the note is in the Repo, don't create note.
            if (!order.IsNew && orderRepository.ContainsNote(order, trimmedNoteText, NoteSource.Downloaded))
            {
                return;
            }
            
            NoteEntity note = new NoteEntity();
            note.Order = order;
            note.UserID = null;
            note.Edited = noteDate;
            note.Source = (int)NoteSource.Downloaded;
            note.Visibility = (int)noteVisibility;
            note.Text = trimmedNoteText;
        }
    }
}
