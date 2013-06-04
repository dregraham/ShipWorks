using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ShipWorks.Templates.Saving
{
    /// <summary>
    /// Event delegate for the SaveCompleted event
    /// </summary>
    public delegate void SaveCompletedEventHandler(object sender, SaveCompletedEventArgs e);

    /// <summary>
    /// EventArgs for the SaveCompleted event
    /// </summary>
    public class SaveCompletedEventArgs : AsyncCompletedEventArgs
    {
        List<string> savedFiles;

        /// <summary>
        /// Constrctor
        /// </summary>
        public SaveCompletedEventArgs(List<string> savedFiles, Exception error, bool canceled, object userState)
            : base(error, canceled, userState)
        {
            this.savedFiles = savedFiles;
        }

        /// <summary>
        /// The list of files that were created by the save operation
        /// </summary>
        public IList<string> SavedFiles
        {
            get { return new ReadOnlyCollection<string>(savedFiles); }
        }
    }
}
