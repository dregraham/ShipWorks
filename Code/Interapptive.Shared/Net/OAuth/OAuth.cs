using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using Interapptive.Shared.Security;

namespace Interapptive.Shared.Net.OAuth
{
    /*
   Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

   Permission is hereby granted, free of charge, to any person obtaining a copy
   of this software and associated documentation files (the "Software"), to deal
   in the Software without restriction, including without limitation the rights
   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
   copies of the Software, and to permit persons to whom the Software is
   furnished to do so, subject to the following conditions:

   The above copyright notice and this permission notice shall be included in
   all copies or substantial portions of the Software.

   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
   THE SOFTWARE.*/

    /// <summary>
    /// OAuth base class
    /// </summary>
    public class OAuth
    {
        /// <summary>
        /// Enrypted credentials for using their api
        /// </summary>
        private string encryptedConsumerKey;
        private string encryptedConsumerSecretKey;
        private string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
        private string consumerKey;
        private string consumerSecretKey;

        /// <summary>
        /// Constructor
        /// </summary>
        public OAuth(string encryptedConsumerKey, string encryptedConsumerSecretKey)
        {
            this.encryptedConsumerKey = encryptedConsumerKey;
            this.encryptedConsumerSecretKey = encryptedConsumerSecretKey;
            Parameters = new System.Collections.Generic.List<Pair<string, string>>();
            AddParameter("oauth_consumer_key", "");
            AddParameter("oauth_nonce", "");
            AddParameter("oauth_signature_method", "HMACSHA1");
            AddParameter("oauth_timestamp", "");
            AddParameter("oauth_version", "1.0");
            OtherParameters = new Dictionary<string, string>();
            RandomGenerator = new Random();
        }

        /// <summary>
        /// Generates a request
        /// </summary>
        /// <returns>The string containing the request</returns>
        public string GenerateRequest()
        {
            string Url = "";
            string Parameters = "";
            string Signature = GenerateSignature(out Url, out Parameters);
            string ReturnUrl = Url.ToString() + "?" + Parameters + "&oauth_signature=" + UrlEncode(Signature);
            return ReturnUrl;
        }

        /// <summary>
        /// Generates the signature
        /// </summary>
        /// <param name="Url">Url</param>
        /// <param name="Parameters">Parameters</param>
        /// <returns>The signature</returns>
        [SuppressMessage("CSharp.Analyzers",
            "CA5350: Do not use insecure cryptographic algorithm SHA1",
            Justification = "This is what OAuth needs for its signature")]
        protected string GenerateSignature(out string Url, out string Parameters)
        {
            Parameters = "";
            Url = "";

            string Base = GenerateBase(out Url, out Parameters);
            using (HMACSHA1 SHA1 = new HMACSHA1())
            {
                SHA1.Key = Encoding.ASCII.GetBytes(UrlEncode(ConsumerSecretKey) + "&" + (string.IsNullOrEmpty(TokenSecret) ? "" : UrlEncode(TokenSecret)));
                return Convert.ToBase64String(SHA1.ComputeHash(Encoding.ASCII.GetBytes(Base)));
            }
        }

        /// <summary>
        /// Does url encoding using uppercase since that is needed for .Net
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>Url encoded string</returns>
        protected string UrlEncode(string Input)
        {
            if (Input == null)
                Input = string.Empty;
            StringBuilder Result = new StringBuilder();
            for (int x = 0; x < Input.Length; ++x)
            {
                if (unreservedChars.IndexOf(Input[x]) != -1)
                    Result.Append(Input[x]);
                else
                    Result.Append("%").Append(String.Format("{0:X2}", (int) Input[x]));
            }
            return Result.ToString();
        }

        /// <summary>
        /// Adds a Parameter if Value is not blank or null.
        /// </summary>
        /// <param name="Key">Key text</param>
        /// <param name="Value">Value text</param>
        protected void AddParameterIfNotNull(string Key, string Value)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                AddParameter(Key, Value);
            }
        }
        /// <summary>
        /// Adds a parameter
        /// </summary>
        /// <param name="Key">Key text</param>
        /// <param name="Value">Value text</param>
        protected void AddParameter(string Key, string Value)
        {
            bool Found = false;
            foreach (Pair<string, string> Pair in Parameters)
            {
                if (Pair.Left == Key)
                {
                    Pair.Right = Value;
                    Found = true;
                    break;
                }
            }
            if (!Found)
            {
                Parameters.Add(new Pair<string, string>(Key, Value));
            }
        }


        /// <summary>
        /// Generates the info used in the signature
        /// </summary>
        /// <param name="UrlString">Url string</param>
        /// <param name="ParameterString">Parameter string</param>
        /// <returns>The base information for the signature</returns>
        [NDependIgnoreLongMethod]
        private string GenerateBase(out string UrlString, out string ParameterString)
        {
            StringBuilder UrlBuilder = new StringBuilder();
            StringBuilder Builder = new StringBuilder();
            StringBuilder ParameterBuilder = new StringBuilder();

            string SignatureMethod = "HMAC-SHA1";

            AddParameter("oauth_consumer_key", ConsumerKey);
            AddParameter("oauth_nonce", RandomGenerator.Next(123400, 9999999).ToString());
            AddParameter("oauth_signature_method", SignatureMethod);
            AddParameter("oauth_timestamp", Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString());
            AddParameter("oauth_version", "1.0");

            if (!string.IsNullOrEmpty(this.Token))
            {
                AddParameter("oauth_token", this.Token);
            }

            if (OtherParameters != null)
            {
                foreach (var item in OtherParameters)
                {
                    AddParameter(item.Key, item.Value);
                }
            }

            Parameters.Sort(new PairComparer());

            string Splitter = "";
            foreach (Pair<string, string> Key in Parameters)
            {
                ParameterBuilder.Append(Splitter)
                    .Append(Key.Left)
                    .Append("=")
                    .Append(UrlEncode(Key.Right));
                Splitter = "&";
            }

            UrlBuilder.Append(Url.Scheme).Append("://").Append(Url.Host);
            if ((Url.Scheme == "http" && Url.Port != 80) || (Url.Scheme == "https" && Url.Port != 443))
                UrlBuilder.Append(":").Append(Url.Port);
            UrlBuilder.Append(Url.AbsolutePath);

            UrlString = UrlBuilder.ToString();
            ParameterString = ParameterBuilder.ToString();

            Builder.Append("GET")
                .Append("&")
                .Append(UrlEncode(UrlBuilder.ToString()))
                .Append("&")
                .Append(UrlEncode(ParameterBuilder.ToString()));

            return Builder.ToString();
        }


        /// <summary>
        /// Url that is being used
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public virtual string Token { get; set; }



        /// <summary>
        /// Token secret
        /// </summary>
        public virtual string TokenSecret { get; set; }


        /// <summary>
        /// Other Parameters
        /// </summary>
        public Dictionary<string, string> OtherParameters { get; private set; }

        /// <summary>
        /// Compilation of parameters
        /// </summary>
        private List<Pair<string, string>> Parameters { get; set; }

        /// <summary>
        /// Random number generator
        /// </summary>
        private Random RandomGenerator { get; set; }

        /// <summary>
        /// ConsumerKey from Etsy, returned decrypted.
        /// </summary>
        private string ConsumerKey
        {
            get
            {
                if (String.IsNullOrEmpty(consumerKey))
                {
                    consumerKey = SecureText.Decrypt(encryptedConsumerKey, "interapptive");
                }
                return consumerKey;
            }
        }

        /// <summary>
        /// Secret Consumer Key from Etsy, returned decrypted.
        /// </summary>
        private string ConsumerSecretKey
        {
            get
            {
                if (String.IsNullOrEmpty(consumerSecretKey))
                {
                    consumerSecretKey = SecureText.Decrypt(encryptedConsumerSecretKey, "interapptive");
                }
                return consumerSecretKey;
            }
        }
    }
}

