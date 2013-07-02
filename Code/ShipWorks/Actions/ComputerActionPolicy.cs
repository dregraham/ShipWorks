using System;
using System.Collections.Generic;
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

            string[] computerIDs = allowedComputersCsv.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string computerID in computerIDs)
            {
                computerEntities.Add(new ComputerEntity(long.Parse(computerID)));
            }

            return computerEntities;
        }

        /// <summary>
        /// Writes the computer IDs of the allowed computers to a comma separated list.
        /// </summary>
        /// <returns></returns>
        public string ToCsv()
        {
            StringBuilder csv = new StringBuilder();

            foreach (ComputerEntity computer in allowedComputers)
            {
                if (csv.Length > 0)
                {
                    csv.Append(", ");
                }

                csv.Append(computer.ComputerID);
            }

            return csv.ToString();
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
