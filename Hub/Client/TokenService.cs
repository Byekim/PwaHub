using Blazored.LocalStorage;

namespace Hub.Client
{
    public class TokenService
    {
        private readonly ILocalStorageService _localStorageService;

        public TokenService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<string> GetJwtTokenAsync()
        {
            var jwtToken = await _localStorageService.GetItemAsync<string>("jwtToken");
            return jwtToken;
        }
    }
}
