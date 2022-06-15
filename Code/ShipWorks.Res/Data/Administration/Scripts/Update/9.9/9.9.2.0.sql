PRINT N'ALTERING [dbo].[AmazonSFPServiceType]'
GO
IF COL_LENGTH(N'[dbo].[AmazonSFPServiceType]', N'PlatformApiCode') IS NULL
	ALTER TABLE [dbo].[AmazonSFPServiceType] ADD [PlatformApiCode] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

PRINT N'CREATING [#SFPServices] TEMP TABLE'
GO
CREATE TABLE #SFPServices (
	[ApiValue] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PlatformApiCode] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

PRINT N'POPULATING [#SFPServices] TEMP TABLE'
GO
INSERT INTO #SFPServices VALUES
('USPS_PTP_FC', 'USPS First Class Mail', 'amazon_usps_first_class_mail'),
('USPS_PTP_MM', 'USPS Media Mail', 'amazon_usps_media_mail'),
('USPS_PTP_PRI', 'USPS Priority Mail', 'amazon_usps_priority_mail_package'),
('USPS_PTP_PRI_SFRB', 'USPS Priority Mail Small Flat Rate Box', 'amazon_usps_priority_mail_small_flat_rate_box'),
('USPS_PTP_PRI_MFRB', 'USPS Priority Mail Medium Flat Rate Box', 'amazon_usps_priority_mail_medium_flat_rate_box'),
('USPS_PTP_PRI_LFRB', 'USPS Priority Mail Large Flat Rate Box', 'amazon_usps_priority_mail_large_flat_rate_box'),
('USPS_PTP_PRI_FRE', 'USPS Priority Mail Flat Rate Envelope', 'amazon_usps_priority_mail_flat_rate_envelope'),
('USPS_PTP_PRI_LFRE', 'USPS Priority Mail Legal Flat Rate Envelope', 'amazon_usps_priority_mail_legal_flat_rate_envelope'),
('USPS_PTP_PRI_PFRE', 'USPS Priority Mail Padded Flat Rate Envelope', 'amazon_usps_priority_mail_padded_flat_rate_envelope'),
('USPS_PTP_PRI_RA', 'USPS Priority Mail Regional Rate Box A', 'amazon_usps_priority_mail_regional_rate_box_a'),
('USPS_PTP_PRI_RB', 'USPS Priority Mail Regional Rate Box B', 'amazon_usps_priority_mail_regional_rate_box_b'),
('USPS_PTP_EXP', 'USPS Priority Mail Express', 'amazon_stamps_usps_priority_mail_express_package'),
('USPS_PTP_EXP_FRE', 'USPS Priority Mail Express Flat Rate Envelope', 'amazon_usps_priority_mail_express_flat_rate_envelope'),
('USPS_PTP_EXP_LFRE', 'USPS Priority Mail Express Legal Flat Rate Envelope', 'amazon_usps_priority_mail_express_legal_flat_rate_envelope'),
('USPS_PTP_EXP_PFRE', 'USPS Priority Mail Express Padded Flat Rate Envelope', 'amazon_usps_priority_mail_express_padded_flat_rate_envelope'),
('USPS_PTP_PSBN', 'USPS Parcel Select', 'amazon_usps_parcel_select'),
('FEDEX_PTP_GROUND', 'FedEx Ground®', 'amazon_fedex_ground'),
('FEDEX_PTP_HOME_DELIVERY', 'FedEx Home Delivery®', 'amazon_fedex_home_delivery'),
('FEDEX_PTP_SECOND_DAY', 'FedEx 2Day®', 'amazon_fedex_2day'),
('FEDEX_PTP_SECOND_DAY_AM', 'FedEx 2Day® A.M.', 'amazon_fedex_2day_am'),
('FEDEX_PTP_EXPRESS_SAVER', 'FedEx Express Saver®', 'amazon_fedex_express_saver'),
('FEDEX_PTP_STANDARD_OVERNIGHT', 'FedEx Standard Overnight®', 'amazon_fedex_standard_overnight'),
('FEDEX_PTP_PRIORITY_OVERNIGHT', 'FedEx Priority Overnight®', 'amazon_fedex_priority_overnight'),
('UPS_PTP_GND', 'UPS Ground', 'amazon_ups_ground'),
('UPS_PTP_3DAY_SELECT', 'UPS 3 Day Select', 'amazon_ups_3_day_select'),
('UPS_PTP_2ND_DAY_AIR', 'UPS 2nd Day Air', 'amazon_ups_2nd_day_air'),
('UPS_PTP_NEXT_DAY_AIR_SAVER', 'UPS Next Day Air Saver', 'amazon_ups_next_day_air_saver'),
('UPS_PTP_NEXT_DAY_AIR', 'UPS Next Day Air', 'amazon_ups_next_day_air'),
('FEDEX_PTP_EXPRESS_SAVER_ONE_RATE', 'FedEx Express Saver One Rate®', 'amazon_fedex_express_saver_one_rate'),
('FEDEX_PTP_PRIORITY_OVERNIGHT_ONE_RATE', 'FedEx Priority Overnight One Rate®', 'amazon_fedex_priority_overnight_rate_one'),
('FEDEX_PTP_STANDARD_OVERNIGHT_ONE_RATE', 'FedEx Standard Overnight One Rate®', 'amazon_fedex_standard_overnight_one_rate'),
('FEDEX_PTP_SECOND_DAY_ONE_RATE', 'FedEx Second Day One Rate®', 'amazon_fedex_second_day_one_rate'),
('FEDEX_PTP_SECOND_DAY_AM_ONE_RATE', 'FedEx Second Day AM One Rate®', 'amazon_fedex_second_day_am_one_rate'),
('ONTRAC_MFN_GROUND', 'OnTrac Ground', 'amazon_ontrac_ground'),
('USPS_PTP_PRI_CUBIC', 'USPS Priority Mail Cubic', 'amazon_usps_priority_mail_cubic'),
('DYNAMEX_PTP_RUSH', 'DYNAMEX Rush', 'amazon_dynamex_rush'),
('DYNAMEX_PTP_SAME', 'DYNAMEX Same Day', 'amazon_dynamex_sameday'),
('ROYAL_MFN_TRACKED_24_LTR', 'RM T24 Letter', 'amazon_rm_t24_ltr'),
('ROYAL_MFN_TRACKED_48_LTR', 'RM T48 Letter', 'amazon_rm_t48_ltr'),
('ROYAL_MFN_TRACKED_24', 'RM T24 Parcel', 'amazon_rm_t24_prcl'),
('ROYAL_MFN_TRACKED_48', 'RM T48 Parcel', 'amazon_rm_t48_prcl'),
('DPD_UK_MFN_PAK', 'DPD Next Day Expresspak', 'amazon_dpd_uk_expak'),
('DPD_UK_MFN_PAK_SAT', 'DPD Next Day Expresspak Saturday', 'amazon_dpd_uk_expak_sat'),
('DPD_UK_MFN_PARCEL', 'DPD Next Day Parcel', 'amazon_dpd_uk_parc'),
('DPD_UK_MFN_PARCEL_SAT', 'DPD Next Day Parcel Saturday', 'amazon_dpd_uk_parc_sat'),
('prime-premium-uk-mfn', 'Amazon Shipping Prime Premium', 'amazon_as_uk_prime_prem'),
('econ-uk-mfn', 'Amazon Shipping Economy', 'amazon_as_uk_econ'),
('UPS_PTP_NEXT_DAY_AIR_SAT', 'UPS Next Day Air® (Saturday)', 'amazon_ups_next_day_air_sat'),
('UPS_PTP_2ND_DAY_AIR_SAT', 'UPS 2nd Day Air® (Saturday)', 'amazon_ups_second_day_air_sat'),
('FEDEX_PTP_PRI_OVERNIGHT_SAT', 'FedEx Priority Overnight® (Saturday)', 'amazon_fedex_priority_overnight_sat'),
('FEDEX_PTP_PRI_OVERN_ONE_R_SAT', 'FedEx Priority Overnight® One Rate (Saturday)', 'amazon_fedex_priority_overnight_one_rate_sat'),
('FEDEX_PTP_SECOND_DAY_SAT', 'FedEx 2Day® (Saturday)', 'amazon_fedex_two_day_sat'),
('FEDEX_PTP_SEC_DAY_ONE_RATE_SAT', 'Fedex 2Day® One Rate (Saturday)', 'amazon_fedex_two_day_one_rate_sat')
GO

PRINT N'UPDATING [dbo].[AmazonSFPServiceType] TABLE FROM [#SFPServices] TEMP TABLE'
GO
UPDATE AmazonSFPServiceType 
SET AmazonSFPServiceType.Description = SFP.Description, AmazonSFPServiceType.PlatformApiCode = SFP.PlatformApiCode
FROM AmazonSFPServiceType ASFP
INNER JOIN #SFPServices SFP
ON ASFP.ApiValue = SFP.ApiValue
GO

PRINT N'INSERTING MISSING VALUES FROM [#SFPServices] TEMP TABLE TO [dbo].[AmazonSFPServiceType]'
GO
INSERT INTO AmazonSFPServiceType (ApiValue, Description, PlatformApiCode)
SELECT ApiValue, Description, PlatformApiCode
FROM #SFPServices
WHERE NOT EXISTS (SELECT ApiValue FROM AmazonSFPServiceType WHERE AmazonSFPServiceType.ApiValue = #SFPServices.ApiValue)
GO

PRINT N'DROPPING [#SFPServices] TEMP TABLE'
GO
DROP TABLE #SFPServices
GO
