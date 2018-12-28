using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.Templates.Controls.DefaultPickListTemplate;

namespace ShipWorks.UI.Dialogs.DefaultPickListTemplate
{
    /// <summary>
    /// View model for DefaultPickListTemplateDialog
    /// </summary>
    [Component]
    public class DefaultPickListTemplateDialogViewModel : IDefaultPickListTemplateDialogViewModel
    {
        private readonly IConfigurationData configurationData;
        private const string PickListsFolderName = "Pick Lists";
        private const string ReportsFolderName = "Reports";

        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultPickListTemplateDialogViewModel(IConfigurationData configurationData, ITemplateManager templateManager)
        {
            this.configurationData = configurationData;

            SavePickListTemplateCommand = new RelayCommand(SavePickListTemplate, () => SelectedPickListTemplate != null);

            PickListTemplates = templateManager.Tree.AllTemplates.Where(t => t.ParentFolder.Name == PickListsFolderName);
            if (PickListTemplates.None())
            {
                PickListTemplates = templateManager.Tree.AllTemplates.Where(t => t.ParentFolder.Name == ReportsFolderName);
            }
        }

        /// <summary>
        /// Link to support article regarding changing the default template after it has been selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Uri SupportArticleLink { get; } = new Uri("http://support.shipworks.com/");

        /// <summary>
        /// All of the templates in the pick list folder
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<TemplateEntity> PickListTemplates { get; set; }

        /// <summary>
        /// The currently selected template
        /// </summary>
        [Obfuscation(Exclude = true)]
        public TemplateEntity SelectedPickListTemplate { get; set; }

        /// <summary>
        /// Command for saving the pick list template
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand SavePickListTemplateCommand { get; set; }

        /// <summary>
        /// Save the id of the selected pick list template as the DefaultPickListTemplateID on the Configuration table
        /// </summary>
        private void SavePickListTemplate()
        {
            if (SelectedPickListTemplate != null)
            {
                configurationData.UpdateConfiguration(x => x.DefaultPickListTemplateID = SelectedPickListTemplate.TemplateID);
            }
        }
    }
}
