using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlString GetTemplateFullName(SqlInt64 templateID)
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.Parameters.AddWithValue("@templateID", templateID);
            cmd.CommandText = @"
                WITH NameChain AS
                (
	                SELECT CAST([Name] as nvarchar(1000)) as Name, ParentFolderID, 1 AS Counter
                      FROM Template
                      WHERE TemplateID = @templateID

                    UNION ALL

                    SELECT CAST(f.[Name] + '\' + r.[Name] as nvarchar(1000)) as [Name], f.ParentFolderID, r.Counter + 1 AS Counter
                       FROM TemplateFolder f INNER JOIN NameChain r ON f.TemplateFolderID = r.ParentFolderID
                )
                SELECT TOP (1) [Name] FROM NameChain ORDER BY Counter DESC";

            return new SqlString((string) cmd.ExecuteScalar());
        }
    }
}

