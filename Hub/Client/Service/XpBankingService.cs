using Hub.Client.Interface;
using Hub.Client.Model;
using Hub.Shared;

namespace Hub.Client.Service
{

    /// <summary>
    /// 아파트뱅킹
    /// </summary>
    public class XpBankingService : iAdditionalService
    {
        public AdditionalService additionalService { get; set; } = new AdditionalService
        {
            name = AdditionalServiceName.XpBanking,
            imageAddress = "",
            tier = Tier.Tier1,
        };
        public async Task ApplyService()
        {
        }
    }

}
