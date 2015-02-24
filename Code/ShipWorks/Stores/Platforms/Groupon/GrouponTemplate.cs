using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.IO;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using log4net;


namespace ShipWorks.Stores.Platforms.Groupon
{
    public class GrouponTemplate
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GrouponStoreType));

        /// <summary>
        /// Install the Groupon Template
        /// </summary>
        public static void InstallGrouponTemplate()
        {
            // Get all templates
            IList<TemplateEntity> templates = TemplateManager.Tree.AllTemplates;

            //Check to see if the template already exists
            if (!templates.Any(t => t.Name == "Groupon Invoice"))
            {
                // Get all the folders
                IList<TemplateFolderEntity> folders = TemplateManager.Tree.AllFolders;

                //Template Tree
                TemplateTree tree = TemplateTree.CreateFrom(folders, templates);

                // If there is an invoice folder, use the first one
                TemplateFolderEntity invoices = folders.FirstOrDefault(f => f.Name == "Invoices");

                if(invoices == null)
                {
                    log.InfoFormat("Cannot install Groupon Template because no Invoices folder exists");
                }
                else
                {
                    //create new template
                    TemplateEntity template = new TemplateEntity();

                    SetTemplateDefaults(template, tree, invoices);

                    try
                    {
                        //save the template
                        TemplateEditingService.SaveTemplate(template);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error saving Groupon Template", ex);
                    }

                }
            }
        }

        /// <summary>
        /// Sets template default properties 
        /// </summary>
        private static void SetTemplateDefaults(TemplateEntity template, TemplateTree tree, TemplateFolderEntity parent )
        {
            //set template properties
            template.Name = "Groupon Invoice";
            template.Xsl = GetXsl("ShipWorks.Stores.Platforms.Groupon.Template.GrouponPackingSlip.xsl");
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
