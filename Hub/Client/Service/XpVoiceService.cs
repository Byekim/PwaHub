using Hub.Client.Interface;
using Hub.Client.Model;
using Hub.Shared;

namespace Hub.Client.Service
{

    /// <summary>
    /// XpVoice Service
    /// </summary>
    public class XpVoiceService : iAdditionalService
    {
        public AdditionalService additionalService { get; set; } = new AdditionalService
        {
            name = AdditionalServiceName.XpVoice,
            imageAddress = @"https://ac.xperp.co.kr/xperp/res/images/ags/Xp보이스.png",
            tier = Tier.Tier1,
        };


        public async Task ApplyService()
        {

        }
    }
}
