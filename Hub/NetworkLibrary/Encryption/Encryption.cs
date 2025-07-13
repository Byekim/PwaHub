using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLibrary.Encryption
{
    public class Encryption<T>
    {        
        private const int KeySize = 344;
        private byte[] Key = null;
        private string sKey = string.Empty;

        public Encryption(string PublicRsaKey)
        {
            Key = CreateKey();
            sKey = Encryption<T>.RsaHelper.Encrypt(Key, PublicRsaKey);
        }

        public Encryption(ref string body, string PrivateRsaKey)
        {
            if (body == null || body.Length <= KeySize)
                return;
            Key = Encryption<T>.RsaHelper.Decrypt(body.Substring(0, KeySize), PrivateRsaKey);
            body = body.Substring(KeySize);
        }

        public Encryption(List<string> list, string PrivateRsaKey)
        {
            if (list == null || list.Count < 2 || list[0].Length != KeySize)
                return;
            Key = Encryption<T>.RsaHelper.Decrypt(list[0], PrivateRsaKey);
            list.RemoveAt(0);
        }

        public string IncludeEncrypt(T value) => string.Format("{0}{1}", sKey, Encrypt(value));

        public List<string> IncludeEncrypt(List<T> values)
        {
            List<string> stringList = new List<string>();
            if (values.Count > 0)
            {
                for (int index = 0; index < values.Count; ++index)
                    stringList.Add(Encrypt(values[index]));
                stringList.Insert(0, sKey);
            }
            return stringList;
        }

        public T Decrypt(string str)
        {
            if (Key != null)
            {
                try
                {
                    return new BinarySerialization<T>().Deserialize(EncryptData(Convert.FromBase64String(str)));
                }
                catch
                {
                }
            }
            return default;
        }

        public List<T> Decrypt(List<string> strs)
        {
            List<T> objList = new List<T>();
            if (Key != null)
            {
                for (int index = 0; index < strs.Count; ++index)
                    objList.Add(Decrypt(strs[index]));
            }
            return objList;
        }

        private string Encrypt(T value)
        {
            byte[] bytes = new BinarySerialization<T>().Serialize(value);
            try
            {
                if (bytes != null && (uint)bytes.Length > 0U)
                    return Convert.ToBase64String(EncryptData(bytes));
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return string.Empty;
        }

        private byte[] EncryptData(byte[] bytes)
        {
            for (int index = 0; index < bytes.Length; ++index)
                bytes[index] = (byte)(Key[index % Key.Length] ^ (uint)bytes[index]);
            return bytes;
        }

        //[Obfuscation(Feature = "renaming", Exclude = true)]
        private byte[] CreateKey()
        {
            byte[] buffer = new byte[50];
            new Random((int)DateTime.Now.Ticks).NextBytes(buffer);
            return buffer;
        }

        private class RsaHelper
        {
            //[Obfuscation(Feature = "renaming", Exclude = true)]
            private const int KeySize = 2048;

            internal static string Encrypt(byte[] bytes, string Key)
            {
                if (!string.IsNullOrEmpty(Key))
                {
                    try
                    {
                        using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(KeySize, new CspParameters()
                        {
                            Flags = CspProviderFlags.UseMachineKeyStore
                        }))
                        {
                            cryptoServiceProvider.FromXmlString(Key);
                            bytes = cryptoServiceProvider.Encrypt(bytes, false);
                            return Convert.ToBase64String(bytes);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
                return string.Empty;
            }

            internal static byte[] Decrypt(string str, string Key)
            {
                if (!string.IsNullOrEmpty(Key))
                {
                    try
                    {
                        using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(KeySize, new CspParameters()
                        {
                            Flags = CspProviderFlags.UseMachineKeyStore
                        }))
                        {
                            cryptoServiceProvider.FromXmlString(Key);
                            byte[] rgb = Convert.FromBase64String(str);
                            return cryptoServiceProvider.Decrypt(rgb, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
                return null;
            }
        }
    }
}
