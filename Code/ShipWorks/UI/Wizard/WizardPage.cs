using System;
using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.UI.Wizard.Design;

namespace ShipWorks.UI.Wizard
{
    /// <summary>
    /// Represents a single page in a wizard.
    /// </summary>
    [Designer(typeof(WizardPageDesigner))]
    [ToolboxItem(false)]
    public partial class WizardPage : UserControl
    {
        // Title and description of the page
        string title;
        string description;

        // The wizard that we are apart of
        WizardForm wizard;

        // If true, clicking next will require elevation
        bool nextRequiresElevation = false;

        // Tracks if this is the first time the Shown event has been raised
        bool firstTimeShown = true;

        // Event handler for when the user hits back
        [Category("Wizard")]
        public event EventHandler<WizardStepEventArgs> StepBack;

        // Event for when the user hits next
        [Category("Wizard")]
        public event EventHandler<WizardStepEventArgs> StepNext;

        // Event handler for when the page is about to be shown.
        [Category("Wizard")]
        public event EventHandler<WizardSteppingIntoEventArgs> SteppingInto;

        // Event raised every time the page is displayed and fully drawn.
        [Category("Wizard")]
        public event EventHandler<WizardPageShownEventArgs> PageShown;

        // Event handler for when user tries to just close
        [Category("Wizard")]
        public event CancelEventHandler Cancelling;

        /// <summary>
        /// Constructor
        /// </summary>
        public WizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the Wizard control we are apart of
        /// </summary>
        internal protected WizardForm Wizard
        {
            get
            {
                return this.wizard;
            }
            set
            {
                wizard = value;
            }
        }

        /// <summary>
        /// Title of the page
        /// </summary>
        [DefaultValue("")]
        [Category("Wizard")]
        public string Title
        {
            get
            {
                if (title == null)
                {
                    return string.Empty;
                }

                return title;
            }
            set
            {
                if (title == value)
                {
                    return;
                }

                title = value;
            }
        }

        /// <summary>
        /// Title of the page
        /// </summary>
        [DefaultValue("")]
        [Category("Wizard")]
        public string Description
        {
            get
            {
                if (description == null)
                {
                    return string.Empty;
                }

                return description;
            }
            set
            {
                if (description == value)
                {
                    return;
                }

                description = value;
            }
        }

        /// <summary>
        /// Indicates that on a Vista or higher operating system, clicking Next will end up requiring elevation.
        /// </summary>
        [DefaultValue(false)]
        [Category("Wizard")]
        public bool NextRequiresElevation
        {
            get
            {
                return nextRequiresElevation;
            }
            set
            {
                nextRequiresElevation = value;
            }
        }

        /// <summary>
        /// Raise the SteppingBack event.
        /// </summary>
        internal void RaiseSteppingBack(WizardStepEventArgs e)
        {
            if (StepBack != null)
            {
                StepBack(this, e);
            }
        }

        /// <summary>
        /// Raise the SteppingNext event.
        /// </summary>
        internal void RaiseSteppingNext(WizardStepEventArgs e)
        {
            if (StepNext != null)
            {
                StepNext(this, e);
            }
        }

        /// <summary>
        /// Raise the SteppingInto event.
        /// </summary>
        internal void RaiseSteppingInto(WizardSteppingIntoEventArgs e)
        {
            if (SteppingInto != null)
            {
                SteppingInto(this, e);
            }
        }

        /// <summary>
        /// Raise the PageShown event
        /// </summary>
        internal void RaisePageShown()
        {
            if (PageShown != null)
            {
                PageShown(this, new WizardPageShownEventArgs(firstTimeShown));
            }

            firstTimeShown = false;
        }

        /// <summary>
        /// Raise the Cancelling event.
        /// </summary>
        internal void RaiseCancelling(CancelEventArgs e)
        {
            if (Cancelling != null)
            {
                Cancelling(this, e);
            }
        }

    }
}
