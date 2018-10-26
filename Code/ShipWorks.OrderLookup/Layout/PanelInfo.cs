using System.Reflection;

namespace ShipWorks.OrderLookup.Layout
{
    /// <summary>
    /// Info used for saving panel state
    /// </summary>
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
        [Obfuscation(Exclude = true)]
        public string Name { get; set; }

        /// <summary>
        /// Initial Expansion state
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Expanded { get; set; }
    }
}