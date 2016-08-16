namespace ShipWorks.Editions
{
    /// <summary>
    /// Wrapper of the EditionManager
    /// </summary>
    public class EditionManagerWrapper : IEditionManager
    {
        /// <summary>
        /// The current set of edition restrictions 
        /// </summary>
        public EditionRestrictionSet ActiveRestrictions =>
            EditionManager.ActiveRestrictions;
    }
}