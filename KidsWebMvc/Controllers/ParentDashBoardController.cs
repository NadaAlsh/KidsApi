using KidsApi.Models.Entites;
using KidsApi.Models.Requests;
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

    [HttpGet]
        public async Task<IActionResult> Index()
        {
          var parentUsername = HttpContext.Session.GetString("Username");
          var parentId = HttpContext.Session.GetString("ParentID");

          var children = await _client.GetChildren(Int32.Parse(parentId));
          ViewBag.username = parentUsername;
          ViewBag.children = children;
          return View();
        }

    [HttpGet]
    public async Task<IActionResult> ChildDetails(int childId)
        {

          var child = await _client.GetDetails(childId);

          return View(child);
        }

    [HttpGet]
    public async Task<IActionResult> EditChildDetails()
    {
      // TODO Get the childe detials form ChildDetails page 
      ChildAccountUpdateRequest child = new ChildAccountUpdateRequest() {

      };
      return View(child);
    }

      [HttpPatch]
    public async Task<IActionResult> EditChildDetails(int id, ChildAccountUpdateRequest model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var request = new ChildAccountUpdateRequest
      {
        Username = model.Username,
        Password = model.Password,
        Points = model.Points,
        Balance = model.Balance,
      };

      var updatedChild = await _client.UpdateDetails(id, request);

      if (updatedChild != null)
      {
        TempData["Message"] = "Child account updated successfully";
        return RedirectToAction("Details", new { id = id }); // or redirect to another action
      }

      ModelState.AddModelError("", "Failed to update child account");
      return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> AddChild()
    {
      var model = new AddChildRequest(); 

      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddChild(AddChildRequest model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      var request = new AddChildRequest
      {
        Username = model.Username,

        Password = model.Password,

        SavingsAccountNumber = model.SavingsAccountNumber,

        Points = model.Points,
   
      };
      var newChild = await _client.AddChild(request);

      if (newChild != null)
      {
        TempData["Message"] = "Child added successfully";
        return RedirectToAction("Index"); 
      }

      ModelState.AddModelError("", "Failed to add child");
      return View(model);
    }

        public async Task<IActionResult> TaskHistory(int childId)
        {
      
            var taskHistory = await _client.GetTaskHistory(childId);
            return View(taskHistory);
        }

    [HttpGet]
    public async Task<IActionResult> AddReward()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddReward(AddRewardRequest model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var request = new AddRewardRequest
      {
             RewardType = model.RewardType,
             Description =model.Description,
             RequiredPoints = model.RequiredPoints,
             ChildId = model.ChildId,

      };

      var newReward = await AddReward(request);

      if (newReward != null)
      {
        TempData["Message"] = "Reward added successfully";
        return RedirectToAction("Rewards"); // or redirect to another action
      }

      ModelState.AddModelError("", "Failed to add reward");
      return View(model);
    }



    public async Task<IActionResult> Balance(int childId)
    {
      var balance = await _client.GetBalance(childId);
      return View(balance);
    }

    public async Task<IActionResult> Points(int childId)
    {
      var points = await _client.GetPoints(childId);
      return View(points);
    }


    [HttpGet]
    public async Task<IActionResult> AddTask()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddTask(TaskRequest model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var request = new TaskRequest
      {
          ChildId = model.ChildId,
          TaskType = model.TaskType,
          Description = model.Description,
          Date = model.Date,
          Points = model.Points,
          CategoryId = model.CategoryId,
      };

      var newTask = await _client.AddTask(request);

      if (newTask != null)
      {
        TempData["Message"] = "Task added successfully";
        return RedirectToAction("Tasks"); // or redirect to another action
      }

      ModelState.AddModelError("", "Failed to add task");
      return View(model);
    }


  }
}
