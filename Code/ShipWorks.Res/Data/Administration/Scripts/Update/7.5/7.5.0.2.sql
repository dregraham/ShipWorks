PRINT N'Altering [dbo].[EbayOrder]'
GO

SET NOCOUNT ON;
DECLARE @r INT;
SET @r = 1;

WHILE @r > 0
BEGIN
    ;with ItemsToUpdate (OrderID, EbayOrderID, EbayTransactionID) AS (
    SELECT TOP(1000) eo.OrderID, eo.EbayOrderID, eoi.EbayTransactionID
    FROM EbayOrder AS eo
    CROSS APPLY
        (SELECT TOP 1 EbayTransactionID
         FROM EbayOrderItem
         WHERE OrderID = eo.OrderID
         ORDER BY OrderID DESC) AS eoi
    WHERE eo.EbayOrderID = 0
    )

    update eo SET eo.EbayOrderID = eoi.EbayTransactionID FROM
    [dbo].[EbayOrder] eo inner join
    ItemsToUpdate eoi ON eo.OrderID = eoi.OrderID

    SET @r = @@ROWCOUNT;
END
