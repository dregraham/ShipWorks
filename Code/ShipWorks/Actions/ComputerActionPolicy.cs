using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions
{
    public class ComputerActionPolicy
    {
        private readonly List<ComputerEntity> allowedComputers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerActionPolicy"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public ComputerActionPolicy(ActionEntity action)
        {
            // TODO: use the list of computer IDs to populate the list of allowed computers via the ReadCsv method
            allowedComputers = new List<ComputerEntity>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerActionPolicy"/> class.
        /// </summary>
        /// <param name="allowedComputersCsv">The allowed computers CSV.</param>
        public ComputerActionPolicy(string allowedComputersCsv)
        {
            allowedComputers = ReadCsv(allowedComputersCsv);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerActionPolicy"/> class.
        /// </summary>
        /// <param name="computers">The computers.</param>
        public ComputerActionPolicy(IEnumerable<ComputerEntity> computers)
        {
            allowedComputers = new List<ComputerEntity>(computers);
        }

        /// <summary>
        /// Reads the a comma separated list of computer IDs and 
        /// </summary>
        /// <param name="allowedComputersCsv">The allowed computers CSV.</param>
        private List<ComputerEntity> ReadCsv(string allowedComputersCsv)
        {
            List<ComputerEntity> computerEntities = new List<ComputerEntity>();

            string[] computerIds = allowedComputersCsv.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string computerID in computerIds)
            {
                computerEntities.Add(new ComputerEntity(long.Parse(computerID)));
            }

            return computerEntities;
        }

        /// <summary>
        /// Writes the computer IDs of the allowed computers to a comma separated list.
        /// </summary>
        /// <returns>A string containing a comma separated list of computer IDs.</returns>
        public string ToCsv()
        {
            string[] computerIds = allowedComputers.Select(c => c.ComputerID.ToString(CultureInfo.InvariantCulture)).ToArray();
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
            return allowedComputers.Any(c => c.ComputerID == computer.ComputerID);
        }
    }
}
