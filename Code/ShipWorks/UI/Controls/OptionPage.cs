using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using ShipWorks.UI.Controls.Design;
using ShipWorks.UI.Utility;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Represents a single page in an OptionControl
    /// </summary>
    [DesignTimeVisible(false)]
    [ToolboxItem(false)]
    [Designer(typeof(OptionPageDesigner))]
    public class OptionPage : Panel
    {
        ThemedBorderProvider themedBorder;

        #region ChildControlCollection

        /// <summary>
        /// Custom ControlCollection
        /// </summary>
        class ChildControlCollection : Control.ControlCollection
        {
            public ChildControlCollection(OptionPage owner)
                : base(owner)
            {

            }

            public override void Add(Control value)
            {
                if (value is OptionPage)
                {
                    throw new ArgumentException("Cannot add an OptionPage as a child of another OptionPage");
                }

                base.Add(value);
            }
        }

        #endregion

        bool leaveFired;
        bool enterFired;

        /// <summary>
        /// Constructor
        /// </summary>
        public OptionPage()
        {
            this.Text = null;

            themedBorder = new ThemedBorderProvider(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OptionPage(string text)
            : this()
        {
            this.Text = text;
        }

        /// <summary>
        /// Returns a string containing the value of the Text property.
        /// </summary>
        public override string ToString()
        {
            return this.Text;
        }

        /// <summary>
        /// Use our own custom control collection
        /// </summary>
        protected override Control.ControlCollection CreateControlsInstance()
        {
            return new ChildControlCollection(this);
        }

        /// <summary>
        /// The name of the OptionPage as displayed in the OptionControl
        /// </summary>
        [Localizable(true), EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;

                UpdateParent();
            }
        }

        /// <summary>
        /// Raises the ParentChanged event
        /// </summary>
        protected override void OnParentChanged(EventArgs e)
        {
            if ((Parent != null) && !(Parent is OptionControl))
            {
                throw new ArgumentException(string.Format("OptionPage can only be added to OptionControl.  Not {0}", Parent.GetType().FullName));
            }

            base.OnParentChanged(e);
        }

        /// <summary>
        /// Constrain location
        /// </summary>
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if ((Parent is OptionControl) && Parent.IsHandleCreated)
            {
                Rectangle displayRectangle = Parent.DisplayRectangle;

                base.SetBoundsCore(
                    displayRectangle.X, 
                    displayRectangle.Y, 
                    displayRectangle.Width, 
                    displayRectangle.Height, (
                    specified == BoundsSpecified.None) ? BoundsSpecified.None : BoundsSpecified.All);
            }
            else
            {
                base.SetBoundsCore(x, y, width, height, specified);
            }
        }

        /// <summary>
        /// Update the parent with the latest property changes from this page
        /// </summary>
        private void UpdateParent()
        {
            OptionControl optionControl = Parent as OptionControl;
            if (optionControl != null)
            {
                optionControl.UpdatePage(this);
            }
        } 

        /// <summary>
        /// Used by the OptionControl. Not intended to be called directly.
        /// </summary>
        internal void FireEnter(EventArgs e)
        {
            enterFired = true;
            OnEnter(e);
        }

        /// <summary>
        /// Used by the OptionControl. Not intended to be called directly.
        /// </summary>
        internal void FireLeave(EventArgs e)
        {
            this.leaveFired = true;
            this.OnLeave(e);
        }

        /// <summary>
        /// Raises the Enter event
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            if (Parent is OptionControl)
            {
                if (enterFired)
                {
                    base.OnEnter(e);
                }
                enterFired = false;
            }
        }

        /// <summary>
        /// Raises the leave event
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            if (Parent is OptionControl)
            {
                if (leaveFired)
                {
                    base.OnLeave(e);
                }

                leaveFired = false;
            }
        }

        #region Events Hidden From Designer

        // Events
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler AutoSizeChanged
        {
            add
            {
                base.AutoSizeChanged += value;
            }
            remove
            {
                base.AutoSizeChanged -= value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler DockChanged
        {
            add
            {
                base.DockChanged += value;
            }
            remove
            {
                base.DockChanged -= value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler EnabledChanged
        {
            add
            {
                base.EnabledChanged += value;
            }
            remove
            {
                base.EnabledChanged -= value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler LocationChanged
        {
            add
            {
                base.LocationChanged += value;
            }
            remove
            {
                base.LocationChanged -= value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler TabIndexChanged
        {
            add
            {
                base.TabIndexChanged += value;
            }
            remove
            {
                base.TabIndexChanged -= value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler TabStopChanged
        {
            add
            {
                base.TabStopChanged += value;
            }
            remove
            {
                base.TabStopChanged -= value;
            }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public new event EventHandler TextChanged
        {
            add
            {
                base.TextChanged += value;
            }
            remove
            {
                base.TextChanged -= value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new event EventHandler VisibleChanged
        {
            add
            {
                base.VisibleChanged += value;
            }
            remove
            {
                base.VisibleChanged -= value;
            }
        }

        #endregion

        #region Properties Hidden From Designer

        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool ShouldSerializeLocation()
        {
            if (base.Left == 0)
            {
                return (base.Top != 0);
            }

            return true;
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override AnchorStyles Anchor
        {
            get
            {
                return base.Anchor;
            }
            set
            {
                base.Anchor = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Localizable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override AutoSizeMode AutoSizeMode
        {
            get
            {
                return AutoSizeMode.GrowOnly;
            }
            set
            {
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                base.Dock = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Point Location
        {
            get
            {
                return base.Location;
            }
            set
            {
                base.Location = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(typeof(Size), "0, 0")]
        public override Size MaximumSize
        {
            get
            {
                return base.MaximumSize;
            }
            set
            {
                base.MaximumSize = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Size MinimumSize
        {
            get
            {
                return base.MinimumSize;
            }
            set
            {
                base.MinimumSize = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size PreferredSize
        {
            get
            {
                return base.PreferredSize;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new int TabIndex
        {
            get
            {
                return base.TabIndex;
            }
            set
            {
                base.TabIndex = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool TabStop
        {
            get
            {
                return base.TabStop;
            }
            set
            {
                base.TabStop = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }

        #endregion
    }
}
