using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Logging
{
    public static class LogExceptionUtility
    {

        /// <summary>
        /// Logs an ORMConcurrencyException that occurred, along with debugging info.
        /// </summary>
        public static string LogOrmConcurrencyException(IEntity2 entity, string additionalInfo, ORMConcurrencyException ormConcurrencyException)
        {
            if (ormConcurrencyException == null)
            {
                return string.Empty;
            }

            string data = string.Empty;
            foreach (var key in ormConcurrencyException.Data.Keys)
            {
                data += string.Format("{0}:{1}{2}", key, ormConcurrencyException.Data[key], Environment.NewLine);
            }

            string ormConcurrencyExceptionMessage = string.Format("An ORMConcurrencyException occurred.  Details:" + Environment.NewLine +
                                                      entity.PrimaryKeyFields[0].Name + ": {0}" + Environment.NewLine +
                                                      "Additional Information: {1}" + Environment.NewLine +
                                                      "Username: {2}" + Environment.NewLine +
                                                      "ComputerID/Computer Name: {3}/{4}" + Environment.NewLine +
                                                      "Data: {5}" + Environment.NewLine +
                                                      "EntityWhichFailed: {6}" + Environment.NewLine,
                                                      entity.Fields[entity.PrimaryKeyFields[0].Name].CurrentValue,
                                                      additionalInfo,
                                                      UserSession.User.Username,
                                                      UserSession.Computer.ComputerID,
                                                      UserSession.Computer.Name,
                                                      data,
                                                      ormConcurrencyException.EntityWhichFailed);

            return ormConcurrencyExceptionMessage;
        }
    }
}
