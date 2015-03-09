CREATE TABLE [dbo].[Shipment] (
    [ShipmentID]                           BIGINT          IDENTITY (1031, 1000) NOT NULL,
    [RowVersion]                           ROWVERSION      NOT NULL,
    [OrderID]                              BIGINT          NOT NULL,
    [ShipmentType]                         INT             NOT NULL,
    [ContentWeight]                        FLOAT (53)      NOT NULL,
    [TotalWeight]                          FLOAT (53)      NOT NULL,
    [Processed]                            BIT             NOT NULL,
    [ProcessedDate]                        DATETIME        NULL,
    [ProcessedUserID]                      BIGINT          NULL,
    [ProcessedComputerID]                  BIGINT          NULL,
    [ShipDate]                             DATETIME        NOT NULL,
    [ShipmentCost]                         MONEY           NOT NULL,
    [Voided]                               BIT             NOT NULL,
    [VoidedDate]                           DATETIME        NULL,
    [VoidedUserID]                         BIGINT          NULL,
    [VoidedComputerID]                     BIGINT          NULL,
    [TrackingNumber]                       NVARCHAR (50)   NOT NULL,
    [CustomsGenerated]                     BIT             NOT NULL,
    [CustomsValue]                         MONEY           NOT NULL,
    [ThermalType]                          INT             NULL,
    [ShipFirstName]                        NVARCHAR (30)   NOT NULL,
    [ShipMiddleName]                       NVARCHAR (30)   NOT NULL,
    [ShipLastName]                         NVARCHAR (30)   NOT NULL,
    [ShipCompany]                          NVARCHAR (60)   NOT NULL,
    [ShipStreet1]                          NVARCHAR (60)   NOT NULL,
    [ShipStreet2]                          NVARCHAR (60)   NOT NULL,
    [ShipStreet3]                          NVARCHAR (60)   NOT NULL,
    [ShipCity]                             NVARCHAR (50)   NOT NULL,
    [ShipStateProvCode]                    NVARCHAR (50)   NOT NULL,
    [ShipPostalCode]                       NVARCHAR (20)   NOT NULL,
    [ShipCountryCode]                      NVARCHAR (50)   NOT NULL,
    [ShipPhone]                            NVARCHAR (25)   NOT NULL,
    [ShipEmail]                            NVARCHAR (100)  NOT NULL,
    [ShipAddressValidationSuggestionCount] INT             NOT NULL,
    [ShipAddressValidationStatus]          INT             NOT NULL,
    [ShipAddressValidationError]           NVARCHAR (300)  NOT NULL,
    [ShipResidentialStatus]                INT             NOT NULL,
    [ShipPOBox]                            INT             NOT NULL,
    [ShipUSTerritory]                      INT             NOT NULL,
    [ShipMilitaryAddress]                  INT             NOT NULL,
    [ResidentialDetermination]             INT             NOT NULL,
    [ResidentialResult]                    BIT             NOT NULL,
    [OriginOriginID]                       BIGINT          NOT NULL,
    [OriginFirstName]                      NVARCHAR (30)   NOT NULL,
    [OriginMiddleName]                     NVARCHAR (30)   NOT NULL,
    [OriginLastName]                       NVARCHAR (30)   NOT NULL,
    [OriginCompany]                        NVARCHAR (60)   NOT NULL,
    [OriginStreet1]                        NVARCHAR (60)   NOT NULL,
    [OriginStreet2]                        NVARCHAR (60)   NOT NULL,
    [OriginStreet3]                        NVARCHAR (60)   NOT NULL,
    [OriginCity]                           NVARCHAR (50)   NOT NULL,
    [OriginStateProvCode]                  NVARCHAR (50)   NOT NULL,
    [OriginPostalCode]                     NVARCHAR (20)   NOT NULL,
    [OriginCountryCode]                    NVARCHAR (50)   NOT NULL,
    [OriginPhone]                          NVARCHAR (25)   NOT NULL,
    [OriginFax]                            NVARCHAR (35)   NOT NULL,
    [OriginEmail]                          NVARCHAR (100)  NOT NULL,
    [OriginWebsite]                        NVARCHAR (50)   NOT NULL,
    [ReturnShipment]                       BIT             NOT NULL,
    [Insurance]                            BIT             NOT NULL,
    [InsuranceProvider]                    INT             NOT NULL,
    [ShipNameParseStatus]                  INT             NOT NULL,
    [ShipUnparsedName]                     NVARCHAR (100)  NOT NULL,
    [OriginNameParseStatus]                INT             NOT NULL,
    [OriginUnparsedName]                   NVARCHAR (100)  NOT NULL,
    [BestRateEvents]                       TINYINT         NOT NULL,
    [ShipSenseStatus]                      INT             NOT NULL,
    [ShipSenseChangeSets]                  XML             NOT NULL,
    [ShipSenseEntry]                       VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_Shipment] PRIMARY KEY CLUSTERED ([ShipmentID] ASC),
    CONSTRAINT [FK_Shipment_Order] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID]),
    CONSTRAINT [FK_Shipment_ProcessedComputer] FOREIGN KEY ([ProcessedComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]),
    CONSTRAINT [FK_Shipment_ProcessedUser] FOREIGN KEY ([ProcessedUserID]) REFERENCES [dbo].[User] ([UserID]),
    CONSTRAINT [FK_Shipment_VoidedComputer] FOREIGN KEY ([VoidedComputerID]) REFERENCES [dbo].[Computer] ([ComputerID]),
    CONSTRAINT [FK_Shipment_VoidedUser] FOREIGN KEY ([VoidedUserID]) REFERENCES [dbo].[User] ([UserID]),
    CONSTRAINT [IX_Shipment_Other] UNIQUE NONCLUSTERED ([ShipmentID] ASC)
);





GO
ALTER TABLE [dbo].[Shipment] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);

GO
CREATE NONCLUSTERED INDEX [IX_Shipment_OrderID]
    ON [dbo].[Shipment]([OrderID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Shipment_OrderID_ShipSenseStatus] ON [dbo].[Shipment] ([OrderID], [Processed], [ShipSenseStatus])
GO
CREATE NONCLUSTERED INDEX [IX_Shipment_ProcessedOrderID] ON [dbo].[Shipment] ([Processed] DESC) INCLUDE ([OrderID])
GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'103', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'ShipmentType';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'ContentWeight';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'TotalWeight';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'7', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'ShipDate';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'ShipmentCost';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'CustomsGenerated';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'CustomsValue';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'ShipStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'ShipState', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'ShipStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'ShipCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'ShipCountry', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'ShipCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'111', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'ResidentialDetermination';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Residential \ Commercial', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'ResidentialDetermination';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'ResidentialResult';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'OriginOriginID';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'OriginStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'OriginState', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'OriginStateProvCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'6', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'OriginCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'OriginCountry', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'OriginCountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'112', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'InsuranceProvider';


GO
EXECUTE sp_addextendedproperty @name = N'AuditName', @value = N'Insurance', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'InsuranceProvider';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'ShipNameParseStatus';


GO
EXECUTE sp_addextendedproperty @name = N'AuditFormat', @value = N'1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shipment', @level2type = N'COLUMN', @level2name = N'OriginNameParseStatus';

