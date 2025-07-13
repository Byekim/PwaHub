using Hub.Client.Interface;
using Hub.Client.Model;
using Hub.Shared;

namespace Hub.Client.Service
{

    /// <summary>
    /// 전자결재
    /// </summary>
    public class XpApprovalService : iAdditionalService
    {
        public AdditionalService additionalService { get; set; } = new AdditionalService
        {
            name = AdditionalServiceName.XpApproval,
            imageAddress = "",
            tier = Tier.Tier1,
        };

        public async Task ApplyService()
        {
        }
    }

}
