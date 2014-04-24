PRINT N'Updating data begin.';

GO

UPDATE Store
SET AddressValidationSetting = 1
WHERE TypeCode = 1 OR TypeCode = 10 OR TypeCode = 18

GO

-- Update UPS profiles to use address validation for residential determination
-- if it was using the default
UPDATE UpsProfile
SET ResidentialDetermination = 4
WHERE ResidentialDetermination = 0

GO

-- Update OnTrac profiles to use address validation for residential determination
-- if it was using the default
UPDATE OnTracProfile
SET ResidentialDetermination = 4
WHERE ResidentialDetermination = 0

GO

PRINT N'Update data complete.';

GO
