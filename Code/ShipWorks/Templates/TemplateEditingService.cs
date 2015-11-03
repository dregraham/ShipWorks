using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Templates.Controls;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Management;
using ShipWorks.Data.Connection;
using ShipWorks.Templates.Processing;
using ShipWorks.Data.Model;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Xml.Linq;
using ShipWorks.Data;
using System.IO;
using Interapptive.Shared;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using log4net;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Provides the context in which templates and the tempalte tree can be edited.
    /// </summary>
    public static class TemplateEditingService
    {
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateEditingService));

        /// <summary>
        /// Move the given node to the specified target folder
        /// </summary>
        public static void MoveToFolder(TemplateTreeNode sourceNode, TemplateFolderEntity targetFolder)
        {
            ValidateParentFolder(sourceNode, targetFolder);

            sourceNode.ParentFolderID = (targetFolder != null) ? targetFolder.TemplateFolderID : (long?) null;
            SaveNode(sourceNode);
        }

        /// <summary>
        /// Copy the given template or folder to the target folder
        /// </summary>
        public static TemplateTreeNode CopyToFolder(TemplateTreeNode sourceNode, TemplateFolderEntity folder)
        {
            if (sourceNode == null)
            {
                throw new ArgumentNullException("sourceNode");
            }

            if (!sourceNode.IsFolder && folder == null)
            {
                throw new TemplateInvalidLocationException("No folder was selected.");
            }

            TemplateTreeNode result;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                if (sourceNode.IsFolder)
                {
                    TemplateFolderEntity copy = CopyFolder(sourceNode.Folder, folder, adapter);
                    result = new TemplateTreeNode(copy);
                }
                else
                {
                    TemplateEntity copy = CopyTemplate(sourceNode.Template, folder, adapter);
                    result = new TemplateTreeNode(copy);
                }

                adapter.Commit();
            }

            return result;
        }

        /// <summary>
        /// Copy the specified folder into the given parent
        /// </summary>
        private static TemplateFolderEntity CopyFolder(TemplateFolderEntity folder, TemplateFolderEntity targetParent, SqlAdapter adapter)
        {
            // Create a copy, exactly the same as the original
            TemplateFolderEntity copy = new TemplateFolderEntity();
            copy.Fields = folder.Fields.CloneAsDirty();
            copy.TemplateTree = folder.TemplateTree;

            // Change the copy to its own unique values
            copy.Name = copy.Name + " (Copy)";
            copy.ParentFolder = targetParent;

            SaveFolder(copy, adapter);

            // Copy all the children
            foreach (TemplateFolderEntity childFolder in folder.ChildFolders)
            {
                CopyFolder(childFolder, copy, adapter);
            }

            // Copy all the templates
            foreach (TemplateEntity template in folder.Templates)
            {
                CopyTemplate(template, copy, adapter);
            }

            return copy;
        }

        /// <summary>
        /// Copy the specified template into the given parent
        /// </summary>
        private static TemplateEntity CopyTemplate(TemplateEntity template, TemplateFolderEntity targetParent, SqlAdapter adapter)
        {
            // Create a copy, exactly the same as the original
            TemplateEntity copy = new TemplateEntity();
            copy.Fields = template.Fields.CloneAsDirty();
            copy.TemplateTree = template.TemplateTree;

            // Change the copy to its unique properties
            copy.Name = copy.Name + " (Copy)";
            copy.ParentFolder = targetParent;

            // Save it
            SaveTemplate(copy, true, adapter);

            // Copy Store specific settings
            foreach (TemplateStoreSettingsEntity storeSettings in TemplateStoreSettingsCollection.Fetch(adapter, TemplateStoreSettingsFields.TemplateID == template.TemplateID))
            {
                TemplateStoreSettingsEntity copiedSettings = new TemplateStoreSettingsEntity(storeSettings.Fields.CloneAsDirty());
                copiedSettings.TemplateID = copy.TemplateID;
                adapter.SaveAndRefetch(copiedSettings);
            }

            // Copy Computer specific settings
            foreach (TemplateComputerSettingsEntity computerSettings in TemplateComputerSettingsCollection.Fetch(adapter, TemplateComputerSettingsFields.TemplateID == template.TemplateID))
            {
                TemplateComputerSettingsEntity copiedSettings = new TemplateComputerSettingsEntity(computerSettings.Fields.CloneAsDirty());
                copiedSettings.TemplateID = copy.TemplateID;
                adapter.SaveAndRefetch(copiedSettings);
            }

            // Copy User specific settings
            foreach (TemplateUserSettingsEntity userSettings in TemplateUserSettingsCollection.Fetch(adapter, TemplateUserSettingsFields.TemplateID == template.TemplateID))
            {
                TemplateUserSettingsEntity copiedSettings = new TemplateUserSettingsEntity(userSettings.Fields.CloneAsDirty());
                copiedSettings.TemplateID = copy.TemplateID;
                adapter.SaveAndRefetch(copiedSettings);
            }

            return copy;
        }

        /// <summary>
        /// Save the given node appropriately depending on if its a Folder or Template
        /// </summary>
        private static void SaveNode(TemplateTreeNode sourceNode)
        {
            if (sourceNode.IsRoot)
            {
                throw new InvalidOperationException("Cannot save the root.");
            }

            if (sourceNode.IsFolder)
            {
                SaveFolder(sourceNode.Folder);
            }
            else
            {
                SaveTemplate(sourceNode.Template);
            }
        }

        /// <summary>
        /// Save the given template.  No resources are updated.
        /// </summary>
        public static void SaveTemplate(TemplateEntity template)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                SaveTemplate(template, false, adapter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Save the given template with the specified adapter.  If updateResources is true, the template must be valid.
        /// </summary>
        public static void SaveTemplate(TemplateEntity template, bool updateResources, SqlAdapter adapter)
        {
            TemplateTree tree = template.TemplateTree as TemplateTree;

            // Validate this template is new, or for this tree
            if (tree == null)
            {
                throw new InvalidOperationException("Trying to save a Template that does not belong to a tree.");
            }

            // Ensure the ParentFolder reference gets set, so that the XSL engine can properly resolve the full template name.
            template.ParentFolder = tree.GetFolder(template.ParentFolderID);
            if (template.ParentFolder == null)
            {
                throw new InvalidOperationException("Could not find ParentFolder in tree when saving template.");
            }

            // Ensure we don't conflict with a rerserved name
            if (template.FullName == @"System\Snippets")
            {
                throw new TemplateException(TemplateHelper.SnippetTemplateReservedNameError);
            }

            TemplateXsl templateXsl = tree.XslCache.FromTemplate(template);

            // If updating resources, we have to have a valid xsl file
            if (updateResources && !templateXsl.IsValid)
            {
                updateResources = false;
            }

            bool isXslEdited = template.Fields[(int) TemplateFieldIndex.Xsl].IsChanged;
            bool isNew = template.IsNew;

            // If updating resources and the template is new, we need to do a save first now to get the ID
            if (updateResources && isNew)
            {
                Save(template, adapter);
            }

            // If the xsl had changed and we need to update resouces
            if (isXslEdited && updateResources)
            {
                UpdateTemplateResources(template);
            }

            // If the template isn't dirty - but a child is - we have to force the template itself to look dirty so TemplateManager's out there (including
            // our own) know to refresh.
            if (!template.IsDirty)
            {
                // Each collection will just have 0 or 1 elements... but using Any is the easiest way to check
                if (template.ComputerSettings.Any(s => s.IsDirty) || template.UserSettings.Any(s => s.IsDirty) || template.StoreSettings.Any(s => s.IsDirty))
                {
                    log.InfoFormat("Forcing template {0} dirty since child settings are dirty.", template.TemplateID);

                    template.Fields[(int) TemplateFieldIndex.Type].IsChanged = true;
                    template.Fields.IsDirty = true;
                }
            }

            // Now save the template
            Save(template, adapter);
        }

        /// <summary>
        /// Save the given folder
        /// </summary>
        public static void SaveFolder(TemplateFolderEntity folder)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                SaveFolder(folder, adapter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Save the given folder using the specified adapter.
        /// </summary>
        public static void SaveFolder(TemplateFolderEntity folder, SqlAdapter adapter)
        {
            TemplateTree tree = folder.TemplateTree as TemplateTree;

            // Validate this template is new, or for this tree
            if (tree == null)
            {
                throw new InvalidOperationException("Trying to save a Folder that does not belong to a tree.");
            }

            // Ensure the ParentFolder reference gets set
            if (folder.ParentFolderID != null)
            {
                folder.ParentFolder = tree.GetFolder(folder.ParentFolderID.Value);
            }
            else
            {
                folder.ParentFolder = null;
            }

            // They can't be using the reserved name "System"
            if (folder.FullName == "System" && !folder.IsSystemFolder)
            {
                throw new TemplateException("The folder name 'System' is reserved and cannot be used.");
            }

            // They can't be using the reserved name "System\Snippets"
            if (folder.FullName == @"System\Snippets" && !folder.IsSnippetsFolder)
            {
                throw new TemplateException(@"The folder name 'System\Snippets' is reserved and cannot be used.");
            }

            Save(folder, adapter);

            // When its a root folder, we have to tell the TemplateTree about it.  There's no other way for it to be picked up
            if (folder.ParentFolder == null)
            {
                tree.IncludeRootFolder(folder);
            }
        }

        /// <summary>
        /// Do the save with appropriate exception translation
        /// </summary>
        private static void Save(EntityBase2 entity, SqlAdapter adapter)
        {
            try
            {
                adapter.SaveAndRefetch(entity);
            }
            catch (Exception ex)
            {
                if (!TemplateHelper.TranslateException(ex))
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete the specified folder.
        /// </summary>
        public static void DeleteFolder(TemplateFolderEntity folder)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                DeleteFolder(folder, adapter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Delete the specified folder.
        /// </summary>
        private static void DeleteFolder(TemplateFolderEntity folder, SqlAdapter adapter)
        {
            // Delete all child folders
            foreach (TemplateFolderEntity childFolder in new List<TemplateFolderEntity>(folder.ChildFolders))
            {
                DeleteFolder(childFolder, adapter);
            }

            // Delete all child templates
            foreach (TemplateEntity template in new List<TemplateEntity>(folder.Templates))
            {
                DeleteTemplate(template, adapter);
            }

            folder.ParentFolder = null;
            adapter.DeleteEntity(folder);
        }

        /// <summary>
        /// Delete the specified template
        /// </summary>
        public static void DeleteTemplate(TemplateEntity template)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                DeleteTemplate(template, adapter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Delete the specified template
        /// </summary>
        private static void DeleteTemplate(TemplateEntity template, SqlAdapter adapter)
        {
            template.ParentFolder = null;
            adapter.DeleteEntity(template);
        }

        /// <summary>
        /// Validate that the given source node can be moved as a child of the specified target folder.
        /// </summary>
        private static void ValidateParentFolder(TemplateTreeNode sourceNode, TemplateFolderEntity targetFolder)
        {
            // A folder can always be moved to top-level
            if (sourceNode.IsFolder && targetFolder == null)
            {
                return;
            }

            // Templates can only go in folders, they cant be top-level
            if (targetFolder == null)
            {
                throw new TemplateInvalidLocationException("No folder was selected.");
            }

            if (sourceNode.IsFolder && sourceNode.Folder == targetFolder)
            {
                throw new TemplateInvalidLocationException("A folder cannot be a child of itself.");
            }

            if (sourceNode.IsFolder && targetFolder.ChildFolders.Contains(sourceNode.Folder))
            {
                throw new TemplateInvalidLocationException("A folder cannot be added to a folder it is already in.");
            }

            if (!sourceNode.IsFolder && targetFolder.Templates.Contains(sourceNode.Template))
            {
                throw new TemplateInvalidLocationException("A template cannot be added to a folder it is already in.");
            }

            // If the parent has the child for its anscestor at some point, then the child cant really be a child
            if (sourceNode.IsFolder && HasAncestor(targetFolder, sourceNode.Folder))
            {
                throw new TemplateInvalidLocationException("A folder cannot be a descendant of itself.");
            }

            // Can't end up creating a System\Snippets template, as it would conflict with our builtin Snippets folder
            if (!sourceNode.IsFolder && sourceNode.Name == "Snippets" && targetFolder.IsSystemFolder)
            {
                throw new TemplateInvalidLocationException(TemplateHelper.SnippetTemplateReservedNameError);
            }
        }

        /// <summary>
        /// Indicates if the child is a descendant of the specified potential ancestor
        /// </summary>
        private static bool HasAncestor(TemplateFolderEntity child, TemplateFolderEntity ancestor)
        {
            // Check all of the potential ancestores immediate children
            foreach (TemplateFolderEntity childFolder in ancestor.ChildFolders)
            {
                // The potential anscestor has this child as an immediate child.
                if (childFolder == child)
                {
                    return true;
                }

                // Check if this folder would be an ancestor
                if (HasAncestor(child, childFolder))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Update the resource usage of the template by replacing local image references with images save in the database.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void UpdateTemplateResources(TemplateEntity template)
        {
            // Load the XML into a document
            XDocument xDocument = XDocument.Parse(template.Xsl, LoadOptions.PreserveWhitespace);

            // This is all the resource keys currently used by the template
            List<long> previousReferences = DataResourceManager.GetConsumerResourceReferenceIDs(template.TemplateID);

            // Keep track of all the resources we create or update
            List<long> updatedReferences = new List<long>();

            // We also need to track all the references we are still using
            List<long> sillReferencing = new List<long>();

            bool updateXslRequired = false;

            // Find all the img's
            foreach (XElement img in xDocument.Descendants("img").ToArray())
            {
                // Not just img.Attribute("src"), so it can be case insensitive.
                XAttribute srcAttribute = img.Attributes().Where(a => a.Name.LocalName.ToLowerInvariant() == "src").FirstOrDefault();
                if ((string) srcAttribute != null)
                {
                    Uri srcUri;

                    // We can only work with absolute URI's.  What what a relative URI be relative to?
                    if (Uri.TryCreate(srcAttribute.Value, UriKind.RelativeOrAbsolute, out srcUri))
                    {
                        string localFilePath = null;

                        // If it's an absolute path check if is an existing file
                        if (srcUri.IsAbsoluteUri)
                        {
                            if (srcUri.IsFile && File.Exists(srcUri.LocalPath))
                            {
                                localFilePath = srcUri.LocalPath;
                            }
                        }
                        // It's relative, see if it is an existing resource
                        else
                        {
                            string resourceFile = Path.Combine(DataPath.CurrentResources, srcAttribute.Value);

                            // The resource file exists
                            if (File.Exists(resourceFile))
                            {
                                // Setting this here is basically saying we are assuming this resource is new to this template and we are going to create a new resource reference for it.
                                localFilePath = resourceFile;

                                // There are two scenarios here. One is that we are already referencing this resource (most common).  The second scenario is this
                                // img was copy\pasted from another template, and we don't reference it yet.  To find this out, we look at the "shipworksid" attribute
                                // and see if we are already referencing it
                                XAttribute idAttribute = img.Attribute("shipworksid");
                                if (idAttribute != null)
                                {
                                    long shipworksID;
                                    if (long.TryParse((string) idAttribute, out shipworksID) && previousReferences.Contains(shipworksID))
                                    {
                                        // Doing this means we won't deal with this resource - and we won't have to
                                        localFilePath = null;

                                        // Mark that we are still referencing this reference
                                        sillReferencing.Add(shipworksID);
                                    }
                                }

                                // If this isn't one we are already referencing correctly (it would be set to null if it was)...
                                if (localFilePath != null)
                                {
                                    // Do a little trickery by create a new file with the same name as it was imported from.  That way the new resource Label will pick up that same filename too.
                                    XAttribute importedFromAttribute = img.Attribute("importedFrom");
                                    if (importedFromAttribute != null)
                                    {
                                        string claimedImportedFilename = Path.GetFileName(importedFromAttribute.Value);
                                        if (Path.GetExtension(claimedImportedFilename) == Path.GetExtension(resourceFile))
                                        {
                                            localFilePath = Path.Combine(DataPath.CreateUniqueTempPath(), claimedImportedFilename);
                                            File.Copy(resourceFile, localFilePath);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // This is certainly not a resource we manage - so clear out the ID information.
                                img.SetAttributeValue("importedFrom", null);
                                img.SetAttributeValue("shipworksid", null);

                                updateXslRequired = true;
                            }
                        }

                        // If its an existing local file, make sure there is a database resource for it
                        if (localFilePath != null)
                        {
                            // Register the resouce
                            DataResourceReference resource = DataResourceManager.CreateFromFile(localFilePath, template.TemplateID);
                            updatedReferences.Add(resource.ReferenceID);

                            // Update the attribute with the new filename
                            srcAttribute.Value = resource.Filename;

                            // Add the shipworks special attributes for template resources
                            img.SetAttributeValue("importedFrom", resource.Label);
                            img.SetAttributeValue("shipworksid", resource.ReferenceID);
                        }
                    }
                }
            }

            // If we made resource updates, regen the xsl
            if (updateXslRequired || updatedReferences.Count > 0)
            {
                template.Xsl = xDocument.ToString(SaveOptions.DisableFormatting);
            }

            // Release all resources that we are no longer using
            foreach (long referenceID in previousReferences.Except(updatedReferences.Concat(sillReferencing)))
            {
                DataResourceManager.ReleaseResourceReference(referenceID);
            }
        }
    }
}