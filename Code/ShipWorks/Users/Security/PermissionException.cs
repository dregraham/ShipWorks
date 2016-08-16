using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Raised when a user does not have permission to perform an action.
    /// </summary>
    [Serializable]
    public class PermissionException : Exception
    {
        UserEntity user;
        PermissionType permissionType;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PermissionException()
        {

        }

        /// <summary>
        /// Common constructor
        /// </summary>
        public PermissionException(UserEntity user, PermissionType permissionType)
            : this(user, permissionType, null)
        {

        }

        /// <summary>
        /// Common constructor
        /// </summary>
        public PermissionException(PermissionType permissionType)
            : base(string.Format("Insufficient permission for '{0}'.", permissionType))
        {
            this.permissionType = permissionType;
        }

        /// <summary>
        /// Common constructor
        /// </summary>
        public PermissionException(UserEntity user, PermissionType permissionType, Exception inner)
            : base(GetMessage(user, permissionType), inner)
        {
            this.user = user;
            this.permissionType = permissionType;
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected PermissionException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        {
            user = (UserEntity)serializationInfo.GetValue(nameof(user), typeof(UserEntity));
            permissionType = (PermissionType)serializationInfo.GetValue(nameof(permissionType), typeof(PermissionType));
        }

        /// <summary>
        /// Generate the message text for the given user and event.
        /// </summary>
        private static string GetMessage(UserEntity user, PermissionType permissionType)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return string.Format("User '{0}' does not have permission for '{1}'.", user.Username, permissionType);
        }

        /// <summary>
        /// The user that did not have adequate permissions
        /// </summary>
        public UserEntity User
        {
            get { return user; }
        }

        /// <summary>
        /// The event the user did not have permissions for.
        /// </summary>
        public PermissionType SecuredEvent
        {
            get { return permissionType; }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(user), user);
            info.AddValue(nameof(permissionType), permissionType);
        }
    }
}
