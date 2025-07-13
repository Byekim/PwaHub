using Microsoft.Extensions.Logging;
using Hub.Shared;

namespace NetworkLibrary.ApiClient
{
    public class ApiCheckClient<T>
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiCheckClient<T>> _logger;

        public ApiCheckClient(HttpClient httpClient, ILogger<ApiCheckClient<T>> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="data"></param>
        /// <param name="address"></param>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public async Task<string> PostRequest(T data, string address, string apiName)
        {
            string result = string.Empty;
            var uri = $@"{address}{apiName}";
            if(!string.IsNullOrEmpty(GlobalVariable.xpErpTokenData.access_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GlobalVariable.xpErpTokenData.token_type, GlobalVariable.xpErpTokenData.access_token);
            }    
            try
            {
                HttpContent content = data.ConvertHttpContent();
                var response = await _httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error PostRequest - {ex.Message}");
            }
            return result;
        }

        /// <summary>
        /// Post MultiPart
        /// </summary>
        /// <param name="data"></param>
        /// <param name="address"></param>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public async Task<string> PostMultiPartRequest(T data, string address, string apiName)
        {
            string result = string.Empty;
            var uri = $@"{address}{apiName}";
            if (!string.IsNullOrEmpty(GlobalVariable.xpErpTokenData.access_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GlobalVariable.xpErpTokenData.token_type, GlobalVariable.xpErpTokenData.access_token);
            }
            try
            {
                var formData = data.AddFormData<T>();                
                var response = await _httpClient.PostAsync(uri, formData);
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error PostRequest - {ex.Message}");
            }
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="address"></param>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public async Task<string> PostUrlEncodedRequest(T data, string address, string apiName)
        {
            string result = string.Empty;
            var uri = $@"{address}{apiName}";

            if (!string.IsNullOrEmpty(GlobalVariable.xpErpTokenData.access_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    GlobalVariable.xpErpTokenData.token_type,
                    GlobalVariable.xpErpTokenData.access_token
                );
            }

            try
            {
                var formData = data.ConvertToFormData<T>();                
                var content = new FormUrlEncodedContent(formData);                
                var response = await _httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
                else
                {                    
                    _logger.LogInformation(Message.connectFailMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error PostUrlEncodedRequest - {ex.Message}");
            }

            return result;
        }
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="data"></param>
        /// <param name="address"></param>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public async Task<string> GetRequest(T data, string address, string apiName)
        {
            string result = string.Empty;
            string param = data.GetProperties<T>();
            address += param;
            var uri = $@"{address}{apiName}?";
            if (!string.IsNullOrEmpty(GlobalVariable.xpErpTokenData.access_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GlobalVariable.xpErpTokenData.token_type, GlobalVariable.xpErpTokenData.access_token);
            }
            try
            {

                using (HttpResponseMessage response = await _httpClient.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error GetRequest - {ex.Message}");
            }

            return result;


        }
        /// <summary>
        /// Put
        /// </summary>
        /// <param name="data"></param>
        /// <param name="address"></param>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public async Task<string> PutRequest(T data, string address, string apiName)
        {
            string result = string.Empty;

            var uri = $@"{address}{apiName}";
            if (!string.IsNullOrEmpty(GlobalVariable.xpErpTokenData.access_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GlobalVariable.xpErpTokenData.token_type, GlobalVariable.xpErpTokenData.access_token);
            }
            try
            {
                HttpContent content = data.ConvertHttpContent();
                using (HttpResponseMessage response = await _httpClient.PutAsync(uri, content))
                {
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error PutRequest - {ex.Message}");
            }

            return result;

        }
    }
}
