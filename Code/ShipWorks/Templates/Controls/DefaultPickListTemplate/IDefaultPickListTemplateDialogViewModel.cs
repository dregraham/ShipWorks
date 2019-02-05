using System;
using System.Collections.Generic;
using System.Windows.Input;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates.Controls.DefaultPickListTemplate
{
    /// <summary>
    /// Represents the view model for the default pick list template
    /// </summary>
    public interface IDefaultPickListTemplateDialogViewModel
    {
        /// <summary>
        /// Link to support article regarding changing the default template after it has been selected
        /// </summary>
        Uri SupportArticleLink { get; }

        /// <summary>
        /// All of the templates in the pick list folder
        /// </summary>
        IEnumerable<TemplateEntity> PickListTemplates { get; set; }

        /// <summary>
        /// The currently selected template
        /// </summary>
        TemplateEntity SelectedPickListTemplate { get; set; }

        /// <summary>
        /// Command for saving the pick list template
        /// </summary>
        ICommand SavePickListTemplateCommand { get; set; }
    }
}