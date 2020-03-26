using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Wraps unsafe calls to assembly.
    /// </summary>
    public interface IAssembly
    {
        /// <summary>
        /// Gets the assembly name of the executing assembly
        /// </summary>
        AssemblyName GetExecutingAssemblyName();
    }
}
