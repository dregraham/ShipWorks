﻿using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Represents the TemplateManager
    /// </summary>
    public interface ITemplateManager
    {
        /// <summary>
        /// The global default live ReadOnly TemplateTree
        /// </summary>
        TemplateTree Tree { get; }

        /// <summary>
        /// Gets all the templates
        /// </summary>
        IList<TemplateEntity> AllTemplates { get; }

        /// <summary>
        /// Fetch the default pick list template
        /// </summary>
        TemplateEntity FetchDefaultPickListTemplate();

        /// <summary>
        /// Ensure the given template is configured
        /// </summary>
        bool EnsureTemplateConfigured(IWin32Window owner, TemplateEntity template);

        /// <summary>
        /// Fetch all of the pick list templates
        /// </summary>
        IEnumerable<TemplateEntity> FetchPickListTemplates();

        /// <summary>
        /// Load the computer specific settings for the template
        /// </summary>
        TemplateComputerSettingsEntity GetComputerSettings(TemplateEntity template);
    }
}