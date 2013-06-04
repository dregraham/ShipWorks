using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Users.Security;
using System.Transactions;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Allows for controling the behavior of auditing on a thread-scoped based
    /// </summary>
    public class AuditBehaviorScope : IDisposable
    {
        [ThreadStatic]
        static int superUserCount;

        [ThreadStatic]
        static List<AuditReason> reasonStack;

        [ThreadStatic]
        static int disabledCount;

        // The active user behavior
        AuditBehaviorUser userBehavior = AuditBehaviorUser.Default;

        // Indicates if we added to the reasonStack
        bool specifiedReason = false;

        // The disabled behavior
        AuditBehaviorDisabledState disabledState = AuditBehaviorDisabledState.Default;

        /// <summary>
        /// Enter the scope with the specified user behavior.
        /// </summary>
        public AuditBehaviorScope(AuditBehaviorUser userBehavior)
            : this(userBehavior, new AuditReason(AuditReasonType.Default), AuditBehaviorDisabledState.Default)
        {
        }

        /// <summary>
        /// Enter a behavior scope that specifies what audit reason the user is currently in
        /// </summary>
        public AuditBehaviorScope(AuditReasonType reasonType)
            : this(AuditBehaviorUser.Default, new AuditReason(reasonType), AuditBehaviorDisabledState.Default)
        {

        }

        /// <summary>
        /// Enter a behavior scope that specifies what audit reason the user is currently in
        /// </summary>
        public AuditBehaviorScope(AuditReason reason)
            : this(AuditBehaviorUser.Default, reason, AuditBehaviorDisabledState.Default)
        {
        }
        
        /// <summary>
        /// Enter a scope with the specified disabled behavior
        /// </summary>
        public AuditBehaviorScope(AuditBehaviorDisabledState disabledState)
            : this(AuditBehaviorUser.Default, new AuditReason(AuditReasonType.Default), disabledState)
        {
        }

        /// <summary>
        /// Enter a scope with the specified behavior
        /// </summary>
        public AuditBehaviorScope(AuditBehaviorUser userBehavior, AuditReason reason)
            : this(userBehavior, reason, AuditBehaviorDisabledState.Default)
        {

        }

        /// <summary>
        /// Enter a scope with the specified behavior
        /// </summary>
        public AuditBehaviorScope(AuditBehaviorUser userBehavior, AuditReason reason, AuditBehaviorDisabledState disabledState)
        {
            this.disabledState = disabledState;
            this.userBehavior = userBehavior;

            if (userBehavior == AuditBehaviorUser.SuperUser)
            {
                // Entring a SuperUser scope during an active transaction would cause a DTC error.  Changing the active user changes the connection
                // string, which then would try to open a seperate connection, causing it to have to go to DTC to stay within the same transaction.
                if (!IsSuperUserActive && Transaction.Current != null)
                {
                    throw new InvalidOperationException("Cannot enter the super user scope while a transaction is already active.");
                }

                superUserCount++;
            }

            if (reason.ReasonType != AuditReasonType.Default)
            {
                if (reasonStack == null)
                {
                    reasonStack = new List<AuditReason>();
                }

                reasonStack.Insert(0, reason);
                specifiedReason = true;
            }

            if (disabledState == AuditBehaviorDisabledState.Disabled)
            {
                disabledCount++;
            }
        }

        /// <summary>
        /// Indicates if the super user is active on the current thread
        /// </summary>
        public static bool IsSuperUserActive
        {
            get { return superUserCount > 0; }
        }

        /// <summary>
        /// The AuditReason value in scope.  If no values have been pushed into scope, this returns null.
        /// </summary>
        public static AuditReason ActiveReason
        {
            get
            {
                if (reasonStack != null && reasonStack.Count > 0)
                {
                    return reasonStack[0];
                }

                return null;
            }
        }

        /// <summary>
        /// Indicates if auditing is currently disabled
        /// </summary>
        public static bool IsDisabled
        {
            get { return disabledCount > 0; }
        }

        /// <summary>
        /// Close the scope
        /// </summary>
        public void Dispose()
        {
            if (userBehavior == AuditBehaviorUser.SuperUser)
            {
                superUserCount--;
            }

            if (specifiedReason)
            {
                reasonStack.RemoveAt(0);
            }

            if (disabledState == AuditBehaviorDisabledState.Disabled)
            {
                disabledCount--;
            }
        }
    }
}
