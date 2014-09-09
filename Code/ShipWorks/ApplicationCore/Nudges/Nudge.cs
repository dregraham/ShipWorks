using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// Class that represents information to be displayed to the user and allow the user perform an action.
    /// </summary>
    public class Nudge
    {
        private readonly List<NudgeOption> nudgeOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Nudge"/> class without any nudge options. (They can
        /// be added later via the NudgeOptions property.
        /// </summary>
        public Nudge(int nudgeID, NudgeType nudgeType, Uri contentUri, Size contentDimensions)
            : this(nudgeID, nudgeType, contentUri, contentDimensions, new List<NudgeOption>())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nudge"/> class.
        /// </summary>
        public Nudge(int nudgeID, NudgeType nudgeType, Uri contentUri, Size contentDimensions, IEnumerable<NudgeOption> nudgeOptions)
        {
            NudgeID = nudgeID;
            NudgeType = nudgeType;

            ContentUri = contentUri;
            ContentDimensions = contentDimensions;

            // Ensure the list is sorted by the index, so buttons get created in the correct order
            this.nudgeOptions = nudgeOptions.OrderBy(option => option.Index).ToList();
        }

        /// <summary>
        /// Identifier of this Nudge
        /// </summary>
        public int NudgeID { get; private set; }

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
        /// List of NudgeOptions
        /// </summary>
        public IEnumerable<NudgeOption> NudgeOptions
        {
            get
            {
                return nudgeOptions;
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
        public List<Button> CreateButtons()
        {
            List<Button> buttons = new List<Button>();

            // Use for loop instead of foreach to avoid the warning of "Access to foreach variable in closure. May 
            // have different behaviour when compiled with different versions of compiler." when setting the click
            // event handler.
            for (int i = 0; i < nudgeOptions.Count; i++)
            {
                NudgeOption option = nudgeOptions[i];

                Button button = new Button
                {
                    Text = option.Text
                };

                button.Click += delegate
                {
                    option.Action.Execute(option);
                };

                using (Graphics g = button.CreateGraphics())
                {
                    // Make sure the width of the button is sufficient for the text being displayed
                    button.Width = (int)(g.MeasureString(button.Text, button.Font).Width + 10);
                }

                buttons.Add(button);
            }

            // Make all the buttons the same width as the widest button
            int maxWidth = buttons.Max(b => b.Width);
            foreach (Button button in buttons)
            {
                button.Width = maxWidth;
            }

            return buttons;
        }
    }
}
