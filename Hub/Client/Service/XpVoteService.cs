using Hub.Client.Interface;
using Hub.Client.Model;
using Hub.Shared;

namespace Hub.Client.Service
{

    /// <summary>
    /// 전자투표
    /// </summary>
    public class XpVoteService : iAdditionalService
    {
        public AdditionalService additionalService { get; set; } = new AdditionalService
        {
            name = AdditionalServiceName.XpVote,
            imageAddress = "",
            tier = Tier.Tier1,
        };
        public async Task ApplyService()
        {
        }
    }
}
