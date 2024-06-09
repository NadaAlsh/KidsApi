using KidsApi.Models.Entites;
using KidsApi.Models.Requests;
using KidsApi.Models.Responses;
using KidsWebMvc.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

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
          ViewBag.children = children;;
          return View();
        }

    [HttpGet]
    [Route("ParentDashBoard/ChildDetails/{id}")]
    public async Task<IActionResult> ChildDetails(int id)
        {
      
          var child = await _client.GetDetails(id);
     
          var rewards = await _client.GetRewards(id);
          var tasks = await _client.GetTasks(id);

          ViewBag.rewards = rewards;
          ViewBag.tasks = tasks;


          return View(child);
        }




    //[HttpGet]
    //public async Task<IActionResult> EditChildDetails(int id)
    //{
    //  // Call the ChildDetails action method to fetch existing child details
    //  var childDetailsActionResult = await ChildDetails(id);

    //  // Check if the ChildDetails action was successful and returned a ViewResult
    //  if (childDetailsActionResult is ViewResult childDetailsViewResult)
    //  {
    //    // Extract the model containing child details from the ChildDetails ViewResult
    //    var childDetailsModel = childDetailsViewResult.Model as ChildAccountResponce;

    //    // Check if the child details model is not null
    //    if (childDetailsModel != null)
    //    {
    //      // Map the ChildAccountResponce model to ChildAccountUpdateRequest
    //      var childUpdateRequest = new ChildAccountUpdateRequest
    //      {
    //        Id = childDetailsModel.Id,
    //        // Populate other properties as needed
    //      };

    //      // Pass the childUpdateRequest model to the EditChildDetails view
    //      return View(childUpdateRequest);
    //    }
    //  }

    //  // Handle the case where child details couldn't be fetched or were invalid
    //  // You can return an error view or redirect to another action
    //  return RedirectToAction("Error");
    //}

    //[HttpPatch]
    //public async Task<IActionResult> EditChildDetails(int id, ChildAccountUpdateRequest model)
    //{

    //  var request = new ChildAccountUpdateRequest
    //  {
    //    Username = model.Username,
    //    Password = model.Password,
    //    Points = model.Points,
    //    Balance = model.Balance,
    //  };

    //  var updatedChild = await _client.UpdateDetails(id, request);

    //  if (updatedChild != null)
    //  {
    //    TempData["Message"] = "Child account updated successfully";
    //    return RedirectToAction("Index", new { id = id }); // or redirect to another action
    //  }

    //  ModelState.AddModelError("", "Failed to update child account");
    //  return View(model);
    //}

    [HttpGet]

    public async Task<IActionResult> EditChildDetails(int id ,ChildAccountResponce responce)
    {
      //var datas = await _client.GetDetails(id);
      //if (datas == null)
      //{
      //  return NotFound();
      //}

      ChildAccountUpdateRequest child = new ChildAccountUpdateRequest()
      {
        Username = responce.Username,
        Password = responce.Password,
        SavingsAccountNumber = responce.SavingsAccountNumber,
        Balance = responce.Balance,
        Points = responce.Points,
      };

     // var child = await _client.GetDetails(id);

      return View(child);
    }

    //[HttpGet]
    //public async Task<IActionResult> EditChildDetails(int id)
    //{
    //  var childDetails = await _client.GetDetails(id);

    //  var childUpdateRequest = new ChildAccountUpdateRequest
    //  {
    //    Username = childDetails.Username,
    //    Password = childDetails.Password,
    //    SavingsAccountNumber = childDetails.SavingsAccountNumber,
    //    Balance = childDetails.Balance,
    //    Points = childDetails.Points
    //  };

    //  return View(childUpdateRequest);
    //}


    [HttpPatch]
    public async Task<IActionResult> EditChildDetails(int id, ChildAccountUpdateRequest model)
    {
      var request = new ChildAccountUpdateRequest
      {
        Username = model.Username,
        Password = model.Password,
        Points = model.Points,
        Balance = model.Balance
      };

      var updatedChild = await _client.UpdateDetails(id, request);

      if (updatedChild)
      {
        TempData["Message"] = "Child account updated successfully";
        return RedirectToAction("Index"); 
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

      var parentId = HttpContext.Session.GetString("ParentID");

      if (parentId != null)
      {
        var request = new AddChildRequest
        {
          ParentId = Int32.Parse(parentId),
          Username = model.Username,

          Password = model.Password,

          SavingsAccountNumber = model.SavingsAccountNumber,

          BaitiAccountNumber = model.BaitiAccountNumber,

          Points = model.Points,

        };
        var isAdded = await _client.AddChild(request);

        if (isAdded)
        {
          TempData["Message"] = "Child added successfully";
          return RedirectToAction("Index");
        }
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
      // Get the list of children from the API
      var parentId = HttpContext.Session.GetString("ParentID");

      var children = await _client.GetChildren(Int32.Parse(parentId));

      ViewBag.Children = children.Select(c => new SelectListItem
      {
        Value = c.Id.ToString(),
        Text = c.Username
      }).ToList();

      var reward = new AddRewardRequest();
      return View(reward);
    }

    [HttpPost]
    public async Task<IActionResult> AddReward(AddRewardRequest model)
    {
      // Get the list of children from the API
      var parentId = HttpContext.Session.GetString("ParentID");

      var children = await _client.GetChildren(Int32.Parse(parentId));

      ViewBag.Children = children.Select(c => new SelectListItem
      {
        Value = c.Id.ToString(),
        Text = c.Username
      }).ToList();

      // Prepare the request to add a reward
      var request = new AddRewardRequest
      {
        RewardType = model.RewardType,
        Description = model.Description,
        RequiredPoints = model.RequiredPoints,
        ChildId = model.ChildId
      };

      var isAdded = await _client.AddReward(request);

      if (isAdded)
      {
        TempData["Message"] = "Reward added successfully";
        return RedirectToAction("Index"); // or redirect to another action
      }

      ModelState.AddModelError("", "Failed to add reward");
      return View(model);
    }

    //[HttpPost]
    //public async Task<IActionResult> AddReward(AddRewardRequest model)
    //{

    //  var children = await _client.GetChildren(model.ChildId);
    //  ViewBag.Children = children.Select(c => new SelectListItem
    //  {
    //    Value = c.Id.ToString(),
    //    Text = c.Username

    //  }).ToList();
    //  var request = new AddRewardRequest
    //  {
    //         RewardType = model.RewardType,
    //         Description =model.Description,
    //         RequiredPoints = model.RequiredPoints,
    //         ChildId = model.ChildId,

    //  };

    //  var isAdded = await _client.AddReward(request);

    //  if (isAdded)
    //  {
    //    TempData["Message"] = "Reward added successfully";
    //    return RedirectToAction("Index"); // or redirect to another action
    //  }

    //  ModelState.AddModelError("", "Failed to add reward");
    //  return View(model);
    //}



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
      var parentId = HttpContext.Session.GetString("ParentID");

      var children = await _client.GetChildren(Int32.Parse(parentId));

      ViewBag.Children = children.Select(c => new SelectListItem
      {
        Value = c.Id.ToString(),
        Text = c.Username
      }).ToList();

      var request = new TaskRequest();
      return View(request);
    }

    [HttpPost]
    public async Task<IActionResult> AddTask(TaskRequest model)
    {

      var parentId = HttpContext.Session.GetString("ParentID");

      if (parentId != null)
      {
        var request = new TaskRequest
        {
          ParentId = Int32.Parse(parentId),
          ChildId = model.ChildId,
          TaskType = model.TaskType,
          Description = model.Description,
          Date = model.Date,
          Points = model.Points,
          CategoryId = model.CategoryId,
        };

          var isAdded = await _client.AddTask(request);

        if (isAdded)
        {
          TempData["Message"] = "Task added successfully";
          return RedirectToAction("Index"); // or redirect to another action
        }

      }

      ModelState.AddModelError("", "Failed to add task");
      return View(model);
    }

  }
}
