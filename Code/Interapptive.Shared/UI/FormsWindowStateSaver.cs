using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Window state saver for a Windows Forms object
    /// </summary>
    internal class FormsWindowStateSaver : IFormsWindowStateSaver
    {
        private Form form;
        private readonly WindowStateSaverOptions options;
        private Dictionary<string, SplitContainer> managedSplitters;

        /// <summary>
        /// Constructor
        /// </summary>
        public FormsWindowStateSaver(Form form, WindowStateSaverOptions options, GenericResult<WindowState> state, string name)
        {
            this.form = form;
            this.options = options;

            State = ApplyState(state, name);

            // Subscribe to parent form's events
            form.Closing += OnClosing;
            form.Resize += OnResize;
            form.Move += OnMove;
        }

        /// <summary>
        /// Current state
        /// </summary>
        public WindowState State { get; }

        /// <summary>
        /// Load the state of the window from the registry
        /// </summary>
        private WindowState ApplyState(GenericResult<WindowState> state, string windowName)
        {
            Rectangle currentScreenBounds = Screen.GetBounds(new Point(form.Left, form.Top));

            return state
                .Match(
                    x => RestoreState(x),
                    ex => CreateState(currentScreenBounds, windowName));
        }

        /// <summary>
        /// Restore the state
        /// </summary>
        private WindowState RestoreState(WindowState state)
        {
            Rectangle currentBounds = form.DesktopBounds;
            Rectangle savedBounds = state.Bounds;

            // If we aren't remembering the position, use the current position
            if ((options & WindowStateSaverOptions.Position) == 0)
            {
                savedBounds.X = currentBounds.X;
                savedBounds.Y = currentBounds.Y;
            }
            else
            {
                // Otherwise, we are applying the position, and need to make sure it's not overridden
                form.StartPosition = FormStartPosition.Manual;
            }

            // If we aren't remembering the size, use the current size
            if ((options & WindowStateSaverOptions.Size) == 0)
            {
                savedBounds.Width = currentBounds.Width;
                savedBounds.Height = currentBounds.Height;
            }

            // In case of multi screen desktops, check if we got the screen the form was when closed.
            Rectangle targetScreenBounds = Screen.GetBounds(new Point(savedBounds.Left, savedBounds.Top));

            if (savedBounds.Left > targetScreenBounds.Right ||
                savedBounds.Right < targetScreenBounds.Left ||
                savedBounds.Top > targetScreenBounds.Bottom ||
                savedBounds.Bottom < targetScreenBounds.Top)
            {
                // The form would be off-screen - just get out and let it open where it would be default
                return state;
            }

            // Restore size and state
            form.DesktopBounds = savedBounds;
            form.WindowState = state.FormState;

            return state;
        }

        /// <summary>
        /// Create a new window state
        /// </summary>
        private WindowState CreateState(Rectangle currentScreenBounds, string windowName)
        {
            // If we don't have an entry for this window, and we are supposed to maximize the size in that scenario, do that now
            if (options.HasFlag(WindowStateSaverOptions.InitialMaximize))
            {
                return CreateMaximizedState(currentScreenBounds, windowName);
            }

            return new WindowState
            {
                Name = windowName,
                Bounds = form.DesktopBounds,
                FormState = form.WindowState
            };
        }

        /// <summary>
        /// Create a maximized state
        /// </summary>
        private static WindowState CreateMaximizedState(Rectangle currentScreenBounds, string windowName)
        {
            Size maxSize = new Size(1210, 1000);

            // Go up to this far from the edges of the screen
            int deflate = 140;

            Rectangle bounds = currentScreenBounds;
            bounds.X += (deflate / 2);
            bounds.Y += (deflate / 2);
            bounds.Width -= deflate;
            bounds.Height -= deflate;

            if (bounds.Width > maxSize.Width)
            {
                int smallerX = bounds.Width - maxSize.Width;

                bounds.X += smallerX / 2;
                bounds.Width -= smallerX;
            }

            if (bounds.Height > maxSize.Height)
            {
                int smallerY = bounds.Height - maxSize.Height;

                bounds.Y += smallerY / 2;
                bounds.Height -= smallerY;
            }

            return new WindowState()
            {
                Name = windowName,
                Bounds = bounds,
                FormState = FormWindowState.Normal
            };
        }

        /// <summary>
        /// Adds the SplitContainer to the elements being remembered
        /// </summary>
        public IFormsWindowStateSaver ManageSplitter(SplitContainer splitContainer) =>
            ManageSplitter(splitContainer, "Splitter");

        /// <summary>
        /// Adds the SplitContainer to the elements being remembered
        /// </summary>
        public IFormsWindowStateSaver ManageSplitter(SplitContainer splitContainer, string name)
        {
            MethodConditions.EnsureArgumentIsNotNull(splitContainer, nameof(splitContainer));

            if (managedSplitters == null)
            {
                managedSplitters = new Dictionary<string, SplitContainer>();
            }

            managedSplitters.Add(name, splitContainer);

            if (State.SplitterDistances.TryGetValue(name, out int distance))
            {
                splitContainer.SplitterDistance = distance;
            }

            return this;
        }

        /// <summary>
        /// Saves the Form size
        /// </summary>
        private void OnResize(object sender, System.EventArgs e)
        {
            if (form.WindowState == FormWindowState.Normal)
            {
                State.Bounds = form.DesktopBounds;
            }
        }

        /// <summary>
        /// Saves the Form position
        /// </summary>
        private void OnMove(object sender, System.EventArgs e)
        {
            if (form.WindowState == FormWindowState.Normal)
            {
                State.Bounds = form.DesktopBounds;
            }

            // Don't be saved in a minimized state, force it to normal
            State.FormState = (form.WindowState == FormWindowState.Minimized) ? FormWindowState.Normal : form.WindowState;
        }

        /// <summary>
        /// Form is closing, save its position information.
        /// </summary>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (managedSplitters != null)
            {
                foreach (KeyValuePair<string, SplitContainer> pair in managedSplitters)
                {
                    State.SplitterDistances[pair.Key] = pair.Value.SplitterDistance;
                }
            }

            form.Closing -= new CancelEventHandler(OnClosing);
            form.Resize -= new EventHandler(OnResize);
            form.Move -= new EventHandler(OnMove);

            form = null;
            managedSplitters = null;
        }
    }
}