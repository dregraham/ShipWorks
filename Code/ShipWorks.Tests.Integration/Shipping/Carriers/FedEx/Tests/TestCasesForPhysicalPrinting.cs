using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.Tests
{
    public static class TestCasesForPhysicalPrinting
    {
        private static List<string> testCasesForLaser = new List<string>();
        private static List<string> testCasesForThermal = new List<string>();
        private static List<string> dangerousGoodsTestCases = new List<string>();

        static TestCasesForPhysicalPrinting()
        {
            testCasesForLaser.Add("FOR1");
            testCasesForLaser.Add("323390"); // Same as FOR1

            testCasesForLaser.Add("LB1");
            testCasesForLaser.Add("323405");
            testCasesForLaser.Add("LB2");
            testCasesForLaser.Add("323406");

            testCasesForLaser.Add("ES4");
            testCasesForLaser.Add("323365"); // Same as ES4

            testCasesForLaser.Add("IPE1");
            testCasesForLaser.Add("413242");
            testCasesForLaser.Add("IPE2");
            testCasesForLaser.Add("413243");
            testCasesForLaser.Add("LIB1");
            testCasesForLaser.Add("413244");
            testCasesForLaser.Add("LIB2");
            testCasesForLaser.Add("413245");

            testCasesForLaser.Add("492");
            testCasesForLaser.Add("502251");
            testCasesForLaser.Add("408");
            testCasesForLaser.Add("605662");
            testCasesForLaser.Add("325");
            testCasesForLaser.Add("605797");
            testCasesForLaser.Add("804");
            testCasesForLaser.Add("605759");

            testCasesForLaser.Add("280");
            testCasesForLaser.Add("605746");

            testCasesForLaser.Add("SO-1009");
            testCasesForLaser.Add("PO-1005");
            testCasesForLaser.Add("IERC");
            testCasesForLaser.Add("IPE");
            testCasesForLaser.Add("1503");
            testCasesForLaser.Add("2504");
            testCasesForLaser.Add("CAF1");
            testCasesForLaser.Add("F-413400");
            testCasesForLaser.Add("OR-4");
            testCasesForLaser.Add("OR4");
            testCasesForLaser.Add("TC-01");
            testCasesForLaser.Add("TC-02");
            testCasesForLaser.Add("TC-03");
            testCasesForLaser.Add("TC-04");
            testCasesForLaser.Add("TC-05");
            testCasesForLaser.Add("TC-06");
            testCasesForLaser.Add("TC-07");
            testCasesForLaser.Add("TC-08");
            testCasesForLaser.Add("SP Returns IMPB");

            testCasesForLaser.Add("F-413400");
            testCasesForLaser.Add("F-413401");
            testCasesForLaser.Add("CAF1");
            testCasesForLaser.Add("CAF-413401");

            dangerousGoodsTestCases.Add("LB1");
            dangerousGoodsTestCases.Add("323405");
            dangerousGoodsTestCases.Add("LB2");
            dangerousGoodsTestCases.Add("323406");
            dangerousGoodsTestCases.Add("LIB1");
            dangerousGoodsTestCases.Add("413244");
            dangerousGoodsTestCases.Add("LIB2");
            dangerousGoodsTestCases.Add("413245");
            dangerousGoodsTestCases.Add("325");
            dangerousGoodsTestCases.Add("605797");
            dangerousGoodsTestCases.Add("SO-1009");
            dangerousGoodsTestCases.Add("ES-1009");

            testCasesForThermal = testCasesForLaser.Except(dangerousGoodsTestCases).ToList();
        }

        public static List<string> TestCasesForLaser => testCasesForLaser;

        public static List<string> TestCasesForThermal => testCasesForThermal;
    }
}
