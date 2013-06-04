using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections;
using System.Collections.Generic;

public partial class UserDefinedFunctions
{
    [SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "GetTemplateDescendantOfFolder_FillRow", TableDefinition = "TemplateID bigint")]
    public static IEnumerable GetTemplateDescendantsOfFolder(SqlInt64 templateFolderID)
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.Parameters.AddWithValue("@templateFolderID", templateFolderID);
            cmd.CommandText = @"
                WITH DescendantFolders AS
                (
	                SELECT @templateFolderID as TemplateFolderID
                    
                    UNION ALL

	                SELECT c.TemplateFolderID
                      FROM TemplateFolder c INNER JOIN DescendantFolders p ON p.TemplateFolderID = c.ParentFolderID
                )
                SELECT TemplateID 
                 FROM Template
                 WHERE ParentFolderID IN (SELECT TemplateFolderID FROM DescendantFolders)";

            List<long> idList = new List<long>();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    idList.Add( (long) reader[0] );
                }
            }

            return idList;
        }
    }

    /// <summary>
    /// The callback function for filling each row with data
    /// </summary>
    public static void GetTemplateDescendantOfFolder_FillRow(object data, out SqlInt64 templateID)
    {
        templateID = (long) data;
    }
};

