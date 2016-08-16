namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// How should the scroll size be limited
    /// </summary>
    public enum LimitScrollSizeMode
    {
        /// <summary>
        /// Do not limit scroll size
        /// </summary>
        None,

        /// <summary>
        /// Limit only the width
        /// </summary>
        Width,

        /// <summary>
        /// Limit only the height
        /// </summary>
        Height,

        /// <summary>
        /// Limit both width and height
        /// </summary>
        Both
    }
}
