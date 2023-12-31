﻿using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Editions.Brown;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.FeatureRestrictions
{
    public class ShipmentTypeRestrictionTest
    {
        [Fact]
        public void EditionFeature_ReturnsEditionFeatureShipmentType()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionFeature result = testObject.EditionFeature;

                Assert.Equal(EditionFeature.ShipmentType, result);
            }
        }

        [Fact]
        public void Check_DoesNotRestrictNoneCarrier()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> shipmentTypeRestriction =
                    new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
                    {
                        {
                            ShipmentTypeCode.None,
                            new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Disabled}
                        }
                    };

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.None);

                Assert.Equal(EditionRestrictionLevel.None, result);
            }
        }

        [Fact]
        public void Check_ReturnsNone_WhenShipmentTypeCodeEndicia_AndNoEndiciaAccounts()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> shipmentTypeRestriction =
                    new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>();

                Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>> endiciaAccountRepo =
                    mock.Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>>();

                endiciaAccountRepo.SetupGet(r => r.Accounts).Returns(new List<EndiciaAccountEntity>());

                Mock<IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>>> endiciaAccountRepoRepo =
                    mock.MockRepository
                        .Create<IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>>>();
                endiciaAccountRepoRepo.Setup(x => x[It.IsAny<ShipmentTypeCode>()]).Returns(endiciaAccountRepo.Object);
                mock.Provide(endiciaAccountRepoRepo.Object);

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.Endicia);

                Assert.Equal(EditionRestrictionLevel.None, result);
            }
        }

        [Fact]
        public void Check_ReturnsHidden_WhenShipmentTypeIsDisabled()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> shipmentTypeRestriction =
                    new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
                    {
                        {
                            ShipmentTypeCode.Usps,
                            new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Disabled}
                        }
                    };

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.Usps);

                Assert.Equal(EditionRestrictionLevel.Hidden, result);
            }
        }

        [Fact]
        public void Check_ReturnsNone_WhenShipmentTypeIsNotDisabled()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> shipmentTypeRestriction =
                    new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
                    {
                        {
                            ShipmentTypeCode.Usps,
                            new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.ShippingAccountConversion}
                        }
                    };

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.Usps);

                Assert.Equal(EditionRestrictionLevel.None, result);
            }
        }

        [Fact]
        public void Check_ReturnsNone_WhenShipmentTypeIsNull()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> shipmentTypeRestriction =
                    new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
                    {
                        {
                            ShipmentTypeCode.Usps,
                            new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.ShippingAccountConversion}
                        }
                    };

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, null);

                Assert.Equal(EditionRestrictionLevel.None, result);
            }
        }

        [Fact]
        public void Check_ReturnsNone_WhenDataIsNotShipmentType()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> shipmentTypeRestriction =
                    new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
                    {
                        {
                            ShipmentTypeCode.Usps,
                            new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.ShippingAccountConversion}
                        }
                    };

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, 123);

                Assert.Equal(EditionRestrictionLevel.None, result);
            }
        }

        [Fact]
        public void Check_ReturnsNone_WhenBrownAndAllowedShipmentType()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                var shipmentTypeRestriction = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                licenseCapabilities.SetupGet(l => l.UpsStatus).Returns(UpsStatus.Discount);

                Mock<IBrownEditionUtility> brownEditionUtility = mock.Mock<IBrownEditionUtility>();
                brownEditionUtility.Setup(u => u.IsShipmentTypeAllowed(It.IsAny<ShipmentTypeCode>()))
                    .Returns(true);

                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.AmazonSFP);

                Assert.Equal(EditionRestrictionLevel.None, result);
            }
        }

        [Fact]
        public void Check_ReturnsHidden_WhenBrownAndNotAllowedShipmentType()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                var shipmentTypeRestriction = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                licenseCapabilities.SetupGet(l => l.UpsStatus).Returns(UpsStatus.Discount);

                Mock<IBrownEditionUtility> brownEditionUtility = mock.Mock<IBrownEditionUtility>();
                brownEditionUtility.Setup(u => u.IsShipmentTypeAllowed(It.IsAny<ShipmentTypeCode>()))
                    .Returns(false);

                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.AmazonSFP);

                Assert.Equal(EditionRestrictionLevel.Hidden, result);
            }
        }

        [Fact]
        public void Check_ReturnsForbidden_WhenBrownDiscountAndShipmentTypeOther()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                var shipmentTypeRestriction = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                licenseCapabilities.SetupGet(l => l.UpsStatus).Returns(UpsStatus.Discount);

                Mock<IBrownEditionUtility> brownEditionUtility = mock.Mock<IBrownEditionUtility>();
                brownEditionUtility.Setup(u => u.IsShipmentTypeAllowed(It.IsAny<ShipmentTypeCode>()))
                    .Returns(false);

                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.Other);

                Assert.Equal(EditionRestrictionLevel.Forbidden, result);
            }
        }

        [Fact]
        public void Check_ReturnsNone_WhenBrownSubsidizedAndShipmentTypeOther()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                var shipmentTypeRestriction = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                licenseCapabilities.SetupGet(l => l.UpsStatus).Returns(UpsStatus.Subsidized);

                Mock<IBrownEditionUtility> brownEditionUtility = mock.Mock<IBrownEditionUtility>();
                brownEditionUtility.Setup(u => u.IsShipmentTypeAllowed(It.IsAny<ShipmentTypeCode>()))
                    .Returns(false);

                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.Other);

                Assert.Equal(EditionRestrictionLevel.None, result);
            }
        }

        [Fact]
        public void Check_ReturnsNone_WhenBrownAndPostalAvailabilityRestrictedAndIsPostalShipmentType()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                var shipmentTypeRestriction = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                licenseCapabilities.SetupGet(c => c.UpsStatus).Returns(UpsStatus.Subsidized);
                licenseCapabilities.SetupGet(c => c.PostalAvailability).Returns(BrownPostalAvailability.ApoFpoPobox);

                Mock<IBrownEditionUtility> brownEditionUtility = mock.Mock<IBrownEditionUtility>();
                brownEditionUtility.Setup(u => u.IsShipmentTypeAllowed(It.IsAny<ShipmentTypeCode>()))
                    .Returns(false);

                var postalUtility = mock.Mock<IPostalUtility>();
                postalUtility.Setup(p => p.IsPostalShipmentType(It.IsAny<ShipmentTypeCode>()))
                    .Returns(true);

                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.AmazonSFP);

                Assert.Equal(EditionRestrictionLevel.None, result);
            }
        }

        [Fact]
        public void Check_ReturnsHidden_WhenGivenWebToolsAndBrown()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                var shipmentTypeRestriction = new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>();

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                licenseCapabilities.SetupGet(c => c.UpsStatus).Returns(UpsStatus.Subsidized);
                licenseCapabilities.SetupGet(c => c.PostalAvailability).Returns(BrownPostalAvailability.AllServices);

                Mock<IBrownEditionUtility> brownEditionUtility = mock.Mock<IBrownEditionUtility>();
                brownEditionUtility.Setup(u => u.IsShipmentTypeAllowed(It.IsAny<ShipmentTypeCode>()))
                    .Returns(false);

                var postalUtility = mock.Mock<IPostalUtility>();
                postalUtility.Setup(p => p.IsPostalShipmentType(It.IsAny<ShipmentTypeCode>()))
                    .Returns(true);

                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();

                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.PostalWebTools);

                Assert.Equal(EditionRestrictionLevel.Hidden, result);
            }
        }

        [Fact]
        public void Check_ReturnsNone_WhenShipmentTypeIsBestRate_AndCapabilitiesIsInTrial_AndDoesNotAllowBestRate()
        {
            // Test that best rate is hidden when UPS accounts exist in ShipWorks
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> shipmentTypeRestriction =
                    new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
                    {
                        {
                            ShipmentTypeCode.Usps,
                            new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Disabled}
                        }
                    };

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                licenseCapabilities.SetupGet(c => c.IsBestRateAllowed).Returns(false);
                licenseCapabilities.SetupGet(c => c.TrialDetails).Returns(new TrialDetails(true, DateTime.MinValue));

                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();
                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.BestRate);

                Assert.Equal(EditionRestrictionLevel.None, result);
            }
        }

        [Fact]
        public void Check_ReturnsHidden_WhenShipmentTypeIsBestRate_AndCapabilitiesDoesNotAllowBestRate()
        {
            // Test that best rate is hidden when UPS accounts exist in ShipWorks
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> shipmentTypeRestriction =
                    new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
                    {
                        {
                            ShipmentTypeCode.Usps,
                            new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Disabled}
                        }
                    };

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                licenseCapabilities.SetupGet(c => c.IsBestRateAllowed).Returns(false);
                licenseCapabilities.SetupGet(c => c.TrialDetails).Returns(new TrialDetails(false, DateTime.MinValue));

                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();
                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.BestRate);

                Assert.Equal(EditionRestrictionLevel.Hidden, result);
            }
        }

        [Fact]
        public void Check_ReturnsNone_WhenShipmentTypeIsBestRate_AndLegacyBestRateIsDisabled_AndCapabilitiesAllowsBestRate()
        {
            // Test that best rate is available when there are not any UPS accounts in ShipWorks and ignores the
            // server setting for customers on the new pricing plan (since the ShipmentTypeRestriction only
            // applies to the CustomerLicense).
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> shipmentTypeRestriction =
                    new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
                    {
                        {
                            ShipmentTypeCode.BestRate,
                            new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Disabled}
                        }
                    };

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                licenseCapabilities.SetupGet(c => c.IsBestRateAllowed).Returns(true);
                licenseCapabilities.SetupGet(c => c.TrialDetails).Returns(new TrialDetails(false, DateTime.MinValue));
                
                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();
                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.BestRate);

                Assert.Equal(EditionRestrictionLevel.None, result);
            }
        }

        [Fact]
        public void Check_ReturnsNone_WhenShipmentTypeIsBestRate_AndCapabilitiesAllowsBestRat_AndUpsStatusIsNone()
        {
            // Test that best rate is hidden when  Best Rate is disabled and there are not any UPS accounts in ShipWorks
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> shipmentTypeRestriction =
                    new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
                    {
                        {
                            ShipmentTypeCode.Usps,
                            new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Disabled}
                        }
                    };

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                licenseCapabilities.SetupGet(c => c.UpsStatus).Returns(UpsStatus.None);
                licenseCapabilities.SetupGet(c => c.IsBestRateAllowed).Returns(true);
                licenseCapabilities.SetupGet(c => c.TrialDetails).Returns(new TrialDetails(false, DateTime.MinValue));
                
                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();
                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.BestRate);

                Assert.Equal(EditionRestrictionLevel.None, result);
            }
        }

        [Theory]
        [InlineData(UpsStatus.Discount)]
        [InlineData(UpsStatus.Subsidized)]
        [InlineData(UpsStatus.Tier1)]
        [InlineData(UpsStatus.Tier2)]
        [InlineData(UpsStatus.Tier3)]
        public void Check_ReturnsHidden_WhenShipmentTypeIsBestRate__AndCapabilitiesAllowBestRate_AndUpsStatusIsNotNone(UpsStatus discount)
        {
            // Test that best rate is hidden when  Best Rate is disabled and there are not any UPS accounts in ShipWorks
            using (AutoMock mock = AutoMock.GetLoose())
            {
                Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>> shipmentTypeRestriction =
                    new Dictionary<ShipmentTypeCode, IEnumerable<ShipmentTypeRestrictionType>>
                    {
                        {
                            ShipmentTypeCode.Usps,
                            new List<ShipmentTypeRestrictionType> {ShipmentTypeRestrictionType.Disabled}
                        }
                    };

                Mock<ILicenseCapabilities> licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                licenseCapabilities.SetupGet(c => c.ShipmentTypeRestriction).Returns(shipmentTypeRestriction);
                licenseCapabilities.SetupGet(c => c.UpsStatus).Returns(discount);
                licenseCapabilities.SetupGet(c => c.IsBestRateAllowed).Returns(true);
                licenseCapabilities.SetupGet(c => c.TrialDetails).Returns(new TrialDetails(false, DateTime.MinValue));

                ShipmentTypeRestriction testObject = mock.Create<ShipmentTypeRestriction>();
                EditionRestrictionLevel result = testObject.Check(licenseCapabilities.Object, ShipmentTypeCode.BestRate);

                Assert.Equal(EditionRestrictionLevel.Hidden, result);
            }
        }
    }
}