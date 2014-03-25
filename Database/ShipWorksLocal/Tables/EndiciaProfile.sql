CREATE TABLE [dbo].[EndiciaProfile] (
    [ShippingProfileID] BIGINT         NOT NULL,
    [EndiciaAccountID]  BIGINT         NULL,
    [StealthPostage]    BIT            NULL,
    [NoPostage]         BIT            NULL,
    [ReferenceID]       NVARCHAR (300) NULL,
    [RubberStamp1]      NVARCHAR (300) NULL,
    [RubberStamp2]      NVARCHAR (300) NULL,
    [RubberStamp3]      NVARCHAR (300) NULL,
    [ScanBasedReturn]   BIT            NULL,
    CONSTRAINT [PK_EndiciaProfile] PRIMARY KEY CLUSTERED ([ShippingProfileID] ASC),
    CONSTRAINT [FK_EndiciaProfile_PostalProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[PostalProfile] ([ShippingProfileID]) ON DELETE CASCADE
);

