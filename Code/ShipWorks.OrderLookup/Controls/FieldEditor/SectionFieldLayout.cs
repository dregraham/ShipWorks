﻿using System;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.OrderLookup.Controls.FieldEditor
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
    }
}
