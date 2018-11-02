using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// Section layout definition
    /// </summary>
    [Serializable]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class SectionLayout 
    {
        /// <summary>
        /// Name
        /// </summary>
        [Description("Name of the section")]
        [JsonIgnore]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Unique ID of the section
        /// </summary>
        [Description("Unique ID of the section")]
        public SectionLayoutIDs Id { get; set; }

        /// <summary>
        /// Is this section selected?
        /// </summary>
        [Description("Is this section selected")]
        public bool Selected { get; set; } = true;

        /// <summary>
        /// Is this section expanded?
        /// </summary>
        [Description("Is this section expanded")]
        public bool Expanded { get; set; } = false;

        /// <summary>
        /// List of fields in this section
        /// </summary>
        [Description("List of fields in this section")]
        public List<SectionFieldLayout> SectionFields { get; set; } = new List<SectionFieldLayout>();

        /// <summary>
        /// Does the section have children
        /// </summary>
        [Description("Does this section have children")]
        public bool HasChildren => SectionFields.Any();

        /// <summary>
        /// Copy the given SectionLayout to this instance.  SectionFields are NOT copied.
        /// </summary>
        public void Copy(SectionLayout toCopy)
        {
            if (toCopy.Id != Id)
            {
                throw new InvalidOperationException("Copying SectionLayouts with different Ids is not supported.");
            }

            Selected = toCopy.Selected;
            Expanded = toCopy.Expanded;
        }

        /// <summary>
        /// Create a clone of this SectionLayout
        /// </summary>
        public SectionLayout Clone()
        {
            return (SectionLayout) MemberwiseClone();
        }
    }
}
