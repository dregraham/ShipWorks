using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Filters.DirtyCounts;

public partial class Triggers
{
    [SqlTrigger(Name = "WorldShipProcessedInvalidShipmentIdTrigger", Target = "WorldShipProcessed", Event = "INSTEAD OF INSERT")]
    public static void WorldShipProcessedInvalidShipmentIdTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            string sql =
                @"	insert into WorldShipProcessed (ShipmentID, PublishedCharges, NegotiatedCharges, TrackingNumber, UspsTrackingNumber, ServiceType, PackageType, UpsPackageID, DeclaredValueAmount, DeclaredValueOption, WorldShipShipmentID, VoidIndicator, NumberOfPackages, LeadTrackingNumber)
		                select ShipmentID, PublishedCharges, NegotiatedCharges, TrackingNumber, UspsTrackingNumber, ServiceType, PackageType, UpsPackageID, DeclaredValueAmount, DeclaredValueOption, WorldShipShipmentID, VoidIndicator, NumberOfPackages, LeadTrackingNumber
		                from inserted i
		                where i.ShipmentID NOT LIKE '%[^0-9]%' ";

            SqlCommand sqlCommand = con.CreateCommand();
            sqlCommand.CommandText = sql;
            sqlCommand.ExecuteNonQuery();
        }
    }
}