using Microsoft.Extensions.Logging;
using NetworkLibrary.Encryption;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace NetworkLibrary
{
    public static class ExpansionFunction
    {
        public static HttpContent ConvertHttpContent<T>(this T value) =>new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
        public static string EncryptData<T>(this T value) => new Encryption<T>(NetworkLibrary.Encryption.Key.PublicKey).IncludeEncrypt(value);
        public static T DecryptData<T>(this string value) => new Encryption<T>(ref value, NetworkLibrary.Encryption.Key.PrivateKey).Decrypt(value);
        public static string CreateJson<T>(this T value) => new JsonSerialization<T>().Serialze(value);
        public static T ReadJson<T>(this string value) => new JsonSerialization<T>().Deserialize(value);
        #region properties
        public static string GetProperties<T>(this T data)
        {
            System.Reflection.PropertyInfo[] props = typeof(T).GetProperties();
            string response = string.Empty;
            foreach (var prop in props)
            {
                response += $"{prop.Name}={prop.GetValue(data, null)}&";
                //string str = string.Format("{0}.{1} = {2}&", typeof(T).Name, prop.Name, prop.GetValue(data, null));
            }
            if (response.Length > 1)
                response.Remove(response.Length - 1);
            return response;
        }

        public static MultipartFormDataContent AddFormData<T>(this T data)
        {
            // 제네릭 타입 데이터의 프로퍼티를 폼 데이터로 변환
            var properties = typeof(T).GetProperties();
            var formData = new MultipartFormDataContent();
            foreach (var property in properties)
            {
                var value = property.GetValue(data);

                if (value is byte[] byteArray) // 파일 데이터 처리
                {
                    formData.Add(new ByteArrayContent(byteArray), property.Name, $"{property.Name}.bin");
                }
                else if (value is string stringValue) // 문자열 데이터 처리
                {
                    formData.Add(new StringContent(stringValue), property.Name);
                }
                else if (value != null) // 기타 데이터 처리
                {
                    formData.Add(new StringContent(value.ToString() ?? string.Empty), property.Name);
                }
            }
            return formData;
        }

        /// <summary>
        /// 제네릭 데이터(T)를 URL 인코딩 가능한 Dictionary<string, string>로 변환하는 헬퍼 메서드
        /// </summary>
        /// <param name="data">요청 데이터</param>
        /// <returns>Dictionary 형태의 데이터</returns>
        public static Dictionary<string, string> ConvertToFormData<T>(this T data)
        {
            var formData = new Dictionary<string, string>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(data);
                if (value != null)
                {
                    formData.Add(property.Name, value.ToString() ?? string.Empty);
                }
            }

            return formData;
        }


        #endregion

     



    }
    internal class JsonSerialization<T>
    {
        public T Deserialize(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception ex)
            {                
                return default(T);
            }

        }

        public string Serialze(T data)
        {
            try
            {
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {                
                return string.Empty;
            }

        }
    }
}
