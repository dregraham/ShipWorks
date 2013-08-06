using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using System.Data.SqlClient;


public partial class Triggers
{
    [SqlTrigger(Target = "Action", Event = "FOR DELETE")]
    public static void ActionDeleteTrigger()
    {
        using (var connection = new SqlConnection("Context connection = true"))
        {
            connection.Open();

            UtilityFunctions.IncreaseDbts(connection);
        }
    }
}
