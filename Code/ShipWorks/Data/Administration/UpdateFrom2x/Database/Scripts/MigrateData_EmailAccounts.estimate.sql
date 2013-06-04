-- ShipWorks V2 to V3 Database Migration Script, generated by CodeSmith
-- For table v2m_EmailAccounts
-- Work Estimation Mode

-- declare table variables
DECLARE
    -- @MasterDatabase will be generated at runtime
    -- @IsArchive will be generated at runtime
    @workEstimate int

IF object_id('dbo.v2m_EmailAccounts') IS NULL
    -- query the renamed table name
    SELECT @workEstimate = COUNT(*) FROM dbo.EmailAccounts
ELSE
    -- query the original table name
    SELECT @workEstimate = COUNT(*) FROM dbo.v2m_EmailAccounts

SELECT @workEstimate as WorkEstimate