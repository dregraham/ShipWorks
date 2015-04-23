using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.IO;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using log4net;


namespace ShipWorks.Stores.Platforms.Choxi
{
    public static class ChoxiTemplate
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(ChoxiStoreType));

        /// <summary>
        /// Install the NoMoreRack Template
        /// </summary>
        public static void InstallChoxiTemplate()
        {
            // Get all templates
            IList<TemplateEntity> templates = TemplateManager.Tree.AllTemplates;

            //Check to see if the template already exists
            if (!templates.Any(t => t.Name == "Choxi Invoice"))
            {
                //Template Tree
                TemplateTree tree = TemplateManager.Tree.CreateEditableClone();

                //create new template
                TemplateEntity template = new TemplateEntity();

                //Folder where we will save the template
                TemplateFolderEntity invoices = GetTemplateFolder(null, tree, "Invoices");

                SetTemplateDefaults(template, tree, invoices);

                try
                {
                    //save the template
                    TemplateEditingService.SaveTemplate(template);
                }
                catch (Exception ex)
                {
                    log.Error("Error saving Choxi Template", ex);
                }
            }
        }

        /// <summary>
        /// Returns folder specified or creates one 
        /// </summary>
        private static TemplateFolderEntity GetTemplateFolder(TemplateFolderEntity parent, TemplateTree tree, string name)
        {
            // Get all the folders
            IList<TemplateFolderEntity> folders = TemplateManager.Tree.AllFolders;

            // If there is an invoice folder, use the first one
            TemplateFolderEntity folder = folders.FirstOrDefault(f => f.Name == name);

            //Check to see if the folder exists, if not create one 
            if (folder != null)
            {
                return folder;
            }
            else
            {
                log.InfoFormat("Creating folder to hold the Choxi Template");

                TemplateFolderEntity newFolder = new TemplateFolderEntity();
                newFolder.Name = name;
                newFolder.ParentFolder = parent;
                newFolder.TemplateTree = tree;

                try
                {
                    TemplateEditingService.SaveFolder(newFolder);
                }
                catch (Exception ex)
                {
                    log.Error("Error saving folder", ex);
                }

                return newFolder;
            }
        }

        /// <summary>
        /// Sets template default properties 
        /// </summary>
        private static void SetTemplateDefaults(TemplateEntity template, TemplateTree tree, TemplateFolderEntity parent)
        {
            //set template properties
            template.Name = "Choxi Invoice";
            template.Xsl = GetXsl("ShipWorks.Stores.Platforms.NoMoreRack.Template.ChoxiPackingSlip.xsl");
            template.Type = (int)TemplateType.Standard;
            template.Context = (int)TemplateInputContext.Order;
            template.OutputFormat = (int)TemplateOutputFormat.Html;
            template.OutputEncoding = "utf-8";
            template.PageMarginLeft = .75;
            template.PageMarginRight = .75;
            template.PageMarginBottom = .75;
            template.PageMarginTop = .75;
            template.PageWidth = 8.5;
            template.PageHeight = 11;
            template.LabelSheetID = -44;
            template.PrintCopies = 1;
            template.PrintCollate = false;
            template.SaveFileName = "Order {//Order/Number}";
            template.ParentFolder = parent;
            template.TemplateTree = tree;
            template.SaveFileFolder = TemplateHelper.DefaultTemplateSaveDirectory;
            template.SaveFilePrompt = 1;
            template.SaveFileBOM = false;
            template.SaveFileOnlineResources = false;
        }

        /// <summary>
        /// Reads XSL file into string
        /// </summary>
        private static string GetXsl(string PathToXsl)
        {
            string xsl = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(PathToXsl))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    xsl = reader.ReadToEnd();
                }
            }

            return xsl;
        }
    }
}
