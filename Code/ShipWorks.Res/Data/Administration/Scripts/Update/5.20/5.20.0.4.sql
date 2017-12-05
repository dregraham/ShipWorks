CREATE NONCLUSTERED INDEX [IX_ProStoresOrder_ConfirmationNumber] 
	ON [dbo].[ProStoresOrder] ( [ConfirmationNumber] ASC )
GO
CREATE NONCLUSTERED INDEX [IX_ProStoresOrderSearch_ConfirmationNumber] 
	ON [dbo].[ProStoresOrderSearch] ( [ConfirmationNumber] ASC )
GO
