using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Shared
{
    public static class GlobalFunction
    {
        public static string GetDescription<T>(T enumValue) where T : Enum
        {
            FieldInfo? field = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute? attribute = field?.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? enumValue.ToString();
        }


        public static string ConvertStringFromDBLink(DBLink dbLink)
        {
            string result = string.Empty;
            switch (dbLink)
            {
                case DBLink.XPRO: 
                    result = "XPERP";
                    break;
                case DBLink.YPRO: 
                    result = "XPERP";
                    break;
                case DBLink.IPRO:
                    result = "XPERP";
                    break;
                case DBLink.KPRO:
                    result = "XPERP";
                    break;
            }
            return result;
        }

        public static string LogResposneData(ILogger logger, HttpResponseMessage response,string endPoint)
        {
            string errorMessage = string.Empty;
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest: // 400
                    errorMessage = "Bad Request: The server could not understand the request.";
                    break;
                case HttpStatusCode.Unauthorized: // 401
                    errorMessage = "Unauthorized: Authentication is required or failed.";
                    break;
                case HttpStatusCode.Forbidden: // 403
                    errorMessage = "Forbidden: You do not have permission to access this resource.";
                    break;
                case HttpStatusCode.NotFound: // 404
                    errorMessage = "Not Found: The requested resource could not be found.";
                    break;
                case HttpStatusCode.InternalServerError: // 500
                    errorMessage = "Internal Server Error: The server encountered an unexpected condition.";
                    break;
                case HttpStatusCode.ServiceUnavailable: // 503
                    errorMessage = "Service Unavailable: The server is currently unable to handle the request.";
                    break;
                default:
                    errorMessage = $"Unexpected status code: {(int)response.StatusCode} ({response.StatusCode}).";
                    break;
            }
            return errorMessage;
        }

        public static string PwRandom(int _totalLen)
        {
            Random rand = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, _totalLen).Select(x => input[rand.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }
        public static string GetRandom(string randomA, string randomB)
        {
            string Temp = string.Empty;
            for (int i = 0; i < randomA.Length; i++)
            {
                {
                    Temp += randomA.Substring(i, 1) + randomB.Substring(i, 1);
                }
            }
            return Temp;
        }

        public static System.String ConvertEncryption(string rawData)
        {
            return getHashValue(SupportClass.ToSByteArray(SupportClass.ToByteArray(rawData)), 34);

        }
        private static System.String convertToHex(sbyte[] data)
        {
            System.Text.StringBuilder buf = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                int halfbyte = (SupportClass.URShift(data[i], 4)) & 0x0F;
                int two_halfs = 0;
                do
                {
                    if ((0 <= halfbyte) && (halfbyte <= 9))
                        buf.Append((char)('0' + halfbyte));
                    else
                        buf.Append((char)('a' + (halfbyte - 10)));
                    halfbyte = data[i] & 0x0F;
                }
                while (two_halfs++ < 1);
            }

            return buf.ToString();
        }
        

        private static System.String getHashValue(sbyte[] pass, int iterations)
        {
            try
            {
                SupportClass.MessageDigestSupport md = SupportClass.MessageDigestSupport.GetInstance("SHA-256");

                md.Update(SupportClass.ToByteArray(pass));

                sbyte[] hash = md.DigestData();
                for (int i = 1; i < iterations; i++)
                {
                    md.Update(SupportClass.ToByteArray(hash));
                    hash = md.DigestData();
                }
                sbyte[] out_Renamed = new sbyte[32];
                Array.Copy((System.Array)hash, 0, (System.Array)out_Renamed, 0, 16);
                sbyte[] left = new sbyte[4];
                Array.Copy(hash, 16, left, 0, 4);
                md.Update(SupportClass.ToByteArray(left));
                hash = md.DigestData();
                Array.Copy(hash, 0, out_Renamed, 16, 16);

                return convertToHex(out_Renamed);
            }
            catch (System.Exception e)
            {
                throw new System.SystemException(e.ToString());
            }
        }

    }
}
