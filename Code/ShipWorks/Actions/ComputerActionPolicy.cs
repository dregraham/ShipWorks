using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions
{
    public class ComputerActionPolicy
    {
        private readonly List<ComputerEntity> allowedComputers;
        private readonly ComputerLimitedType actionComputerLimitationType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerActionPolicy"/> class.
        /// </summary>
        public ComputerActionPolicy(ComputerLimitedType computerLimitationType, string internalComputerLimitedList) //, long triggeringComputerID)
        {
            allowedComputers = ReadCsv(internalComputerLimitedList);
            actionComputerLimitationType = (ComputerLimitedType)computerLimitationType;
        }

        /// <summary>
        /// Turns a list of comma separated list of computer IDs into a List of ComputerEntity objects.
        /// </summary>
        /// <param name="allowedComputersCsv">The allowed computers CSV.</param>
        /// <returns>A List of ComputerEntity objects.</returns>
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
            switch (actionComputerLimitationType)
            {
                case ComputerLimitedType.None:
                    return true;
                case ComputerLimitedType.TriggeringComputer:
                case ComputerLimitedType.NamedList:
                    return allowedComputers.Any(c => c.ComputerID == computer.ComputerID);
                default:
                    throw new ArgumentOutOfRangeException(string.Format("{0} is an unknown ComputerLimitationType.", EnumHelper.GetDescription(actionComputerLimitationType)));
            }
        }
    }
}
