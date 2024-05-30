using System.Net.Http.Headers;
using KidsApi.Models.Requests;
using KidsApi.Models.Responses;

namespace KidsWebMvc.API
{
    public class KidsApiClient
    {
            private readonly HttpClient _api;
            public KidsApiClient(IHttpContextAccessor accessor, IHttpClientFactory factory)
            {
                _api = factory.CreateClient("KidsApi");

                var token = accessor.HttpContext.Session.GetString("Token");
                _api.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            }
        public async Task<bool> Register(SignUpRequest request)
        {
            var response = await _api.PostAsJsonAsync("/api/login/Registor", request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<string> Login(string username, string password)
        {
            var response = await _api.PostAsJsonAsync("/api/login",
                new UserLoginRequest { Username = username, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<UserLoginResponce>();

                var token = tokenResponse.Token;
                return token;
            }
            return "";
        }
    }
}
    
