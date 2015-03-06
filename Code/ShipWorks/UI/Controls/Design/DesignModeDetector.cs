namespace ShipWorks.UI.Controls.Design
{
    /// <summary>
    /// Class for detecting if the application is in DesignMode
    /// </summary>
    public static class DesignModeDetector
    {
        /// <summary>
        /// Determines if we are currently in design mode.
        /// </summary>
        public static bool IsDesignerHosted()
        {
            // We were testing for the DesignMode of the specified control or its parent.  Unfortunately, this
            // wasn't sufficient because that check is only valid after the constructor and there was an instance
            // where OnLoad of the WeightControl was getting called in the constructor of the PostalServiceBase
            // which caused Visual Studio to crash
            return Program.ExecutionMode == null;
        }
    }
}
