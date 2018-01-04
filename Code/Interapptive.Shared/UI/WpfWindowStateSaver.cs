using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Window state saver for a WPF window
    /// </summary>
    internal class WpfWindowStateSaver : IWpfWindowStateSaver
    {
        private Window window;
        private readonly WindowStateSaverOptions options;

        /// <summary>
        /// Constructor
        /// </summary>
        public WpfWindowStateSaver(Window window, WindowStateSaverOptions options, GenericResult<WindowState> state, string windowName)
        {
            this.window = window;
            this.options = options;

            State = ApplyState(window, state, windowName);

            // Subscribe to parent window's events
            window.Closing += OnClosing;
            window.SizeChanged += OnResize;
            window.LocationChanged += OnMove;
        }

        /// <summary>
        /// Get the window state
        /// </summary>
        public WindowState State { get; }

        /// <summary>
        /// Load the state of the window from the registry
        /// </summary>
        private WindowState ApplyState(Window window, GenericResult<WindowState> state, string windowName) =>
            state.Match(
                x => RestoreExistingState(window, x),
                ex => CreateState(window, windowName));

        /// <summary>
        /// Create a new state
        /// </summary>
        private WindowState CreateState(Window window, string name)
        {
            // If we don't have an entry for this window, and we are supposed to maximize the size in that scenario, do that now
            if (options.HasFlag(WindowStateSaverOptions.InitialMaximize))
            {
                throw new NotImplementedException("WpfWindowStateSaver does not yet support InitialMaximize");
            }

            return new WindowState
            {
                Name = name,
                BoundsWpf = window.DesktopBounds(),
                FormState = (FormWindowState) window.WindowState,
            };
        }

        /// <summary>
        /// Restore the existing window state
        /// </summary>
        private WindowState RestoreExistingState(Window window, WindowState state)
        {
            var currentBounds = window.DesktopBounds();
            var savedBounds = state.BoundsWpf;

            // If we aren't remembering the position, use the current position
            if (!options.HasFlag(WindowStateSaverOptions.Position))
            {
                savedBounds.X = currentBounds.X;
                savedBounds.Y = currentBounds.Y;
            }
            else
            {
                // Otherwise, we are applying the position, and need to make sure it's not overridden
                window.WindowStartupLocation = WindowStartupLocation.Manual;
            }

            // If we aren't remembering the size, use the current size
            if ((options & WindowStateSaverOptions.Size) == 0)
            {
                savedBounds.Width = currentBounds.Width;
                savedBounds.Height = currentBounds.Height;
            }

            // In case of multi screen desktops, check if we got the screen the window was when closed.
            if (savedBounds.Left > SystemParameters.VirtualScreenWidth ||
                savedBounds.Right < SystemParameters.VirtualScreenLeft ||
                savedBounds.Top > SystemParameters.VirtualScreenHeight ||
                savedBounds.Bottom < SystemParameters.VirtualScreenTop)
            {
                // The window would be off-screen - just get out and let it open where it would be default
                return state;
            }

            // Restore size and state
            window.SetDesktopBounds(savedBounds);
            window.WindowState = (System.Windows.WindowState) state.FormState;

            return state;
        }

        /// <summary>
        /// Saves the Form size
        /// </summary>
        private void OnResize(object sender, System.EventArgs e)
        {
            if (window.WindowState == System.Windows.WindowState.Normal)
            {
                State.BoundsWpf = window.DesktopBounds();
            }
        }

        /// <summary>
        /// Saves the Form position
        /// </summary>
        private void OnMove(object sender, System.EventArgs e)
        {
            if (window.WindowState == System.Windows.WindowState.Normal)
            {
                State.BoundsWpf = window.DesktopBounds();
            }

            // Don't be saved in a minimized state, force it to normal
            State.FormState = (window.WindowState == System.Windows.WindowState.Minimized) ? FormWindowState.Normal : (FormWindowState) window.WindowState;
        }

        /// <summary>
        /// Form is closing, save its position.
        /// </summary>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            window.Closing -= OnClosing;
            window.SizeChanged -= OnResize;
            window.LocationChanged -= OnMove;

            window = null;
        }
    }
}
