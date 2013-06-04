-- ShipWorks V2 to V3 Database Migration Script, generated by CodeSmith
-- For table Users
-- Work Estimation Mode

-- declare table variables
DECLARE
    -- @MasterDatabase will be generated at runtime
    -- @IsArchive will be generated at runtime
    @workEstimate int

IF object_id('dbo.Users') IS NOT NULL
    -- query the renamed table name
    SELECT @workEstimate = COUNT(*) FROM dbo.Users
ELSE
    SELECT @workEstimate = 0

SELECT @workEstimate as WorkEstimate