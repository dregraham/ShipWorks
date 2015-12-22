using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.Xsl;
using System.Xml.XPath;
using ShipWorks.Templates.Processing.TemplateXml;
using System.Diagnostics;
using ShipWorks.UI;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using Interapptive.Shared;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Processes templates
    /// </summary>
    public static class TemplateProcessor
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateProcessor));

        // Tag regex
        static Regex regexPartition = new Regex("<TemplatePartition>(.*?)</TemplatePartition>", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// Process the given template for the specified list of selected entities
        /// </summary>
        public static IList<TemplateResult> ProcessTemplate(TemplateEntity template, List<long> idList)
        {
            return ProcessTemplate(template, idList, new ProgressItem("Unused"));
        }

        /// <summary>
        /// Process the given template for the specified list of selected entities.  If the progress item has not already been started, it
        /// will be started and completed by this function. If if has alread been started, it will be left in the Running state.
        /// </summary>
        [NDependIgnoreLongMethod]
        public static IList<TemplateResult> ProcessTemplate(TemplateEntity template, List<long> idList, IProgressReporter progress)
        {
            if (idList == null)
            {
                throw new ArgumentNullException("idList");
            }

            Stopwatch sw = Stopwatch.StartNew();

            bool completeProgressOnReturn = false;

            // If the progress hasnt started yet, we do it now
            if (progress.Status == ProgressItemStatus.Pending)
            {
                progress.Starting();
                completeProgressOnReturn = true;
            }

            try
            {
                // Make sure the XSL is valid
                TemplateXsl templateXsl = TemplateXslProvider.FromTemplate(template);
                if (!templateXsl.IsValid)
                {
                    throw templateXsl.CompileException;
                }

                progress.Detail = "Caching resources...";
                DataResourceManager.LoadConsumerResourceReferences(template.TemplateID);

                List<TemplateResult> results = new List<TemplateResult>();

                progress.Detail = "Determining data for context...";
                List<TemplateInput> inputList = TemplateContextTranslator.Translate(idList, template);

                // Process each context item
                for (int i = 0; i < inputList.Count; i++)
                {
                    TemplateInput templateInput = inputList[i];

                    TemplateTranslationContext context = new TemplateTranslationContext(template, templateInput, progress);
                    context.SetContextPosition(i, inputList.Count);

                    results.AddRange(PerformTemplateTransform(template, context));
                }

                if (completeProgressOnReturn)
                {
                    progress.Detail = "Done";
                    progress.Completed();
                }

                log.DebugFormat("ProcessTemplate: {0}", sw.Elapsed.TotalSeconds);

                return results;
            }
            catch (TemplateCancelException)
            {
                if (completeProgressOnReturn)
                {
                    progress.Detail = "Canceled";
                    progress.Completed();
                }

                throw;
            }
            // Can happen if the template is trying to write to a file that it can't, in script code
            catch (UnauthorizedAccessException ex)
            {
                if (completeProgressOnReturn)
                {
                    progress.Failed(ex);
                }

                throw new TemplateProcessException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                if (completeProgressOnReturn)
                {
                    progress.Failed(ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Process the template using the given list of entities as the primary input.  Can result in more than one result
        /// if the template utilizes the "TemplatePartition" tag.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static List<TemplateResult> PerformTemplateTransform(TemplateEntity template, TemplateTranslationContext context)
        {
            context.ProgressDetail = "Processing template...";

            List<TemplateResult> results = new List<TemplateResult>();
            TemplateXsl templateXsl = TemplateXslProvider.FromTemplate(template);

            TemplateXPathNavigator xpath = new TemplateXPathNavigator(context);
            TemplateResult result = templateXsl.Transform(xpath);

            // For reports its just a single result, or if there are no TemplateOutput sections to deal with
            if (template.Type == (int) TemplateType.Report || !templateXsl.HasPartitions)
            {
                results.Add(result);
            }
            else
            {
                // We need to split by TemplatePartition
                string content = result.ReadResult();

                // Split by TemplateParition
                MatchCollection matches = regexPartition.Matches(content);

                // If there are none, just use the whole result
                if (matches.Count == 0)
                {
                    results.Add(result);
                }
                else
                {
                    // We need a result per match
                    foreach (Match match in matches)
                    {
                        StringBuilder partition = new StringBuilder(content);

                        // Now go through the matches sorted in reverse order from where they start.  This makes it so their indexes
                        // in the string don't change as we remove shit.
                        foreach (Match strip in matches.Cast<Match>().OrderByDescending(m => m.Index))
                        {
                            // This is the match we are preserving for this partition
                            if (strip == match)
                            {
                                // Remove the whole match completey
                                partition.Remove(match.Index, match.Length);

                                // Insert the replaced part
                                partition.Insert(match.Index, match.Groups[1].Value);
                            }
                            // We are completly stripping this one
                            else
                            {
                                partition.Remove(strip.Index, strip.Length);
                            }
                        }

                        if (result.InMemory)
                        {
                            results.Add(new TemplateResult(result.XPathSource, partition.ToString()));
                        }
                        else
                        {
                            string file = TemplateXsl.GetNextTemplateResultTempFile();
                            File.WriteAllText(file, partition.ToString(), templateXsl.TargetEncoding);

                            results.Add(new TemplateResult(result.XPathSource, file, templateXsl.TargetEncoding));
                        }
                    }
                }
            }

            return results;
        }
    }
}
