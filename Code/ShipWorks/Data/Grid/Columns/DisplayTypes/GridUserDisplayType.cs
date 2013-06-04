using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using System.Drawing;
using ShipWorks.Users.Security;
using ShipWorks.Properties;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Column DisplayType for showing a user name based on a UserID
    /// </summary>
    public class GridUserDisplayType : GridColumnDisplayType
    {
        bool showIcon = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridUserDisplayType()
        {

        }

        /// <summary>
        /// The format editor for the display type.
        /// </summary>
        public override GridColumnDisplayEditor CreateEditor()
        {
            return new GridUserDisplayEditor(this);
        }

        /// <summary>
        /// Indicates if the icon representing the entity type should be displayed
        /// </summary>
        public bool ShowIcon
        {
            get { return showIcon; }
            set { showIcon = value; }
        }

        /// <summary>
        /// Provide the data to be used by the display functions
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            object value = base.GetEntityValue(entity);

            if (value != null)
            {
                UserEntity user = UserManager.GetUser((long) value);

                if (user != null)
                {
                    Image icon = null;

                    if (user.UserID == SuperUser.UserID)
                    {
                        icon = Resources.sw_cubes_16;
                    }

                    else if (user.IsDeleted)
                    {
                        icon = Resources.user_deleted_16;
                    }

                    else
                    {
                        icon = Resources.user_16;
                    }

                    return new object[] { user.Username, icon };
                }
            }

            return new object[] { "", null };
        }

        /// <summary>
        /// Format the UserID into the user name
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            return (string) ((object[]) value)[0];
        }

        /// <summary>
        /// Get the image to display for the user
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            if (!showIcon)
            {
                return null;
            }

            return (Image) ((object[]) value)[1];
        }

        /// <summary>
        /// Default width for user column
        /// </summary>
        public override int DefaultWidth
        {
            get
            {
                return 100;
            }
        }
    }
}
