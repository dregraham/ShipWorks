using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Data;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration
{
    /// <summary>
    /// Utility class for dealing with post migration stuff
    /// </summary>
    public static class Post2xMigrationUtility
    {
        /// <summary>
        /// Indicates if the given 2x PostMigration step has been marked as complete.  If PostMigration is not in progress, just returns true.
        /// </summary>
        public static bool IsStepComplete(Post2xMigrationStep step)
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
                {
                    cmd.CommandText = @"
                    IF (OBJECT_ID('v2m_PostMigrationProgress') > 0)
                      SELECT COUNT(PostMigrationProgressID) FROM v2m_PostMigrationProgress WHERE Identifier = @step
                    ELSE
                      SELECT 1";
                    cmd.Parameters.AddWithValue("@step", (int)step);

                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        /// <summary>
        /// Mark the given step as complete
        /// </summary>
        public static void MarkStepComplete(Post2xMigrationStep step)
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
                {
                    cmd.CommandText = @"
                    IF (OBJECT_ID('v2m_PostMigrationProgress') > 0)
                      INSERT INTO v2m_PostMigrationProgress (Identifier) VALUES (@step)";
                    cmd.Parameters.AddWithValue("@step", (int)step);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
