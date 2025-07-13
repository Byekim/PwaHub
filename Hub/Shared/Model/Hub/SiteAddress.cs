using Hub.Shared;

namespace Hub.Shared.Model.Hub
{
    public class SiteAddress
    {
        public DBLink dbLink { get; set; }
        public string? siteName { get; set; }
        public string? imageAddress { get; set; }
    }
    /*
    public class SiteAddress2
    {
        public string? aptCd { get; set; }
        public DBLink dbLink { get; set; }
        public string? siteName { get; set; }
        public string? imageAddress { get; set; }
    }*/
}
