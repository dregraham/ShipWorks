using System;
using System.IO;
using Interapptive.Shared.Pdf;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Core.Tests.Integration.Shipping
{
    [Collection("Database collection")]
    [Trait("Category", "Manual")]
    public class PdfTests : IDisposable
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly DataContext context;

        public PdfTests(DatabaseFixture db, ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void ColorPDF_RendersCrisply()
        {
            var name = Path.GetTempFileName();
            var pdfStream = GetType().Assembly.GetManifestResourceStream("ShipWorks.Core.Tests.Integration.Shipping.LTLOriginal.pdf");
            var document = new PdfColorDocument();

            var paths = document.SavePages(pdfStream, (s, i) =>
            {
                var path = Path.Combine(Path.GetTempPath(), $"{name}-{i}.png");

                using (var file = File.OpenWrite(path))
                {
                    s.WriteTo(file);
                }

                return path;
            });

            foreach (var path in paths)
            {
                testOutputHelper.WriteLine($"Check image at {path}");
            }
        }

        public void Dispose() => context.Dispose();
    }
}