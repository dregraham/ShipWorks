CREATE TABLE [dbo].[OnTracProfile] (
    [ShippingProfileID]        BIGINT         NOT NULL,
    [OnTracAccountID]          BIGINT         NULL,
    [ResidentialDetermination] INT            NULL,
    [Service]                  INT            NULL,
    [SaturdayDelivery]         BIT            NULL,
    [SignatureRequired]        BIT            NULL,
    [PackagingType]            INT            NULL,
    [Weight]                   FLOAT (53)     NULL,
    [DimsProfileID]            BIGINT         NULL,
    [DimsLength]               FLOAT (53)     NULL,
    [DimsWidth]                FLOAT (53)     NULL,
    [DimsHeight]               FLOAT (53)     NULL,
    [DimsWeight]               FLOAT (53)     NULL,
    [DimsAddWeight]            BIT            NULL,
    [Reference1]               NVARCHAR (300) NULL,
    [Reference2]               NVARCHAR (300) NULL,
    [Instructions]             NVARCHAR (300) NULL,
    CONSTRAINT [PK_OnTracProfile] PRIMARY KEY CLUSTERED ([ShippingProfileID] ASC),
    CONSTRAINT [FK_OnTracProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID])
);

