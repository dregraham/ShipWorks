WITH AffectedLeafs AS
(
	SELECT FilterID FROM FilterOrderMatch WHERE OrderID = 97232006
),
IncludedInFilters AS
(
    SELECT *, 1 as Included
    FROM FilterNode
    WHERE FilterID IN (SELECT FilterID FROM AffectedLeafs)

    UNION ALL

    SELECT f.*, 1 as Included
    FROM IncludedInFilters a INNER JOIN FilterNode f ON f.FilterNodeID = a.ParentFilterNodeID
),
NotInFilters AS
(
    SELECT *, 0 as Included
    FROM FilterNode
    WHERE FilterNodeID NOT IN (SELECT FilterNodeID FROM IncludedInFilters)
)
SELECT * From NotInFilters UNION
SELECT * From IncludedInFilters