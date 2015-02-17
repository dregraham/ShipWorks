using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using log4net;
using ShipWorks.Templates.Controls;
using ActiproSoftware.SyntaxEditor;
using ShipWorks.Templates.Media;
using ShipWorks.Templates.Saving;
using ShipWorks.Stores;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Administration.UpdateFrom2x.LegacyCode
{
    /// <summary>
    /// Utility class to read the settings from the old v2 settings xml format
    /// </summary>
    public static class Template2xHelper
    {
        static readonly ILog log = LogManager.GetLogger(typeof(Template2xHelper));

        static Dictionary<string, string> shipmentTypeNameMapping = new Dictionary<string, string>
            {
                { "UPS OnLine Tools",  "UPS" },
                { "WorldShip",         "UPS (WorldShip)" },
                { "Endicia DAZzle",    "USPS (Endicia)" },
                { "Stamps.com",        "USPS" },
                { "USPS Download",     "USPS (w/o Postage)" },
                { "Other",             "Custom" }
            };

        /// <summary>
        /// Convert any template content that needs updated to a 3x compatible equivalent.  
        /// </summary>
        public static bool ConvertTemplateContent(XDocument xDocument)
        {
            bool changed = false;

            changed |= FixupGraphicOrderTemplateImages(xDocument);
            changed |= FixupShipmentTypeCodes(xDocument);
            changed |= FixupImgSuresizeTags(xDocument);

            return changed;
        }

        /// <summary>
        /// Templates derived from Graphic Order Template in V2 took advantage of the fact that when using xsl:import, img tags would be relative to the folder doing the importing.
        /// In v3 there are no more folders, so this gets screwed up, and images arent imported properly.  This fixes that.
        /// </summary>
        private static bool FixupGraphicOrderTemplateImages(XDocument xDocument)
        {
            if (!IsDerivedFromGraphicOrderTemplate(xDocument))
            {
                return false;
            }

            SetImageSectionContent(xDocument, "outputPageHeader",         "imgInvoice", "images/pageheader.jpg");
            SetImageSectionContent(xDocument, "outputShipBillHeader",     "imgBillShip", "images/billshipping.jpg");
            SetImageSectionContent(xDocument, "outputOrderDetailsHeader", "imgOrderInfo", "images/orderinfo.jpg");
            SetImageSectionContent(xDocument, "outputOrderTotalsHeader",  "imgOrderTotals", "images/ordertotals.jpg");

            return true;
        }

        /// <summary>
        /// Some of the text-based ShipmentTypeCode values changed for 3x, and will break 2x tests
        /// </summary>
        private static bool FixupShipmentTypeCodes(XDocument xDocument)
        {
            bool changed = false;

            foreach (XAttribute attribute in xDocument.Descendants().SelectMany(x => x.Attributes()))
            {
                foreach (var pair in shipmentTypeNameMapping)
                {
                    if (attribute.Value.Contains(pair.Key))
                    {
                        attribute.Value = attribute.Value.Replace(pair.Key, pair.Value);
                        changed = true;
                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// In ShipWorks 2x we allowed id=suresize on imgs.  In 3x it can only be on divs
        /// </summary>
        private static bool FixupImgSuresizeTags(XDocument xDocument)
        {
            bool changed = false;

            foreach (XElement imgTag in xDocument.Descendants("img").ToList())
            {
                XAttribute idAttribute = imgTag.Attribute("id");
                if (idAttribute != null && idAttribute.Value == "suresize")
                {
                    changed = true;

                    idAttribute.Remove();

                    // Needs to be in a div instead
                    imgTag.ReplaceWith(
                        new XElement("div",
                            new XAttribute("id", "suresize"),
                            imgTag));
                }
            }

            return changed;
        }

        /// <summary>
        /// Ensure the given section of the xDocument exists and set it to the specified content.  If it already exists with content, it is left alone
        /// </summary>
        private static void SetImageSectionContent(XDocument xDocument, string sectionName, string imgID, string imgSrc)
        {
            XNamespace xslNamespace = "http://www.w3.org/1999/XSL/Transform";

            XElement templateTag = xDocument.Descendants(xslNamespace + "template").FirstOrDefault(t => (string) t.Attribute("name") == sectionName);
            if (templateTag == null)
            {
                templateTag = new XElement(xslNamespace + "template",
                       new XAttribute("name", sectionName),
                       new XElement("img", new XAttribute("id", imgID), new XAttribute("src", imgSrc)));

                xDocument.Root.Add(templateTag);
                xDocument.Root.Add("\r\n\r\n");
            }
        }

        /// <summary>
        /// Determines if the given template xsl:import's Graphic ORder Template
        /// </summary>
        private static bool IsDerivedFromGraphicOrderTemplate(XDocument xDocument)
        {
            XNamespace xslNamespace = "http://www.w3.org/1999/XSL/Transform";

            foreach (XElement importTag in xDocument.Descendants(xslNamespace + "import").ToList())
            {
                // Not just img.Attribute("href"), so it can be case insensitive.
                XAttribute hrefAttribute = importTag.Attributes().Where(a => a.Name.LocalName.ToLowerInvariant() == "href").FirstOrDefault();
                if ((string) hrefAttribute != null)
                {
                    if (((string) hrefAttribute).EndsWith("Graphic Order Template"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Process the settings from the version2 template
        /// </summary>
        public static void ReadTemplateSettings(TemplateEntity template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            // First apply the defaults, anything we find in the settings section will override
            TemplateHelper.ApplyDefaultSettings(template, TemplateType.Standard, TemplateOutputFormat.Html);

            // It's possible that the template XSL is not a valid XML document (if the user edited\screwed it up), but we still want to try to get the settings out as best we can
            int settingsStart = template.Xsl.IndexOf("<sw:settings>");
            int settingsStop = template.Xsl.IndexOf("</sw:settings>");

            // If we couldn't find the settings, there's nothing more we can do
            if (settingsStart == -1 || settingsStop == -1)
            {
                return;
            }

            // Extract the settings portion
            string settingsXml = template.Xsl.Substring(settingsStart, settingsStop + "</sw:settings>".Length - settingsStart);

            // Next lets remove the sw: tags so we don't have to deal with namespacing and import into an XElement
            try
            {
                XElement settings = XElement.Parse(settingsXml.Replace("sw:", ""));

                template.Type = (int) GetTemplateType((string) settings.XPathSelectElement("general/type"));
                template.Context = (int) GetTemplateContext((string) settings.XPathSelectElement("general/context"));

                template.PageWidth = GetSettingsValue(settings, "page-setup/width", template.PageWidth);
                template.PageHeight = GetSettingsValue(settings, "page-setup/height", template.PageHeight);
                template.PageMarginLeft = GetSettingsValue(settings, "page-setup/margins/left", template.PageMarginLeft);
                template.PageMarginRight = GetSettingsValue(settings, "page-setup/margins/right", template.PageMarginRight);
                template.PageMarginTop = GetSettingsValue(settings, "page-setup/margins/top", template.PageMarginTop);
                template.PageMarginBottom = GetSettingsValue(settings, "page-setup/margins/bottom", template.PageMarginBottom);

                template.LabelSheetID = GetLabelSheetID(settings);

                template.SaveFileName = GetSettingsValue(settings, "saving/filename", template.SaveFileName);
                template.SaveFilePrompt = (int) GetSavePromptSetting(settings);

                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    // Have to go through each store and see if any email settings exist that need imported
                    foreach (StoreEntity store in StoreManager.GetAllStores())
                    {
                        int oldStoreID = MigrationRowKeyTranslator.TranslateKeyToV2(store.StoreID, con);

                        XElement emailSettings = settings.XPathSelectElement(string.Format("email/store[@id={0}]", oldStoreID));
                        if (emailSettings != null)
                        {
                            TemplateStoreSettingsEntity storeSettings = TemplateHelper.GetStoreSettings(template, store.StoreID);
                            LoadEmailSettings(store, storeSettings, emailSettings, con);
                        }
                    }
                }
            }
            catch (XmlException ex)
            {
                log.Error(string.Format("Could not load settings from v2 template '{0}' due to malformed settings section.", template.FullName), ex);
            }

            // This gets the OutputFormat and OutputEncoding set, which are read from actual XSL tags
            try
            {
                TemplateSettingsSynchronizer.UpdateSettingsFromXsl(template);
            }
            catch (XmlException ex)
            {
                log.Error(string.Format("Could not load settings from v2 template '{0}' due to invalid XML.", template.FullName), ex);
            }

            // Remove the settings portion
            string newXsl = template.Xsl.Remove(settingsStart, settingsXml.Length);

            // We also need to run it through the SyntaxEditor document, which normalizes line endings and stuff, so it doesn't look dirty the next time the user just opens it and clicks close in the editor dlg
            Document document = new Document();
            document.AutoConvertTabsToSpaces = true;
            document.Text = newXsl;

            // Set the updated XSL
            template.Xsl = document.Text;
        }

        /// <summary>
        /// Load the email settings from the v2 xml into the v3 object
        /// </summary>
        private static void LoadEmailSettings(StoreEntity store, TemplateStoreSettingsEntity storeSettings, XElement emailSettings, SqlConnection con)
        {
            // There are settings to load, so don't use defaults
            storeSettings.EmailUseDefault = false;

            int oldEmailAccountID = GetSettingsValue(emailSettings, "account", -1);

            // Possible account doesn't exist, if it was deleted and template was never resaved, so have to catch
            if (oldEmailAccountID != -1)
            {
                try
                {
                    // 0 in v2 meant store default
                    if (oldEmailAccountID == 0)
                    {
                        storeSettings.EmailAccountID = store.DefaultEmailAccountID;
                    }
                    else
                    {
                        storeSettings.EmailAccountID = MigrationRowKeyTranslator.TranslateKeyToV3(oldEmailAccountID, MigrationRowKeyType.EmailAccount, con);
                    }
                }
                catch (NotFoundException)
                {
                    log.WarnFormat("Template settings email account {0} does not exist.", oldEmailAccountID);
                }
            }

            storeSettings.EmailSubject = GetSettingsValue(emailSettings, "subject", storeSettings.EmailSubject);
            storeSettings.EmailTo = GetSettingsValue(emailSettings, "to", storeSettings.EmailTo);
            storeSettings.EmailCc = GetSettingsValue(emailSettings, "cc", storeSettings.EmailCc);
            storeSettings.EmailBcc = GetSettingsValue(emailSettings, "bcc", storeSettings.EmailBcc);
        }

        /// <summary>
        /// Determinet he falue to use for the save prompt
        /// </summary>
        private static SavePromptWhen GetSavePromptSetting(XElement settings)
        {
            XElement savePrompt = settings.XPathSelectElement("saving/multiPromptEach");
            if (savePrompt != null)
            {
                if ((bool) savePrompt)
                {
                    return SavePromptWhen.Always;
                }
            }

            return SavePromptWhen.Once;
        }

        /// <summary>
        /// Read the label sheet information from the template settings
        /// </summary>
        private static long GetLabelSheetID(XElement settings)
        {
            string brandName = (string) settings.XPathSelectElement("label-sheet/brand");
            string sheetName = (string) settings.XPathSelectElement("label-sheet/name");

            if (!string.IsNullOrWhiteSpace(brandName) && !string.IsNullOrWhiteSpace(sheetName))
            {
                LabelSheetBrand brand = LabelSheetManager.BuiltinBrands.FirstOrDefault(b => b.Name == brandName);
                if (brand != null)
                {
                    LabelSheetEntity sheet = brand.Sheets.FirstOrDefault(s => s.Name == sheetName);
                    if (sheet != null)
                    {
                        return sheet.LabelSheetID;
                    }
                }
            }

            return LabelSheetManager.DefaultLabelSheetID;
        }

        /// <summary>
        /// Get the value of the element pointed to be the specified selector, defaulting if not present
        /// </summary>
        private static T GetSettingsValue<T>(XElement settings, string selector, T defaultValue)
        {
            XElement element = settings.XPathSelectElement(selector);
            if (element == null)
            {
                return defaultValue;
            }
            else
            {
                return (T) Convert.ChangeType((string) element, typeof(T));
            }
        }

        /// <summary>
        /// Get the TemplateType value for the given v2 type string
        /// </summary>
        private static TemplateType GetTemplateType(string type)
        {
            switch (type)
            {
                case "standard": return TemplateType.Standard;
                case "label": return TemplateType.Label;
                case "report": return TemplateType.Report;
            }

            return TemplateType.Standard;
        }

        /// <summary>
        /// Get the TemplateInputContext value for the given v2 string
        /// </summary>
        private static TemplateInputContext GetTemplateContext(string context)
        {
            switch (context)
            {
                case "customer": return TemplateInputContext.Customer;
                case "order": return TemplateInputContext.Order;
                case "shipment": return TemplateInputContext.Shipment;
                case "package": return TemplateInputContext.Shipment;
            }

            return TemplateInputContext.Auto;
        }
    }
}
