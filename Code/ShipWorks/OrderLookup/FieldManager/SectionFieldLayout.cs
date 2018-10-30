using System;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

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
        [JsonIgnore]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Unique ID of the field
        /// </summary>
        [Description("Unique ID of the field")]
        public SectionLayoutFieldIDs Id { get; set; }

        /// <summary>
        /// Is this field selected?
        /// </summary>
        [Description("Is this field selected")]
        public bool Selected { get; set; } = true;

        /// <summary>
        /// Copy the given SectionFieldLayout to this instance.
        /// </summary>
        public void Copy(SectionFieldLayout toCopy)
        {
            if (toCopy.Id != Id)
            {
                throw new InvalidOperationException("Copying SectionFieldLayouts with different Ids is not supported.");
            }

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
