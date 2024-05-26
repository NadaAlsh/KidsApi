using KidsApi.Models.Entites;
using KidsApi.Models.Requests;
using KidsApi.Models.Responses;
using KidsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace KidsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {
        private readonly TokenService _service;
        private readonly KidsContext _context;

        public ChildController(TokenService service, KidsContext context)
        {
            _service = service;
            _context = context;
        }
        [HttpPost("Login")]
        public IActionResult ChildLogin(ChildLoginRequest request)
        {
            var response = _service.ChildGenerateToken(request.Username, request.Password);

            if (response.IsValid)
            {
                return Ok(new ChildLoginResponse{ Token = response.Token });
            }
            else
            {
                return BadRequest("Username and/or Password is wrong");
            }



        }
              public class UserLogin()
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }


        [HttpGet]
        public IActionResult GetTasks(int parentId)
        {
            var tasks = _context.Tasks.Where(t => t.ParentId == parentId).ToList();
            return Ok(tasks);
        }

        [HttpPut("{childId}/tasks/{taskId}/complete")]
        public IActionResult CompleteTask(int childId, int taskId)
        {
            var child = _context.Child.FirstOrDefault(c => c.Id == childId);
            var task = child.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (child == null || task == null)
            {
                return NotFound();
            }

            task.isCompleted = true;
            _context.SaveChanges();

            return Ok();
        }

       }
    }

