using Hub.Shared.Model.Hub.Login;

namespace Hub.Server.Interfaces.Service.Hub
{
    public interface iXpHubService
    {
        public Task GetXpErpToken();
        public Task<ReponseXperpResult<List<ResponseXperpLogin>>> GetXpErpLoginData(RequestXpErpBase request);
        public Task<ResponseXpErpPremium?> GetXpErpPremiumData(RequestXpErpBase request);
        public Task<ResponseXperpPremiumConfig?> GetXpErpPremiumConfig(RequestXpErpBase request, string aptCd);
        public Task<ResponseXpErpVote?> GetXpErpVote(RequestXpErpBase request);
        public Task<ResponseXperpMenuYn?> GetXpErpMenuYn(RequestXpErpBase request, string aptCd);
        public Task<ResponseXpErpDanji?> GetXpErpDanji(RequestXpErpBase request, string aptCd);
        public Task<string> CreateJwtToken(string userId);
        public Task<T> GetXperDataWithTokenCheck<T>(Func<Task<T>> fetchDataFunc) where T : class, new();
    }
}
