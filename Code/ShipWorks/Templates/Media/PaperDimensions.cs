using System;
using System.Collections.Generic;
using System.Text;
using Interapptive.Shared.Utility;
using System.Reflection;

namespace ShipWorks.Templates.Media
{
    /// <summary>
    /// The dimensions of a sheet of paper.
    /// </summary>
    public class PaperDimensions
    {
        // User visible name of the paper details
        string description;

        // Width of the paper
        double width;

        // Height of the paper
        double height;

        // All the builtin page sizes we support
        static PaperDimensions[] commonPapers = new PaperDimensions[]
        {
            new PaperDimensions("Letter (8 ½ x 11 in)", 8.5, 11),
            new PaperDimensions("Letter landscape (11 x 8 ½ in)", 11, 8.5),
            new PaperDimensions("Legal (8 ½ x 14 in)", 8.5, 14),
            new PaperDimensions("Legal landscape (14 x 8 ½ in)", 14, 8.5),
            new PaperDimensions("Mini (4 ¼ x 5 in)", 4.25, 5),
            new PaperDimensions("Vertical half sheet (4 ¼ x 10 in)", 4.25, 10),
            new PaperDimensions("Vertical half sheet landscape (10  x 4 ¼ in)", 10, 4.25),
            new PaperDimensions("A4 (21 x 29.7 cm)", 8.27, 11.7),
            new PaperDimensions("A4 landscape (29.7 x 21 cm)", 11.7, 8.27),
            new PaperDimensions("Custom", 0, 0)
        };

        // Default paper
        static PaperDimensions defaultPaper = commonPapers[0];

        /// <summary>
        /// Constructor.
        /// </summary>
        private PaperDimensions(string description, double width, double height)
        {
            this.description = description;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Gets the array of support paper details
        /// </summary>
        public static PaperDimensions[] SupportedDimensions
        {
            get
            {
                return commonPapers;
            }
        }

        /// <summary>
        /// Gets the default paper dimensions
        /// </summary>
        public static PaperDimensions Default
        {
            get
            {
                return defaultPaper;
            }
        }

        /// <summary>
        /// Returns PaperDetails that has the given description
        /// </summary>
        public static PaperDimensions FromDescription(string description)
        {
            foreach (PaperDimensions details in SupportedDimensions)
            {
                if (details.Description == description)
                {
                    return details;
                }
            }

            throw new NotFoundException(string.Format("PaperDimensions '{0}' is not valid.", description));
        }

        /// <summary>
        /// Returns PaperDetails matching the given dimensions.
        /// </summary>
        public static PaperDimensions FromDimensions(double width, double height)
        {
            foreach (PaperDimensions details in SupportedDimensions)
            {
                if (details.Width == width && details.Height == height)
                {
                    return details;
                }
            }

            return FromDescription("Custom");
        }

        /// <summary>
        /// Description of the paper size, as displayed to user
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Description
        {
            get
            {
                return description;
            }
        }

        /// <summary>
        /// Width of the page
        /// </summary>
        public double Width
        {
            get
            {
                return width;
            }
        }

        /// <summary>
        /// Height of the page
        /// </summary>
        public double Height
        {
            get
            {
                return height;
            }
        }

        /// <summary>
        /// Gets whether this PaperDetails is a custom size
        /// </summary>
        public bool IsCustom
        {
            get
            {
                return description == "Custom";
            }
        }
    }
}
