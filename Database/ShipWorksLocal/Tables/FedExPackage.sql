CREATE TABLE [dbo].[FedExPackage] (
    [FedExPackageID]                      BIGINT          IDENTITY (1061, 1000) NOT NULL,
    [ShipmentID]                          BIGINT          NOT NULL,
    [Weight]                              FLOAT (53)      NOT NULL,
    [DimsProfileID]                       BIGINT          NOT NULL,
    [DimsLength]                          FLOAT (53)      NOT NULL,
    [DimsWidth]                           FLOAT (53)      NOT NULL,
    [DimsHeight]                          FLOAT (53)      NOT NULL,
    [DimsWeight]                          FLOAT (53)      NOT NULL,
    [DimsAddWeight]                       BIT             NOT NULL,
    [SkidPieces]                          INT             NOT NULL,
    [Insurance]                           BIT             CONSTRAINT [DF_FedExPackage_Insured] DEFAULT ((0)) NOT NULL,
    [InsuranceValue]                      MONEY           NOT NULL,
    [InsurancePennyOne]                   BIT             NOT NULL,
    [DeclaredValue]                       MONEY           NOT NULL,
    [TrackingNumber]                      VARCHAR (50)    NOT NULL,
    [PriorityAlert]                       BIT             NOT NULL,
    [PriorityAlertEnhancementType]        INT             NOT NULL,
    [PriorityAlertDetailContent]          NVARCHAR (1024) NOT NULL,
    [DryIceWeight]                        FLOAT (53)      NOT NULL,
    [ContainsAlcohol]                     BIT             NOT NULL,
    [DangerousGoodsEnabled]               BIT             NOT NULL,
    [DangerousGoodsType]                  INT             NOT NULL,
    [DangerousGoodsAccessibilityType]     INT             NOT NULL,
    [DangerousGoodsCargoAircraftOnly]     BIT             NOT NULL,
    [DangerousGoodsEmergencyContactPhone] NVARCHAR (16)   NOT NULL,
    [DangerousGoodsOfferor]               NVARCHAR (128)  NOT NULL,
    [DangerousGoodsPackagingCount]        INT             NOT NULL,
    [HazardousMaterialNumber]             NVARCHAR (16)   NOT NULL,
    [HazardousMaterialClass]              NVARCHAR (8)    NOT NULL,
    [HazardousMaterialProperName]         NVARCHAR (64)   NOT NULL,
    [HazardousMaterialPackingGroup]       INT             NOT NULL,
    [HazardousMaterialQuantityValue]      FLOAT (53)      NOT NULL,
    [HazardousMaterialQuanityUnits]       INT             NOT NULL,
    CONSTRAINT [PK_FedExPackage] PRIMARY KEY CLUSTERED ([FedExPackageID] ASC),
    CONSTRAINT [FK_FedExPackage_FedExShipment] FOREIGN KEY ([ShipmentID]) REFERENCES [dbo].[FedExShipment] ([ShipmentID]) ON DELETE CASCADE
);


GO
CREATE TRIGGER [dbo].[FedExPackageDeleteTrigger]
    ON [dbo].[FedExPackage]
    AFTER DELETE
    AS  EXTERNAL NAME [ShipWorks.SqlServer].[Triggers].[FedExPackageDeleteTrigger]

