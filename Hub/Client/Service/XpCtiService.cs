using Hub.Client.Interface;
using Hub.Client.Model;
using Hub.Shared;

namespace Hub.Client.Service
{
    /// <summary>
    /// XpCtiService
    /// </summary>
    public class XpCtiService : iAdditionalService
    {
        private readonly FileSystemService _fileSystemService;

        public AdditionalService additionalService { get; set; } = new AdditionalService
        {
            name = AdditionalServiceName.XpCti,
            imageAddress = @"https://ac.xperp.co.kr/xperp/res/images/ags/Xp통화매니저.png",
            tier = Tier.Tier1,
        };

        public XpCtiService(FileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }

        public async Task ApplyService()
        {
            //var fileName = await FileSystemService.OpenFileAsync();
        }
    }

}
