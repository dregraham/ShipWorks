CREATE TABLE [dbo].[OtherProfile] (
    [ShippingProfileID] BIGINT        NOT NULL,
    [Carrier]           NVARCHAR (50) NULL,
    [Service]           NVARCHAR (50) NULL,
    CONSTRAINT [PK_OtherProfile] PRIMARY KEY CLUSTERED ([ShippingProfileID] ASC),
    CONSTRAINT [FK_OtherProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
);

