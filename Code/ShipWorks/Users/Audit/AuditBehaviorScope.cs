using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Transactions;
using Interapptive.Shared.Collections;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Allows for controlling the behavior of auditing on a thread-scoped based
    /// </summary>
    /// <remarks>
    /// We can not use ThreadLocal anymore since it does not work with async tasks.
    /// https://particular.net/blog/the-dangers-of-threadlocal
    /// </remarks>
    public class AuditBehaviorScope : IDisposable
    {
        static AsyncLocal<int> superUserCount = new AsyncLocal<int>();
        static AsyncLocal<Stack<AuditReason>> reasonStack = new AsyncLocal<Stack<AuditReason>>();
        static AsyncLocal<Stack<AuditState>> stateStack = new AsyncLocal<Stack<AuditState>>();

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

            PushSuperUser(userBehavior);
            PushReason(reason);
            PushState(auditState);
        }

        /// <summary>
        /// Push whether this is a super user, if necessary
        /// </summary>
        /// <param name="userBehavior"></param>
        private void PushSuperUser(AuditBehaviorUser userBehavior)
        {
            if (userBehavior != AuditBehaviorUser.SuperUser)
            {
                return;
            }

            ValidateScope(!IsSuperUserActive, "userBehavior");

            superUserCount.Value++;
        }

        /// <summary>
        /// Push the reason on the stack, if necessary
        /// </summary>
        private void PushReason(AuditReason reason)
        {
            if (reason.ReasonType == AuditReasonType.Default)
            {
                return;
            }

            if (reasonStack.Value == null)
            {
                reasonStack.Value = new Stack<AuditReason>();
            }

            bool changingReason = (reasonStack.Value.None() ||
                reasonStack.Value.Peek().ReasonType != reason.ReasonType ||
                reasonStack.Value.Peek().ReasonDetail != reason.ReasonDetail);

            ValidateScope(changingReason, "reason");

            reasonStack.Value.Push(reason);
            needPopReason = true;
        }

        /// <summary>
        /// Push the state on the stack, if necessary
        /// </summary>
        /// <param name="auditState"></param>
        private void PushState(AuditState auditState)
        {
            if (auditState == AuditState.Default)
            {
                return;
            }

            if (stateStack.Value == null)
            {
                stateStack.Value = new Stack<AuditState>();
            }

            bool changingState = (stateStack.Value.None() || stateStack.Value.Peek() != auditState);

            ValidateScope(changingState, "state");

            stateStack.Value.Push(auditState);
            needPopState = true;
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
        public static bool IsSuperUserActive => superUserCount.Value > 0;

        /// <summary>
        /// The AuditReason value in scope.  If no values have been pushed into scope, this returns null.
        /// </summary>
        public static AuditReason ActiveReason => reasonStack.Value.DefaultIfEmptyOrNull().First();

        /// <summary>
        /// Indicates if auditing is currently disabled
        /// </summary>
        public static AuditState ActiveState => stateStack.Value.DefaultIfEmptyOrNull(AuditState.Enabled).First();

        /// <summary>
        /// Close the scope
        /// </summary>
        public void Dispose()
        {
            if (userBehavior == AuditBehaviorUser.SuperUser)
            {
                superUserCount.Value--;
            }

            if (needPopReason)
            {
                reasonStack.Value.Pop();
            }

            if (needPopState)
            {
                stateStack.Value.Pop();
            }
        }
    }
}
