UPDATE ActionTask
SET InputSource = -1
WHERE TaskIdentifier IN ('PurgeDatabase');
GO
