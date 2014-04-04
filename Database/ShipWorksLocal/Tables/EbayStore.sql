CREATE TABLE [dbo].[EbayStore] (
    [StoreID]                      BIGINT           NOT NULL,
    [eBayUserID]                   NVARCHAR (50)    NOT NULL,
    [eBayToken]                    TEXT             NOT NULL,
    [eBayTokenExpire]              DATETIME         NOT NULL,
    [AcceptedPaymentList]          VARCHAR (30)     NOT NULL,
    [DownloadItemDetails]          BIT              NOT NULL,
    [DownloadPayPalDetails]        BIT              NOT NULL,
    [PayPalApiCredentialType]      SMALLINT         NOT NULL,
    [PayPalApiUserName]            NVARCHAR (255)   NOT NULL,
    [PayPalApiPassword]            NVARCHAR (80)    NOT NULL,
    [PayPalApiSignature]           NVARCHAR (80)    NOT NULL,
    [PayPalApiCertificate]         VARBINARY (2048) NULL,
    [DomesticShippingService]      NVARCHAR (50)    NOT NULL,
    [InternationalShippingService] NVARCHAR (50)    NOT NULL,
    [FeedbackUpdatedThrough]       DATETIME         NULL,
    CONSTRAINT [PK_EbayStore] PRIMARY KEY CLUSTERED ([StoreID] ASC),
    CONSTRAINT [FK_EbayStore_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID])
);

