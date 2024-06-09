using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using KidsApi.Models.Entites;
using KidsApi.Models.Requests;
using KidsApi.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure.Core;

namespace KidsWebMvc.API
{
    public class KidsApiClient
    {
            private readonly HttpClient _api;

    //   private readonly IHttpContextAccessor _httpContextAccessor;
    public KidsApiClient(HttpClient httpClient, IHttpContextAccessor accessor, IHttpClientFactory factory)
    {
      _api = httpClient;
      _api.BaseAddress = new Uri("https://kidsapi20240528084240.azurewebsites.net");
      _api.DefaultRequestHeaders.Accept.Clear();
      _api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      var token = accessor.HttpContext.Session.GetString("Token");
      _api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<bool> Register(SignUpRequest request)
        {
            var response = await _api.PostAsJsonAsync("/api/parent/Registor", request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

    public async Task<UserLoginResponce> Login(string username, string password)
    {
      var response = await _api.PostAsJsonAsync("/api/parent/login",
          new UserLoginRequest { Username = username, Password = password });

      if (response.IsSuccessStatusCode)
      {
        var tokenResponse = await response.Content.ReadFromJsonAsync<UserLoginResponce>();

        return tokenResponse;
      }
      return null;
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
            var response = await _api.GetAsync($"api/parent/GetChildren/{parentId}");
            if (response.IsSuccessStatusCode)
            {
              var children = await response.Content.ReadFromJsonAsync<IEnumerable<Child>>();

              return children;
            }

          return new List<Child>();
        }

        public async Task<IEnumerable<MyTask>> GetTasks(int childId)
        {
            var response = await _api
                .GetFromJsonAsync<IEnumerable<MyTask>>($"api/Child/{childId}/GetTasks");
            return response;
        }


        public async Task<bool> AddTask(TaskRequest request)
        {
           var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responce = await _api.PostAsync("api/parent/AddTask", content);

            return responce.IsSuccessStatusCode;
        }


        public async Task<ChildAccountResponce> GetDetails(int id)
        {
      var response = await _api
                .GetFromJsonAsync<ChildAccountResponce>($"api/Parent/ChildDetails/{id}");

      return response;
        }

    //public async Task<bool> UpdateDetails(int id, ChildAccountUpdateRequest request)
    //{
    //  //var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

    //  var response = await _api.PatchAsync($"api/parent/ChildDetails/{id}", request);
    //  try
    //  {
    //    return response.IsSuccessStatusCode;


    //  }
    //  catch 
    //  {
    //    return false;
    //  }

    //}
    public async Task<bool> UpdateDetails(int id, ChildAccountUpdateRequest request)
    {
      var json = JsonSerializer.Serialize(request);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      var response = await _api.PatchAsync($"api/parent/ChildDetails/{id}", content);
      try
      {
        return response.IsSuccessStatusCode ;
      }
      catch
      {
        return false;
      }
    }

    public async Task<bool> AddReward(AddRewardRequest req)
        {

         var json = JsonSerializer.Serialize(req);
         var content = new StringContent(json, Encoding.UTF8, "application/json");
         var response = await _api.PostAsync("api/parent/AddRewards", content);

            return response.IsSuccessStatusCode;
        }
        public async Task<List<Reward>> GetRewards(int childId)
        {
            var response = await _api.GetAsync($"api/child/{childId}/GetRewards");
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
        public async Task<Boolean> AddChild(AddChildRequest req)
        {
            var json = JsonSerializer.Serialize(req);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _api.PostAsync("api/parent/addchild", content);
           
            return response.IsSuccessStatusCode;
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

    public async Task<ClaimedRewardResponce> ClaimReward(int childId, ClaimedRewards request)
    {
      var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
      var response = await _api.PostAsync($"api/child/{childId}/claimThisReward", content);
      response.EnsureSuccessStatusCode();

      var newClaimedReward = await response.Content.ReadFromJsonAsync<ClaimedRewardResponce>();
      return newClaimedReward;
    }
    public async Task<ChildAccountResponce> GetChildDetails(int id)
    {
      var response = await _api.GetFromJsonAsync<ChildAccountResponce>($"api/Parent/Child/{id}");
      return response;
    }

  }
}




    
