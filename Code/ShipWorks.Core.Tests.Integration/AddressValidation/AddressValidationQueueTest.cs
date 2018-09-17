using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Enums;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Connection;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Syncfusion.XlsIO;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.AddressValidation
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class AddressValidationQueueTest : IDisposable
    {
        private readonly DataContext context;

        public AddressValidationQueueTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                AddressValidationWebClientValidateAddressResult validationResult = new AddressValidationWebClientValidateAddressResult();
                AddressValidationResult suggestion = new AddressValidationResult();
                suggestion.Street1 = "NEW SUGGESTION";
                validationResult.AddressValidationResults.Add(suggestion);

                mock.Override<IAddressValidationWebClient>()
                    .Setup(v => v.ValidateAddressAsync(It.IsAny<AddressAdapter>()))
                    .ReturnsAsync(validationResult);
            });
        }

        [Theory]
        [MemberData(nameof(ParseOrderResponse))]
        public async Task ValidationQueue_ValidatesOrdersWithPendingStatusSetsStatusToHasSuggestions_WhenStoreIsSetToValidateAndNotify(AddressAdapter testAddress)
        {
            Modify.Order(context.Order)
                .Set(x => testAddress.CopyTo(x.ShipPerson))
                .Set(x => x.ShipAddressValidationStatus = (int) AddressValidationStatusType.Pending)
                .Save();
            
            Modify.Store(context.Store)
                .Set(x => x.DomesticAddressValidationSetting = AddressValidationStoreSettingType.ValidateAndNotify)
                .Set(x => x.Enabled = true)
                .Save();
                        
            await AddressValidationQueue.ValidatePendingOrdersAndShipments();

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                sqlAdapter.FetchEntity(context.Order);
            }

            Assert.Equal(AddressValidationStatusType.HasSuggestions, (AddressValidationStatusType) context.Order.ShipAddressValidationStatus);

            AddressAdapter addressAfterOperation = new AddressAdapter();
            context.Order.ShipPerson.CopyTo(addressAfterOperation);

            Assert.True(testAddress.Equals(addressAfterOperation));
        }

        public static IEnumerable<object[]> ParseOrderResponse()
        {
            using (Stream stream = Assembly.GetAssembly(typeof(AddressValidationQueueTest)).GetManifestResourceStream("ShipWorks.Core.Tests.Integration.AddressValidation.TestAddresses.xlsx"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    reader.ReadToEnd();
                    stream.Seek(0, SeekOrigin.Begin);
                    using (ExcelEngine excelEngine = new ExcelEngine())
                    {
                        IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
                        foreach (IRange row in workbook.Worksheets[0].Rows.Skip(1))
                        {
                            yield return new[] {
                                new AddressAdapter()
                                    {
                                        Street1 = row.Cells[0].Value,
                                        Street2 = row.Cells[1].Value,
                                        Street3 = row.Cells[2].Value,
                                        City = row.Cells[3].Value,
                                        StateProvCode = row.Cells[4].Value,
                                        PostalCode = row.Cells[5].Value,
                                        CountryCode = row.Cells[6].Value
                                    }
                            };
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
