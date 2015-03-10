using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Reprsents a column that allows a user to take an action on a row.
    /// </summary>
    public class GridActionDisplayType : GridColumnDisplayType
    {
        // Statically supplied action text
        string actionText;

        // User provided callback that can dynamically supply the action text
        Func<object, string> actionTextProvider;

        // Data passed to anyone handling the action
        object actionData;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridActionDisplayType(string actionText, object actionData)
            : this(actionData, null)
        {
            this.actionText = actionText;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GridActionDisplayType(Func<object, string> actionTextProvider, object actionData)
            : this(actionData, null)
        {
            this.actionTextProvider = actionTextProvider;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GridActionDisplayType(Func<object, string> actionTextProvider, Action<object, GridHyperlinkClickEventArgs> clickAction, Func<object, bool> linkEnabledProvider)
            : this(clickAction, linkEnabledProvider)
        {
            this.actionTextProvider = actionTextProvider;
        }

        /// <summary>
        /// Common constructor
        /// </summary>
        private GridActionDisplayType(object actionData, Func<object, bool> linkEnabledProvider)
        {
            this.actionData = actionData;

            PreviewInputType = GridColumnPreviewInputType.LiteralString;

            GridHyperlinkDecorator linkDecorator = new GridHyperlinkDecorator();
            if (linkEnabledProvider != null)
            {
                linkDecorator.QueryEnabled += (sender, args) => args.Enabled = linkEnabledProvider(args.Value);
            }

            Decorate(linkDecorator);
        }

        /// <summary>
        /// Get the text to display, which will be our action text.
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            if (actionTextProvider != null)
            {
                return actionTextProvider(value);
            }

            return actionText;
        }

        /// <summary>
        /// Get data associated with the link that can be used to determine course of action to take when clicked
        /// </summary>
        public object ActionData
        {
            get
            {
                return actionData;
            }
        }
    }
}
