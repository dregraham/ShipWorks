PRINT N'Altering [dbo].[EbayOrder]'
GO
;with ItemsToUpdate (OrderID, EbayOrderID, EbayItemID, EbayTransactionID) AS (
SELECT eo.OrderID, eo.EbayOrderID, eoi.EbayItemID, eoi.EbayTransactionID
FROM EbayOrder AS eo
CROSS APPLY
   (SELECT TOP 1 EbayItemID, EbayTransactionID
    FROM EbayOrderItem
    WHERE OrderID = eo.OrderID
    ORDER BY OrderID DESC) AS eoi
WHERE eo.EbayOrderID = eoi.EbayItemID
)
update eo SET eo.EbayOrderID = eoi.EbayTransactionID FROM
[dbo].[EbayOrder] eo inner join
ItemsToUpdate eoi ON eo.OrderID = eoi.OrderID
GO