using Hub.Shared;

namespace Hub.Client.Model
{
    public class AdditionalService
    {
        public AdditionalServiceName name { get; set; }
        
        public string? imageAddress { get; set; }

        public Tier tier { get; set; }
    }
}
