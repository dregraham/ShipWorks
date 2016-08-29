using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.IO.Text.HtmlAgilityPack;
using System.IO;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Helper class for processing images in an html document
    /// </summary>
    public class TemplateHtmlImageProcessor
    {
        bool localImages = true;
        bool onlineImages = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateHtmlImageProcessor()
        {

        }

        /// <summary>
        /// Local disk images should be processed
        /// </summary>
        public bool LocalImages
        {
            get { return localImages; }
            set { localImages = value; }
        }

        /// <summary>
        /// Online images should be processed
        /// </summary>
        public bool OnlineImages
        {
            get { return onlineImages; }
            set { onlineImages = value; }
        }

        /// <summary>
        /// Process the given html for images.  The resultant html is returned.
        /// </summary>
        public string Process(string html, Action<HtmlAttribute, Uri, string> imageHandler)
        {
            HtmlAgilityDocument agilityDoc = new HtmlAgilityDocument();
            agilityDoc.LoadHtml(html);

            // First remove the base tag that references our resource path.  We won't need it anymore since we are localizing the images.
            HtmlNode baseNode = agilityDoc.DocumentNode.SelectSingleNode("//base");
            if (baseNode != null)
            {
                baseNode.ParentNode.RemoveChild(baseNode);
            }

            // Find all images, we'll need to copy them to a local folder
            foreach (HtmlNode imgNode in agilityDoc.DocumentNode.SelectNodes("//img"))
            {
                HtmlAttribute srcAttribute = imgNode.Attributes["src"];
                Uri srcUri;

                if (srcAttribute != null && srcAttribute.Value.Trim().Length > 0 && Uri.TryCreate(srcAttribute.Value, UriKind.RelativeOrAbsolute, out srcUri))
                {
                    // We assume all relative references are local, since templates live on disk
                    if (!srcUri.IsAbsoluteUri)
                    {
                        srcUri = new Uri(Path.Combine(DataPath.CurrentResources, srcAttribute.Value));
                    }

                    string imageName = Path.GetFileName(srcUri.LocalPath);

                    // But if there is an importedFrom, we'll use the original filename from which the template got the file.
                    HtmlAttribute importedFromAtt = imgNode.Attributes["importedFrom"];
                    if (importedFromAtt != null && importedFromAtt.Value.Trim().Length > 0)
                    {
                        imageName = importedFromAtt.Value;

                        // Remove our custom stuff from the output
                        imgNode.Attributes.Remove("importedFrom");
                        imgNode.Attributes.Remove("shipworksid");
                    }

                    // See if it should be processed
                    if ( (localImages && srcUri.Scheme == Uri.UriSchemeFile && File.Exists(srcUri.LocalPath)) ||
                         (onlineImages && srcUri.Scheme != Uri.UriSchemeFile) )
                    {
                        imageHandler(srcAttribute, srcUri, imageName);
                    }
                }
            }

            return agilityDoc.DocumentNode.OuterHtml;
        }
    }
}
