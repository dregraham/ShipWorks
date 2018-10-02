using System;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// Fields in a section layout definition
    /// </summary>
    [Serializable]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class SectionFieldLayout
    {
        /// <summary>
        /// Name
        /// </summary>
        [Description("Name of the field")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Unique ID of the field
        /// </summary>
        [Description("Unique ID of the field")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Is this field selected?
        /// </summary>
        [Description("Is this field selected")]
        public bool Selected { get; set; } = true;

        /// <summary>
        /// Row that this field should be in
        /// </summary>
        [Description("Row that this field should be in")]
        public int Row { get; set; } = 0;

        /// <summary>
        /// Copy the given SectionFieldLayout to this instance.
        /// </summary>
        public void Copy(SectionFieldLayout toCopy)
        {
            if (!toCopy.Id.Equals(Id, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("Copying SectionFieldLayouts with different Ids is not supported.");
            }

            Name = toCopy.Name;
            Row = toCopy.Row;
            Selected = toCopy.Selected;
        }

        /// <summary>
        /// Create a clone of this SectionFieldLayout
        /// </summary>
        public SectionFieldLayout Clone()
        {
            return (SectionFieldLayout) MemberwiseClone();
        }
    }
}
