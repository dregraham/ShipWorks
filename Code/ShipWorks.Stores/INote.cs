using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using System;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Handles Note related tasks
    /// </summary>
    public interface INote
    {
        /// <summary>
        /// Adds the note to the order if it doesn't already exist
        /// </summary>
        void Add(OrderEntity order, string noteText, DateTime noteDate, NoteVisibility noteVisibility);
    }
}