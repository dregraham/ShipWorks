PRINT N'Removing AmazonSFPProfile Reference1 values that are not allowed'
GO

UPDATE AmazonSFPProfile
SET Reference1 = ''
WHERE Reference1 LIKE '%[^0-9a-zA-Z]%' 
