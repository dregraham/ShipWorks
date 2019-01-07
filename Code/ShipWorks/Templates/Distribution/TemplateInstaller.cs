using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Autofac;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Templates.Management.Skeletons;
using ShipWorks.Templates.Media;
using ShipWorks.Templates.Saving;

namespace ShipWorks.Templates.Distribution
{
    /// <summary>
    /// Responsible for installing new templates and adding edited\updated versions of templates
    /// </summary>
    public class TemplateInstaller
    {
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateInstaller));

        string sourceDirectory;

        /// <summary>
        /// Represents a template that is pending installation
        /// </summary>
        class TemplateInstallInfo
        {
            string targetFullName;

            /// <summary>
            /// The full name including parent folders of the source template
            /// </summary>
            public string SourceFullName { get; set; }

            /// <summary>
            /// The folder that the template will be installed into.
            /// </summary>
            public string TargetFullName
            {
                get
                {
                    return targetFullName ?? SourceFullName;
                }
                set
                {
                    targetFullName = value;
                }
            }
        }

        // List of templates pending installation
        List<TemplateInstallInfo> templatesToInstall = new List<TemplateInstallInfo>();

        /// <summary>
        /// Raised when a template is about to be installed
        /// </summary>
        public event EventHandler<TemplateInstallingEventArgs> TemplateInstalling;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateInstaller(string sourceDirectory)
        {
            this.sourceDirectory = sourceDirectory;
        }

        /// <summary>
        /// Install the given embedded template into the TemplateTree
        /// </summary>
        public TemplateEntity InstallTemplate(string templateFullName, TemplateTree tree)
        {
            if (templatesToInstall.Count > 0)
            {
                throw new InvalidOperationException("You cannot combined calls to InstallTemplate and AddToInstallQueue");
            }

            AddToInstallQueue(templateFullName);

            return InstallQueuedTemplates(tree)[0];
        }

        /// <summary>
        /// Add the given template to the queue of templates to be installed.  If the template is not being installed to its originall location the targetFolder parameter can be specified.
        /// The point of doing it this way in batch as a queue is so that the installer can automatically fixup all xsl:import tags to refer to each template in the queue's no location
        /// if it's folder or name changes.
        /// </summary>
        public void AddToInstallQueue(string sourceFullName, string targetFullName = null)
        {
            templatesToInstall.Add(new TemplateInstallInfo { SourceFullName = sourceFullName, TargetFullName = targetFullName });
        }

        /// <summary>
        /// Install all the templates that have been added for installation by calls to AddToInstallQueue
        /// </summary>
        public List<TemplateEntity> InstallQueuedTemplates(TemplateTree tree)
        {
            List<TemplateEntity> templates = new List<TemplateEntity>();

            // Install each template in the list
            foreach (TemplateInstallInfo installInfo in templatesToInstall)
            {
                TemplateEntity template = InstallTemplate(installInfo, tree);

                templates.Add(template);
            }

            // Now we need to go back through and process all there resources. We have to do this after they are all installed in case they reference each other.  If references
            // were missing, the template would be considered invalid, and would resources would not get processed.
            foreach (TemplateEntity template in templates)
            {
                // The editing service only bothers to update resources if it thinks the XSL has changed
                template.Fields[(int) TemplateFieldIndex.Xsl].IsChanged = true;

                TemplateEditingService.SaveTemplate(template, true, SqlAdapter.Default);
            }

            templatesToInstall.Clear();

            return templates;
        }

        /// <summary>
        /// Install the given template based on the installation information
        /// </summary>
        private TemplateEntity InstallTemplate(TemplateInstallInfo installInfo, TemplateTree tree)
        {
            log.InfoFormat("Installing template [{0}] to [{1}]", installInfo.SourceFullName, installInfo.TargetFullName);

            if (TemplateInstalling != null)
            {
                TemplateInstalling(this, new TemplateInstallingEventArgs(installInfo.SourceFullName, installInfo.TargetFullName));
            }

            // We need to create all necessary folders
            TemplateFolderEntity parent = CreateParentFolders(TemplateHelper.GetTemplateFolder(installInfo.TargetFullName), tree);

            // Now create the template
            return CreateTemplate(installInfo, parent);
        }

        /// <summary>
        /// Create an instance of the given template in the specified parent
        /// </summary>
        private TemplateEntity CreateTemplate(TemplateInstallInfo installInfo, TemplateFolderEntity parent)
        {
            string name = installInfo.TargetFullName.Split('\\').Last();

            TemplateEntity template = GetOrCreateTemplate(parent, name);
            template.ParentFolder = parent;
            template.Name = name;
            template.TemplateTree = parent.TemplateTree;

            // Read the template XSL.
            template.Xsl = PrepareTemplateXsl(installInfo);

            ReadTemplateSettings(template, installInfo.SourceFullName);

            // Save the template.
            TemplateEditingService.SaveTemplate(template, true, SqlAdapter.Default);

            return template;
        }

        /// <summary>
        /// Get a template with the given name in the specified parent, or else create a new one
        /// </summary>
        private TemplateEntity GetOrCreateTemplate(TemplateFolderEntity parent, string name)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ISqlAdapter sqlAdapter = lifetimeScope.Resolve<ISqlAdapterFactory>().Create();

                EntityCollection<TemplateEntity> templates = sqlAdapter.GetCollectionFromPredicate<TemplateEntity>(x =>
                    x.Add(TemplateFields.ParentFolderID == parent.TemplateFolderID).AddWithAnd(TemplateFields.Name == name), 1);

                return templates.FirstOrDefault() ?? new TemplateEntity();
            }
        }

        /// <summary>
        /// Read the templateXsl to be used for the template
        /// </summary>
        private string PrepareTemplateXsl(TemplateInstallInfo installInfo)
        {
            string templateDiretory = Path.Combine(sourceDirectory, installInfo.SourceFullName);
            string templateFile = Path.Combine(templateDiretory, "template.xsl");

            if (!File.Exists(templateFile))
            {
                return TemplateSkeletons.GetTemplateSkeleton(TemplateType.Standard, TemplateOutputFormat.Html);
            }

            string templateXsl = File.ReadAllText(templateFile).Trim();

            try
            {
                // Load the XML into a document
                XDocument xDocument = XDocument.Parse(templateXsl, LoadOptions.PreserveWhitespace);

                // We need to know if we changed it so we can know if we should regenerate it
                bool changed = false;

                // First we have to fixup the imports, which may be needed by subsequent steps
                changed |= FixupXslImports(xDocument);

                // And finally prepare image paths for any known relative image locations
                changed |= PrepareImagePaths(xDocument, templateDiretory);

                // If we updated stuff we need to get the updated XSL
                if (changed)
                {
                    templateXsl = xDocument.ToString(SaveOptions.DisableFormatting);
                }
            }
            catch (XmlException ex)
            {
                log.Warn(string.Format("Failed to process content for template [{0}] due to invalid XML.", installInfo.SourceFullName), ex);
            }

            return templateXsl.Trim();
        }

        /// <summary>
        /// Look at each xsl:import in the document, and for any that refer to a template in the install queue, translate them to the template's new target location.
        /// </summary>
        private bool FixupXslImports(XDocument xDocument)
        {
            bool changed = false;

            XNamespace xslNamespace = "http://www.w3.org/1999/XSL/Transform";

            foreach (XElement importTag in xDocument.Descendants(xslNamespace + "import").ToList())
            {
                // Not just img.Attribute("href"), so it can be case insensitive.
                XAttribute hrefAttribute = importTag.Attributes().Where(a => a.Name.LocalName.ToLowerInvariant() == "href").FirstOrDefault();
                if ((string) hrefAttribute != null)
                {
                    TemplateInstallInfo importedTemplate = templatesToInstall.FirstOrDefault(t => t.SourceFullName == hrefAttribute.Value);
                    if (importedTemplate != null && importedTemplate.SourceFullName != importedTemplate.TargetFullName)
                    {
                        hrefAttribute.Value = importedTemplate.TargetFullName;

                        changed = true;
                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// Prepare the image paths in the document by making relative images that have a valid path on disk into absolute images that the template resource importer will find.
        /// </summary>
        private static bool PrepareImagePaths(XDocument xDocument, string templateDiretory)
        {
            bool changed = false;

            // Find all the img's in the xsl content that are relative - which will be relative to the embedded resources for the template
            foreach (XElement img in xDocument.Descendants("img").ToArray())
            {
                // Not just img.Attribute("src"), so it can be case insensitive.
                XAttribute srcAttribute = img.Attributes().Where(a => a.Name.LocalName.ToLowerInvariant() == "src").FirstOrDefault();
                if ((string) srcAttribute != null)
                {
                    Uri srcUri;

                    // Look for relative URI's, which by convention for distributed templates refer to embedded resources in the same folder as the template.xsl
                    if (Uri.TryCreate(srcAttribute.Value, UriKind.Relative, out srcUri))
                    {
                        string relativePath = srcAttribute.Value.Replace("/", @"\");
                        if (relativePath.StartsWith(@"\"))
                        {
                            relativePath = relativePath.Remove(0, 1);
                        }

                        string fullImagePath = Path.Combine(templateDiretory, relativePath);

                        // If the file exists update the template to point to its full path.  If it doesn't exist, its possible that it's a mistake - but its also valid
                        // because it could be an XSL construct like src="{Thumbnail}"
                        if (File.Exists(fullImagePath))
                        {
                            // Update the attribute to be an absolute path
                            srcAttribute.Value = fullImagePath;

                            // Will need to update the xsl from the new document content
                            changed = true;
                        }
                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// Read the template settings from the embedded xml settings content and apply them to the template
        /// </summary>
        private void ReadTemplateSettings(TemplateEntity template, string templateFullName)
        {
            string settingsXml = File.ReadAllText(Path.Combine(Path.Combine(sourceDirectory, templateFullName), "settings.xml")); ;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(settingsXml);

            XPathNavigator xpath = xmlDocument.CreateNavigator();

            // General
            template.Type = (int) EnumHelper.GetEnumList<TemplateType>().Single(t => t.Description == XPathUtility.Evaluate(xpath, "//General/Type", "")).Value;
            template.Context = (int) EnumHelper.GetEnumList<TemplateInputContext>().Single(c => c.Description == XPathUtility.Evaluate(xpath, "//General/Context", "")).Value;
            template.OutputFormat = (int) EnumHelper.GetEnumList<TemplateOutputFormat>().Single(f => f.Description == XPathUtility.Evaluate(xpath, "//General/OutputFormat", "")).Value;
            template.OutputEncoding = XPathUtility.Evaluate(xpath, "//General/OutputEncoding", "utf-8");

            // Page Setup
            template.PageWidth = XPathUtility.Evaluate(xpath, "//PageSetup/Width", 8.5);
            template.PageHeight = XPathUtility.Evaluate(xpath, "//PageSetup/Height", 11.0);
            template.PageMarginLeft = XPathUtility.Evaluate(xpath, "//PageSetup/MarginLeft", .75);
            template.PageMarginRight = XPathUtility.Evaluate(xpath, "//PageSetup/MarginRight", .75);
            template.PageMarginTop = XPathUtility.Evaluate(xpath, "//PageSetup/MarginTop", .75);
            template.PageMarginBottom = XPathUtility.Evaluate(xpath, "//PageSetup/MarginBottom", .75);

            // Label setup
            template.LabelSheetID = XPathUtility.Evaluate(xpath, "//LabelSheet/LabelSheetID", LabelSheetManager.DefaultLabelSheetID);

            // Printing
            template.PrintCopies = XPathUtility.Evaluate(xpath, "//Printing/Copies", 1);
            template.PrintCollate = XPathUtility.Evaluate(xpath, "//Printing/Collate", false);

            // Saving
            template.SaveFileName = XPathUtility.Evaluate(xpath, "//Saving/Filename", "Order {//Order/Number}");
            template.SaveFileFolder = TemplateHelper.DefaultTemplateSaveDirectory;
            template.SaveFilePrompt = (int) SavePromptWhen.Once;
            template.SaveFileBOM = false;
            template.SaveFileOnlineResources = false;
        }

        /// <summary>
        /// Ensure each parent folder referenced by the given template exists.  Folders are created into the given tree
        /// </summary>
        private static TemplateFolderEntity CreateParentFolders(string templateFullName, TemplateTree tree)
        {
            List<string> folderPath = templateFullName.Split('\\').ToList();

            // Can't have a template at the root level
            if (folderPath.Count == 0)
            {
                throw new InvalidOperationException(string.Format("Embedded template {0} sits at the root level.", templateFullName));
            }

            // We start at the root
            TemplateFolderEntity parent = null;

            // Go through each folder and make sure it exists
            foreach (string folderName in folderPath)
            {
                // Determine the list of siblings from the parent to see if this folder exists
                var siblingFolders = (parent != null) ? parent.ChildFolders : tree.RootFolders;

                TemplateFolderEntity folder = siblingFolders.FirstOrDefault(f => f.Name == folderName);

                if (folder != null)
                {
                    parent = folder;
                }
                else
                {
                    folder = new TemplateFolderEntity();
                    folder.Name = folderName;
                    folder.ParentFolder = (parent != null) ? parent : null;
                    folder.TemplateTree = tree;

                    TemplateEditingService.SaveFolder(folder);
                    log.InfoFormat("Created template folder [{0}]", folder.FullName);

                    // This is the new parent
                    parent = folder;
                }
            }

            return parent;
        }
    }
}
