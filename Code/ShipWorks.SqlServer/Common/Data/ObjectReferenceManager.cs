using System;
using System.Data.Common;

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
        public static long SetReference(long consumerID, string key, long objectID, string reason, DbConnection con)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException(string.Format("The 'ReferenceKey' cannot be empty. ({0}, {1})", consumerID, objectID));
            }

            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            DbCommand cmd = con.CreateCommand();
            cmd.CommandText = @"
                MERGE INTO ObjectReference as T
                  USING (SELECT @consumerID as 'ConsumerID', @key as 'ReferenceKey', @objectID as 'ObjectID', @reason as 'Reason') AS S
                  ON T.ConsumerID = S.ConsumerID AND T.ReferenceKey = S.ReferenceKey
                WHEN MATCHED THEN
                  UPDATE SET T.ObjectID = S.ObjectID, T.Reason = S.Reason
                WHEN NOT MATCHED THEN
                  INSERT (ConsumerID, ReferenceKey, ObjectID, Reason) VALUES (S.ConsumerID, S.ReferenceKey, S.ObjectID, S.Reason)
                OUTPUT INSERTED.ObjectReferenceID;";
            cmd.AddParameterWithValue("@consumerID", consumerID);
            cmd.AddParameterWithValue("@key", key);
            cmd.AddParameterWithValue("@objectID", objectID);
            cmd.AddParameterWithValue("@reason", reason ?? (object) DBNull.Value);

            return (long) cmd.ExecuteScalar();
        }

        /// <summary>
        /// Release all references of the given consumer
        /// </summary>
        public static void ClearReferences(long consumerID, DbConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            DbCommand cmd = con.CreateCommand();
            cmd.CommandText = "DELETE ObjectReference WHERE ConsumerID = @consumerID";
            cmd.AddParameterWithValue("@consumerID", consumerID);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Clear the usage of the given consumer and the specified key
        /// </summary>
        public static void ClearReference(long consumerID, string key, DbConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            DbCommand cmd = con.CreateCommand();
            cmd.CommandText = "DELETE ObjectReference WHERE ConsumerID = @consumerID AND ReferenceKey = @key";
            cmd.AddParameterWithValue("@consumerID", consumerID);
            cmd.AddParameterWithValue("@key", key);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Clear the specific ObjectReferenceID
        /// </summary>
        public static void ClearReference(long referenceID, DbConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            DbCommand cmd = con.CreateCommand();
            cmd.CommandText = "DELETE ObjectReference WHERE ObjectReferenceID = @referenceID";
            cmd.AddParameterWithValue("@referenceID", referenceID);
            cmd.ExecuteNonQuery();
        }
    }
}
