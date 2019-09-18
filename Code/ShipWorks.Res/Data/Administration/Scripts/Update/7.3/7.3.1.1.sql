PRINT N'Altering [dbo].[EbayOrder]'
GO
;with ItemsToUpdate (OrderID, EbayOrderID, EbayItemID) AS (
SELECT eo.OrderID, eo.EbayOrderID, eoi.EbayItemID
FROM EbayOrder AS eo
CROSS APPLY
    (SELECT TOP 1 EbayItemID
     FROM EbayOrderItem
     WHERE OrderID = eo.OrderID
     ORDER BY OrderID DESC) AS eoi
WHERE eo.EbayOrderID = 0
)
update eo SET eo.EbayOrderID = eoi.EbayItemID FROM
[dbo].[EbayOrder] eo inner join
ItemsToUpdate eoi ON eo.OrderID = eoi.OrderID
GO
