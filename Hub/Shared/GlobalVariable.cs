using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hub.Shared.Model.Hub.Login;
using Hub.Shared.Model.Hub;

namespace Hub.Shared
{
    public  class GlobalVariable
    {
        private static readonly string _logFileDir = $@"{AppDomain.CurrentDomain.BaseDirectory}Log";
        //public static IConfigurationRoot appSettings => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
        public static readonly string logFileDir = GlobalVariable._logFileDir;

        public static readonly string commonPath = $@"yourSettingPath";
        public static readonly string commonXperpApiPath = $@"yourIniPath";
        public static ResponseBannerData resposneBannerData = new ResponseBannerData() { body = string.Empty, url = string.Empty };
        public static ResponseXperpToken xpErpTokenData = new ResponseXperpToken() { access_token = string.Empty, expires_in = 0, token_type = string.Empty };        //Real
        public static readonly string cognitoClientId = "yourContigoClientId";
        public static readonly string cognitoClientSecret = "yourSecretid";
        public static readonly string cognitoServerAddress = @"yourServerAddress";
        public static readonly string xpErpApiServerAddress = @"yourApiServerAddress";
        public static readonly string hubServerApi = @"api/HubLogin/";
        public static readonly string voiceServerApi = @"api/Voice/";
        public static readonly string authKey = "youKey";
        public static readonly int maxIdLength = 5;
        public static readonly long telegramChatId = 5L;//yourTelegramChatId;
    }
}
