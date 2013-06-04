using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for the user node
    /// </summary>
    public class UserOutline : ElementOutline
    {
        UserEntity user;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("ID", () => user.UserID);

            AddElement("Username", () => user.Username);
            AddElement("IsAdmin", () => user.IsAdmin);
        }

        /// <summary>
        /// Create a databound version of the outline, based on the given data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            // Possible to be passing in null if something didn't have a user
            if (data == null)
            {
                return null;
            }

            return new UserOutline(Context) { user = (UserEntity) data };
        }
    }
}
