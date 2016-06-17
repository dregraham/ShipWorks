using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Editing.Rating;


namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    public class UspsExpress1RateConsolidatorTest
    {
        UspsExpress1RateConsolidator consolidator;

        Image Express1Logo;
        Image UspsLogo;

        public UspsExpress1RateConsolidatorTest()
        {
            consolidator = new UspsExpress1RateConsolidator();
            Express1Logo = new Bitmap(1, 1);
            UspsLogo = new Bitmap(1, 1);
        }

        [Fact]
        public void Conslidate_DoesNotThrow_WhenExpress1ApiThrows()
        {
            consolidator.Consolidate(CreateEmptyRateGroup(), CreateTaskThatThrows<Exception>());
        }

        [Fact]
        public void Consolidate_ReturnsOnlyUspsRates_WhenExpress1ApiThrows()
        {
            RateGroup rates = consolidator.Consolidate(CreatePopulatedRateGroup(), CreateTaskThatThrows<Exception>());
            foreach (RateResult rate in rates.Rates)
            {
                Assert.Equal(ShipmentTypeCode.Usps, rate.ShipmentType);
            }
        }

        [Fact]
        public void Consolidate_AddsErrorFooter_WhenExpress1ApiThrows()
        {
            RateGroup rates = consolidator.Consolidate(CreatePopulatedRateGroup(), CreateTaskThatThrows<Exception>());
            Assert.True(rates.FootnoteFactories.OfType<ExceptionsRateFootnoteFactory>().Any(), "None of the footnotes are exceptions");
        }

        [Fact]
        public void Consolidate_ReturnsOnlyUspsRates_WhenExpress1RatesAreEmpty()
        {
            RateGroup rateGroup = CreatePopulatedRateGroup();
            RateGroup rateResults = consolidator.Consolidate(rateGroup, CreateTaskThatReturns(new List<RateResult>()));

            Assert.Equal(rateGroup.Rates.Count, rateResults.Rates.Count);

            foreach (RateResult rate in rateGroup.Rates)
            {
                Assert.NotNull(rateResults.Rates.SingleOrDefault(x => x.Description == rate.Description && x.ShipmentType == rate.ShipmentType && x.AmountOrDefault == rate.AmountOrDefault));
            }
        }

        [Fact]
        public void Consolidate_ReturnsOnlyUspsRates_WhenNoExpress1RatesMatchServiceType()
        {
            RateGroup rateGroup = CreatePopulatedRateGroup();
            Task<RateGroup> task = CreateTaskThatReturnsRates(x => x.Add(new RateResult("Express 1 Rate", "1", 1.00m, new PostalRateSelection(PostalServiceType.InternationalExpress, PostalConfirmationType.None))));

            RateGroup rateResults = consolidator.Consolidate(rateGroup, task);

            Assert.Equal(rateGroup.Rates.Count, rateResults.Rates.Count);

            foreach (RateResult rate in rateGroup.Rates)
            {
                Assert.NotNull(rateResults.Rates.SingleOrDefault(x => x.Description == rate.Description && x.ShipmentType == rate.ShipmentType && x.AmountOrDefault == rate.AmountOrDefault));
            }
        }

        [Fact]
        public void Consolidate_ReturnsOnlyUspsRates_WhenExpress1RatesMatchButAreMoreExpensive()
        {
            RateGroup rateGroup = CreatePopulatedRateGroup();
            Task<RateGroup> task = CreateTaskThatReturnsRates(x =>
            {
                x.Add(new RateResult("\tDelivery Confirmation ($0.00)", string.Empty, 5.01m, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Delivery)));
                x.Add(new RateResult("\tSignature Confirmation ($2.00)", string.Empty, 7.01m, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Signature)));  
            });

            RateGroup rateResults = consolidator.Consolidate(rateGroup, task);

            Assert.Equal(rateGroup.Rates.Count, rateResults.Rates.Count);

            foreach (RateResult rate in rateGroup.Rates)
            {
                Assert.NotNull(rateResults.Rates.SingleOrDefault(x => x.Description == rate.Description && x.ShipmentType == rate.ShipmentType && x.AmountOrDefault == rate.AmountOrDefault));
            }
        }

        [Fact]
        public void Consolidate_ReturnsOnlyUspsRates_WhenExpress1RatesMatchAndAreEqual()
        {
            RateGroup rateGroup = CreatePopulatedRateGroup();
            Task<RateGroup> task = CreateTaskThatReturnsRates(x =>
            {
                x.Add(new RateResult("\tDelivery Confirmation ($0.00)", string.Empty, 5.00m, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Delivery)));
                x.Add(new RateResult("\tSignature Confirmation ($2.00)", string.Empty, 7.00m, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Signature)));
            });

            RateGroup rateResults = consolidator.Consolidate(rateGroup, task);

            Assert.Equal(rateGroup.Rates.Count, rateResults.Rates.Count);

            foreach (RateResult rate in rateGroup.Rates)
            {
                Assert.NotNull(rateResults.Rates.SingleOrDefault(x => x.Description == rate.Description && x.ShipmentType == rate.ShipmentType && x.AmountOrDefault == rate.AmountOrDefault));
            }
        }

        [Fact]
        public void Consolidate_ReturnsMergedRates_WhenExpress1RatesMatchAndAreLess()
        {
            RateGroup rateGroup = CreatePopulatedRateGroup();
            Task<RateGroup> task = CreateTaskThatReturnsRates(x =>
            {
                x.Add(new RateResult("\tDelivery Confirmation ($0.00)", string.Empty, 4.99m, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Delivery)) { ShipmentType = ShipmentTypeCode.Express1Usps });
                x.Add(new RateResult("\tSignature Confirmation ($2.00)", string.Empty, 6.99m, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Signature)) { ShipmentType = ShipmentTypeCode.Express1Usps });
            });

            RateGroup rateResults = consolidator.Consolidate(rateGroup, task);

            Assert.Equal(rateGroup.Rates.Count, rateResults.Rates.Count);

            foreach (RateResult rate in rateResults.Rates.Where(x => x.Tag != null && x.Selectable))
            {
                PostalRateSelection tag = rate.Tag as PostalRateSelection;
                ShipmentTypeCode expectedType = tag.ServiceType == PostalServiceType.PriorityMail ? ShipmentTypeCode.Express1Usps : ShipmentTypeCode.Usps;

                Assert.Equal(expectedType, rate.ShipmentType);
            }
        }

        [Fact]
        public void Consolidate_ReturnsExpress1Icon_WhenAllExpress1ConfirmationMatchAndAreLess()
        {
            RateGroup rateGroup = CreatePopulatedRateGroup();
            Task<RateGroup> task = CreateTaskThatReturnsRates(x =>
            {
                x.Add(new RateResult("Priority", "2") { Tag = new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.None), ProviderLogo = Express1Logo});
                x.Add(new RateResult("\tDelivery Confirmation ($0.00)", string.Empty, 4.99m, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Delivery)) { ShipmentType = ShipmentTypeCode.Express1Usps });
                x.Add(new RateResult("\tSignature Confirmation ($2.00)", string.Empty, 6.99m, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Signature)) { ShipmentType = ShipmentTypeCode.Express1Usps });
            });

            RateGroup rateResults = consolidator.Consolidate(rateGroup, task);

            RateResult priorityRateHeader = rateResults.Rates.Single(rate => rate.Selectable == false && (rate.Tag as PostalRateSelection).ServiceType == PostalServiceType.PriorityMail);

            Assert.Equal(Express1Logo, priorityRateHeader.ProviderLogo);
        }

        [Fact]
        public void Consolidate_ReturnsUspsIcon_WhenAllExpress1ConfirmationMatchAndAreSame()
        {
            RateGroup rateGroup = CreatePopulatedRateGroup();
            Task<RateGroup> task = CreateTaskThatReturnsRates(x =>
            {
                x.Add(new RateResult("Priority", "2") { Tag = new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.None), ProviderLogo = Express1Logo });
                x.Add(new RateResult("\tDelivery Confirmation ($0.00)", string.Empty, 5.00m, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Delivery)) { ShipmentType = ShipmentTypeCode.Express1Usps });
                x.Add(new RateResult("\tSignature Confirmation ($2.00)", string.Empty, 7.00m, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Signature)) { ShipmentType = ShipmentTypeCode.Express1Usps });
            });

            RateGroup rateResults = consolidator.Consolidate(rateGroup, task);

            RateResult priorityRateHeader = rateResults.Rates.Single(rate => rate.Selectable == false && (rate.Tag as PostalRateSelection).ServiceType == PostalServiceType.PriorityMail);

            Assert.Equal(UspsLogo, priorityRateHeader.ProviderLogo);
        }

        [Fact]
        public void Consolidate_ReturnsUspsIcon_WhenOneExpress1ConfirmationMatchIsLess()
        {
            RateGroup rateGroup = CreatePopulatedRateGroup();
            Task<RateGroup> task = CreateTaskThatReturnsRates(x =>
            {
                x.Add(new RateResult("Priority", "2") { Tag = new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.None), ProviderLogo = Express1Logo });
                x.Add(new RateResult("\tDelivery Confirmation ($0.00)", string.Empty, 5.00m, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Delivery)) { ShipmentType = ShipmentTypeCode.Express1Usps });
                x.Add(new RateResult("\tSignature Confirmation ($2.00)", string.Empty, 6.99m, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Signature)) { ShipmentType = ShipmentTypeCode.Express1Usps });
            });

            RateGroup rateResults = consolidator.Consolidate(rateGroup, task);

            RateResult priorityRateHeader = rateResults.Rates.Single(rate => rate.Selectable == false && (rate.Tag as PostalRateSelection).ServiceType == PostalServiceType.PriorityMail);

            Assert.Equal(UspsLogo, priorityRateHeader.ProviderLogo);
        }

        /// <summary>
        /// Create an Express1 rate retrieval task that returns the specified rate results
        /// </summary>
        private static Task<RateGroup> CreateTaskThatReturns(IEnumerable<RateResult> rateResults)
        {
            TaskCompletionSource<RateGroup> completionSource = new TaskCompletionSource<RateGroup>();
            completionSource.SetResult(new RateGroup(rateResults));
            return completionSource.Task;
        }

        /// <summary>
        /// Create an Express1 rate retrieval task that returns rates as configured
        /// </summary>
        private static Task<RateGroup> CreateTaskThatReturnsRates(Action<List<RateResult>> addAction)
        {
            List<RateResult> rateResults = new List<RateResult>();
            if (addAction != null)
            {
                addAction(rateResults);
            }

            foreach (RateResult rate in rateResults)
            {
                rate.CarrierDescription = "Express1";
                rate.ShipmentType = ShipmentTypeCode.Express1Usps;
            }

            TaskCompletionSource<RateGroup> completionSource = new TaskCompletionSource<RateGroup>();
            completionSource.SetResult(new RateGroup(rateResults));
            return completionSource.Task;
        }

        /// <summary>
        /// Create an Express1 rate retrieval task that throws an exception
        /// </summary>
        private static Task<RateGroup> CreateTaskThatThrows<T>() where T : Exception, new()
        {
            TaskCompletionSource<RateGroup> completionSource = new TaskCompletionSource<RateGroup>();
            completionSource.SetException(new T());
            return completionSource.Task;
        }

        /// <summary>
        /// Create a rate group that has no rates
        /// </summary>
        private static RateGroup CreateEmptyRateGroup()
        {
            return new RateGroup(new List<RateResult>());   
        }

        /// <summary>
        /// Create a rate group that's populated with normal looking rates
        /// </summary>
        private RateGroup CreatePopulatedRateGroup()
        {
            UspsAccountEntity account = new UspsAccountEntity();

            List<RateResult> rates = new List<RateResult>
            {
                new RateResult("Priority", "2") { Tag = new UspsPostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.None, account), ProviderLogo = UspsLogo },
                new RateResult("\tDelivery Confirmation ($0.00)", string.Empty, 5.00m, new UspsPostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Delivery, account)),
                new RateResult("\tSignature Confirmation ($2.00)", string.Empty, 7.00m, new UspsPostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.Signature, account)),
                new RateResult("Priority Mail Express", "1-2", 24.00m, new UspsPostalRateSelection(PostalServiceType.ExpressMail, PostalConfirmationType.None, account)),
                new RateResult("Media Mail", "6") { Tag = new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.None, account), ProviderLogo = UspsLogo },
                new RateResult("\tDelivery Confirmation ($0.50)", string.Empty, 2.50m, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Delivery, account)),
                new RateResult("\tSignature Confirmation ($2.00)", string.Empty, 4.00m, new UspsPostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.Signature, account)),
                new RateResult("Library Mail", "2-8") { Tag = new UspsPostalRateSelection(PostalServiceType.LibraryMail, PostalConfirmationType.None, account), ProviderLogo = UspsLogo },
                new RateResult("\tDelivery Confirmation ($0.50)", string.Empty, 2.00m, new UspsPostalRateSelection(PostalServiceType.LibraryMail, PostalConfirmationType.Delivery, account)),
                new RateResult("\tSignature Confirmation ($2.00)", string.Empty, 3.50m, new UspsPostalRateSelection(PostalServiceType.LibraryMail, PostalConfirmationType.Signature, account)),
                new RateResult("Parcel Select", "6") { Tag = new UspsPostalRateSelection(PostalServiceType.ParcelSelect, PostalConfirmationType.None, account), ProviderLogo = UspsLogo },
                new RateResult("\tDelivery Confirmation ($0.50)", string.Empty, 6.00m, new UspsPostalRateSelection(PostalServiceType.ParcelSelect, PostalConfirmationType.Delivery, account)),
                new RateResult("\tSignature Confirmation ($2.00)", string.Empty, 7.50m, new UspsPostalRateSelection(PostalServiceType.ParcelSelect, PostalConfirmationType.Signature, account))
            };

            foreach (RateResult rate in rates)
            {
                rate.CarrierDescription = "USPS";
                rate.ShipmentType = ShipmentTypeCode.Usps;
            }

            return new RateGroup(rates);
        }
    }
}
