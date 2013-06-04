using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using ShipWorks.Stores.Platforms.Ebay.Authorization.Readers;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Repositories;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Writers;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization
{
    /// <summary>
    /// An interface for creating token readers, writers, and repositories.
    /// </summary>
    public interface ITokenFactory
    {
        /// <summary>
        /// Creates an ITokenReader reading a string of token data.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>An ITokenReader object.</returns>
        ITokenReader CreateReader(string tokenData);

        /// <summary>
        /// Creates an ITokenReader capable of reading from a file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>An ITokenReader object.</returns>
        ITokenReader CreateReader(FileInfo file);

        /// <summary>
        /// Creates the writer.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <returns>An ITokenWriter object.</returns>
        ITokenWriter CreateWriter(Stream stream);

        /// <summary>
        /// Creates the writer.
        /// </summary>
        /// <param name="file">The file to write to.</param>
        /// <returns>An ITokenWriter object</returns>
        ITokenWriter CreateWriter(FileInfo file);

        /// <summary>
        /// Creates the repository.
        /// </summary>
        /// <param name="license">The license.</param>
        /// <returns>An ITokenRepository object.</returns>
        ITokenRepository CreateRepository(string license);
    }
}
