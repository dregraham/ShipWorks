CREATE TABLE [dbo].[PostalProfile] (
    [ShippingProfileID]         BIGINT        NOT NULL,
    [Service]                   INT           NULL,
    [Confirmation]              INT           NULL,
    [Weight]                    FLOAT (53)    NULL,
    [PackagingType]             INT           NULL,
    [DimsProfileID]             BIGINT        NULL,
    [DimsLength]                FLOAT (53)    NULL,
    [DimsWidth]                 FLOAT (53)    NULL,
    [DimsHeight]                FLOAT (53)    NULL,
    [DimsWeight]                FLOAT (53)    NULL,
    [DimsAddWeight]             BIT           NULL,
    [NonRectangular]            BIT           NULL,
    [NonMachinable]             BIT           NULL,
    [CustomsContentType]        INT           NULL,
    [CustomsContentDescription] NVARCHAR (50) NULL,
    [ExpressSignatureWaiver]    BIT           NULL,
    [SortType]                  INT           NULL,
    [EntryFacility]             INT           NULL,
    CONSTRAINT [PK_PostalProfile] PRIMARY KEY CLUSTERED ([ShippingProfileID] ASC),
    CONSTRAINT [FK_PostalProfile_ShippingProfile] FOREIGN KEY ([ShippingProfileID]) REFERENCES [dbo].[ShippingProfile] ([ShippingProfileID]) ON DELETE CASCADE
);

