using System;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;

namespace ShipWorks.UI.Dialogs.Popup
{
    /// <summary>
    /// Popup window that displays for a couple of seconds then hides
    /// </summary>
    [Component(Service = typeof(IPopup), RegisterAs = RegistrationType.SpecificService)]
    public partial class Popup : InteropWindow, IPopup
    {

        static volatile object locker = new Object();

        /// <summary>
        /// Constructor
        /// </summary>
        public Popup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Show and hide the window using the storyBoard defined in the xaml
        /// </summary>
        public void ShowAction()
        {
            lock (locker)
            {
                Storyboard storyboard = (Storyboard) FindResource("storyBoard");

                storyboard.Stop();
                storyboard.Seek(TimeSpan.Zero);
                Visibility = Visibility.Visible;

                storyboard.Begin();
            }
        }

        /// <summary>
        /// Sets the ViewModel to the DataContext
        /// </summary>
        public void SetViewModel(IPopupViewModel viewModel)
        {
            DataContext = viewModel;
        }

        /// <summary>
        /// Once the storyboard completes, hide the control
        /// </summary>
        private void OnStoryboardCompleted(object sender, EventArgs e)
        {
            bool locked = false;
            Monitor.TryEnter(locker, 0, ref locked);
            if(!locked)
            {
                Visibility = Visibility.Collapsed;
            }
        }
    }
}
