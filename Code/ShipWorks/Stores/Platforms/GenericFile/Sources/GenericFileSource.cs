using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources
{
    /// <summary>
    /// Provides the base interface for the interaction of the downloaders with the file sources
    /// </summary>
    public abstract class GenericFileSource : IDisposable
    {
        #region Disposable Pattern
        
        /// <summary>
        /// Finalizer
        /// </summary>
        ~GenericFileSource()
        {
            Dispose (false);
        }
        

        /// <summary>
        /// IDisposable implementation
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Access for derived classes to dispose correctly
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {

        }

        #endregion

        /// <summary>
        /// Get the next file contained in the source
        /// </summary>
        public abstract GenericFileInstance GetNextFile();

        /// <summary>
        /// Handle the successful completion of importing the given file
        /// </summary>
        public abstract void HandleSuccess(GenericFileInstance file);

        /// <summary>
        /// Handle the error as configured in the file source for the given file that encountered an error.
        /// </summary>
        public abstract void HandleError(GenericFileInstance file, GenericFileStoreException ex);
    }
}
