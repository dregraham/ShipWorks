using System;
using System.Drawing;
using System.IO;
using ShipWorks.ApplicationCore;
using System.Drawing.Imaging;
using ShipWorks.Shipping;
using System.Windows.Forms;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for an individual Label element
    /// </summary>
    public class LabelStandardOutline : ElementOutline
    {
        Orientation orientation;
        ImageFormat imageFormat;

        TemplateLabelData labelData;
        LabelImageInfo imageInfo;

        class LabelImageInfo
        {
            public int Width { get; set; }
            public int Height { get; set; }

            public float HorizontalResolution { get; set; }
            public float VerticalResolution { get; set; }

            public string Filename { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelStandardOutline(TemplateTranslationContext context, ImageFormat imageFormat)
            : base(context)
        {
            this.imageFormat = imageFormat;

            AddAttribute("name", () => labelData.Name);
            AddAttribute("orientation", () => (orientation == Orientation.Vertical) ? "tall" : "wide");
            AddAttribute("width", () => ImageInfo.Width);
            AddAttribute("height", () => ImageInfo.Height);
            AddAttribute("widthInches", () => string.Format("{0:0.##}", ImageInfo.Width / ImageInfo.HorizontalResolution));
            AddAttribute("heightInches", () => string.Format("{0:0.##}", ImageInfo.Height / ImageInfo.VerticalResolution));

            AddTextContent(() => ImageInfo.Filename);
        }        
        
        /// <summary>
        /// Get information on the image details of the label
        /// </summary>
        private LabelImageInfo ImageInfo
        {
            get
            {
                if (imageInfo == null)
                {
                    Image labelImage;
                    var resource = labelData.Resource;

                    // Start out assuming the label as-is is the orientation we need
                    string labelPath = resource.GetAlternateFilename(TemplateLabelUtility.GetFileExtension(imageFormat));
                    try
                    {
                        try
                        {
                            labelImage = TemplateLabelUtility.LoadImageFromDisk(Path.Combine(DataPath.CurrentResources,
                                labelPath));
                        }
                        catch (OutOfMemoryException)
                        {
                            // The resouce file is probably corrupt, so try regenerating the alternate file
                            // and try to load again. If it fails again, just let it bubble up as this is a 
                            // legitimate exception
                            resource.RegenerateAlternateFile(TemplateLabelUtility.GetFileExtension(imageFormat));
                            labelImage =
                                TemplateLabelUtility.LoadImageFromDisk(Path.Combine(DataPath.CurrentResources,
                                    labelPath));
                        }

                        // We need one orientation, but the original label is the other
                        if ((orientation == Orientation.Horizontal && labelImage.Height > labelImage.Width) ||
                            (orientation == Orientation.Vertical && labelImage.Width > labelImage.Height))
                        {
                            labelPath = TemplateLabelUtility.GenerateRotatedLabel(RotateFlipType.Rotate90FlipNone,
                                labelPath);

                            // We don't need the tall image, just the wide
                            labelImage.Dispose();
                            labelImage =
                                TemplateLabelUtility.LoadImageFromDisk(Path.Combine(DataPath.CurrentResources,
                                    labelPath));
                        }
                    }
                    catch (OutOfMemoryException ex)
                    {
                        throw new TemplateException(
                            "ShipWorks was unable to read the label data. The label is likely invalid. Please void this label and create a new one. " +
                            $"{Environment.NewLine} {Environment.NewLine}If the problem persists please make sure your comptuer has sufficient memory.",
                            ex);
                    }

                    imageInfo = new LabelImageInfo();
                    imageInfo.Width = labelImage.Width;
                    imageInfo.Height = labelImage.Height;
                    imageInfo.VerticalResolution = labelImage.VerticalResolution;
                    imageInfo.HorizontalResolution = labelImage.HorizontalResolution;
                    imageInfo.Filename = labelPath;

                    // Very important to cleanup
                    labelImage.Dispose();
                }

                return imageInfo;
            }
        }

        /// <summary>
        /// Clone a new instance of the outline bound to the given data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            var tuple = (Tuple<TemplateLabelData, Orientation>) data;

            return new LabelStandardOutline(Context, imageFormat) { labelData = tuple.Item1, orientation = tuple.Item2 };
        }
    }
}
