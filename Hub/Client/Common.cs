using Hub.Client.Interface;
using Hub.Client.Model;
using Hub.Client.Service;
using Hub.Shared;
using Hub.Shared.Model.Hub.Login;
using Hub.Shared.Voice;

namespace Hub.Client
{
    public static class Common
    {

        public static List<KeyValuePair<string, string>> speakerList =
            Enum.GetValues<Speaker>()
                .Select(e => new KeyValuePair<string, string>(e.ToString(), GlobalFunction.GetDescription(e)))
                .ToList();
        public static List<GroupMaster> groupMasters = new List<GroupMaster>();

      
        public static ResonseLoginData loginData = new ResonseLoginData();
        public static ResponseXperpLogin userData = new ResponseXperpLogin();
        public static readonly List<SiteAddress> agsSiteList = new List<SiteAddress>()
        {
            new SiteAddress
            {
                 dbLink = DBLink.XPRO,
                 siteName =   "ags.xperp.co.kr",
                 imageAddress = "https://ac.xperp.co.kr/xperp/res/images/ags/New_AGS.png"
            },
            new SiteAddress
            {
                 dbLink = DBLink.YPRO,
                 siteName =   "ags2.xperp.co.kr",
                 imageAddress = "https://ac.xperp.co.kr/xperp/res/images/ags/New_AGS2.png"

            },
            new SiteAddress
            {
                 dbLink = DBLink.IPRO,
                 siteName =   "ags4.xperp.co.kr",
                imageAddress = "https://ac.xperp.co.kr/xperp/res/images/ags/New_AGS4.png"
            },
            new SiteAddress
            {
                 dbLink = DBLink.KPRO,
                 siteName =   "ags4.xperp.co.kr",
                 imageAddress = "https://ac.xperp.co.kr/xperp/res/images/ags/New_AGS4.png"
            },
        };

        public static readonly List<iAdditionalService> appliedServices = new List<iAdditionalService>()
        {
            new XpVoiceService(),
            //new XpCtiService(),
            new XpApprovalService(),
            new XpDocService(),
            new XpVoteService(),
            new XpBankingService(),
            new XpAptDeskService(),
        };
    }


}
