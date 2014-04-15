PRINT N'Updating data begin.';

GO

UPDATE Store
SET AddressValidationSetting = 1
WHERE TypeCode = 1 OR TypeCode = 10 OR TypeCode = 18

GO

PRINT N'Update data complete.';

GO
