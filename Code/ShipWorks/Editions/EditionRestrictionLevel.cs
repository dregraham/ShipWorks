namespace ShipWorks.Editions
{
    /// <summary>
    /// Represents how the edition restricts use of the relevant item
    /// </summary>
    public enum EditionRestrictionLevel
    {
        /// <summary>
        /// No restrictions
        /// </summary>
        None,

        /// <summary>
        /// Not allowed to even see it
        /// </summary>
        Hidden,

        /// <summary>
        /// Can see it - but can't upgrade from it.
        /// </summary>
        Forbidden,

        /// <summary>
        /// It's possible to use it if you upgrade your edition
        /// </summary>
        RequiresUpgrade
    }
}
