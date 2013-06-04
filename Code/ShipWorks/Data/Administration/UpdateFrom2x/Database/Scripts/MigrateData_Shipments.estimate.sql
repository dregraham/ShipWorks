-- ShipWorks V2 to V3 Database Migration Script, generated by CodeSmith
-- For table v2m_Shipments
-- Work Estimation Mode

-- declare table variables
DECLARE
    -- @MasterDatabase will be generated at runtime
    -- @IsArchive will be generated at runtime
    @workEstimate int

IF object_id('dbo.v2m_Shipments') IS NULL
    -- query the renamed table name
    SELECT @workEstimate = COUNT(*) FROM dbo.Shipments
ELSE
    -- query the original table name
    SELECT @workEstimate = COUNT(*) FROM dbo.v2m_Shipments

SELECT @workEstimate as WorkEstimate