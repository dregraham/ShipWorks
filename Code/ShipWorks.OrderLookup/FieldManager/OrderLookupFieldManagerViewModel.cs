using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// View model for the order lookup field manager
    /// </summary>
    [Component]
    public class OrderLookupFieldManagerViewModel : IOrderLookupFieldManager
    {
        private readonly Func<IOrderLookupFieldManager, IOrderLookupFieldManagerDialog> createDialog;
        private readonly IMessageHelper messageHelper;
        private readonly IOrderLookupFieldLayoutRepository fieldLayoutRepository;
        private IOrderLookupFieldManagerDialog dialog;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFieldManagerViewModel(
            Func<IOrderLookupFieldManager, IOrderLookupFieldManagerDialog> createDialog,
            IOrderLookupFieldLayoutRepository fieldLayoutRepository,
            IMessageHelper messageHelper)
        {
            this.fieldLayoutRepository = fieldLayoutRepository;
            this.createDialog = createDialog;
            this.messageHelper = messageHelper;

            Save = new RelayCommand(() => SaveAction());
        }

        /// <summary>
        /// Sections of fields
        /// </summary>
        [Obfuscation]
        public IEnumerable<SectionLayout> Sections { get; private set; }

        /// <summary>
        /// Save the user's changes
        /// </summary>
        [Obfuscation]
        public ICommand Save { get; }

        /// <summary>
        /// Show the field manager dialog
        /// </summary>
        /// <remarks>
        public Unit ShowManager()
        {
            Sections = fieldLayoutRepository.Fetch().ToImmutableArray();

            dialog = createDialog(this);

            if (messageHelper.ShowDialog(dialog) == true)
            {
                fieldLayoutRepository.Save(Sections);
            }

            return Unit.Default;
        }

        /// <summary>
        /// Close the dialog affirmatively
        /// </summary>
        private void SaveAction()
        {
            dialog.DialogResult = true;
            dialog.Close();
        }
    }
}
