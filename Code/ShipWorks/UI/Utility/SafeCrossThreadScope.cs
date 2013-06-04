using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace ShipWorks.UI.Utility
{
    /// <summary>
    /// A scope within which it's safe to have cross thread calls (access Control.Handle on a non-UI thread)
    /// </summary>
    public class SafeCrossThreadScope : IDisposable
    {
        static ConstructorInfo crossThreadSafeScopeConstructor;

        IDisposable actualScope;

        /// <summary>
        /// Static constructor
        /// </summary>
        static SafeCrossThreadScope()
        {
            Type crossThreadSafeScopeType = typeof(Control).Assembly.GetType("System.Windows.Forms.Control+MultithreadSafeCallScope");

            if (crossThreadSafeScopeType == null)
            {
                throw new InvalidOperationException("Could not reflect on MultithreadSafeCallScope type.");
            }

            crossThreadSafeScopeConstructor = crossThreadSafeScopeType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);

            if (crossThreadSafeScopeConstructor == null)
            {
                throw new InvalidOperationException("Could not reflect on MultithreadSafeCallScope constructor");
            }
        }

        /// <summary>
        /// Creates a new instance of a scope that must be disposed to be exited.
        /// </summary>
        public SafeCrossThreadScope()
        {
            actualScope = (IDisposable) crossThreadSafeScopeConstructor.Invoke(null);
        }

        /// <summary>
        /// End the scope
        /// </summary>
        public void Dispose()
        {
            actualScope.Dispose();
        }
    }
}
