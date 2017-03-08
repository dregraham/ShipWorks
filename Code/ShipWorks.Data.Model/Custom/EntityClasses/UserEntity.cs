using SD.LLBLGen.Pro.ORMSupportClasses;
using System;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extention of the LLBLGen UserEntity
    /// </summary>
    public partial class UserEntity
    {
        /// <summary>
        /// The constant SuperUserID
        /// </summary>
        public static long SuperUserID
        {
            get { return (0xFACED * 1000) + 2; }
        }

        /// <summary>
        /// The internal private superuser name, actually saved to the database
        /// </summary>
        public static string SuperUserInternalName
        {
            get { return "_@system"; }
        }

        /// <summary>
        /// The name we use to display and represent the SuperUser
        /// </summary>
        public static string SuperUserDisplayName
        {
            get { return "System"; }
        }

        /// <summary>
        /// Post-process a value right before being returned via a property getter
        /// </summary>
        protected override void PostProcessValueToGet(IFieldInfo fieldToGet, ref object valueToReturn)
        {
            base.PostProcessValueToGet(fieldToGet, ref valueToReturn);

            if (fieldToGet.FieldIndex == (int) UserFieldIndex.Username && UserID == SuperUserID)
            {
                valueToReturn = SuperUserDisplayName;
            }
        }

        /// <summary>
        /// Create the pre-fetch path used to load user related table(s)
        /// </summary>
        private static readonly Lazy<IPrefetchPath2> fullPrefetchPath = new Lazy<IPrefetchPath2>(() =>
        {
            IPrefetchPath2 prefetchPath = new PrefetchPath2(EntityType.UserEntity);

            prefetchPath.Add(UserEntity.PrefetchPathShortcutOverrides);
            prefetchPath.Add(UserEntity.PrefetchPathSettings);

            return prefetchPath;
        });

        /// <summary>
        /// Create the pre-fetch path used to load user related table(s)
        /// </summary>
        public static IPrefetchPath2 FullPrefetchPath
        {
            get
            {
                return fullPrefetchPath.Value;
            }
        }
    }
}
