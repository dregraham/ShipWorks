using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ShipWorks.SqlServer.Common.Data
{
    /// <summary>
    /// Helps track usage of various objects in the system.  Such as filters, templates, and resources.  There is also a version
    /// of this class that lives outside of this SqlServer assembly, that does things that this one can't since it does not have
    /// access to the data model.
    /// </summary>
    public static class ObjectReferenceManager
    {
        /// <summary>
        /// Marks the given object as being referenced by the specified consumer.  The reason can be used as needed
        /// by other parts of ShipWorks (for example, when deleting a filter, it could show the reasons of
        /// who is using the filter).  If the consumer already has a referenced object
        /// under the given key, the reason is updated.
        /// </summary>
        public static long SetReference(long consumerID, string key, long objectID, string reason, SqlConnection con)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException(string.Format("The 'ReferenceKey' cannot be empty. ({0}, {1})", consumerID, objectID));
            }

            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"
                MERGE INTO ObjectReference as T
                  USING (SELECT @consumerID as 'ConsumerID', @key as 'ReferenceKey', @objectID as 'ObjectID', @reason as 'Reason') AS S
                  ON T.ConsumerID = S.ConsumerID AND T.ReferenceKey = S.ReferenceKey
                WHEN MATCHED THEN
                  UPDATE SET T.ObjectID = S.ObjectID, T.Reason = S.Reason
                WHEN NOT MATCHED THEN
                  INSERT (ConsumerID, ReferenceKey, ObjectID, Reason) VALUES (S.ConsumerID, S.ReferenceKey, S.ObjectID, S.Reason)
                OUTPUT INSERTED.ObjectReferenceID;";
            cmd.Parameters.AddWithValue("@consumerID", consumerID);
            cmd.Parameters.AddWithValue("@key", key);
            cmd.Parameters.AddWithValue("@objectID", objectID);
            cmd.Parameters.AddWithValue("@reason", reason ?? (object) DBNull.Value);

            return (long) cmd.ExecuteScalar();
        }

        /// <summary>
        /// Release all references of the given consumer
        /// </summary>
        public static void ClearReferences(long consumerID, SqlConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "DELETE ObjectReference WHERE ConsumerID = @consumerID";
            cmd.Parameters.AddWithValue("@consumerID", consumerID);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Clear the usage of the given consumer and the specified key
        /// </summary>
        public static void ClearReference(long consumerID, string key, SqlConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "DELETE ObjectReference WHERE ConsumerID = @consumerID AND ReferenceKey = @key";
            cmd.Parameters.AddWithValue("@consumerID", consumerID);
            cmd.Parameters.AddWithValue("@key", key);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Clear the specific ObjectReferenceID
        /// </summary>
        public static void ClearReference(long referenceID, SqlConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "DELETE ObjectReference WHERE ObjectReferenceID = @referenceID";
            cmd.Parameters.AddWithValue("@referenceID", referenceID);
            cmd.ExecuteNonQuery();
        }
    }
}
