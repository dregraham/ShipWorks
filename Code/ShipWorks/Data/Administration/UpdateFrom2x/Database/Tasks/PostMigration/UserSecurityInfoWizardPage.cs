using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration
{
    /// <summary>
    /// Page for informing the user that there is no more control over users
    /// </summary>
    public partial class UserSecurityInfoWizardPage : WizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserSecurityInfoWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            // See if there are any non-admins
            int count = SqlAdapter.Default.GetDbCount(new UserEntity().Fields, new RelationPredicateBucket(UserFields.IsAdmin == false & UserFields.IsDeleted == false));

            // If there are no non-admins, we don't need to show the page
            if (count == 0)
            {
                e.Skip = true;
            }
        }
    }
}
