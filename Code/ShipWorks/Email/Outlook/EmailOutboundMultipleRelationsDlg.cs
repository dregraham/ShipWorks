using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data;
using ShipWorks.Data.Model.FactoryClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Model;

namespace ShipWorks.Email.Outlook
{
    /// <summary>
    /// Window for showing the objects an email is related to when there are multiple of them
    /// </summary>
    public partial class EmailOutboundMultipleRelationsDlg : Form
    {
        long emailOutboundID;

        Guid gridSettingsKey = new Guid("{3EF3AA06-2F7E-4aa6-BC3B-34CA2849F4F2}");

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailOutboundMultipleRelationsDlg(long emailOutboundID)
        {
            InitializeComponent();

            this.emailOutboundID = emailOutboundID;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            QueryableEntityGateway gateway = new QueryableEntityGateway(EntityType.EmailOutboundRelationEntity,
                new RelationPredicateBucket(
                EmailOutboundRelationFields.EmailOutboundID == emailOutboundID &
                EmailOutboundRelationFields.RelationType == (int) EmailOutboundRelationType.RelatedObject));

            // Prepare for paging
            entityGrid.InitializeGrid();

            // Prepare configurable columns
            entityGrid.InitializeColumns(new StandardGridColumnStrategy(gridSettingsKey, GridColumnDefinitionSet.EmailOutboundRelation, null));
            entityGrid.SaveColumnsOnClose(this);

            // Load the rows
            entityGrid.OpenGateway(gateway);
        }
    }
}
