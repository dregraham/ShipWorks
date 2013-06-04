using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using System.Drawing;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid.Columns;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Grid
{
    /// <summary>
    /// A grid display type for the ChannelAdvisor flag that can be associated with an order
    /// </summary>
    public class GridChannelAdvisorFlagDisplayType : GridColumnDisplayType
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GridChannelAdvisorFlagDisplayType));

        /// <summary>
        /// Initializes a new instance of the <see cref="GridChannelAdvisorFlagDisplayType"/> class.
        /// </summary>
        /// <param name="sortMethod">The sort method.</param>
        public GridChannelAdvisorFlagDisplayType()
            : base()
        {
            ShowFlag = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show flag].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show flag]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowFlag { get; set; }

        /// <summary>
        /// Create the editor the user can use to edit properties of the type
        /// </summary>
        /// <returns></returns>
        public override GridColumnDisplayEditor CreateEditor()
        {
            return new GridChannelAdvisorFlagDisplayEditor(this);
        }

        /// <summary>
        /// Get the value that will be passed to the GetDisplayImage and GetDisplayText functions
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            ChannelAdvisorOrderEntity caOrder = entity as ChannelAdvisorOrderEntity;
            if (caOrder == null)
            {
                return Tuple.Create(ChannelAdvisorFlagType.NoFlag, "");
            }

            return Tuple.Create((ChannelAdvisorFlagType) caOrder.FlagType, caOrder.FlagDescription);
        }

        /// <summary>
        /// Gets the display image.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected override Image GetDisplayImage(object value)
        {
            Image displayImage = null;

            if (ShowFlag)
            {
                displayImage = EnumHelper.GetImage(((Tuple<ChannelAdvisorFlagType, string>) value).Item1);
            }

            return displayImage;
        }

        /// <summary>
        /// Gets the display text.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected override string GetDisplayText(object value)
        {
            return ((Tuple<ChannelAdvisorFlagType, string>) value).Item2;
        }
    }
}
