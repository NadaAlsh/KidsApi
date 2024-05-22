using KidsApi.Models.Entites;
using KidsApi.Models.Requests;
using KidsApi.Models.Responses;
using KidsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    }
}
