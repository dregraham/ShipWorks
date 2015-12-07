using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using System.CodeDom.Compiler;
using ShipWorks.ApplicationCore;
using System.IO;
using Microsoft.CSharp;
using System.Security.Cryptography;
using Interapptive.Shared.UI;
using System.Text.RegularExpressions;
using System.Collections;
using System.Xml.Linq;
using Interapptive.Shared;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Class containing functions intended to be called from within XSL
    /// </summary>
    public class TemplateXslExtensions
    {        
        // Custom compiler cache 
        static Dictionary<string, CompilerResults> compilerCache = new Dictionary<string,CompilerResults>();

        /// <summary>
        /// Takes an unformatted dateTime string and formats it as 'm/d/yyyy'
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ToShortDate(string dateTime)
        {
            // Parse automatically converts to Local
            DateTime date = DateTime.Parse(dateTime);
            return date.ToShortDateString();
        }

        /// <summary>
        /// Takes an unformatted dateTime string and formats it as 'h:mm tt'
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ToShortTime(string dateTime)
        {
            // Parse automatically converts to Local
            DateTime date = DateTime.Parse(dateTime);
            return date.ToShortTimeString();
        }

        /// <summary>
        /// Takes an unformatted dateTime string and formats it as 'm/d/yyyy h:mm tt'
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ToShortDateTime(string dateTime)
        {
            return ToShortDate(dateTime) + " " + ToShortTime(dateTime);
        }

        /// <summary>
        /// Format the dateTime using arbitrary format
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string FormatDateTime(string dateTime, string format)
        {
            return DateTime.Parse(dateTime).ToString(format);
        }

        /// <summary>
        /// Return the base64 encoding of the given input
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Base64(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        /// <summary>
        /// Run the function with the given code and parameters
        /// </summary>
        [Obfuscation(Exclude = true)]
        public object Function(string code)
        {
            return InternalRunCode(code, new List<object>());
        }

        /// <summary>
        /// Run the function with the given code and parameters
        /// </summary>
        [Obfuscation(Exclude = true)]
        public object Function(string code, object value1)
        {
            return InternalRunCode(code, new List<object> { value1 });
        }

        /// <summary>
        /// Run the function with the given code and parameters
        /// </summary>
        [Obfuscation(Exclude = true)]
        public object Function(string code, object value1, object value2)
        {
            return InternalRunCode(code, new List<object> { value1, value2 });
        }

        /// <summary>
        /// Run the function with the given code and parameters
        /// </summary>
        [Obfuscation(Exclude = true)]
        public object Function(string code, object value1, object value2, object value3)
        {
            return InternalRunCode(code, new List<object> { value1, value2, value3 });
        }

        /// <summary>
        /// Run the function with the given code and parameters
        /// </summary>
        [Obfuscation(Exclude = true)]
        public object Function(string code, object value1, object value2, object value3, object value4)
        {
            return InternalRunCode(code, new List<object> { value1, value2, value3, value4 });
        }

        /// <summary>
        /// Run the function with the given code and parameters
        /// </summary>
        [Obfuscation(Exclude = true)]
        [NDependIgnoreTooManyParams]
        public object Function(string code, object value1, object value2, object value3, object value4, object value5)
        {
            return InternalRunCode(code, new List<object> { value1, value2, value3, value4, value5 });
        }

        /// <summary>
        /// Run the given code with the specified values as inputs
        /// </summary>
        [NDependIgnoreLongMethod]
        private object InternalRunCode(string code, List<object> values)
        {
            string hashKey = GetCodeHash(code);

            Match match = Regex.Match(code, @"^\s*(?<signature>[(][^)]*?[)])\s*=>\s*(?<body>{.*})", RegexOptions.Singleline);
            if (!match.Success)
            {
                throw new TemplateProcessException("The code must be in the form '(int value1, string value2) => { return result; }");
            }

            string signature = match.Groups["signature"].Value;
            string body = match.Groups["body"].Value;

            Match inputs = Regex.Match(signature, @"^[(]((?<input>(?<types>[\w<>]+)\s+\w+)\s*,?\s*)*[)]$");
            if (!inputs.Success)
            {
                throw new TemplateProcessException("The function signature was not in a valid format (Type parameterName, etc)");
            }

            if (inputs.Groups["types"].Captures.Count != values.Count)
            {
                throw new TemplateProcessException("The number of inputs given does not match the number of parameters specified.");
            }

            CompilerResults results;
            if (!compilerCache.TryGetValue(hashKey, out results))
            {
                // The parameters of the compile
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateInMemory = true;
                parameters.TreatWarningsAsErrors = false;
                parameters.GenerateExecutable = false;
                parameters.CompilerOptions = "/optimize";

                // Add the references
                string[] references = 
                { 
                    "System.dll",
                    "System.Data.dll",
                    "System.Windows.Forms.dll",
                    "System.Web.dll",
                    "System.Core.dll",
                    "System.Xml.dll",
                    "System.Drawing.dll"
                };
                parameters.ReferencedAssemblies.AddRange(references);

                // Wrap the code in an assembly
                StringBuilder assembly = new StringBuilder();
                assembly.AppendLine("using System;");
                assembly.AppendLine("using System.Linq;");
                assembly.AppendLine("using System.Collections.Generic;");
                assembly.AppendLine("using System.Text;");
                assembly.AppendLine("using System.IO;");
                assembly.AppendLine("using System.Windows.Forms;");
                assembly.AppendLine("using System.Web;");
                assembly.AppendLine("using System.Net;");
                assembly.AppendLine("using System.Xml;");
                assembly.AppendLine("using System.Xml.XPath;");
                assembly.AppendLine("using System.Drawing;");

                assembly.AppendLine("namespace ShipWorks {");
                assembly.AppendLine("public class Wrapper { ");
                assembly.Append("public static object GeneratedFunction");
                assembly.Append(signature);
                assembly.Append(" { ");
                assembly.Append(body);
                assembly.Append("} } }");

                // Has to be in C#
                CSharpCodeProvider provider = new CSharpCodeProvider();
                results = provider.CompileAssemblyFromSource(parameters, assembly.ToString());

                // Cache the result, regardless of success or not
                compilerCache[GetCodeHash(code)] = results;
            }

            // If there are errors, throw an Exception
            if (results.Errors.HasErrors)
            {
                string text = "Compile error in custom code: ";
                foreach (CompilerError ce in results.Errors)
                {
                    text += string.Format("\r\n{0}: {1}", ce.ErrorNumber, ce.ErrorText);
                }

                throw new TemplateProcessException(text);
            }

            Module module = results.CompiledAssembly.GetModules()[0];
            Type wrapperType = null;
            MethodInfo generatedMethod = null;

            // Try to get the wrapper type
            if (module != null)
            {
                wrapperType = module.GetType("ShipWorks.Wrapper");
            }

            // Try to get the method
            if (wrapperType != null)
            {
                generatedMethod = wrapperType.GetMethod("GeneratedFunction");
            }

            // Execute it
            if (generatedMethod != null)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    values[i] = PrepareFunctionValue(values[i], inputs.Groups["types"].Captures[i].Value);
                }

                object result = generatedMethod.Invoke(null, values.ToArray());

                // See if the result is enumerable
                ICollection collection = result as ICollection;
                if (collection != null)
                {
                    // Build a node set to return
                    XElement root = new XElement("Return");
                    foreach (object entry in collection)
                    {
                        root.Add(new XElement("Value", entry));
                    }

                    // Return a node iterator to the caller to traverse
                    return root.CreateNavigator().SelectChildren(XPathNodeType.Element);
                }
                else
                {
                    return result;
                }
            }
            else
            {
                throw new TemplateProcessException("Something unknown went wrong running the code.");
            }
        }

        /// <summary>
        /// Prepare a funtion value for being passed int the expecting function
        /// </summary>
        private static object PrepareFunctionValue(object value, string type)
        {
            Type targetType = Type.GetType(GetFullTypeName(type), true, true);

            // If it's a node set, use its value
            XPathNodeIterator xpath = value as XPathNodeIterator;
            if (xpath != null)
            {
                if (xpath.Count > 0)
                {
                    if (xpath.CurrentPosition == 0)
                    {
                        xpath.MoveNext();
                    }

                    value = xpath.Current.Value;
                }
                else
                {
                    value = "";
                }
            }

            try
            {
                return Convert.ChangeType(value, targetType);
            }
            catch (FormatException)
            {
                throw new TemplateProcessException(string.Format("Could not convert '{0}' to '{1}'.", value, type));
            }
        }

        /// <summary>
        /// Get the full type name of the given type
        /// </summary>
        private static string GetFullTypeName(string type)
        {
            // If it contains a ".", it's already qualified
            if (type.Contains("."))
            {
                return type;
            }

            Dictionary<string, string> builtin = new Dictionary<string,string>
                {
                    {"bool", "Boolean"},
                    {"byte", "Byte"},
                    {"sbyte", "SByte"},
                    {"char", "Char"},
                    {"decimal", "Decimal"},
                    {"double", "Double"},
                    {"float", "Single"},
                    {"int", "Int32"},
                    {"uint", "UInt32"},
                    {"long", "Int64"},
                    {"ulong", "UInt64"},
                    {"object", "Object"},
                    {"short", "Int16"},
                    {"ushort", "UInt16"},
                    {"string", "String"},
                };

            // See if we can translate it
            string result;
            if (builtin.TryGetValue(type, out result))
            {
                type = result;
            }

            // If its a builtin type, prepend system
            if (builtin.ContainsValue(type))
            {
                type = "System." + type;
            }

            return type;
        }

        /// <summary>
        /// Get the hash of the given source code for saving in our dictionary
        /// </summary>
        private string GetCodeHash(string code)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            return Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(code)));
        }

        /// <summary>
        /// Generates a key value for an OrderItem.  The XPathNavigator should be positioned
        /// such that the OrderItem is the current node.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string GetOrderItemKeyValue(XPathNodeIterator itemNode, bool optionSpecific)
        {
            if (!itemNode.MoveNext())
            {
                return "";
            }

            XPathNavigator xpath = itemNode.Current;

            // It starts out with the item code
            string key = XPathUtility.Evaluate(xpath, "Code", "");

            if (optionSpecific)
            {
                // Then add in all the option info
                XPathNodeIterator options = xpath.Select("Option");

                while (options.MoveNext())
                {
                    key += XPathUtility.Evaluate(options.Current, "Name", "") + XPathUtility.Evaluate(options.Current, "Description", "");
                }
            }

            return key;
        }
    }
}
