using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Interapptive.Shared.Collections;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Cache of compiled XSL for templates and tokens
    /// </summary>
    public sealed class TemplateXslCache
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateXslCache));

        // Get the tree for which templates in this cache come from
        TemplateTree templateTree;

        // The cache of compiled XSL for templates
        Dictionary<string, TemplateXsl> templateXslCache = new Dictionary<string, TemplateXsl>();

        // The cache of compiled XSL for tokens
        LruCache<string, TemplateXsl> tokenXslCache = new LruCache<string, TemplateXsl>(50);

        // Used to determine if we are entering a recursive import situation
        static AsyncLocal<Stack<string>> _recursionStack = new AsyncLocal<Stack<string>>();

        /// <summary>
        /// Create a new cache for the specified tree.  If initializeFrom is not null, then the internal cache data will be initialized from it.
        /// </summary>
        public TemplateXslCache(TemplateTree templateTree, TemplateXslCache initializeFrom)
        {
            this.templateTree = templateTree;

            if (initializeFrom != null)
            {
                foreach (var pair in initializeFrom.templateXslCache)
                {
                    this.templateXslCache[pair.Key] = pair.Value;
                }

                foreach (string key in initializeFrom.tokenXslCache.Keys)
                {
                    this.tokenXslCache[key] = initializeFrom.tokenXslCache[key];
                }
            }
        }

        /// <summary>
        /// Get the TemplateXsl object for the given template.
        /// </summary>
        public TemplateXsl FromTemplate(TemplateEntity template)
        {
            if (templateTree != template.TemplateTree)
            {
                throw new InvalidOperationException("Attempting to get TemplateXsl for template not associated with the cache's tree.");
            }

            // Need to lock in case multiple threads are trying to compile the same template at the same time
            lock (templateXslCache)
            {
                // If this template is already on the stack, that indicates a recursive xsl:import situation.  There is no need to check it, as its
                // already in the process of being checked.  Also we know for a fact that it will end up showing an error anyway thrown by the XSL compiler
                // that there were recursive imports.
                if (RecursionStack.Contains(template.FullName))
                {
                    // If we are here then we know the TemplateXsl has at least been pushed in to the cache - it may be in the middle of compiling, but we do know for sure
                    // that it will be present.
                    return templateXslCache[template.FullName];
                }

                RecursionStack.Push(template.FullName);

                try
                {
                    // See if we have it cached
                    TemplateXsl templateXsl;
                    if (templateXslCache.TryGetValue(template.FullName, out templateXsl))
                    {
                        // First see if the XSL has changed since we cached it
                        if (string.CompareOrdinal(template.Xsl, templateXsl.Xsl) != 0)
                        {
                            // Force its cache replacement
                            templateXsl = null;
                        }
                        // Now see if any of the dependant templates have changed
                        else
                        {
                            // Only check direct imports, so they get checked in order of importation
                            foreach (TemplateXslImport xslImport in templateXsl.XslImports.Where(i => i.DirectImport))
                            {
                                TemplateEntity importedTemplate = templateTree.FindTemplate(xslImport.TemplateFullName);

                                // An imported template has been deleted, thats not cool.
                                if (importedTemplate == null)
                                {
                                    // If the import version is Guid.Empty, that means we already knew it was missing.
                                    if (xslImport.TemplateXslVersion != Guid.Empty)
                                    {
                                        templateXsl = null;
                                    }
                                }
                                else
                                {
                                    // Get the latest from cache
                                    TemplateXsl importedXsl = FromTemplate(importedTemplate);
                                    if (importedXsl.Version != xslImport.TemplateXslVersion)
                                    {
                                        templateXsl = null;
                                    }
                                }
                            }
                        }

                        if (templateXsl == null)
                        {
                            //   log.DebugFormat("TemplateXsl Cache: [{0}] OLD.", template.FullName);
                        }
                    }
                    else
                    {
                        // log.DebugFormat("TemplateXsl Cache: [{0}] NEW.", template.FullName);
                    }

                    if (templateXsl != null)
                    {
                        //log.DebugFormat("TemplateXsl Cache: [{0}] CURRENT.", template.FullName);
                    }

                    if (templateXsl == null)
                    {
                        // Add it to the cache
                        templateXsl = new TemplateXsl(template.FullName, template.Xsl);
                        templateXslCache[template.FullName] = templateXsl;

                        templateXsl.Compile(templateTree);
                    }

                    return templateXsl;
                }
                finally
                {
                    RecursionStack.Pop();
                }
            }
        }

        /// <summary>
        /// Get the compiled XSL for the given tokenized text
        /// </summary>
        public TemplateXsl FromToken(string tokenText)
        {
            // Need to lock in case multiple threads are trying to compile the same template at the same time.
            // We use templateXslCache instead of tokenXslCache b\c compiling of tokens may call into using templates.
            lock (templateXslCache)
            {
                string tokenXsl =
                    TemplateTokenProcessor.TokenXslHeader +
                    TemplateTokenProcessor.ConvertToXsl(tokenText) +
                    TemplateTokenProcessor.TokenXslFooter;

                TemplateXsl templateXsl = tokenXslCache[tokenXsl];
                if (templateXsl == null)
                {
                    templateXsl = new TemplateXsl("Token", tokenXsl);
                    tokenXslCache[tokenXsl] = templateXsl;

                    templateXsl.Compile(templateTree);
                }

                return templateXsl;
            }
        }

        /// <summary>
        /// Helps keep track of when FromTemplate is being entered recursively, which would indicate recursive xsl:imports
        /// </summary>
        private static Stack<string> RecursionStack
        {
            get
            {
                // ThreadStatic so we don't have to lock, but do have to do it for each thread.
                if (_recursionStack.Value == null)
                {
                    _recursionStack.Value = new Stack<string>();
                }

                return _recursionStack.Value;
            }
        }
    }
}
