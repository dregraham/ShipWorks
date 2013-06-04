using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandRibbon;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace ShipWorks.UI.Controls.SandRibbon
{
    /// <summary>
    /// A label with an image that is animatable
    /// </summary>
    public class ImageLabel : ControlContainerBase
    {
        /// <summary>
        /// Raised when the control is double-clicked
        /// </summary>
        public event EventHandler DoubleClick;

        /// <summary>
        /// Raised when the control is clicked
        /// </summary>
        public event EventHandler Click;

        // The chosen cursor to use
        Cursor cursor = Cursors.Default;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageLabel()
            : base(new PictureBox())
        {

        }

        /// <summary>
        /// The PictureBox control that is contained by this control
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PictureBox PictureBox
        {
            get
            {
                return Control as PictureBox;
            }
        }

        /// <summary>
        /// The cursor to use
        /// </summary>
        [Category("Appearance")]
        public Cursor Cursor
        {
            get { return cursor; }
            set { cursor = value; }
        }

        /// <summary>
        /// The mouse has lifted from the control
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (base.BoundsWithoutPadding.Contains(e.X, e.Y))
            {
                if (Click != null)
                {
                    Click(this, EventArgs.Empty);
                }
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// The control has been double clicked
        /// </summary>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (DoubleClick != null)
            {
                DoubleClick(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Set the user's desired cursor
        /// </summary>
        protected override void OnSetCursor(Point position)
        {
            if (base.BoundsWithoutPadding.Contains(position))
            {
                Cursor.Current = cursor;
            }
            else
            {
                base.OnSetCursor(position);
            }
        }
    }
}
