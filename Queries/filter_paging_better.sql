;WITH NumberedResults as
(
    select o.OrderID, o.RowVersion, ROW_NUMBER() OVER(ORDER BY OrderDate, OrderID) as RowNumber
    from [Order] o
),
FindDescendants AS
(
    SELECT *
    FROM FilterNode
    WHERE FilterID = 9010

    UNION ALL

    SELECT f.*
    FROM FindDescendants d INNER JOIN FilterNode f ON d.FilterNodeID = f.ParentFilterNodeID

),
FindFilterLeafs AS
(
	SELECT f.FilterID
	FROM FindDescendants f
	WHERE (SELECT COUNT(FilterNodeID) FROM FilterNode WHERE ParentFilterNodeID = f.FilterNodeID) = 0
),
FilteredResults as
(
    select * FROM NumberedResults o where o.RowNumber > 0 AND 
		OrderID IN (SELECT m.OrderID FROM FilterOrderMatch m WHERE m.FilterID IN (SELECT l.FilterID FROM FindFilterLeafs l))
)
SELECT top(100) * FROM FilteredResults ORDER BY RowNumber ASC
