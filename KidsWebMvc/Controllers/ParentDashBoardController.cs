using KidsWebMvc.API;
using Microsoft.AspNetCore.Mvc;

namespace KidsWebMvc.Controllers
{
    public class ParentDashBoardController : Controller
    {
        private readonly KidsApiClient _client;
        public ParentDashBoardController(KidsApiClient client)
        {
            _client = client;
        }
        public async Task<IActionResult> Index(int parentId)
        {
            var child = await _client.GetChildren(parentId);
            return View(child);
        }

        public async Task<IActionResult> Details()
        {


            return View();
        }


        public async Task<IActionResult> AddChild()
        {
            return View();
        }

        public async Task<IActionResult> AddTask()
        {
            return View();
        }
        public async Task<IActionResult> TaskJistory()
        {
            return View();
        }

        public async Task<IActionResult> AddReward()
        {
            return View();
        }

        public async Task<IActionResult> Requests()
        {
            return View();
        }



    }
}