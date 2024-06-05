using Azure.Core;
using KidsApi.Models.Entites;
using KidsApi.Models.Requests;
using KidsApi.Models.Responses;
using KidsApi.Services;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.EntityFrameworkCore;

namespace KidsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController : ControllerBase
    {


        private readonly TokenService service;
        private readonly KidsContext context;

        public ParentController(TokenService service, KidsContext context)
        {
            this.service = service;
            this.context = context;
        }

        [HttpPost("login")]

        public IActionResult Login(UserLoginRequest loginDetails)
        {
            var parent = context.Parent.FirstOrDefault(c => c.Username == loginDetails.Username);
            var response = service.GenerateToken(loginDetails.Username, loginDetails.Password);
            if (response.IsValid)
            {
                return Ok(new UserLoginResponce {
                        Token = response.Token,
                        ParentId = parent.ParentId,
                        Username = parent.Username,
                    });

           
            }
            return BadRequest("Username and/or Password is wrong");
        }

        [HttpPost("Registor")]
        public IActionResult Registor(SignUpRequest request)
        {

            var newUser = Parent.Create(request.Username, request.Password, request.PhoneNumber, request.Email, request.IsAdmin);

            newUser.Username = request.Username;
            //newUser.Password = request.Password;
            newUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);


            context.Parent.Add(newUser);
            context.SaveChanges();

            return Ok(new { Message = "User Created" });

        }

        [HttpGet("{parentId}/GetTasks")]
        public IActionResult GetTasks(int parentId)
        {
            var tasks = context.Task.Where(t => t.ParentId == parentId).ToList();
            if (tasks.Any())
            {
                return Ok(tasks);
            }
            return NotFound("No tasks found for this parent.");
        }

        [HttpPost("AddTask")]
        public IActionResult PostTask(TaskRequest request)
        {
            var category = context.Categories.FirstOrDefault(c => c.CategoryId == request.CategoryId);


            if (category == null)
            {
                return BadRequest("Invalid category ID.");
            }
            var parent = context.Parent.Find(request.ParentId);
            if (parent == null)
            {
                return NotFound($"Parent with id {request.ParentId} not found");
            }
            var newTask = new Models.Entites.MyTask
            {
                TaskType = request.TaskType,
                Description = request.Description,
                Date = request.Date,
                Points = request.Points,
                childId = request.ChildId,
                CategoryId = category.CategoryId,
                ParentId = request.ParentId,
            };

            context.Task.Add(newTask);
             context.SaveChanges();

            return Ok(new { Message = "Task created" });
        }

        //private List<SelectListItem> GetCategories()
        //{
        //    var categories = context.Categories.Select(c => new SelectListItem
        //    {
        //        Value = c.CategoryId.ToString(),
        //        Text = c.Name
        //    }).ToList();

        //    return categories;
        //}

        [HttpGet("Details/{id}")]
        [ProducesResponseType(typeof(ChildAccountResponce), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ChildAccountResponce> Details([FromRoute] int id)
        {
            var child = context.Child.Find(id);
            if (child == null)
            {
                return NotFound();
            }

            return Ok(new ChildAccountResponce
            {
                Id = child.Id,
                Username = child.Username,
                Password = child.Password,
                SavingsAccountNumber = child.SavingsAccountNumber,
                BaitiAccountNumber = child.BaitiAccountNumber,
                Points = child.Points,
            });
        }

        [HttpPatch("Details/{id}")]
        [ProducesResponseType(typeof(ChildAccountResponce), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ChildAccountResponce> Details([FromRoute] int id, [FromBody] ChildAccountUpdateRequest request)
        {
            var child = context.Child.Find(id);
            if (child == null)
            {
                return NotFound();
            }

            if (request != null)
            {
                // Update editable fields
                child.Username = request.Username;
                child.Password = request.Password;
                child.SavingsAccountNumber = request.SavingsAccountNumber;
                child.Points = request.Points;
                child.Balance = request.Balance;
                context.SaveChanges();
            }

            return Ok(new ChildAccountResponce
            {
                Id = child.Id,
                Username = child.Username,
                Password = child.Password,
                SavingsAccountNumber = child.SavingsAccountNumber,
                BaitiAccountNumber = child.BaitiAccountNumber,
                Points = child.Points,
            });
        }
        //[HttpPost("AddChild")]
        //public IActionResult AddChild(AddChildRequest req)
        //{
        //    var newChild = new Child()
        //    {
        //        Id = req.Id,
        //        Username = req.Username,
        //        Password = req.Password,
        //        SavingsAccountNumber = req.SavingsAccountNumber,
        //        Points = req.Points,
        //        ParentId = req.ParentId,

        //    };
        //    context.Child.Add(newChild);
        //    context.SaveChanges();

        //    return CreatedAtAction(nameof(Details), new { Id = newChild.Id }, newChild);
        //}

        [HttpPost("addchild")]
        public ActionResult AddChild([FromBody] AddChildRequest request)
        {
            // validate request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // create new child entity
            var child = new Child
            {
                Username = request.Username,
                Password = request.Password,
                ParentId = request.ParentId,
                BaitiAccountNumber = request.BaitiAccountNumber,
                Points = request.Points,

            };

            // add child to database
            context.Child.Add(child);
            context.SaveChanges();

            // create parent-child relationship
            var parentChildRelationship = new ParentChildRelationship
            {
                ParentId = request.ParentId,
                ChildId = child.Id
            };
            context.ParentChildRelationships.Add(parentChildRelationship);
            context.SaveChanges();

            return Ok("Child added successfully!");
        }
        
        [HttpGet("GetChildren/{parentId}")]
        public IActionResult GetChildren(int parentId)
        {
            var children = context.Child
                .Where(c => c.ParentId == parentId)
                .ToList();

            if (children == null || children.Count == 0)
            {
                return NotFound();
            }

            return Ok(children);
        }

        //[HttpPost("AddReward")]
        //public IActionResult AddReward([FromBody] AddRewardRequest req)
        //{
        //    var newReward = new Reward()
        //    {
        //        Id = req.Id,
        //        RequiredPoints = req.RequiredPoints,
        //        Description = req.Description,
        //        RewardType = req.RewardType,
        //        children = new List<Child>()
        //    };

        //    foreach (var childId in req.children)
        //    {
        //        var child = context.Child.Find(childId);
        //        if (child != null)
        //        {
        //            newReward.children.Add(child);
        //        }
        //    }

        //    context.Rewards.Add(newReward);
        //    context.SaveChanges();

        //    return CreatedAtAction(nameof(Details), new { Id = newReward.Id }, newReward);
        //}

        //[HttpPost("AddReward")]
        //public IActionResult AddReward([FromBody] AddRewardRequest req)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var newReward = new Reward
        //    {
        //        RequiredPoints = req.RequiredPoints,
        //        Description = req.Description,
        //        RewardType = req.RewardType,
        //        Children = req.children.Select(childId => context.Child.Find(childId)).Where(child => child != null).ToList()
        //    };

        //    context.Rewards.Add(newReward);
        //    context.SaveChanges();

        //    return CreatedAtAction(nameof(Details), new { Id = newReward.Id }, newReward);
        //}
        //[HttpPost("AddReward")]
        //public IActionResult Add(AddRewardRequest req)
        //{
        //    var newReward = new Reward()
        //    {
        //        Id = req.Id,
        //        RequiredPoints = req.RequiredPoints,
        //        Description = req.Description,
        //        RewardType = req.RewardType,
        //        children = req.children
        //    };

        //    context.Rewards.Add(newReward);
        //    context.SaveChanges();

        //    return CreatedAtAction(nameof(Details), new { Id = newReward.Id }, newReward);
        //}

        [HttpGet("GetAllRewardsForParents")]
        public IActionResult GetAllRewards()
        {
            var rewards = context.Rewards.ToList();

            if (rewards == null || rewards.Count == 0)
            {
                return NotFound();
            }

            return Ok(rewards);
        }


        [HttpGet("balance")]
        public ActionResult<GetBalanceResponse> GetBalance(int parentId)
        {
            var parent = context.Parent.FirstOrDefault(p => p.ParentId == parentId);
            if (parent == null)
            {
                return NotFound();
            }

            var balanceResponse = new GetBalanceResponse
            {
                Balance = parent.Balance
            };

            return Ok(balanceResponse);
        }

        [HttpGet("children/{childId}/points")]
        public ActionResult<decimal> GetChildPoints(int childId)
        {
            var child = context.Child.FirstOrDefault(c => c.Id == childId);
            if (child == null)
            {
                return NotFound();
            }

            var pointsResponse = new PointsResponse
            {
                Points = child.Points
            };

            return Ok(pointsResponse);
        }


        //[HttpGet("child/{childId}/taskhistory")]
        //public ActionResult<List<TaskHistoryResponse>> GetTaskHistory(int childId)
        //{
        //    var childTasks = context.Task
        // .Where(t => t.Id == childId && t.isCompleted)
        // .ToList();

        //    var taskHistory = childTasks.Select(t => new TaskHistoryResponse
        //    {
        //        TaskId = t.Id,
        //        CategoryName = t.Category.Name,
        //        Description = t.Description,
        //        Points = t.Points,
        //        CompletionDate = t.Date
        //    }).ToList();

        //    return Ok(taskHistory);
        //}


        [HttpPost("Addrewards")]
        public IActionResult AddReward(AddRewardRequest request)
        {
            var parent = context.Parent.Include(p => p.Rewards).Include(p => p.children);

            if (parent == null)
            {
                return NotFound();
            }

            var reward = new Reward
            {
                RewardType = request.RewardType,
                Description = request.Description,
                RequiredPoints = request.RequiredPoints,
                ChildId = request.ChildId,

            };

           
            

            context.Rewards.Add(reward);
            context.SaveChanges();

            return CreatedAtAction(nameof(AddReward), new { rewardId = reward.Id }, reward);
        }



    

        [HttpGet("child/{childId}/taskhistory")]
        public ActionResult<List<TaskHistoryResponse>> GetTaskHistory(int childId)
        {
            var taskHistory = context.Task
                .Where(t => t.childId == childId && t.isCompleted)
                .Select(t => new TaskHistoryResponse
                {
                    TaskId = t.Id,
                   // CategoryName= t.Category.Name,
                    Description = t.Description,
                    Points = t.Points,
                    CompletionDate = t.Date
                })
                .ToList();

            return Ok(taskHistory);
        }
    }
}

   


