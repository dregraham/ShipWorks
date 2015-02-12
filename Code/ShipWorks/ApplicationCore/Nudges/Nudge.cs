using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Nudges.Buttons;

namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// Class that represents information to be displayed to the user and allow the user perform an action.
    /// </summary>
    public class Nudge
    {
        private readonly List<NudgeOption> nudgeOptions;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Nudge"/> class.
        /// </summary>
        public Nudge(int nudgeID, string name, NudgeType nudgeType, Uri contentUri, Size contentDimensions)
        {
            NudgeID = nudgeID;
            Name = name;
            NudgeType = nudgeType;

            ContentUri = contentUri;
            ContentDimensions = contentDimensions;

            // Ensure the list is sorted by the index, so buttons get created in the correct order
            nudgeOptions = new List<NudgeOption>();
        }

        /// <summary>
        /// Identifier of this Nudge
        /// </summary>
        public int NudgeID { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Type of Nudge
        /// </summary>
        public NudgeType NudgeType { get; private set; }

        /// <summary>
        /// Uri to the content to be displayed.
        /// </summary>
        public Uri ContentUri { get; private set; }

        /// <summary>
        /// Gets the dimensions that the content should appear within.
        /// </summary>
        public Size ContentDimensions { get; private set; }

        /// <summary>
        /// List of NudgeOptions ordered by the NudgeOption.Index value.
        /// </summary>
        public IEnumerable<NudgeOption> NudgeOptions
        {
            get
            {
                return nudgeOptions.OrderBy(option => option.Index);
            }
        }

        /// <summary>
        /// Adds the given nudge option to the collection of options for this nudge.
        /// </summary>
        public void AddNudgeOption(NudgeOption nudgeOption)
        {
            nudgeOptions.Add(nudgeOption);
        }

        /// <summary>
        /// Creates buttons from the nudge options. The order of the buttons will be based 
        /// on the index of each nudge option.
        /// </summary>
        public List<NudgeOptionButton> CreateButtons()
        {
            List<NudgeOptionButton> buttons = NudgeOptions.Select(option => option.CreateButton()).ToList();

            // Make all the buttons the same width as the widest button
            int maxWidth = buttons.Max(b => b.Width);
            foreach (NudgeOptionButton button in buttons)
            {
                button.Width = maxWidth;
            }

            return buttons;
        }
    }
}
