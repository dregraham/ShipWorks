using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Determines whether computers are eligible to run actions.
    /// </summary>
    public class ComputerActionPolicy
    {
        private readonly List<long> allowedComputerIDs;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerActionPolicy"/> class.
        /// </summary>
        public ComputerActionPolicy(string internalComputerLimitedList)
        {
            allowedComputerIDs = ReadCsv(internalComputerLimitedList);
        }

        /// <summary>
        /// Turns a list of comma separated list of IDs into a List of ID values.
        /// </summary>
        /// <param name="idsCsv">The CSV of IDs.</param>
        /// <returns>A List of ID values.</returns>
        private static List<long> ReadCsv(string idsCsv)
        {
            return idsCsv
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => long.Parse(x))
                .ToList();
        }

        /// <summary>
        /// Writes the computer IDs of the allowed computers to a comma separated list.
        /// </summary>
        /// <returns>A string containing a comma separated list of computer IDs.</returns>
        public string ToCsv()
        {
            string[] computerIds = allowedComputerIDs.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray();
            return string.Join(", ", computerIds);
        }

        /// <summary>
        /// Determines whether the policy allows the specified computer to execute the action.
        /// </summary>
        /// <param name="computer">The computer.</param>
        /// <returns>
        ///   <c>true</c> if the computer is allowed to execute the action; otherwise, <c>false</c>.
        /// </returns>
        public bool IsComputerAllowed(ComputerEntity computer)
        {
            if (allowedComputerIDs.Count == 0)
            {
                return true;
            }

            // This can be called in any thread and may happen after a user has logged out
            return computer != null ? allowedComputerIDs.Contains(computer.ComputerID) : false;
        }
    }
}
