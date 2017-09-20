using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates.Distribution
{
    /// <summary>
    /// Class for working with the templates builtin to ShipWorks
    /// </summary>
    public static class BuiltinTemplates
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BuiltinTemplates));

        // The root namespace where all the templates to distribute go
        static string sourceDirectory = Path.Combine(DataPath.ShipWorksTemp, "TemplateSource");

        /// <summary>
        /// Update the set of installed templates to reflect the current set of Interapptive distributed templates
        /// </summary>
        public static void UpdateTemplates(IWin32Window owner)
        {
            Version swVersion = Assembly.GetExecutingAssembly().GetName().Version;

            // Determine what template version is installed right now
            Version installed = new Version(SystemData.Fetch().TemplateVersion);

            // Special case for us internally to develop
            if (swVersion.Major == 0 && InterapptiveOnly.MagicKeysDown)
            {
                using (InternalTestVersionDlg dlg = new InternalTestVersionDlg())
                {
                    if (dlg.ShowDialog(owner) == DialogResult.OK)
                    {
                        swVersion = dlg.ShipWorksVersion;
                        installed = dlg.TemplateVersion;
                    }
                }
            }

            // Fake it if running under development, otherwise they'd try to install themselves every time
            if (swVersion.Major == 0)
            {
                // Has to be set to the biggest number we check below
                swVersion = new Version("5.18.0.0000");
            }

            // No default templates are installed yet - we are safe to do the initial install
            if (installed == new Version("0.0.0.0"))
            {
                PerformInitialInstall();

                UpdateDatabaseTemplateVersion(swVersion);
            }
            else
            {
                InstallTemplate("Shipper Productivity", "Reports", "3.7.0.5018", false);

                // SingleScan templates, check to make sure that the OrderSingleScan snippet does not exist
                // If the same snippet is installed twice it breaks all templates in ShipWorks
                if (installed < new Version("5.10.0.0000") &&
                    TemplateManager.Tree.AllTemplates.None(t => t.Name == "OrderSingleScan" && t.ParentFolderID == TemplateBuiltinFolders.SnippetsFolderID))
                {
                    InstallTemplate(@"System\Snippets\OrderSingleScan", TemplateManager.Tree.CreateEditableClone());
                    InstallTemplate(@"Packing Slips\Single Scan", TemplateManager.Tree.CreateEditableClone());
                    InstallTemplate(@"Invoices\Single Scan", TemplateManager.Tree.CreateEditableClone());

                    UpdateDatabaseTemplateVersion(swVersion);
                }

                if (installed < new Version("5.17.0.0000") &&
                    TemplateManager.Tree.AllTemplates.None(t => t.Name == "ItemGroup" && t.ParentFolderID == TemplateBuiltinFolders.SnippetsFolderID))
                {
                    InstallTemplate(@"Packing Slips\Standard Grouping by SKU", TemplateManager.Tree.CreateEditableClone());
                    InstallTemplate(@"Invoices\Standard Grouping by SKU", TemplateManager.Tree.CreateEditableClone());
                    InstallTemplate(@"System\Snippets\ItemGroup", TemplateManager.Tree.CreateEditableClone());
                    
                    UpdateDatabaseTemplateVersion(swVersion);
                }
                
                InstallTemplate("Package Level Details", @"Reports\Exports", "5.19.0000", false);
                InstallTemplate("Shipments by Provider", @"Reports\Financials", "5.19.0.0000", false);
                InstallTemplate("Standard 4x6", "Labels", "5.19.0.0000", false);
            }
        }

        /// <summary>
        /// Install the given template for the specific version
        /// </summary>
        /// <param name="name">the template name</param>
        /// <param name="folderPath">the path to the template</param>
        /// <param name="version">the version that this template is being installed for</param>
        /// <param name="replaceExisting">if a template with a matching name exists should we replace it</param>
        private static void InstallTemplate(string name, string folderPath, string version, bool replaceExisting)
        {
            if (new Version(SystemData.Fetch().TemplateVersion) < new Version(version))
            {
                if (replaceExisting || TemplateManager.Tree.AllTemplates.None(t => t.Name == name))
                {
                    InstallTemplate($"{folderPath}\\{name}", TemplateManager.Tree.CreateEditableClone());
                    UpdateDatabaseTemplateVersion(Assembly.GetExecutingAssembly().GetName().Version);
                }
            }
        }

        /// <summary>
        /// Install the given embedded template into the TemplateTree
        /// </summary>
        public static TemplateEntity InstallTemplate(string templateFullName, TemplateTree tree)
        {
            string templateDiretory = Path.Combine(sourceDirectory, templateFullName);
            string templateFile = Path.Combine(templateDiretory, "template.xsl");

            // If the file doesn't exist, we may not yet have extracted the templates. This can be the case especially when installing individual templates from the
            // InstallTemplate function directly
            if (!File.Exists(templateFile))
            {
                ExtractSourceTemplates();
            }

            TemplateInstaller installer = new TemplateInstaller(sourceDirectory);

            return installer.InstallTemplate(templateFullName, tree);
        }

        /// <summary>
        /// Install all the templates as the initial template install
        /// </summary>
        private static void PerformInitialInstall()
        {
            log.InfoFormat("Installing initial template set.");

            // Load the names of embedded templates.  Each string is of the form Folder\[Folder\]*Name and has no .xsl extension on it
            List<string> templateList = ExtractSourceTemplates();

            // Create the editable tree we can install into
            TemplateTree tree = TemplateManager.Tree.CreateEditableClone();

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                TemplateInstaller installer = new TemplateInstaller(sourceDirectory);

                // Add each template to the install queue
                foreach (string templateFullName in templateList)
                {
                    installer.AddToInstallQueue(templateFullName);
                }

                // Install them all
                installer.InstallQueuedTemplates(tree);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Update the database with the version that we updated\installed the current set of templates
        /// </summary>
        private static void UpdateDatabaseTemplateVersion(Version swVersion)
        {
            log.InfoFormat("Updating database TemplateVersion to {0}", swVersion);
            SystemDataEntity data = SystemData.Fetch();
            data.TemplateVersion = swVersion.ToString(4);
            SystemData.Save(data);
        }

        /// <summary>
        /// Extract the templates source.zip and return the list of templates contained in it.
        /// </summary>
        private static List<string> ExtractSourceTemplates()
        {
            Directory.CreateDirectory(sourceDirectory);

            string sourceFile = Path.Combine(sourceDirectory, "source.zip");
            using (Stream stream = Assembly.Load("ShipWorks.Res").GetManifestResourceStream("ShipWorks.Res.Templates.Distribution.Source.zip"))
            {
                StreamUtility.WriteToFile(stream, sourceFile);
            }

            List<string> templates = new List<string>();

            // Unzip the entire zip file
            using (ZipReader reader = new ZipReader(sourceFile))
            {
                foreach (ZipReaderItem item in reader.ReadItems())
                {
                    item.Extract(Path.Combine(sourceDirectory, item.Name));

                    // If this is the template.xsl, add it to the list
                    if (item.Name.EndsWith("template.xsl"))
                    {
                        templates.Add(item.Name.Replace(@"\template.xsl", ""));
                    }
                }
            }

            return templates;
        }

        /// <summary>
        /// Reinstall all the builtin templates
        /// </summary>
        internal static void ReinstallTemplates() => PerformInitialInstall();
    }
}
