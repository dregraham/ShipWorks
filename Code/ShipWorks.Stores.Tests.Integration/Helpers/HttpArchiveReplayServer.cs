using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarSharp;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Stores.Tests.Integration.Helpers
{
    /// <summary>
    /// Simple web server that will replay an HTTP Archive file
    /// </summary>
    public class HttpArchiveReplayServer
    {
        private static Random rand = new Random();
        readonly string uri;
        readonly string harFileBase;

        /// <summary>
        /// Constructor
        /// </summary>
        public HttpArchiveReplayServer(string harFileBase)
        {
            this.harFileBase = harFileBase;
            this.uri = $"http://localhost:{rand.Next(10000, 64000)}/";
        }

        /// <summary>
        /// Start the server
        /// </summary>
        public IDisposable Start(string harFileName) =>
            WebApp.Start(uri, x => HandleRequests(x, harFileBase + "." + harFileName));

        /// <summary>
        /// Translates the URL of an API to use this server
        /// </summary>
        public string TranslateUrl(string url) =>
            new Uri(new Uri(uri), new Uri(url).AbsolutePath).ToString();

        /// <summary>
        /// Handle incoming requests
        /// </summary>
        private void HandleRequests(IAppBuilder app, string archiveFile)
        {
            var archiveContent = GetType().Assembly.GetEmbeddedResourceString(archiveFile);
            var archive = HarConvert.Deserialize(archiveContent);
            var entryEnumerator = archive.Log.Entries.GetEnumerator();

            app.Run(context =>
            {
                var entry = GetNextEntry(entryEnumerator, context);

                if (VerifyRequestMatchesExpected(entry, context))
                {
                    PopulateResponse(entry, context);
                }

                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// Get the next entry in the archive
        /// </summary>
        private Entry GetNextEntry(IEnumerator<Entry> entryEnumerator, IOwinContext context)
        {
            if (!entryEnumerator.MoveNext())
            {
                throw new InvalidOperationException($"A request was made to {context.Request.Uri} but there are no responses left in the archive");
            }

            return entryEnumerator.Current;
        }

        /// <summary>
        /// Populate the response to send to the client
        /// </summary>
        private void PopulateResponse(Entry entry, IOwinContext context)
        {
            context.Response.StatusCode = entry.Response.Status;

            foreach (var header in entry.Response.Headers.Where(x => x.Name != "Content-Encoding"))
            {
                context.Response.Headers.Add(header.Name, new[] { header.Value });
            }

            context.Response.Write(entry.Response.Content.Text);
        }

        /// <summary>
        /// Verify that the request matches the expected request
        /// </summary>
        private bool VerifyRequestMatchesExpected(Entry entry, IOwinContext context)
        {
            if (entry.Request.Url.AbsolutePath != context.Request.Uri.AbsolutePath)
            {
                context.Response.StatusCode = 505;
                context.Response.Write($"[{{\"message\": \"{context.Request.Uri.AbsolutePath} was requested but {entry.Request.Url.AbsolutePath} was expected\"}}]");

                return false;
            }

            return true;
        }
    }
}
