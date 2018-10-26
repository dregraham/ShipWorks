using System.Reflection;

namespace ShipWorks.OrderLookup.Layout
{
    /// <summary>
    /// Info used for saving panel state
    /// </summary>
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public struct PanelInfo
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PanelInfo(string name, bool expanded)
        {
            Name = name;
            Expanded = expanded;
        }

        /// <summary>
        /// Name of Panel
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initial Expansion state
        /// </summary>
        public bool Expanded { get; set; }
    }
}