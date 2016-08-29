using Interapptive.Shared.IO.Text.HtmlAgilityPack;
using ShipWorks.ApplicationCore;
using System;
using System.IO;
using System.Net;
using System.Text;

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

        public string Blah(string input)
        {
            var zplString = Encoding.UTF8.GetString(Convert.FromBase64String(input));
            byte[] zpl = Encoding.UTF8.GetBytes(zplString);

            // adjust print density (8dpmm), label width (4 inches), label height (6 inches), and label index (0) as necessary
            var request = (HttpWebRequest) WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = zpl.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(zpl, 0, zpl.Length);
            requestStream.Close();

            try
            {
                byte[] imageBytes;
                var response = (HttpWebResponse) request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                using (MemoryStream memStream = new MemoryStream())
                {
                    byte[] buffer = new byte[16*1024];
                    int read;
                    while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memStream.Write(buffer, 0, read);
                    }
                    imageBytes = memStream.ToArray();
                }


                //var reader = new StreamReader(responseStream);
                //var imageString = reader.ReadToEnd();
                //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(imageString);

                //Form prompt = new Form();
                //prompt.Width = 500;
                //prompt.Height = 500;
                //prompt.Text = "test text";

                //PictureBox PictureBox1 = new PictureBox(); 
                //var image = new Bitmap(responseStream);

                //prompt.Controls.Add(PictureBox1);
                //prompt.ShowDialog();

                return "data:image/png;base64," + System.Convert.ToBase64String(imageBytes);

            }
            catch (WebException e)
            {
                Console.WriteLine("Error: {0}", e.Status);
                return null;
            }
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
