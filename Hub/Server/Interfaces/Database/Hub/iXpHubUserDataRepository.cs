using Hub.Shared.Voice;
using Hub.Shared;
using Hub.Shared.Model.Hub;

namespace Hub.Server.Interfaces.Database.Hub
{
    public interface iXpHubUserDataRepository
    {
        Task<List<ResponseXpHubUserGroup>> GetXpHubUserGroupData(string id, string aptCd,string dbLink);
    }
}
