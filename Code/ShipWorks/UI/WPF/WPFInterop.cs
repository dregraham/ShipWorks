using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using ShipWorks.UI.Wizard;
using WinForms = System.Windows.Forms;

namespace ShipWorks.UI.WPF
{
    /// <summary>
    /// Provides WPF and WinForms button interoperability
    /// </summary>
    public class WPFButtonInterop
    {
        public object AcceptButton { get; set; }

        private IEnumerable<WinForms::Button> winButtons;

        private WinForms::Form form;

        private Thickness acceptThickness = new Thickness(2);

        private Thickness defaultThickness = new Thickness(1);

        private SolidColorBrush acceptBrush = (SolidColorBrush) (new BrushConverter().ConvertFrom("#00a2ed"));

        /// <summary>
        /// Sets the event handlers for providing the AcceptButton
        /// </summary>
        public void HookEvents(WinForms::Form form, ElementHost host)
        {
            var control = host.Child;
            this.form = form;
            control.PreviewKeyDown -= HandleWpfKeyEvent;
            control.PreviewKeyDown += HandleWpfKeyEvent;

            var buttons = FindVisualChildren<Button>(control);
            foreach (var button in buttons)
            {
                button.GotFocus -= GotFocusHandler;
                button.GotFocus += GotFocusHandler;

                button.LostFocus -= LostFocusHandler;
                button.LostFocus += LostFocusHandler;
            }

            winButtons = form.Controls.OfType<WinForms::Button>();
            foreach (var button in winButtons)
            {
                button.GotFocus -= WinGotFocus;
                button.GotFocus += WinGotFocus;
            }
        }

        /// <summary>
        /// Sets the AcceptButton if it is on the Form
        /// </summary>
        public void SetAcceptButton(WinForms::Form form)
        {
            var windowFocused = FindWindowFocused(form);
            if (windowFocused != null)
            {
                AcceptButton = form.AcceptButton;
            }           
        }

        /// <summary>
        /// Finds the visual children of the provided type of the DependecyObject
        /// </summary>
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                var numChildren = VisualTreeHelper.GetChildrenCount(depObj);
                for (int i = 0; i < numChildren; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T) child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// Custom command key processing for hitting tab so that our tab order is correct
        /// </summary>
        private WinForms::Control FindWindowFocused(WinForms::Control control)
        {
            var container = control as WinForms::IContainerControl;
            while (container != null)
            {
                control = container.ActiveControl;
                container = control as WinForms::IContainerControl;
            }
            return control;
        }

        /// <summary>
        /// Invokes the OnClick event of the current AcceptButton
        /// </summary>
        public void InvokeClick()
        {
            var wpfButton = AcceptButton as Button;
            if(wpfButton != null)
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(wpfButton);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            }
            else
            {
                var winButton = AcceptButton as WinForms::Button;
                winButton?.PerformClick();
            }
        }

        /// <summary>
        /// Sets the accept button and button style when a WinForms button gets the focus
        /// </summary>
        private void WinGotFocus(object sender, EventArgs e)
        {
            var button = sender as WinForms::Button;
            button.NotifyDefault(true);
            AcceptButton = button;
        }

        /// <summary>
        /// Invokes the OnClick event for WPF buttons
        /// </summary>
        private void HandleWpfKeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                InvokeClick();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Sets the accept button and button style when a WPF button gets the focus
        /// </summary>
        private void GotFocusHandler(object sender, EventArgs e)
        {
            AcceptButton = sender as Button;           
            SetWPFStyle();
            form.AcceptButton.NotifyDefault(false);
        }

        /// <summary>
        /// Clears the wpf button style when it looses focus
        /// </summary>
        private void LostFocusHandler(object sender, EventArgs e)
        {
            ClearWPFStyle();
        }

        /// <summary>
        /// Clears the wpf button style from the AcceptButton
        /// </summary>
        private void ClearWPFStyle()
        {
            var oldButton = AcceptButton as Button;
            if (oldButton != null)
            {
                oldButton.BorderBrush = System.Windows.SystemColors.ActiveBorderBrush;
                oldButton.BorderThickness = defaultThickness;
            }
        }

        /// <summary>
        /// Sets the wpf button style from the AcceptButton
        /// </summary>
        private void SetWPFStyle()
        {
            var button = AcceptButton as Button;
            if (button != null)
            {
                button.BorderBrush = acceptBrush;
                button.BorderThickness = acceptThickness;
            }
        }      
    }
}
