using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Control for editing weight
    /// </summary>
    public class KeyboardShortcutDisplay : Control
    {
        public static readonly DependencyProperty ShortcutCommandProperty =
            DependencyProperty.Register("ShortcutCommand", typeof(KeyboardShortcutCommand), typeof(KeyboardShortcutDisplay));

        [Obfuscation(Exclude = true)]
        public static readonly DependencyProperty ShortcutTextProperty =
            DependencyProperty.Register("ShortcutText", typeof(string), typeof(KeyboardShortcutDisplay));

        /// <summary>
        /// Constructor
        /// </summary>
        static KeyboardShortcutDisplay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyboardShortcutDisplay), new FrameworkPropertyMetadata(typeof(KeyboardShortcutDisplay)));
        }

        /// <summary>
        /// Command for which to show shortcuts
        /// </summary>
        [Bindable(true)]
        [Obfuscation(Exclude = true)]
        public KeyboardShortcutCommand ShortcutCommand
        {
            get { return (KeyboardShortcutCommand) GetValue(ShortcutCommandProperty); }
            set { SetValue(ShortcutCommandProperty, value); }
        }

        /// <summary>
        /// Apply the template
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (DesignModeDetector.IsDesignerHosted())
            {
                SetCurrentValue(ShortcutTextProperty, "Ctrl+Shift+W");
                return;
            }

            ShortcutEntity shortcut = IoC.BeginLifetimeScope().Resolve<IShortcutManager>().GetWeighShortcut();
            
            SetCurrentValue(ShortcutTextProperty, new KeyboardShortcutData(shortcut).ShortcutText);
        }
    }
}
