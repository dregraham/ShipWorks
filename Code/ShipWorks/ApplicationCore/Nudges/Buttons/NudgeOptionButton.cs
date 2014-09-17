using System;
using System.Drawing;
using System.Windows.Forms;
using log4net;

namespace ShipWorks.ApplicationCore.Nudges.Buttons
{
    /// <summary>
    /// An abstract class that should be used for presenting a NudgeOption on a form. This button
    /// will coordinate the logging of the selected option and then allow derived classes a chance
    /// to handle the Click event as needed via the HandleClick method.
    /// </summary>
    public abstract class NudgeOptionButton : Button
    {
        private readonly NudgeOption option;
        private Form hostForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="NudgeOptionButton"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        protected NudgeOptionButton(NudgeOption option)
        {
            this.option = option;

            TextChanged += OnTextChanged;
            Click += OnClick;
        }

        /// <summary>
        /// Gets the NudgeOption that this button is for.
        /// </summary>
        protected NudgeOption Option
        {
            get { return option; }
        }

        /// <summary>
        /// Gets the form that this button is hosted within.
        /// </summary>
        protected Form HostForm
        {
            get { return hostForm ?? (hostForm = FindForm()); }
        }
        /// <summary>
        /// An abstract method allowing derived classes to perform any logic that is needed when the button is clicked. 
        /// </summary>
        public abstract void HandleClick();

        /// <summary>
        /// Called when [text changed] to resize the button so the text displays appropriately.
        /// </summary>
        private void OnTextChanged(object sender, EventArgs e)
        {
            using (Graphics g = CreateGraphics())
            {
                // Make sure the width of the button is sufficient for the text being displayed
                int requiredWidth = (int)(g.MeasureString(Text, Font).Width + 10);
                if (Width < requiredWidth)
                {
                    Width = requiredWidth;
                }
            }
        }

        /// <summary>
        /// Called when the button is clicked. This will log that the button's nudge option 
        /// was selected and then allow derived classes a chance to handle the button click
        /// to perform any specific functionality (closing a dialog, closing ShipWorks, etc.)
        /// </summary>
        private void OnClick(object sender, EventArgs eventArgs)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // Log the that the option was selected and allow derived 
                // classes to handle the event.
                option.Log();
            }
            catch (Exception exception)
            {
                // Just make a note of the exception in the log file. This is more for our own 
                // informational and/or reporting purposes, so it's not critical if it wasn't
                // logged successfully.
                LogManager.GetLogger(GetType()).WarnFormat("Could not log the nudge option for nudge {0}. {1}", option.Owner.NudgeID, exception.Message);
            }

            HandleClick();
        }
    }
}
