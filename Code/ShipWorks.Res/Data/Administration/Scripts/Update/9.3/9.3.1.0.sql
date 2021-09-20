PRINT N'Altering [dbo].[AmeriCommerceStore]'
GO
alter table [AmeriCommerceStore] alter column [Password] nvarchar(280) not null;
GO
PRINT N'Altering [dbo].[BuyDotComStore]'
GO
alter table [BuyDotComStore] alter column [FtpPassword] nvarchar(200) not null;
GO
PRINT N'Altering [dbo].[ChannelAdvisorStore]'
GO
alter table [ChannelAdvisorStore] alter column [RefreshToken] nvarchar(800) not null;
GO
PRINT N'Altering [dbo].[EmailAccount]'
GO
alter table [EmailAccount] alter column [IncomingPassword] nvarchar(600) not null;
GO
PRINT N'Altering [dbo].[EmailAccount]'
GO
alter table [EmailAccount] alter column [OutgoingPassword] nvarchar(600) not null;
GO
PRINT N'Altering [dbo].[EndiciaAccount]'
GO
alter table [EndiciaAccount] alter column [ApiInitialPassword] nvarchar(1000) not null;
GO
PRINT N'Altering [dbo].[EndiciaAccount]'
GO
alter table [EndiciaAccount] alter column [ApiUserPassword] nvarchar(1000) not null;
GO
PRINT N'Altering [dbo].[EndiciaAccount]'
GO
alter table [EndiciaAccount] alter column [WebPassword] nvarchar(1000) not null;
GO
PRINT N'Altering [dbo].[EtsyStore]'
GO
alter table [EtsyStore] alter column [OAuthToken] nvarchar(200) not null;
GO
PRINT N'Altering [dbo].[EtsyStore]'
GO
alter table [EtsyStore] alter column [OAuthTokenSecret] nvarchar(200) not null;
GO
PRINT N'Altering [dbo].[FtpAccount]'
GO
alter table [FtpAccount] alter column [Password] nvarchar(200) not null;
GO
PRINT N'Altering [dbo].[GenericModuleStore]'
GO
alter table [GenericModuleStore] alter column [ModulePassword] nvarchar(320) not null;
GO
PRINT N'Altering [dbo].[iParcelAccount]'
GO
alter table [iParcelAccount] alter column [Password] nvarchar(200) not null;
GO
PRINT N'Altering [dbo].[JetStore]'
GO
alter table [JetStore] alter column [Secret] nvarchar(400) not null;
GO
PRINT N'Altering [dbo].[MarketplaceAdvisorStore]'
GO
alter table [MarketplaceAdvisorStore] alter column [Password] nvarchar(200) not null;
GO
PRINT N'Altering [dbo].[MivaStore]'
GO
alter table [MivaStore] alter column [EncryptionPassphrase] nvarchar(200) not null;
GO
PRINT N'Altering [dbo].[OnTracAccount]'
GO
alter table [OnTracAccount] alter column [Password] nvarchar(200) not null;
GO
PRINT N'Altering [dbo].[OrderPaymentDetail]'
GO
alter table [OrderPaymentDetail] alter column [Value] nvarchar(400) not null;
GO
PRINT N'Altering [dbo].[ProStoresStore]'
GO
alter table [ProStoresStore] alter column [LegacyPassword] varchar(600) not null;
GO
PRINT N'Altering [dbo].[ShippingSettings]'
GO
alter table [ShippingSettings] alter column [UpsAccessKey] nvarchar(200) null;
GO
PRINT N'Altering [dbo].[ShippingSettings]'
GO
alter table [ShippingSettings] alter column [FedExPassword] nvarchar(200) null;
GO
PRINT N'Altering [dbo].[ShopSiteStore]'
GO
alter table [ShopSiteStore] alter column [Password] nvarchar(200) not null;
GO
PRINT N'Altering [dbo].[UpsAccount]'
GO
alter table [UpsAccount] alter column [Password] nvarchar(100) not null;
GO
PRINT N'Altering [dbo].[UspsAccount]'
GO
alter table [UspsAccount] alter column [Password] nvarchar(400) not null;
GO
PRINT N'Altering [dbo].[VolusionStore]'
GO
alter table [VolusionStore] alter column [WebPassword] varchar(280) not null;
GO
PRINT N'Altering [dbo].[YahooStore]'
GO
alter table [YahooStore] alter column [TrackingUpdatePassword] varchar(400) not null;
GO