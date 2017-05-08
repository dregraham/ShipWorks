using System;

namespace Interapptive.Shared.ComponentRegistration
{
    /// <summary>
    /// How should components be registered
    /// </summary>
    [Flags]
    public enum RegistrationType
    {
        /// <summary>
        /// Register the component as itself
        /// </summary>
        Self = 1,

        /// <summary>
        /// Register the component as its implemented interfaces
        /// </summary>
        ImplementedInterfaces = 2,
    }
}
