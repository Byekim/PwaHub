using Hub.Client.Model;
using Hub.Shared;
using System.Xml.Linq;

namespace Hub.Client.Interface
{
    
    public interface iAdditionalService
    {
        AdditionalService additionalService { get; set; }
        Task ApplyService(); 
    }

    // ExecuteClass
    public class Execute
    {       
        public List<iAdditionalService> AppliedServices { get; } = new List<iAdditionalService>();

        public void AddService(iAdditionalService service)
        {
            service.ApplyService();
            AppliedServices.Add(service);
        }

        public void AllServiceExecute()
        {
            foreach (var service in AppliedServices)
            {
                service.ApplyService();
            }
        }   
    }
}
