using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using KidsApi.Models.Entites;
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
            var response = await _api.PostAsJsonAsync("/api/Registor", request);
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

        public async Task<IEnumerable<Child>> GetChildren(int parentId)
        {
            var response = await _api
                .GetFromJsonAsync<IEnumerable<Child>>($"api/parent/GetChildren/{parentId}");
            return response;
        }

        public async Task<ChildAccountResponce> GetDetails(int id)
        {
            var response = await _api
                .GetFromJsonAsync<ChildAccountResponce>($"api/child/Details/{id}");
            return response;
        }
        public async Task<Reward> AddReward(AddRewardRequest req)
        {
            var content = new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json");
            var response = await _api.PostAsync("api/parent/AddReward", content);
            response.EnsureSuccessStatusCode();

            var newReward = await response.Content.ReadFromJsonAsync<Reward>();
            return newReward;
        }
        public async Task<List<Reward>> GetAllRewards()
        {
            var response = await _api.GetAsync("api/parent/GetAllRewards");
            response.EnsureSuccessStatusCode();

            var rewards = await response.Content.ReadFromJsonAsync<List<Reward>>();
            return rewards;
        }
        public async Task<Child> AddChild(AddChildRequest req)
        {
            var json = JsonSerializer.Serialize(req);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _api.PostAsync("api/parent/Add", content);
            response.EnsureSuccessStatusCode();

            var newChild = await response.Content.ReadFromJsonAsync<Child>();
            return newChild;
        }
    }
}
    
