using Hub.Client.Model;
using Hub.Shared;

namespace Hub.Client.Interface
{
    public interface iSiteAddress
    {
        SiteAddress siteAddress { get; set; }
        Task ApplyService();
    }

    public class SiteAddressService : iSiteAddress
    {
        public SiteAddress siteAddress { get; set; } = new SiteAddress();
        
        public async Task ApplyService()
        {
        }
    }
}
