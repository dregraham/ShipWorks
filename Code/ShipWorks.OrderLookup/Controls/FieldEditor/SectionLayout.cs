using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.OrderLookup.Controls.FieldEditor
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
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Unique ID of the section
        /// </summary>
        [Description("Unique ID of the section")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Row that this section should be in
        /// </summary>
        [Description("Row that this section should be in")]
        public int Row { get; set; } = 0;

        /// <summary>
        /// Column that this section should be in
        /// </summary>
        [Description("Column that this section should be in")]
        public int Column { get; set; } = 0;

        /// <summary>
        /// Is this section selected?
        /// </summary>
        [Description("Is this section selected")]
        public bool Selected { get; set; } = true;

        /// <summary>
        /// List of fields in this section
        /// </summary>
        [Description("List of fields in this section")]
        public IEnumerable<SectionFieldLayout> SectionFields { get; set; } = new List<SectionFieldLayout>();
    }
}
