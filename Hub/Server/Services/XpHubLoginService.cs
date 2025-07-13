using Hub.Server.Models;
using Hub.Shared;
using Microsoft.AspNetCore.Mvc;
using NetworkLibrary.ApiClient;
using NetworkLibrary;
using Hub.Shared.Model.Hub.Login;
using static System.Net.WebRequestMethods;
using System;
using Hub.Shared.Model.Hub;
using System.Net;
using System.Collections.Generic;
using Hub.Shared.Voice.Request;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Hub.Server.SignalR;
using Hub.Server.Common;
using Hub.Shared.Interface;
using Hub.Server.Interfaces.Service.Hub;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq.Dynamic.Core.Tokenizer;

namespace Hub.Server.Services
{
    public class XpHubLoginService : iXpHubService
    {
        private readonly XpHubDbContext _dbContext;
        private readonly ILogger<XpHubLoginService> _logger;
        private readonly HttpClient client = new HttpClient();

        public XpHubLoginService()
        {

        }

        public XpHubLoginService(XpHubDbContext dbContext, ILogger<XpHubLoginService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public async Task<T> GetXperDataWithTokenCheck<T>(Func<Task<T>> fetchDataFunc) where T : class, new()
        {
            var response = await fetchDataFunc();
            if (IsValidResponse(response))
            {
                //_logger.LogInformation(Message.retryMessage);

                response = await RetriveTokenFromDatabase(fetchDataFunc);
               // _logger.LogInformation(Message.retryMessage);
                if (IsValidResponse(response))
                {
                    //_logger.LogInformation(Message.retryMessage);
                    response = await RefreshTokenAndRetry(fetchDataFunc);
                }
                else
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GlobalVariable.xpErpTokenData.token_type, GlobalVariable.xpErpTokenData.access_token);
                }
            }
            return response;
        }

        private bool IsValidResponse<T>(T response) where T : class
        {
            return response == null || (response is iResponseWithUserId userResponse && string.IsNullOrEmpty(userResponse.userId));
        }

        private async Task<T> RetriveTokenFromDatabase<T>(Func<Task<T>> fetchDataFunc) where T : class
        {
            await this.GetXperpTorkenFromDatabase();
            var response = await fetchDataFunc(); // 비동기 호출
            return response;
        }

        private async Task<T> RefreshTokenAndRetry<T>(Func<Task<T>> fetchDataFunc) where T : class
        {
            await this.GetXpErpToken();
            var response = await fetchDataFunc();
            return response;
        }


        /// <summary>
        /// Database 에 토큰값 저장
        /// </summary>
        /// <param name="tokenData"></param>
        /// <param name="filePath"></param>
        private void SaveTokenDataToDatabase(ResponseXperpToken tokenData)
        {

            try
            {
                if (tokenData != null && _dbContext != null)
                {
                    _dbContext.tokenDatas.Add(tokenData);
                }
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
            }
        }

        #region  허브 로그인시 사용할 API들
        /// <summary>
        /// token 가져오기
        /// </summary>       
        public async Task GetXpErpToken()
        {
            RequestXpErpToken request = new RequestXpErpToken
            {
                client_id = GlobalVariable.cognitoClientId,
                client_secret = GlobalVariable.cognitoClientSecret,
                grant_type = "client_credentials"
            };
            // 폼 데이터 변환
            var formData = request.ConvertToFormData();
            // FormUrlEncodedContent로 변환
            var content = new FormUrlEncodedContent(formData);
            try
            {
                var response = await client.PostAsync(GlobalVariable.cognitoServerAddress + "token", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonSerializer.Deserialize<ResponseXperpToken>(responseBody);
                    if (tokenResponse != null)
                    {
                        GlobalVariable.xpErpTokenData = tokenResponse;
                        SaveTokenDataToDatabase(GlobalVariable.xpErpTokenData);
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GlobalVariable.xpErpTokenData.token_type, GlobalVariable.xpErpTokenData.access_token);

                    }
                    else
                    {
                        _logger.LogInformation(Message.tokenFailMessage);
                    }
                }
                else
                {
                    _logger.LogInformation(Message.connectFailMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Db 에서 Token 값 읽어오기 (다른 어플리케이션에서 갱신했을수가 있다)
        /// </summary>
        /// <returns></returns>
        private async Task GetXperpTorkenFromDatabase()
        {
            if (_dbContext != null)
            {
                GlobalVariable.xpErpTokenData = _dbContext.tokenDatas.FirstOrDefault();
            }
        }

        /// <summary>
        /// Login 처리
        /// </summary>        
        public async Task<ReponseXperpResult<List<ResponseXperpLogin>>> GetXpErpLoginData(RequestXpErpBase request)
        {
            if (GlobalVariable.xpErpTokenData == null || string.IsNullOrEmpty(GlobalVariable.xpErpTokenData.access_token))
            {
                //_logger.LogInformation(Message.tokenExfirelMessage, nameof(GetXpErpLoginData));
                return default;
            }
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GlobalVariable.xpErpTokenData.token_type, GlobalVariable.xpErpTokenData.access_token);
            var content = new StringContent(request.CreateJson(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(GlobalVariable.xpErpApiServerAddress + "login", content);
            if (!response.IsSuccessStatusCode)
            {
                GlobalFunction.LogResposneData(_logger, response, GlobalVariable.xpErpApiServerAddress + "login");
                 throw new Exception(Message.connectFailMessage);
            }
            string temp = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(temp))
            {
                _logger.LogInformation(Message.deSerialzeFailMessage, nameof(GetXpErpLoginData));
                return null;
            }
            ReponseXperpResult<List<ResponseXperpLogin>> result = temp.ReadJson<ReponseXperpResult<List<ResponseXperpLogin>>>();
            return result;            
        }

        /// <summary>
        /// 프리미엄처리
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>        
        public async Task<ResponseXpErpPremium?> GetXpErpPremiumData(RequestXpErpBase request)
        {
            if (GlobalVariable.xpErpTokenData == null || string.IsNullOrEmpty(GlobalVariable.xpErpTokenData.access_token))
            {
                //_logger.LogInformation(Message.tokenExfirelMessage, nameof(GetXpErpPremiumData));
                return null;
            }
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GlobalVariable.xpErpTokenData.token_type, GlobalVariable.xpErpTokenData.access_token);

            string getData = request.GetProperties();
            string url = $@"{GlobalVariable.xpErpApiServerAddress}{request.userId}/premium?{getData}";
            
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string message = GlobalFunction.LogResposneData(_logger, response, url);
                _logger.LogInformation(message, nameof(GetXpErpPremiumData));
                throw new Exception(Message.xpErpErrorMessage);
            }
            string temp = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(temp))
            {
                return null;
            }
            ReponseXperpResult<ResponseXpErpPremium> result = temp.ReadJson<ReponseXperpResult<ResponseXpErpPremium>>();
            if (result.status == XpHubApiStatus.SUCCESS.ToString())
            {
                return result.data;
            }
            else
            {
                _logger.LogInformation(Message.connectFailMessage, nameof(GetXpErpPremiumData));
                return null;
            }
        }

        /// <summary>
        /// 프리미엄Config 처리
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>        
        public async Task<ResponseXperpPremiumConfig?> GetXpErpPremiumConfig(RequestXpErpBase request,string aptCd)
        {
            if (GlobalVariable.xpErpTokenData == null || string.IsNullOrEmpty(GlobalVariable.xpErpTokenData.access_token))
            {
               // _logger.LogInformation(Message.tokenExfirelMessage, nameof(GetXpErpPremiumData));
                return null;
            }
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(GlobalVariable.xpErpTokenData.token_type, GlobalVariable.xpErpTokenData.access_token);
            string getData = request.GetProperties();
            string url = $@"{GlobalVariable.xpErpApiServerAddress}{aptCd}/premium-config?{getData}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string message  = GlobalFunction.LogResposneData(_logger, response, url);
                _logger.LogInformation(message, nameof(GetXpErpPremiumConfig));
                throw new Exception(Message.xpErpErrorMessage);
            }
            string temp = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(temp))
            {
                return null;
            }
            ReponseXperpResult<ResponseXperpPremiumConfig> result = temp.ReadJson<ReponseXperpResult<ResponseXperpPremiumConfig>>();
            if (result.status == XpHubApiStatus.SUCCESS.ToString())
            {
                return result.data;
            }
            else
            {
                _logger.LogInformation(Message.connectFailMessage, nameof(GetXpErpPremiumConfig));
                return null;
            }
        }

        /// <summary>
        /// 전자투표
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>       
        public async Task<ResponseXpErpVote?> GetXpErpVote(RequestXpErpBase request)
        {

            if (GlobalVariable.xpErpTokenData == null || string.IsNullOrEmpty(GlobalVariable.xpErpTokenData.access_token))
            {
                return default;
            }
            string getData = request.GetProperties();
            string url = $@"{GlobalVariable.xpErpApiServerAddress}{request.userId}/vote?{getData}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {                
                string message = GlobalFunction.LogResposneData(_logger, response, url);
                _logger.LogInformation(message, nameof(GetXpErpVote)); 
                return default;
            }
            string temp = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(temp))
            {
                return null;
            }
            ReponseXperpResult<ResponseXpErpVote> result = temp.ReadJson <ReponseXperpResult<ResponseXpErpVote>>();
            if (result.status == XpHubApiStatus.SUCCESS.ToString())
            {
                return result.data;
            }
            else
            {
                _logger.LogInformation(Message.connectFailMessage, nameof(GetXpErpVote));
                return null;
            }
        }

        /// <summary>
        /// 아파트단지 사용자별 메뉴접근 yn
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>        
        public async Task<ResponseXperpMenuYn?> GetXpErpMenuYn(RequestXpErpBase request,string aptCd)
        {
            if (GlobalVariable.xpErpTokenData == null || string.IsNullOrEmpty(GlobalVariable.xpErpTokenData.access_token))
            {
                return null;
            }
            string getData = request.GetProperties();
            string url = $@"{GlobalVariable.xpErpApiServerAddress}{aptCd}/menu-yn?{getData}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(Message.xpErpErrorMessage);
            }

            string temp = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(temp))
            {
                return null;
            }
            ReponseXperpResult<List<ResponseXperpMenuYn>> tempList = temp.ReadJson<ReponseXperpResult<List<ResponseXperpMenuYn>>>();
            if (tempList.status == XpHubApiStatus.SUCCESS.ToString())
            {
                ResponseXperpMenuYn result = tempList.data.Find(o => o.userId == request.userId) ?? default;

                if (result != null)
                {
                    result.url = @"https://apt.aptdesk.com/aptdesk/member/xphublogin_proc.jsp?aptCd";
                }
                return result;

            }
            else
            {
                _logger.LogInformation(Message.connectFailMessage, nameof(GetXpErpVote));
                return null;
            }

        }

        /// <summary>
        /// 단지정보 불러오기
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>        
        public async Task<ResponseXpErpDanji?> GetXpErpDanji(RequestXpErpBase request,string aptCd)
        {
            if (GlobalVariable.xpErpTokenData == null || string.IsNullOrEmpty(GlobalVariable.xpErpTokenData.access_token))
            {
                return null;
            }
            string getData = request.GetProperties();
            string url = $@"{GlobalVariable.xpErpApiServerAddress}{aptCd}/danji?{getData}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(Message.xpErpErrorMessage);
            }
            string temp = await response.Content.ReadAsStringAsync();
            ReponseXperpResult<ResponseXpErpDanji> result = temp.ReadJson<ReponseXperpResult<ResponseXpErpDanji>>();
            if (result.status == XpHubApiStatus.SUCCESS.ToString())
            {
                return result.data;
            }
            else
            {
                _logger.LogInformation(Message.connectFailMessage, nameof(GetXpErpVote));
                return null;
            }
        }
        #endregion


        public Task<string> CreateJwtToken(string userId)
        {
            // JWT 생성
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Common.Common.secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userId) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token =  tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }


    }
}
