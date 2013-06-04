using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.SqlServer.General;

public partial class Triggers
{
    [SqlTrigger(Target="TemplateUserSettings", Event="FOR INSERT, UPDATE, DELETE")]
    public static void TemplateUserSettingsTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            foreach (long settingsID in UtilityFunctions.GetTriggerPrimaryKeys(con))
            {
                // If deleting the settings, remove all references
                if (SqlContext.TriggerContext.TriggerAction == TriggerAction.Delete)
                {
                    ObjectReferenceManager.ClearReferences(settingsID, con);
                }
                else
                {
                    // See if the PreviewFilterNodeID column is affected
                    if (SqlContext.TriggerContext.IsUpdatedColumn(UtilityFunctions.GetColumnNameOrdinalMap(con, UtilityFunctions.GetTriggerTableName())["PreviewFilterNodeID"]))
                    {
                        // Now add a reference to the new one
                        long newFilterNodeID = GetFieldValue("inserted", settingsID, "PreviewFilterNodeID", con);
                        if (newFilterNodeID > 0)
                        {
                            ObjectReferenceManager.SetReference(
                                settingsID,
                                "PreviewFilterNodeID",
                                newFilterNodeID,
                                string.Format("Preview for template '[ID]{0}[/ID]' for user '[ID]{1}[/ID]'",
                                    GetFieldValue("inserted", settingsID, "TemplateID", con),
                                    GetFieldValue("inserted", settingsID, "UserID", con)),
                                con);
                        }
                        else
                        {
                            ObjectReferenceManager.ClearReference(settingsID, "PreviewFilterNodeID", con);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Return the field value that has the given TemplateUserSettingsID from the specified table
    /// </summary>
    private static long GetFieldValue(string table, long settingsID, string column, SqlConnection con)
    {
        SqlCommand cmd = con.CreateCommand();
        cmd.CommandText = string.Format("SELECT COALESCE({0}, 0) FROM {1} WHERE TemplateUserSettingsID = {2}", column, table, settingsID);

        return (long) cmd.ExecuteScalar();
    }
}
