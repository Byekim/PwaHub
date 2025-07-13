using Hub.Client.Interface;
using Hub.Client.Model;
using Hub.Shared;

namespace Hub.Client.Service
{

    /// <summary>
    /// 아파트데스크
    /// </summary>
    public class XpAptDeskService : iAdditionalService
    {
        public AdditionalService additionalService { get; set; } = new AdditionalService
        {
            name = AdditionalServiceName.AptDesk,
            imageAddress = "",
            tier = Tier.Tier2,
        };
        public async Task ApplyService()
        {
        }
    }
}
