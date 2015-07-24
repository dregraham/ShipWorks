using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// ComboBox capable of showing a UI state where multiple values are selected
    /// </summary>
    public class MultiValueComboBox : PromptComboBox
    {
        bool isMultiValued = false;

        // Currently setting MultiValued to true
        bool inSetMultiValue = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public MultiValueComboBox()
        {

        }

        /// <summary>
        /// Get \ set whether the box represents a null value.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool MultiValued
        {
            get
            {
                return isMultiValued;
            }
            set
            {
                if (value == isMultiValued)
                {
                    return;
                }

                isMultiValued = value;

                if (isMultiValued)
                {
                    inSetMultiValue = true;

                    SelectedIndex = -1;

                    if (DropDownStyle == ComboBoxStyle.DropDown)
                    {
                        Text = "";
                    }

                    inSetMultiValue = false;
                }

                Invalidate();
            }
        }

        [Browsable(false)]
        public override string PromptText
        {
            get
            {
                return MultiValueExtensions.MultiText;
            }
            set
            {

            }
        }

        /// <summary>
        /// Controls when the prompt should be drawn - which is when we are multivalued
        /// </summary>
        protected override bool ShouldDrawPrompt()
        {
            return MultiValued;
        }

        /// <summary>
        /// When the text changes, we are no longer null.  We do need this in addition to the handler.  The handler is required
        /// for when the user is typing.  This is required for cases where the underlying Text property is "", and its being set to "", in 
        /// which case the TextChanged handler would not have fired.
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;

                if (isMultiValued && !inSetMultiValue)
                {
                    isMultiValued = false;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// When the index changes, we are no longer null.  We do need this in addition to the handler.  The handler is required
        /// for when the user is manuall changing.  This is required for cases where the underlying SelectedIndexChanged property
        /// is the same, and we wouldn't get the handler - but we always wan't manually setting this to clear the multi-value flag.
        /// </summary>
        public override int SelectedIndex
        {
            get
            {
                return base.SelectedIndex;
            }
            set
            {
                base.SelectedIndex = value;

                if (isMultiValued && !inSetMultiValue)
                {
                    isMultiValued = false;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// When the text changes, we are no longer null.
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (isMultiValued && !inSetMultiValue)
            {
                isMultiValued = false;
                Invalidate();
            }
        }

        /// <summary>
        /// When the selection changes, we are no longer null
        /// </summary>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            if (isMultiValued && !inSetMultiValue)
            {
                isMultiValued = false;
                Invalidate();
            }
        }

        /// <summary>
        /// Focus is entering the control
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            Invalidate();
        }

        /// <summary>
        /// Control is losing focus
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            Invalidate();
        }

        /// <summary>
        /// Bind the combo box to an enum, preserving existing selection
        /// </summary>
        public void BindToEnumAndPreserveSelection<T>(Func<T, bool> includer) where T : struct
        {
            ApplyBindingAndPreserveSelection(() => EnumHelper.BindComboBox(this, includer));
        }

        /// <summary>
        /// Update the data source, preserving existing selection
        /// </summary>
        public void BindDataSourceAndPreserveSelection<T>(IList<T> dataSource)
        {
            ApplyBindingAndPreserveSelection(() => DataSource = dataSource);
        }

        /// <summary>
        /// Apply the specified binding, preserving the existing selection
        /// </summary>
        private void ApplyBindingAndPreserveSelection(Action bindingMethod)
        {
            bool previousMulti = MultiValued;
            object previousValue = SelectedValue;

            bindingMethod();

            // Set back the previous value
            if (previousMulti)
            {
                MultiValued = true;
            }
            else if (previousValue != null)
            {
                SelectedValue = previousValue;
                if (SelectedIndex == -1)
                {
                    SelectedIndex = 0;
                }
            }

        }
    }
}
