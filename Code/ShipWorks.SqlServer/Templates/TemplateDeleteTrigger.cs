using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Common.Data;

public partial class Triggers
{
    [SqlTrigger(Target="Template", Event="FOR DELETE")]
    public static void TemplateDeleteTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // Delete all references that the templates hold.  This is for resource usage, and whatever else.
            foreach (long templateID in UtilityFunctions.GetTriggerPrimaryKeys(con))
            {
                ObjectReferenceManager.ClearReferences(templateID, con);
            }
        }
    }
}
