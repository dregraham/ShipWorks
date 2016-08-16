using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SqlObjectReferenceManager = ShipWorks.SqlServer.Common.Data.ObjectReferenceManager;
using ShipWorks.Data.Connection;
using System.Text.RegularExpressions;
using Interapptive.Shared;
using ShipWorks.Data.Model;
using ShipWorks.Templates;
using ShipWorks.Users;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Data;

namespace ShipWorks.Data
{
    /// <summary>
    /// Helps track usage of various objects in the system.  Such as filters, templates, and resources.  There is also a version
    /// of this class that lives inside of the SqlServer assembly that does the real work, and can be used within triggers
    /// </summary>
    public static class ObjectReferenceManager
    {
        static Regex idReasonRegex = new Regex(@"\[ID\](?<ID>\d+)\[/ID\]",
           RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        /// <summary>
        /// Marks the given object as being referenced by the specified consumer.  The reason can be used as needed
        /// by other parts of ShipWorks (for example, when deleting a filter, it could show the reasons of
        /// who is using the filter).  If the consumer is already using the object, then a new reference is not added,
        /// but the reason is updated.
        /// </summary>
        public static long SetReference(long consumerID, string key, long objectID)
        {
            return SetReference(consumerID, key, objectID, null);
        }

        /// <summary>
        /// Marks the given object as being referenced by the specified consumer.  The reason can be used as needed
        /// by other parts of ShipWorks (for example, when deleting a filter, it could show the reasons of
        /// who is using the filter).  If the consumer already has a reference by the given key name, then the reference is updated with 
        /// the new consumerID and reason.
        /// </summary>
        public static long SetReference(long consumerID, string key, long objectID, string reason)
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                return SqlObjectReferenceManager.SetReference(consumerID, key, objectID, reason, con);
            }
        }

        /// <summary>
        /// Clears the reference of the given consumerID and given reference key, if it exists
        /// </summary>
        public static void ClearReference(long consumerID, string key)
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                SqlObjectReferenceManager.ClearReference(consumerID, key, con);
            }
        }

        /// <summary>
        /// Clear all references used by the given consumerID
        /// </summary>
        public static void ClearReferences(long consumerID)
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                SqlObjectReferenceManager.ClearReferences(consumerID, con);
            }
        }

        /// <summary>
        /// Clear the specific object reference of the given ObjectReferenceID
        /// </summary>
        public static void ClearReference(long referenceID)
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                SqlObjectReferenceManager.ClearReference(referenceID, con);
            }
        }

        /// <summary>
        /// Returns the combined reasons of how each of the object ID's in the list are in use
        /// by a consumer.  The results are limited to non-empty descriptions, and duplicates are removed.
        /// </summary>
        [NDependIgnoreLongMethod]
        public static List<string> GetReferenceReasons(List<long> objectIDs)
        {
            if (objectIDs.Count == 0)
            {
                return new List<string>();
            }

            List<string> reasons = new List<string>();
            
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                SqlCommand cmd = SqlCommandProvider.Create(con);
                cmd.CommandText = string.Format(@"
                SELECT DISTINCT Reason 
                FROM ObjectReference 
                WHERE Reason IS NOT NULL AND
                      ObjectID IN ({0})", GetInList(objectIDs));

                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        reasons.Add((string) reader[0]);
                    }
                }
            }

            List<string> translated = new List<string>();

            // We have to go through and resolve any ID's in the description
            foreach (string reason in reasons)
            {
                string updated = reason;

                foreach (Match match in idReasonRegex.Matches(reason))
                {
                    long id;
                    if (long.TryParse(match.Groups["ID"].Value, out id))
                    {
                        string replace = "?";

                        switch (EntityUtility.GetEntityType(id))
                        {
                            case EntityType.UserEntity:
                                UserEntity user = new UserEntity(id);
                                SqlAdapter.Default.FetchEntity(user);
                                if (user.Fields.State == EntityState.Fetched)
                                {
                                    replace = user.Username;
                                }

                                break;

                            default:
                                replace = ObjectLabelManager.GetLabel(id).ShortText;
                                break;
                        }

                        updated = updated.Replace(match.Value, replace);
                    }
                }

                if (updated.Trim().Length > 0)
                {
                    translated.Add(updated);
                }
            }

            return translated;
        }

        /// <summary>
        /// Produce a string suitable for use with a SQL 'IN' statement
        /// </summary>
        private static StringBuilder GetInList(List<long> objectIDs)
        {
            StringBuilder inList = new StringBuilder();
            foreach (long objectID in objectIDs)
            {
                if (inList.Length > 0)
                {
                    inList.Append(",");
                }

                inList.AppendFormat("{0}", objectID);
            }
            return inList;
        }
    }
}
