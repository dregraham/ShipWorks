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
        private readonly int nudgeID;
        private readonly NudgeType nudgeType;
        private readonly Uri contentUri;
        private readonly SortedList<int, NudgeOption> nudgeOptions;
        private readonly Size contentDimensions;

        /// <summary>
        /// Constructor
        /// </summary>
        public Nudge(int nudgeID, NudgeType nudgeType, Uri contentUri, IEnumerable<NudgeOption> nudgeOptions, Size contentDimensions)
        {
            this.nudgeID = nudgeID;
            this.nudgeType = nudgeType;
            this.contentUri = contentUri;
            this.contentDimensions = contentDimensions;
            
            this.nudgeOptions = new SortedList<int, NudgeOption>();
            nudgeOptions.ToList().ForEach(no => this.nudgeOptions.Add(no.Index, no));
        }

        /// <summary>
        /// Identifier of this Nudge
        /// </summary>
        public int NudgeID
        {
            get { return nudgeID; }
        }

        /// <summary>
        /// Type of Nudge
        /// </summary>
        public NudgeType NudgeType
        {
            get { return nudgeType; }
        }

        /// <summary>
        /// Uri to the content to be displayed.
        /// </summary>
        public Uri ContentUri
        {
            get { return contentUri; }
        }

        /// <summary>
        /// Gets the dimensions that the content should appear within.
        /// </summary>
        public Size ContentDimensions
        {
            get { return contentDimensions; }
        }

        /// <summary>
        /// List of NudgeOptions
        /// </summary>
        public SortedList<int, NudgeOption> NudgeOptions
        {
            get { return nudgeOptions; }
        }

        /// <summary>
        /// Creates buttons needed for this Nudge
        /// </summary>
        /// <returns>Sorted list of buttons</returns>
        public SortedList<int, Button> CreateButtons()
        {
            SortedList<int, Button> buttons = new SortedList<int, Button>();

            foreach (NudgeOption nudgeOption in nudgeOptions.Values)
            {
                Button button = new Button
                {
                    Text = nudgeOption.Text
                };

                button.Click += delegate {
                        nudgeOption.Action.Execute();
                    };

                buttons.Add(nudgeOption.Index, button);
            }

            return buttons;
        }
    }
}
