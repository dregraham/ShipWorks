using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Represents the TemplateManager
    /// </summary>
    [Service]
    public interface ITemplateManager
    {
        /// <summary>
        /// The global default live ReadOnly TemplateTree
        /// </summary>
        TemplateTree Tree { get; }
    }
}