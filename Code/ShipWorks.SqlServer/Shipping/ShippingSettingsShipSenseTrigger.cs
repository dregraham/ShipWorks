using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Common.Data;

public partial class Triggers
{
    [SqlTrigger(Target = "ShippingSettings", Event = "FOR INSERT,UPDATE")]
    public static void ShippingSettingsShipSenseTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // Disable ShipSense triggers when ShipSense is disabled, enable them when they are enabled.
            string sql = @"
                        IF (EXISTS(SELECT ShipSenseEnabled FROM ShippingSettings WHERE ShipSenseEnabled = 1))
	                        BEGIN
		                        ALTER TABLE [Order] ENABLE TRIGGER [OrderShipmentShipSenseStatusTrigger]
		                        ALTER TABLE [Order] ENABLE TRIGGER [OrderShipSenseRecognitionStatusTrigger]
		                        ALTER TABLE [ShipSenseKnowledgebase] ENABLE TRIGGER [ShipSenseKnowledgebaseUpdateTrigger]
	                        END
                        ELSE
	                        BEGIN
		                        ALTER TABLE [Order] DISABLE TRIGGER [OrderShipmentShipSenseStatusTrigger]
		                        ALTER TABLE [Order] DISABLE TRIGGER [OrderShipSenseRecognitionStatusTrigger]
		                        ALTER TABLE [ShipSenseKnowledgebase] DISABLE TRIGGER [ShipSenseKnowledgebaseUpdateTrigger]
	                        END
                    ";

            using (SqlCommand sqlCommand = con.CreateCommand())
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = sql;
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
