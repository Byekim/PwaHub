using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hub.Shared
{
    public class SupportClass
    {
        public static int URShift(int number, int bits)
        {
            if (number >= 0)
                return number >> bits;
            else
                return (number >> bits) + (2 << ~bits);
        }
        public static int URShift(int number, long bits)
        {
            return URShift(number, (int)bits);
        }
        public static long URShift(long number, int bits)
        {
            if (number >= 0)
                return number >> bits;
            else
                return (number >> bits) + (2L << ~bits);
        }
        public static long URShift(long number, long bits)
        {
            return URShift(number, (int)bits);
        }
        public class MessageDigestSupport
        {
            private System.Security.Cryptography.HashAlgorithm algorithm;
            private byte[] data;
            private int position;
            private string algorithmName;

            public System.Security.Cryptography.HashAlgorithm Algorithm
            {
                get
                {
                    return this.algorithm;
                }
                set
                {
                    this.algorithm = value;
                }
            }
            public byte[] Data
            {
                get
                {
                    return this.data;
                }
                set
                {
                    this.data = value;
                }
            }

            public string AlgorithmName
            {
                get
                {
                    return this.algorithmName;
                }
            }
            public MessageDigestSupport(System.String algorithm)
            {
                if (algorithm.Equals("SHA-1"))
                {
                    this.algorithmName = "SHA";
                }
                else
                {
                    this.algorithmName = algorithm;
                }
                this.Algorithm = (System.Security.Cryptography.HashAlgorithm)System.Security.Cryptography.CryptoConfig.CreateFromName(this.algorithmName);
                this.position = 0;
            }
            public sbyte[] DigestData()
            {
                sbyte[] result = ToSByteArray(this.Algorithm.ComputeHash(this.data));
                this.Reset();
                return result;
            }
            public sbyte[] DigestData(byte[] newData)
            {
                this.Update(newData);
                return this.DigestData();
            }
            public void Update(byte[] newData)
            {
                if (position == 0)
                {
                    this.Data = newData;
                    this.position = this.Data.Length - 1;
                }
                else
                {
                    byte[] oldData = this.Data;
                    this.Data = new byte[newData.Length + position + 1];
                    oldData.CopyTo(this.Data, 0);
                    newData.CopyTo(this.Data, oldData.Length);

                    this.position = this.Data.Length - 1;
                }
            }
            public void Update(byte newData)
            {
                byte[] newDataArray = new byte[1];
                newDataArray[0] = newData;
                this.Update(newDataArray);
            }
            public void Update(byte[] newData, int offset, int count)
            {
                byte[] newDataArray = new byte[count];
                System.Array.Copy(newData, offset, newDataArray, 0, count);
                this.Update(newDataArray);
            }
            public void Reset()
            {
                this.data = null;
                this.position = 0;
            }
            public override string ToString()
            {
                return this.Algorithm.ToString();
            }
            public static MessageDigestSupport GetInstance(System.String algorithm)
            {
                return new MessageDigestSupport(algorithm);
            }
            public static bool EquivalentDigest(System.SByte[] firstDigest, System.SByte[] secondDigest)
            {
                bool result = false;
                if (firstDigest.Length == secondDigest.Length)
                {
                    int index = 0;
                    result = true;
                    while (result && index < firstDigest.Length)
                    {
                        result = firstDigest[index] == secondDigest[index];
                        index++;
                    }
                }

                return result;
            }
        }   
        public static byte[] ToByteArray(sbyte[] sbyteArray)
        {
            byte[] byteArray = new byte[sbyteArray.Length];
            for (int index = 0; index < sbyteArray.Length; index++)
                byteArray[index] = (byte)sbyteArray[index];
            return byteArray;
        }
        public static byte[] ToByteArray(string sourceString)
        {
            byte[] byteArray = new byte[sourceString.Length];
            for (int index = 0; index < sourceString.Length; index++)
                byteArray[index] = (byte)sourceString[index];
            return byteArray;
        }
        public static byte[] ToByteArray(object[] tempObjectArray)
        {
            byte[] byteArray = new byte[tempObjectArray.Length];
            for (int index = 0; index < tempObjectArray.Length; index++)
                byteArray[index] = (byte)tempObjectArray[index];
            return byteArray;
        }
        public static sbyte[] ToSByteArray(byte[] byteArray)
        {
            sbyte[] sbyteArray = new sbyte[byteArray.Length];
            for (int index = 0; index < byteArray.Length; index++)
                sbyteArray[index] = (sbyte)byteArray[index];
            return sbyteArray;
        }
        public static void WriteStackTrace(System.Exception throwable, System.IO.TextWriter stream)
        {
            stream.Write(throwable.StackTrace);
            stream.Flush();
        }
    }
}
