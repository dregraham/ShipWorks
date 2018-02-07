using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// Edit a list of string values
    /// </summary>
    [Component]
    public class StringValueListEditorViewModel : IStringValueListEditorViewModel, INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        private readonly Func<IStringValueListEditorViewModel, IStringValueListEditorDialog> createDialog;
        private readonly IMessageHelper messageHelper;
        private IStringValueListEditorDialog dialog;

        /// <summary>
        /// Constructor
        /// </summary>
        public StringValueListEditorViewModel(
            IMessageHelper messageHelper,
            Func<IStringValueListEditorViewModel, IStringValueListEditorDialog> createDialog)
        {
            this.messageHelper = messageHelper;
            this.createDialog = createDialog;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            ConfirmEdit = new RelayCommand(() => ConfirmEditAction());
            CancelEdit = new RelayCommand(() => CancelEditAction());
        }

        /// <summary>
        /// A property value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Confirm the list edit
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ConfirmEdit { get; }

        /// <summary>
        /// Cancel the list edit
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CancelEdit { get; }

        /// <summary>
        /// List of items separated by new lines
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Items { get; set; }

        /// <summary>
        /// Edit the given list
        /// </summary>
        public GenericResult<IEnumerable<string>> EditList(IWin32Window owner, IEnumerable<string> values)
        {
            Items = values.Combine(Environment.NewLine);

            dialog = createDialog(this);
            dialog.LoadOwner(owner);

            if (dialog.ShowDialog() == true)
            {
                return Items.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }

            return GenericResult.FromError<IEnumerable<string>>("canceled");
        }

        /// <summary>
        /// Confirm edit
        /// </summary>
        private void ConfirmEditAction()
        {
            dialog.DialogResult = true;
            dialog.Close();
        }

        /// <summary>
        /// Cancel edit
        /// </summary>
        private void CancelEditAction()
        {
            dialog.Close();
        }
    }
}
