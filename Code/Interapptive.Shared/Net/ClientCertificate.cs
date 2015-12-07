//
// PEM file decoding adapted from OpenSSLKey
//      .NET 2.0  OpenSSL Public & Private Key Parser
//      Copyright (C) 2008  	JavaScience Consulting
// 
//  http://www.jensign.com/opensslkey/opensslkey.cs
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security;
using System.Text.RegularExpressions;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Class for working with PayPal certificates
    /// </summary>
    [NDependIgnore]
    public class ClientCertificate
    {
        X509Certificate2 certificate;

        /// <summary>
        /// Gets the underlying certificate object
        /// </summary>
        public X509Certificate2 X509Certificate
        {
            get { return certificate; }
        }

        /// <summary>
        /// Gets the certificate name/subject
        /// </summary>
        public string Subject
        {
            get
            {
                return (certificate == null) ? "" : certificate.Subject;
            }
        }

        /// <summary>
        /// Gets the name part from the certificate subject.
        /// </summary>
        public string Name
        {
            get
            {
                return (certificate == null) ? "" : certificate.GetNameInfo(X509NameType.SimpleName, false);
            }
        }

        #region PEM parsing from OpenSSLKey

        /// <summary>
        /// Compares byte arrays
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        } 

        /// <summary>
        /// Decode PCKS8 private key
        /// </summary>
        public static RSACryptoServiceProvider DecodePkcs8PrivateKeyInfo(byte[] pkcs8)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            // this byte[] includes the sequence byte and terminal encoded null 
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            MemoryStream mem = new MemoryStream(pkcs8);
            int lenstream = (int)mem.Length;
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;

            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;


                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    return null;

                seq = binr.ReadBytes(15);		//read the Sequence OID
                if (!CompareBytearrays(seq, SeqOID))	//make sure Sequence for OID is correct
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x04)	//expect an Octet string 
                    return null;

                bt = binr.ReadByte();		//read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
                if (bt == 0x81)
                    binr.ReadByte();
                else
                    if (bt == 0x82)
                        binr.ReadUInt16();
                //------ at this stage, the remaining sequence should be the RSA private key

                byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
                return rsacsp;
            }

            finally { binr.Close(); }

        }

        /// <summary>
        /// RSA Key decoding utility method
        /// </summary>
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
            {
                count = binr.ReadByte();	// data size in next byte
            }
            else
            {
                if (bt == 0x82)
                {
                    highbyte = binr.ReadByte();	// data size in next 2 bytes
                    lowbyte = binr.ReadByte();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    count = BitConverter.ToInt32(modint, 0);
                }
                else
                {
                    count = bt;		// we already have the data size
                }
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        /// <summary>
        /// Parses binary ans.1 RSA private key; returns RSACryptoServiceProvider
        /// </summary>
        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)	//version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(new CspParameters { KeyContainerName = Guid.NewGuid().ToString() });
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            finally { binr.Close(); }
        }

        /// <summary>
        /// Get the Binary RSA Private Key
        /// </summary>
        /// <param name="instr"></param>
        /// <returns></returns>
        public static byte[] DecodeOpenSslPrivateKey(String encodedOpenSSLKey)
        {
            const string pemprivheader = "-----BEGIN RSA PRIVATE KEY-----";
            const string pemprivfooter = "-----END RSA PRIVATE KEY-----";
            string pemstr = encodedOpenSSLKey.Trim();
            byte[] binkey;
            if (!pemstr.StartsWith(pemprivheader) || !pemstr.EndsWith(pemprivfooter))
                return null;

            //remove headers/footers, if present
            StringBuilder sb = new StringBuilder(pemstr);
            sb.Replace(pemprivheader, "");  
            sb.Replace(pemprivfooter, "");

            //get string after removing leading/trailing whitespace
            String pvkstr = sb.ToString().Trim();	

            try
            {  
                // if there are no PEM encryption info lines, this is an UNencrypted PEM private key
                binkey = Convert.FromBase64String(pvkstr);
                return binkey;
            }
            catch (FormatException)
            {		
                //if can't b64 decode, it must be an encrypted private key
                return null;
            }
        }

        #endregion

        #region PEM Handling

        /// <summary>
        /// Loads the certificate from PEM file contents.
        /// </summary>
        public void LoadFromPem(string certPEMText, string pkeyPEMText)
        {
            // pull out the certificate
            string certText = GetCertString(certPEMText, "CERTIFICATE", false);
            byte[] certBytes = Convert.FromBase64String(certText);
            X509Certificate2 cert = new X509Certificate2(certBytes);

            // pull out the private key and decode it
            try
            {
                // PEM file format
                string pemKeyText = GetCertString(pkeyPEMText, "RSA PRIVATE KEY", true);
                byte[] rsaKey = DecodeOpenSslPrivateKey(pemKeyText);

                // associate the private key to the certificate
                cert.PrivateKey = DecodeRSAPrivateKey(rsaKey);
            }
            catch (CryptographicException)
            {
                // try PCKS 8 Format
                string pkcs8Text = GetCertString(pkeyPEMText, "PRIVATE KEY", false);

                try
                {
                    // base 64 decode
                    byte[] pkcs8 = Convert.FromBase64String(pkcs8Text);

                    // convert it to an RSACryptoServiceProvider
                    cert.PrivateKey = DecodePkcs8PrivateKeyInfo(pkcs8);
                }
                catch (FormatException ex)
                {
                    throw new CryptographicException(ex.Message, ex);
                }
            }

            // this is the certificate we want
            certificate = cert;
        }

        /// <summary>
        /// Imports a certificate spread out over two files.
        /// </summary>
        public void LoadFromPemFiles(string certPemFile, string privKeyPemFile)
        {
            if (!File.Exists(certPemFile))
            {
                throw new FileNotFoundException("Certificate file could not be found.", certPemFile);
            }

            if (!File.Exists(privKeyPemFile))
            {
                throw new FileNotFoundException("Certificate file could not be found.", privKeyPemFile);
            }

            string certPEMText = File.ReadAllText(certPemFile);
            string pkeyPEMText = File.ReadAllText(privKeyPemFile);

            LoadFromPem(certPEMText, pkeyPEMText);
        }

        /// <summary>
        /// Loads an X509 Certificate from a PEM file containing both the certificate and private key
        /// </summary>
        public void LoadFromPemFile(string combinedPemFile)
        {
            if (!File.Exists(combinedPemFile))
            {
                throw new FileNotFoundException("Certificate file could not be found.", combinedPemFile);
            }

            // get the text, contains both the certificate and the private key
            string pemFileText = File.ReadAllText(combinedPemFile);

            string certText = GetCertString(pemFileText, "CERTIFICATE", true);
            string pkeyPEMText = GetCertString(pemFileText, "RSA PRIVATE KEY", true);

            LoadFromPem(certText, pkeyPEMText);
        }

        #endregion

        /// <summary>
        /// Extracts a certificate block from PEM text baed on header/footer delimeters
        /// </summary>
        private static string GetCertString(string certificate, string delimeter, bool includeDelimeter)
        {
            string pattern = String.Format("-----BEGIN {0}-----(.*)-----END {0}-----", delimeter);

            Match match = Regex.Match(certificate, pattern, RegexOptions.Singleline);
            if (match.Success)
            {
                if (includeDelimeter)
                {
                    return match.Groups[0].Value;
                }
                else
                {
                    return match.Groups[1].Value;
                }
            }
            else
            {
                throw new CryptographicException(string.Format("Unable to locate the certificate block {0}.", delimeter));
            }
        }

        /// <summary>
        /// Inserts the certificate into the Certificate Store at the desired name and location and 
        /// returns the certificate subject so it can be located again.
        /// </summary>
        public string AddToCertificateStore(StoreName storeName, StoreLocation storeLocation)
        {
            if (certificate == null)
            {
                throw new InvalidOperationException("Certificate has not been loaded.");
            }

            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadWrite);
            try
            {
                // add to the store
                store.Add(certificate);

                // get the name
                return certificate.Subject;
            }
            finally
            {
                // make sure we close the store
                store.Close();
            }
        }


        /// <summary>
        /// Loads a saved certificate
        /// </summary>
        public void Import(byte[] serializedCertificate)
        {
            certificate = new X509Certificate2(serializedCertificate, "shipworks", X509KeyStorageFlags.Exportable);
        }

        /// <summary>
        /// Saves the certificate to a byte array for storage
        /// </summary>
        public byte[] Export()
        {
            if (certificate == null)
            {
                throw new InvalidOperationException("Certificate has not been loaded.");
            }

            return certificate.Export(X509ContentType.Pfx, "shipworks");
        }

        #region equality

        /// <summary>
        /// Equality check
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is ClientCertificate))
            {
                return false;
            }

            return Equals(obj as ClientCertificate);
        }

        /// <summary>
        /// Compares the underlying X509Certificates for equality
        /// </summary>
        public bool Equals(ClientCertificate other)
        {
            if (other == null)
            {
                return false;
            }

            // must be loaded certificates to be equal
            if (certificate == null || other.certificate == null)
            {
                return false;
            }

            return certificate.Equals(other.certificate);
        }

        /// <summary>
        /// Unique hashcode
        /// </summary>
        public override int GetHashCode()
        {
            if (certificate == null)
            {
                return base.GetHashCode();
            }
            else
            {
                return certificate.GetHashCode();
            }
        }

        #endregion
    }
}
