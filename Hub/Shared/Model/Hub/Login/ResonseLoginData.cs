using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Hub.Shared.Model.Hub.Login
{
    public class ResonseLoginData
    {
        public ResultMsgStatus resultMsgStatus { get; set; }
        public string? message { get; set; }
        public List<ResponseXperpLogin>? responseXperpLogins { get; set; }
        public ResponseXpErpDanji? responseXpErpDanji { get; set; }
        public ResponseXperpMenuYn? responeXperpMenuYn { get; set; }
        public ResponseXperpPremiumConfig? responseXperpPremiumConfig { get; set; }
        public ResponseXpErpPremium? responseXpErpPremium { get; set; }
        public ResponseXpErpVote? responseXpErpVote { get; set; }
        public ResponseXpHubUserGroup? responseHubUserGroup { get; set; }
        public string securityToken { get; set; }
    }
}
