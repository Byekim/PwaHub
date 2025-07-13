using Hub.Client.Interface;
using Hub.Client.Model;
using Hub.Shared;

namespace Hub.Client.Service
{
    /// <summary>
    /// 문서함
    /// </summary>
    public class XpDocService : iAdditionalService
    {
        public AdditionalService additionalService { get; set; } = new AdditionalService
        {
            name = AdditionalServiceName.XpDoc,
            imageAddress = "",
            tier = Tier.Tier1,
        };
        public async Task ApplyService()
        {
        }
    }

}
