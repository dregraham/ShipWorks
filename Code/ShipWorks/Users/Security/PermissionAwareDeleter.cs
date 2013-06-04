﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Utility class that helps to delete items that require permissions to delete, and returns any failed ones in the issue list
    /// </summary>
    public class PermissionAwareDeleter
    {
        // The permission needed to do the delete
        PermissionType permissionType;
        Control owner;
 
        /// <summary>
        /// Raised when the execute is complete.  Raised on the UI thread of the owner.
        /// </summary>
        public event EventHandler ExecuteCompleted;

        /// <summary>
        /// Constructor
        /// </summary>
        public PermissionAwareDeleter(Control owner, PermissionType permissionType)
        {
            this.owner = owner;
            this.permissionType = permissionType;
        }

        /// <summary>
        /// Delete the specified collection of keys asyncronously
        /// </summary>
        public void DeleteAsync(IEnumerable<long> keys)
        {
            if (!keys.Any())
            {
                throw new InvalidOperationException("No keys to delete");
            }

            EntityType entityType = EntityUtility.GetEntityType(keys.First());

            string name;

            switch (entityType)
            {
                case EntityType.CustomerEntity:
                    name = "Customers";
                    break;

                case EntityType.OrderEntity:
                    name = "Orders";
                    break;

                default:
                    throw new InvalidOperationException("Unhandled EntityType.");
            }

            string title = string.Format("Delete {0}", name);
            string message = string.Format("ShipWorks is deleting {0}.", name.ToLowerInvariant());

            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(owner,
                title,
                message,
                "Deleting {0} of {1}");

            // What to execute then the async operation is done
            executor.ExecuteCompleted += (sender, e) =>
            {
                if (e.Issues.Count > 0)
                {
                    MessageHelper.ShowWarning(owner, string.Format("Some {0} were not deleted due to insufficient permission.", name.ToLowerInvariant()));
                }

                if (ExecuteCompleted != null)
                {
                    ExecuteCompleted(this, EventArgs.Empty);
                }
            };

            // What to execute for each input item
            executor.ExecuteAsync((entityID, state, issueAdder) =>
            {
                PermissionScope scope = PermissionHelper.GetScope(permissionType);

                if (UserSession.Security.HasPermission(permissionType, (scope == PermissionScope.Store) ? entityID : (long?) null))
                {
                    try
                    {
                        DeletionService.DeleteEntity(entityID);
                    }
                    catch (SqlForeignKeyException)
                    {
                        // Just ignore the error - this won't happen very often.  The only way this could happen is if some children
                        // got added to the order after the deletion routine had already deleted the existing children, but before
                        // it had deleted the actual order.
                    }
                }
                else
                {
                    issueAdder.Add(entityID);
                }
            },

            // The input items to execute each time for
            keys);
        }
    }
}
