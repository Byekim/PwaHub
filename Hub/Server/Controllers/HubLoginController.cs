using Hub.Shared;
using Microsoft.AspNetCore.Mvc;
using NetworkLibrary.ApiClient;
using NetworkLibrary;
using Hub.Shared.Model.Hub.Login;
using Hub.Server.Common;
using Hub.Server.Interfaces.Service.Hub;
using Hub.Shared.Model.Hub;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hub.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HubLoginController : ControllerBase
    {
        private readonly iXpHubService _iXpDataHub;
        public HubLoginController(iXpHubService iXpDataHub)
        {
            this._iXpDataHub = iXpDataHub;
        }

        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ResonseLoginData> Login(RequestXpErpBase request)
        {
            ResonseLoginData response = await this.IsValidUser(request);
            if(response.resultMsgStatus != ResultMsgStatus.OK)
            {
                return response;
            }
            //Group User 인지 체크해서 groupUser 라면 GroupUser로 리턴

            if (response.responseXperpLogins?.Count > 0 )
            {
                response.responseXpErpPremium = await this._iXpDataHub.GetXpErpPremiumData(request);
                response.responeXperpMenuYn = await this._iXpDataHub.GetXpErpMenuYn(request, response.responseXperpLogins[0].aptCd);
                response.responseXpErpDanji = await this._iXpDataHub.GetXpErpDanji(request, response.responseXperpLogins[0].aptCd);
                response.securityToken =  await this._iXpDataHub.CreateJwtToken(request.userId);
            }
            
            response.resultMsgStatus = ResultMsgStatus.OK;
            return response;
        }

        private async Task<ResonseLoginData> IsValidUser(RequestXpErpBase request)
        {
            ResonseLoginData response = new ResonseLoginData();
            response.resultMsgStatus = ResultMsgStatus.ERROR;
            var temp = await this._iXpDataHub.GetXperDataWithTokenCheck(() => this._iXpDataHub.GetXpErpLoginData(request));

            if (temp == null)
            {
                response.message = Message.loginErrorMessage2;
                response.resultMsgStatus = ResultMsgStatus.NO_DATA;
                return response;
            }
            if (temp.status != XpHubApiStatus.SUCCESS.ToString())
            {
                response.message = temp.message;
                response.resultMsgStatus = ResultMsgStatus.NO_DATA;
                return response;
            }
            XpErpUserStatus xpErpUserStatus = XpErpUserStatus.N;
            response.responseXperpLogins = temp.data;
            string pw = GlobalFunction.ConvertEncryption(request.passWord);
            bool flag = false;
            foreach (ResponseXperpLogin data in response.responseXperpLogins)
            {
                if (pw.Equals(data.userPw))
                {
                    xpErpUserStatus = string.IsNullOrEmpty(data.useYn) ? XpErpUserStatus.F : (XpErpUserStatus)Enum.Parse(typeof(XpErpUserStatus), data.useYn);
                    flag = true;
                    break;
                }
            }
            if (flag == false)
            {
                response.message = Message.loginErrorMessage3;
                response.resultMsgStatus = ResultMsgStatus.WRONG_PW;
                return response;
            }
            else if(xpErpUserStatus == XpErpUserStatus.X)
            {
                response.message = Message.loginErrorMessage6;
                response.resultMsgStatus = ResultMsgStatus.NOT_AGENT;
                return response;
            }
            
            response.resultMsgStatus = ResultMsgStatus.OK;
            return response;
        }

        /// <summary>
        /// 부가서비스
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("PremiumConfigData")]
        public async Task<ResponseXperpPremiumConfig> PremiumConfigData(RequestXpErpBase request)
        {
            return await this._iXpDataHub.GetXpErpPremiumConfig(request,request.aptCd);
        }

        [HttpPost("GetXpErpVote")]
        public async Task<ResponseXpErpVote?> GetXpErpVote(RequestXpErpBase request)
        {
            var response = await this._iXpDataHub.GetXpErpVote(request);

            return response;
        }

        [HttpPost("Logout")]
        public ResonseLoginData Logout(RequestXpErpBase request)
        {
            return new ResonseLoginData { };
        }


    }
}
