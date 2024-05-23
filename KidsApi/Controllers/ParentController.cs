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
    public class ParentController: ControllerBase
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

            var response = service.GenerateToken(loginDetails.Username, loginDetails.Password);
            if (response.IsValid)
            {
                return Ok(new UserLoginResponce { Token = response.Token });
            }
            return BadRequest("Username and/or Password is wrong");
        }

        [HttpPost("Registor")]
        public IActionResult Registor(SignUpRequest request)
        {

            var newUser = Parent.Create(request.Username, request.Password , request.PhoneNumber,request.Email, request.IsAdmin);

            newUser.Username = request.Username;
            //newUser.Password = request.Password;
            newUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);


            context.Parent.Add(newUser);
            context.SaveChanges();

            return Ok(new { Message = "User Created" });

        }

        [HttpGet("{parentId}/tasks")]
        public IActionResult GetTasks(int parentId)
        {
            var tasks = context.Tasks.Where(t => t.ParentId == parentId).ToList();
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
            var newTask = new Models.Entites.Task
            {
               
                TaskType = request.TaskType,
                Description = request.Description,
                Date = request.Date,
                Points = request.Points,
                childId = request.ChildId,
                CategoryId = category.CategoryId


            };

            context.Tasks.Add(newTask);
            context.SaveChanges();

            return Ok(new { Message = "Task created", TaskId = newTask.Id });
        }

        private List<SelectListItem> GetCategories()
        {
            var categories = context.Categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList();

            return categories;
        }

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
        [HttpPost]
        public IActionResult Add(AddChildRequest req)
        {
            var newChild = new Child()
            {
                Id = req.Id,
                Username = req.Username,
                Password = req.Password,
                SavingsAccountNumber = req.SavingsAccountNumber,
                Points = req.Points,
                ParentId = req.ParentId,

            };
            context.Child.Add(newChild);
            context.SaveChanges();

            return CreatedAtAction(nameof(Details), new { Id = newChild.Id }, newChild);
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

        [HttpGet]
        public IActionResult GetAllRewards()
        {
            var rewards = context.Rewards.ToList();

            if (rewards == null || rewards.Count == 0)
            {
                return NotFound();
            }

            return Ok(rewards);
        }
      }

    }


