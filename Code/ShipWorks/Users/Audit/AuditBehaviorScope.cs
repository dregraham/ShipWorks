using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Users.Security;
using System.Transactions;
using ShipWorks.SqlServer.Data.Auditing;

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
        static List<AuditState> stateStack;

        // The active user behavior
        AuditBehaviorUser userBehavior = AuditBehaviorUser.Default;

        // Indicates what we've added to the stacks
        bool needPopReason = false;
        bool needPopState = false;

        /// <summary>
        /// Enter the scope with the specified user behavior.
        /// </summary>
        public AuditBehaviorScope(AuditBehaviorUser userBehavior)
            : this(userBehavior, new AuditReason(AuditReasonType.Default), AuditState.Default)
        {
        }

        /// <summary>
        /// Enter a behavior scope that specifies what audit reason the user is currently in
        /// </summary>
        public AuditBehaviorScope(AuditReasonType reasonType)
            : this(AuditBehaviorUser.Default, new AuditReason(reasonType), AuditState.Default)
        {

        }

        /// <summary>
        /// Enter a behavior scope that specifies what audit reason the user is currently in
        /// </summary>
        public AuditBehaviorScope(AuditReason reason)
            : this(AuditBehaviorUser.Default, reason, AuditState.Default)
        {
        }
        
        /// <summary>
        /// Enter a scope with the specified disabled behavior
        /// </summary>
        public AuditBehaviorScope(AuditState auditState)
            : this(AuditBehaviorUser.Default, new AuditReason(AuditReasonType.Default), auditState)
        {
        }

        /// <summary>
        /// Enter a scope with the specified behavior
        /// </summary>
        public AuditBehaviorScope(AuditBehaviorUser userBehavior, AuditReason reason)
            : this(userBehavior, reason, AuditState.Default)
        {

        }

        /// <summary>
        /// Enter a scope with the specified behavior
        /// </summary>
        public AuditBehaviorScope(AuditBehaviorUser userBehavior, AuditReason reason, AuditState auditState)
        {
            this.userBehavior = userBehavior;

            if (userBehavior == AuditBehaviorUser.SuperUser)
            {
                ValidateScope(!IsSuperUserActive, "userBehavior");

                superUserCount++;
            }

            if (reason.ReasonType != AuditReasonType.Default)
            {
                if (reasonStack == null)
                {
                    reasonStack = new List<AuditReason>();
                }

                bool changingReason = (reasonStack.Count == 0 || reasonStack[0].ReasonType != reason.ReasonType || reasonStack[0].ReasonDetail != reason.ReasonDetail);

                ValidateScope(changingReason, "reason");

                reasonStack.Insert(0, reason);
                needPopReason = true;
            }

            if (auditState != AuditState.Default)
            {
                if (stateStack == null)
                {
                    stateStack = new List<AuditState>();
                }

                bool changingState = (stateStack.Count == 0 || stateStack[0] != auditState);

                ValidateScope(changingState, "state");

                stateStack.Insert(0, auditState);
                needPopState = true;
            }
        }

        /// <summary>
        /// Validates that it's OK to change the properties of the active scope
        /// </summary>
        private void ValidateScope(bool changing, string property)
        {
            if (changing && Transaction.Current != null)
            {
                throw new InvalidOperationException("Cannot change connection-altering property when transaction is already in progress: " + property);
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
        public static AuditState ActiveState
        {
            get
            {
                if (stateStack != null && stateStack.Count > 0)
                {
                    return stateStack[0];
                }

                return AuditState.Enabled;
            }
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

            if (needPopReason)
            {
                reasonStack.RemoveAt(0);
            }

            if (needPopState)
            {
                stateStack.RemoveAt(0);
            }
        }
    }
}
