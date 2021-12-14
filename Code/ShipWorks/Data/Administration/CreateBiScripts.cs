using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Amazon.SWA;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Data.Administration
{
    public class CreateBiScripts : ICommandLineCommandHandler
    {

        public string CommandName => "CreateBiScripts";
        private readonly StringBuilder sqlScript = new StringBuilder();
        
        public Task Execute(List<string> args)
        {
            SqlSession.Initialize();
            ShippingSettings.InitializeForCurrentDatabase();

            CreateShipmentTypeSqlScript("ShipmentType");
            CreateSqlScript("ServiceType", GetServiceTypes());
            CreateSqlScript("PackagingType", GetPackageTypes());
            
            System.IO.File.WriteAllText(@"..\..\Queries\GenerateShipWorksTypes.sql", sqlScript.ToString());

            return Task.CompletedTask;
        }

        private List<(int parentCode, int Code, string Name)> GetPackageTypes()
        {
            var packageTypes = new List<(int parentCode, int Code, string Name)>();
            packageTypes.AddRange(GetValues<UpsPackagingType>((int) ShipmentTypeCode.UpsOnLineTools));
            packageTypes.AddRange(GetValues<UpsPackagingType>((int) ShipmentTypeCode.UpsWorldShip));
            packageTypes.AddRange(GetValues<PostalPackagingType>((int) ShipmentTypeCode.Endicia));
            packageTypes.AddRange(GetValues<PostalPackagingType>((int) ShipmentTypeCode.PostalWebTools));
            packageTypes.AddRange(GetValues<FedExPackagingType>((int) ShipmentTypeCode.FedEx));
            packageTypes.AddRange(GetValues<PostalPackagingType>((int) ShipmentTypeCode.Express1Endicia));
            packageTypes.AddRange(GetValues<OnTracPackagingType>((int) ShipmentTypeCode.OnTrac));
            packageTypes.AddRange(GetValues<PostalPackagingType>((int) ShipmentTypeCode.Express1Usps));
            packageTypes.AddRange(GetValues<PostalPackagingType>((int) ShipmentTypeCode.Usps));
            return packageTypes;
        }

        private List<(int parentCode, int Code, string Name)> GetServiceTypes()
        {
            var serviceTypes = new List<(int parentCode, int Code, string Name)>();

            serviceTypes.AddRange(GetValues<UpsServiceType>((int) ShipmentTypeCode.UpsOnLineTools));
            serviceTypes.AddRange(GetValues<UpsServiceType>((int) ShipmentTypeCode.UpsWorldShip));
            serviceTypes.AddRange(GetValues<PostalServiceType>((int) ShipmentTypeCode.Endicia));
            serviceTypes.AddRange(GetValues<PostalServiceType>((int) ShipmentTypeCode.PostalWebTools));
            serviceTypes.AddRange(GetValues<FedExServiceType>((int) ShipmentTypeCode.FedEx));
            serviceTypes.AddRange(GetValues<PostalServiceType>((int) ShipmentTypeCode.Express1Endicia));
            serviceTypes.AddRange(GetValues<OnTracServiceType>((int) ShipmentTypeCode.OnTrac));
            serviceTypes.AddRange(GetValues<iParcelServiceType>((int) ShipmentTypeCode.iParcel));
            serviceTypes.AddRange(GetValues<PostalServiceType>((int) ShipmentTypeCode.Express1Usps));
            serviceTypes.AddRange(GetValues<PostalServiceType>((int) ShipmentTypeCode.Usps));
            serviceTypes.AddRange(GetValues<DhlExpressServiceType>((int) ShipmentTypeCode.DhlExpress));
            serviceTypes.AddRange(GetValues<AsendiaServiceType>((int) ShipmentTypeCode.Asendia));
            serviceTypes.AddRange(GetValues<AmazonSWAServiceType>((int) ShipmentTypeCode.AmazonSWA));
            return serviceTypes;
        }

        private void CreateSqlScript(string tableName, List<(int parentCode, int Code, string Name)> types)
        {
            sqlScript.AppendLine(
                $"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{tableName}]') AND type IN (N'U'))");
            sqlScript.AppendLine($"DROP TABLE {tableName}");
            sqlScript.AppendLine("GO");
            sqlScript.AppendLine(
                $"CREATE TABLE {tableName} (ShipmentTypeID int NOT NULL, {tableName}ID int NOT NULL, Name varchar(50), CONSTRAINT PK_{tableName} PRIMARY KEY (ShipmentTypeID, {tableName}ID))");
            sqlScript.AppendLine("GO");
            sqlScript.AppendLine($"INSERT INTO {tableName} (ShipmentTypeID, {tableName}ID, Name) VALUES");

            var serviceValueStrings =
                types.Select(service => $"({service.parentCode}, {service.Code}, '{service.Name}')").ToArray();

            var joinedValueStrings = string.Join($", {Environment.NewLine}", serviceValueStrings);
            sqlScript.AppendLine(joinedValueStrings);
        }

        private void CreateShipmentTypeSqlScript(string tableName)
        {
            sqlScript.AppendLine(
                $"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{tableName}]') AND type IN (N'U'))");
            sqlScript.AppendLine($"DROP TABLE {tableName}");
            sqlScript.AppendLine("GO");
            sqlScript.AppendLine($"CREATE TABLE {tableName} ({tableName}ID int NOT NULL, Name varchar(50), CONSTRAINT PK_{tableName} PRIMARY KEY ({tableName}ID))");
            sqlScript.AppendLine("GO");
            sqlScript.AppendLine($"INSERT INTO {tableName} ({tableName}ID, Name) VALUES");
            
            var shipmentTypes = GetValues<ShipmentTypeCode>(-1);
            var serviceValueStrings =
                shipmentTypes.Select(service => $"({service.Code}, '{service.Name}')").ToArray();

            var joinedValueStrings = string.Join($", {Environment.NewLine}", serviceValueStrings);
            sqlScript.AppendLine(joinedValueStrings);
        }

        private IEnumerable<(int parentCode, int Code, string Name)> GetValues<T>(int shipmentType) where T : struct
        {
            var values = Enum.GetNames(typeof(T));
            var returnValue = new List<(int parentCode, int Code, string Name)>();
            foreach (var value in values)
            {
                var parsedValue = Enum.Parse(typeof(T), value);
                var description = EnumHelper.GetDescription((Enum) parsedValue);
                returnValue.Add((parentCode: shipmentType, Code: (int) parsedValue, Name: description));
            }

            return returnValue;
        }
    }
}