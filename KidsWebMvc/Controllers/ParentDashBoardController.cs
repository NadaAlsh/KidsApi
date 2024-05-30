using Microsoft.AspNetCore.Mvc;

namespace KidsWebMvc.Controllers
{
    public class ParentDashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
