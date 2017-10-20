using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Container for an immutable stack that looks like a normal stack
    /// </summary>
    /// <remarks>
    /// This class is necessary because of the way AsyncLocal works with sub tasks. Changing the value
    /// of an AsyncLocal in a sub-task won't be seen by the parent task, but changing the contents
    /// of the value should.  We're using an immutable stack here because we were getting an exception
    /// iterating over the stack as something else changed it.
    /// </remarks>
    public class ImmutableStackContainer<T> : IEnumerable<T>
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
        public T Peek() => stack.Peek();

        /// <summary>
        /// Push an item on the stack
        /// </summary>
        public void Push(T item) =>
            stack = stack.Push(item);

        /// <summary>
        /// Pop an item off the stack
        /// </summary>
        public void Pop() =>
            stack = stack.Pop();
    }
}
