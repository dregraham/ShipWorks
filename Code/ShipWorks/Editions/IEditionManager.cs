namespace ShipWorks.Editions
{
    /// <summary>
    /// Interface for the EditionManager
    /// </summary>
    public interface IEditionManager
    {
        /// <summary>
        /// Gets the active restrictions.
        /// </summary>
        EditionRestrictionSet ActiveRestrictions { get; }
    }
}