using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using KidsApi.Models.Entites;
using KidsApi.Models.Requests;
using KidsApi.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KidsWebMvc.API
{
    public class KidsApiClient
    {
            private readonly HttpClient _api;

            private readonly IHttpContextAccessor _httpContextAccessor;
            public KidsApiClient(IHttpContextAccessor accessor, IHttpClientFactory factory)
            {
                _api = factory.CreateClient("KidsApi");
               _httpContextAccessor = accessor;

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
            var response = await _api.PostAsJsonAsync("/api/parent/login",
                new UserLoginRequest { Username = username, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<UserLoginResponce>();

                var token = tokenResponse.Token;

                return token;
            }
            return "";
        }
        public async Task<string> ChildLogin(string username, string password)
        {
            var response = await _api.PostAsJsonAsync("/api/child/login",
                new ChildLoginRequest { Username = username, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<ChildLoginResponse>();

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

        public async Task<IEnumerable<MyTask>> GetTasks(int childId)
        {
            var response = await _api
                .GetFromJsonAsync<IEnumerable<MyTask>>($"api/child/GetTasks/{childId}");
            return response;
        }
        public async Task<MyTask> AddTask(TaskRequest request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _api.PostAsync("api/AddTask", content);
            response.EnsureSuccessStatusCode();

            var newTask = await response.Content.ReadFromJsonAsync<MyTask>();
            return newTask;
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
        public async Task<List<Reward>> GetRewards(int parentId)
        {
            var response = await _api.GetAsync("api/child/{Id}/GetRewards");
            response.EnsureSuccessStatusCode();

            var rewards = await response.Content.ReadFromJsonAsync<List<Reward>>();
            return rewards;
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

        public async Task<IActionResult> CompleteTask(int childId, int taskId)
        {
            var apiUrl = $"api/{childId}/tasks/{taskId}/complete";
            var response = await _api.PutAsync(apiUrl, null);
            response.EnsureSuccessStatusCode();

            return new OkObjectResult(new { Message = "Points added successfully and task completed." });
        }

        public async Task<List<GetBalanceResponse>> GetBalance(int childId)
        {
            var response = await _api.GetAsync("api/child/GetBalance");
            response.EnsureSuccessStatusCode();

            var rewards = await response.Content.ReadFromJsonAsync<List<GetBalanceResponse>>();
            return rewards;
        }

        public async Task<List<GetBalanceResponse>> GetChildBalance(int parentId)
        {
            var response = await _api.GetAsync("api/parent/balance");
            response.EnsureSuccessStatusCode();

            var rewards = await response.Content.ReadFromJsonAsync<List<GetBalanceResponse>>();
            return rewards;
        }

        public async Task<List<PointsResponse>> GetPoints(int childId)
        {
            var response = await _api.GetAsync("api/child/GetPoints/{childId}");
            response.EnsureSuccessStatusCode();

            var rewards = await response.Content.ReadFromJsonAsync<List<PointsResponse>>();
            return rewards;
        }


        public async Task<List<PointsResponse>> GetChildPoints(int childId)
        {
            var response = await _api.GetAsync("api/parent/children/{childId}/points");
            response.EnsureSuccessStatusCode();

            var rewards = await response.Content.ReadFromJsonAsync<List<PointsResponse>>();
            return rewards;
        }

        public async Task<Transfer> TransferPointsToMoney(int parentId, int childId, TransferRequest request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _api.PostAsync("api/child/{parentId}/transfer/{childId}", content);
            response.EnsureSuccessStatusCode();

            var newTransfer = await response.Content.ReadFromJsonAsync<Transfer>();
            return newTransfer;
        }

        public async Task<List<ClaimedRewardResponce>> GetClaimedRewards(int childId)
        {
            var response = await _api.GetFromJsonAsync<List<ClaimedRewardResponce>>($"api/child/{childId}/claimedrewards");
            return response;
        }

        public async Task<List<TaskHistoryResponse>> GetTaskHistory(int childId)
        {
            var response = await _api.GetFromJsonAsync<List<TaskHistoryResponse>>($"api/parent/child/{childId}/taskhistory");
            return response;
        }
    }
}
    
