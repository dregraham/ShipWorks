using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        static AsyncLocal<StackContainer<AuditReason>> reasonStack = new AsyncLocal<StackContainer<AuditReason>>();
        static AsyncLocal<StackContainer<AuditState>> stateStack = new AsyncLocal<StackContainer<AuditState>>();

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
        private void PushSuperUser(AuditBehaviorUser behavior)
        {
            if (behavior != AuditBehaviorUser.SuperUser)
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
                reasonStack.Value = new StackContainer<AuditReason>();
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
                stateStack.Value = new StackContainer<AuditState>();
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

        /// <summary>
        /// Container for an immutable stack that looks like a normal stack
        /// </summary>
        /// <remarks>
        /// This class is necessary because of the way AsyncLocal works with sub tasks. Changing the value
        /// of an AsyncLocal in a sub-task won't be seen by the parent task, but changing the contents
        /// of the value should.  We're using an immutable stack here because we were getting an exception
        /// iterating over the stack as something else changed it.
        /// </remarks>
        private class StackContainer<T> : IEnumerable<T>
        {
            /// <summary>
            /// Immutable stack that holds the actual data
            /// </summary>
            private ImmutableStack<T> stack = ImmutableStack.Create<T>();

            /// <summary>
            /// Get the enumerator of the stack
            /// </summary>
            public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>) stack).GetEnumerator();

            /// <summary>
            /// Get the enumerator of the stack
            /// </summary>
            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) stack).GetEnumerator();

            /// <summary>
            /// Peek at the end of the stack
            /// </summary>
            internal T Peek() => stack.Peek();

            /// <summary>
            /// Push an item on the stack
            /// </summary>
            internal void Push(T item) =>
                stack = stack.Push(item);

            /// <summary>
            /// Pop an item off the stack
            /// </summary>
            internal void Pop() =>
                stack = stack.Pop();
        }
    }
}
