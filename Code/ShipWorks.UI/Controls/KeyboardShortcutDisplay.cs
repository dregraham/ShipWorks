using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using Interapptive.Shared.Collections;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Shared.IO.KeyboardShortcuts;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Control for editing weight
    /// </summary>
    public class KeyboardShortcutDisplay : Control
    {
        public static readonly DependencyProperty ShortcutCommandProperty =
            DependencyProperty.Register("ShortcutCommand", typeof(KeyboardShortcutCommand), typeof(KeyboardShortcutDisplay));

        public static readonly DependencyProperty DefaultShortcutProperty =
            DependencyProperty.Register("DefaultShortcut", typeof(string), typeof(KeyboardShortcutDisplay));

        public static readonly DependencyProperty AllShortcutsProperty =
            DependencyProperty.Register("AllShortcuts", typeof(IEnumerable<string>), typeof(KeyboardShortcutDisplay));

        public static readonly DependencyProperty HasShortcutsProperty =
            DependencyProperty.Register("HasShortcuts", typeof(bool), typeof(KeyboardShortcutDisplay));

        public static readonly DependencyProperty HasAdditionalShortcutsProperty =
            DependencyProperty.Register("HasAdditionalShortcuts", typeof(bool), typeof(KeyboardShortcutDisplay));

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

            var translator = IoC.BeginLifetimeScope().Resolve<IKeyboardShortcutTranslator>();
            IEnumerable<string> shortcuts = translator.GetShortcuts(ShortcutCommand);

            SetCurrentValue(DefaultShortcutProperty, shortcuts.FirstOrDefault());
            SetCurrentValue(HasShortcutsProperty, shortcuts.Any());
            SetCurrentValue(AllShortcutsProperty, shortcuts);
            SetCurrentValue(HasAdditionalShortcutsProperty, shortcuts.IsCountGreaterThan(1));
        }
    }
}
