using System;
using System.ComponentModel;
using System.Threading.Tasks;
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

        // Tracks if this is the first time the Shown event has been raised
        bool firstTimeShown = true;

        // Event handler for when the user hits back
        [Category("Wizard")]
        public event EventHandler<WizardStepEventArgs> StepBack;

        // Event for when the user hits next
        [Category("Wizard")]
        public event EventHandler<WizardStepEventArgs> StepNext;

        /// <summary>
        /// Handle a StepNext async event
        /// </summary>
        public Func<object, WizardStepEventArgs, Task> StepNextAsync;

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
        protected internal WizardForm Wizard { get; set; }

        /// <summary>
        /// Title of the page
        /// </summary>
        [DefaultValue("")]
        [Category("Wizard")]
        public string Title
        {
            get
            {
                return title ?? string.Empty;
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
                return description ?? string.Empty;
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
        public bool NextRequiresElevation { get; set; } = false;

        /// <summary>
        /// Raise the SteppingBack event.
        /// </summary>
        internal void RaiseSteppingBack(WizardStepEventArgs e)
        {
            StepBack?.Invoke(this, e);
        }

        /// <summary>
        /// Raise the SteppingNext event.
        /// </summary>
        internal async Task RaiseSteppingNext(WizardStepEventArgs e)
        {
            if (StepNextAsync != null)
            {
                await StepNextAsync(this, e).ConfigureAwait(true);
            }

            StepNext?.Invoke(this, e);
        }

        /// <summary>
        /// Raise the SteppingInto event.
        /// </summary>
        internal void RaiseSteppingInto(WizardSteppingIntoEventArgs e)
        {
            SteppingInto?.Invoke(this, e);
        }

        /// <summary>
        /// Raise the PageShown event
        /// </summary>
        internal void RaisePageShown()
        {
            PageShown?.Invoke(this, new WizardPageShownEventArgs(firstTimeShown));

            firstTimeShown = false;
        }

        /// <summary>
        /// Raise the Canceling event.
        /// </summary>
        internal void RaiseCancelling(CancelEventArgs e)
        {
            Cancelling?.Invoke(this, e);
        }
    }
}
