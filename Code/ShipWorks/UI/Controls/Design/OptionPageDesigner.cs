using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Windows.Forms.Design.Behavior;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShipWorks.UI.Controls.Design
{
    /// <summary>
    /// Designer for the OptionPage
    /// </summary>
    public class OptionPageDesigner : ScrollableControlDesigner
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OptionPageDesigner()
        {
            AutoResizeHandles = true;
        }

        /// <summary>
        /// Controls what the designed component can have as a parent.
        /// </summary>
        public override bool CanBeParentedTo(IDesigner parentDesigner)
        {
            return ((parentDesigner != null) && (parentDesigner.Component is OptionControl));
        }

        /// <summary>
        /// Get the glyph
        /// </summary>
        protected override ControlBodyGlyph GetControlGlyph(GlyphSelectionType selectionType)
        {
            this.OnSetCursor();
            return new ControlBodyGlyph(Rectangle.Empty, Cursor.Current, this.Control, this);
        }

        /// <summary>
        /// Provided so the OptionControlDesigner can call
        /// </summary>
        internal void OnDragDropInternal(DragEventArgs de)
        {
            this.OnDragDrop(de);
        }

        /// <summary>
        /// Provided so the OptionControlDesigner can call
        /// </summary>
        internal void OnDragEnterInternal(DragEventArgs de)
        {
            this.OnDragEnter(de);
        }

        /// <summary>
        /// Provided so the OptionControlDesigner can call
        /// </summary>
        internal void OnDragLeaveInternal(EventArgs e)
        {
            this.OnDragLeave(e);
        }

        /// <summary>
        /// Provided so the OptionControlDesigner can call
        /// </summary>
        internal void OnDragOverInternal(DragEventArgs e)
        {
            this.OnDragOver(e);
        }

        /// <summary>
        /// Provided so the OptionControlDesigner can call
        /// </summary>
        internal void OnGiveFeedbackInternal(GiveFeedbackEventArgs e)
        {
            this.OnGiveFeedback(e);
        }

        /// <summary>
        /// Override the selection rulws
        /// </summary>
        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = base.SelectionRules;
                if (Control.Parent is OptionControl)
                {
                    selectionRules &= ~SelectionRules.AllSizeable;
                    selectionRules |= SelectionRules.Locked;
                }

                return selectionRules;
            }
        }

        /// <summary>
        /// Paint the adorning border
        /// </summary>
        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            Panel component = (Panel) base.Component;
            if (component.BorderStyle == BorderStyle.None)
            {
                DrawBorder(pe.Graphics);
            }

            base.OnPaintAdornments(pe);
        }

        /// <summary>
        /// Draw a border
        /// </summary>
        protected virtual void DrawBorder(Graphics graphics)
        {
            Panel component = (Panel) base.Component;

            if ((component != null) && component.Visible)
            {
                using (Pen borderPen = CreateBorderPen())
                {
                    Rectangle clientRectangle = this.Control.ClientRectangle;

                    clientRectangle.Width--;
                    clientRectangle.Height--;

                    graphics.DrawRectangle(borderPen, clientRectangle);
                }
            }
        }

        /// <summary>
        /// Create the Pen used to draw the border
        /// </summary>
        protected Pen CreateBorderPen()
        {
            Color color = (Control.BackColor.GetBrightness() < 0.5) ? ControlPaint.Light(Control.BackColor) : ControlPaint.Dark(Control.BackColor);
            Pen pen = new Pen(color);
            pen.DashStyle = DashStyle.Dash;
            return pen;
        }
    }

}
