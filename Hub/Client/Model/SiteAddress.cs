using Hub.Shared;

namespace Hub.Client.Model
{
    public class SiteAddress
    {
        public string? aptCd { get; set; }
        public DBLink dbLink { get; set; }
        public string? siteName { get; set; }
        public string? imageAddress { get; set; }
    }
}
