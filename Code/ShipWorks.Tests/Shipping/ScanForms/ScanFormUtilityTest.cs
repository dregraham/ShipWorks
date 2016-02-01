using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.ScanForms;
using Moq;
using SandRibbon = Divelements.SandRibbon;
using System.Windows.Forms;

namespace ShipWorks.Tests.Shipping.ScanForms
{
    public class ScanFormUtilityTest
    {
        private Mock<IScanFormAccountRepository> accountRepository;
        private Mock<IScanFormCarrierAccount> carrierAccount;
        
        private Mock<IScanFormBatchPrinter> printer;
        private Mock<IScanFormBatchShipmentRepository> batchShipmentRepository;

        private List<IScanFormAccountRepository> repositories;

        private SandRibbon.MenuItem createSandMenuItem;
        private SandRibbon.Menu printMenu;

        public ScanFormUtilityTest()
        {
            carrierAccount = new Mock<IScanFormCarrierAccount>();            

            accountRepository = new Mock<IScanFormAccountRepository>();
            accountRepository.Setup(r => r.GetAccounts())
                .Returns
                (
                    new List<IScanFormCarrierAccount>()
                    {
                        carrierAccount.Object
                    }
                );

            printer = new Mock<IScanFormBatchPrinter>();
            printer.Setup(p => p.Print(It.IsAny<IWin32Window>(), It.IsAny<ScanFormBatch>())).Returns(true);

            batchShipmentRepository = new Mock<IScanFormBatchShipmentRepository>();
            batchShipmentRepository.Setup(b => b.GetShipmentCount(It.IsAny<ScanFormBatch>())).Returns(1);

            ScanForm sampleForm = new ScanForm(carrierAccount.Object, 123, 1000, string.Empty, DateTime.Parse("10/26/2012 3:30 PM"));
            ScanFormBatch sampleBatch = new ScanFormBatch(carrierAccount.Object, printer.Object, new List<ScanForm> { sampleForm }, batchShipmentRepository.Object)
            {
                CreatedDate = DateTime.Parse("10/26/2012 3:30 PM")
            };

            carrierAccount.Setup(c => c.GetDescription()).Returns("Carrier Description");
            carrierAccount.Setup(c => c.GetExistingScanFormBatches()).Returns(new List<ScanFormBatch> { sampleBatch });

            repositories = new List<IScanFormAccountRepository>();
            repositories.Add(accountRepository.Object);
            
            createSandMenuItem = new SandRibbon.MenuItem();
            printMenu = new SandRibbon.Menu();
        }

        #region CreateScanFormMenu Tests

        [Fact]
        public void PopulateCreateScanFormMenu_DelegatesToAccountRepository()
        {
            ScanFormUtility.PopulateCreateScanFormMenu(createSandMenuItem, repositories);

            accountRepository.Verify(r => r.GetAccounts(), Times.Once());
        }

        [Fact]
        public void PopulateCreateScanFormMenu_HasZeroChildElments_WhenOneAccount()
        {
            ScanFormUtility.PopulateCreateScanFormMenu(createSandMenuItem, repositories);
            Assert.Equal(0, createSandMenuItem.Items.Count);
        }

        [Fact]
        public void PopulateCreateScanFormMenu_MenuTextEndsWithEllipsis_WhenOneAccount()
        {
            ScanFormUtility.PopulateCreateScanFormMenu(createSandMenuItem, repositories);
            Assert.Equal("...", createSandMenuItem.Text.Substring(createSandMenuItem.Text.Length - 3, 3));
        }

        [Fact]
        public void PopulateCreateScanFormMenu_MenuContainsChildMenu_WhenMoreThanOneAccount()
        {
            // Add another repository to our list to run this test
            repositories.Add(accountRepository.Object);

            ScanFormUtility.PopulateCreateScanFormMenu(createSandMenuItem, repositories);

            Assert.NotNull(createSandMenuItem.Items);
            Assert.Equal(1, createSandMenuItem.Items.Count);
            Assert.IsAssignableFrom<SandRibbon.Menu>(createSandMenuItem.Items[0]);
        }

        [Fact]
        public void PopulateCreateScanFormMenu_SubMenuChildItemsEqualsAccounts_WhenMoreThanOneAccount()
        {
            // Add another repository to our list to run this test
            repositories.Add(accountRepository.Object);

            ScanFormUtility.PopulateCreateScanFormMenu(createSandMenuItem, repositories);

            SandRibbon.Menu childMenu = (SandRibbon.Menu)createSandMenuItem.Items[0];
            Assert.Equal(repositories.Count, childMenu.Items.Count);
        }

        [Fact]
        public void PopulateCreateScanFormMenu_SubMenuChildItemsTextDoNotEndWithEllipsis_WhenMoreThanOneAccount()
        {
            // Add another repository to our list to run this test
            repositories.Add(accountRepository.Object);

            ScanFormUtility.PopulateCreateScanFormMenu(createSandMenuItem, repositories);

            SandRibbon.Menu childMenu = (SandRibbon.Menu)createSandMenuItem.Items[0];

            foreach (SandRibbon.MenuItem item in childMenu.Items)
            {
                Assert.NotEqual("...", item.Text.Substring(item.Text.Length - 3, 3));
            }
        }

        #endregion CreateScanFormMenu Tests


        [Fact]
        public void PopulatePrintScanFormMenu_DelegatesToAccountRepository()
        {
            ScanFormUtility.PopulatePrintScanFormMenu(printMenu, repositories);

            accountRepository.Verify(r => r.GetAccounts(), Times.Once());
        }

        [Fact]
        public void PopulatePrintScanFormMenu_HasZeroChildElments_WhenOneAccount()
        {
            ScanFormUtility.PopulatePrintScanFormMenu(printMenu, repositories);
            Assert.Equal(0, createSandMenuItem.Items.Count);
        }


        [Fact]
        public void PopulatePrintScanFormMenu_MenuTextContainsLocalTimeAndShipmentCount_WhenOneAccount()
        {
            ScanFormUtility.PopulatePrintScanFormMenu(printMenu, repositories);

            SandRibbon.MenuItem childMenuItem = (SandRibbon.MenuItem)printMenu.Items[0];
            DateTime expectedDateTime = DateTime.Parse("10/26/2012 3:30 PM").ToLocalTime();
            string expectedString = string.Format("{0:MM/dd/yy h:mm tt} ({1} shipments)", expectedDateTime, 1);

            Assert.Equal(expectedString, childMenuItem.Text);
        }

        [Fact]
        public void PopulatePrintScanFormMenu_ChildMenuItemsEqualNumberOfAccounts_WhenMoreThanOneAccount()
        {
            // Add another repository to our list to run this test
            repositories.Add(accountRepository.Object);

            ScanFormUtility.PopulatePrintScanFormMenu(printMenu, repositories);

            Assert.Equal(repositories.Count, printMenu.Items.Count);
        }

        [Fact]
        public void PopulatePrintScanFormMenu_FormMenuItemsOrderedDescending_WhenMoreThanOneBatch()
        {
            // Add another form to our list of existing forms
            carrierAccount.Setup(c => c.GetExistingScanFormBatches())
                                       .Returns
                                        (
                                            new List<ScanFormBatch>
                                            {
                                                new ScanFormBatch(carrierAccount.Object, printer.Object, new List<ScanForm> { new ScanForm(carrierAccount.Object, 123, 1000, string.Empty, DateTime.Parse("10/26/2012 3:30 PM")) }, batchShipmentRepository.Object) { CreatedDate = DateTime.Parse("10/26/2012 3:30 PM")},
                                                new ScanFormBatch(carrierAccount.Object, printer.Object, new List<ScanForm> { new ScanForm(carrierAccount.Object, 123, 1000, string.Empty, DateTime.Parse("10/22/2012 5:48 AM")) }, batchShipmentRepository.Object) { CreatedDate = DateTime.Parse("10/22/2012 5:48 AM")},
                                                new ScanFormBatch(carrierAccount.Object, printer.Object, new List<ScanForm> { new ScanForm(carrierAccount.Object, 123, 1000, string.Empty, DateTime.Parse("10/28/2012 8:24 PM")) }, batchShipmentRepository.Object) { CreatedDate = DateTime.Parse("10/28/2012 8:24 PM")}
                                            }
                                        );

            ScanFormUtility.PopulatePrintScanFormMenu(printMenu, repositories);

            // Grab the first menu item and make sure it's the latest one in the list
            SandRibbon.MenuItem childMenuItem = (SandRibbon.MenuItem)printMenu.Items[0];
            DateTime expectedDateTime = DateTime.Parse("10/28/2012 8:24 PM").ToLocalTime();
            string expectedString = string.Format("{0:MM/dd/yy h:mm tt} ({1} shipments)", expectedDateTime, 1);

            Assert.Equal(expectedString, childMenuItem.Text);
        }

        [Fact]
        public void PopulatePrintScanFormMenu_MenuItemText()
        {
            // Make our lives easier and order the forms by create date so we don't have to figure it out in the assertions
            List<ScanFormBatch> batches = new List<ScanFormBatch>
                                            {
                                                new ScanFormBatch(carrierAccount.Object, printer.Object, new List<ScanForm> { new ScanForm(carrierAccount.Object, 123, 1000, string.Empty, DateTime.Parse("10/26/2012 3:30 PM")) }, batchShipmentRepository.Object) { CreatedDate = DateTime.Parse("10/26/2012 3:30 PM")},
                                                new ScanFormBatch(carrierAccount.Object, printer.Object, new List<ScanForm> { new ScanForm(carrierAccount.Object, 123, 1000, string.Empty, DateTime.Parse("10/22/2012 5:48 AM")) }, batchShipmentRepository.Object) { CreatedDate = DateTime.Parse("10/22/2012 5:48 AM")},
                                                new ScanFormBatch(carrierAccount.Object, printer.Object, new List<ScanForm> { new ScanForm(carrierAccount.Object, 123, 1000, string.Empty, DateTime.Parse("10/28/2012 8:24 PM")) }, batchShipmentRepository.Object) { CreatedDate = DateTime.Parse("10/28/2012 8:24 PM")}
                                            }.OrderByDescending(b => b.CreatedDate).ToList();

            carrierAccount.Setup(c => c.GetExistingScanFormBatches()).Returns(batches.OrderByDescending(f => f.CreatedDate));


            ScanFormUtility.PopulatePrintScanFormMenu(printMenu, repositories);

            for (int i = 0; i < printMenu.Items.Count; i++)
            {
                SandRibbon.MenuItem childMenuItem = (SandRibbon.MenuItem)printMenu.Items[i];
                DateTime expectedDateTime = batches[i].CreatedDate.ToLocalTime();
                string expectedString = string.Format("{0:MM/dd/yy h:mm tt} ({1} shipments)", expectedDateTime, batches[i].ShipmentCount);

                Assert.Equal(expectedString, childMenuItem.Text);
            }
        }
        
        [Fact]
        public void PopulatePrintScanFormMenu_SubMenuChildItemIsNotEnabled_WhenNoScanFormBatchesForAccount()
        {
            carrierAccount.Setup(c => c.GetExistingScanFormBatches()).Returns(new List<ScanFormBatch>());

            ScanFormUtility.PopulatePrintScanFormMenu(printMenu, repositories);

            SandRibbon.MenuItem childMenuItem = (SandRibbon.MenuItem)printMenu.Items[0];
            Assert.False(childMenuItem.Enabled);
        }

        [Fact]
        public void PopulatePrintScanFormMenu_SubMenuChildItemTextIsNone_WhenNoScanFormBatchesForAccount()
        {
            carrierAccount.Setup(c => c.GetExistingScanFormBatches()).Returns(new List<ScanFormBatch>());

            ScanFormUtility.PopulatePrintScanFormMenu(printMenu, repositories);

            SandRibbon.MenuItem childMenuItem = (SandRibbon.MenuItem)printMenu.Items[0];
            Assert.Equal("(none)", childMenuItem.Text);
        }

        [Fact]
        public void PopulatePrintScanFormMenu_MenuItemTextIsNone_WhenAccountCountIsZero()
        {
            ScanFormUtility.PopulatePrintScanFormMenu(printMenu, new List<IScanFormAccountRepository>());

            SandRibbon.MenuItem childMenuItem = (SandRibbon.MenuItem) printMenu.Items[0];
            Assert.Equal("(none)", childMenuItem.Text);
        }
    }
}
