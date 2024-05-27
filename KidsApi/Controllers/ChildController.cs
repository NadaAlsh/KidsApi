using KidsApi.Models.Entites;
using KidsApi.Models.Requests;
using KidsApi.Models.Responses;
using KidsApi.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        //[HttpPost("Login")]
        //public IActionResult ChildLogin(ChildLoginRequest request)
        //{
        //    var response = _service.ChildGenerateToken(request.Username, request.Password);

        //    if (response.IsValid)
        //    {
        //        return Ok(new ChildLoginResponse { Token = response.Token });
        //    }
        //    else
        //    {
        //        return BadRequest("Username and/or Password is wrong");
        //    }



        //}

        [HttpPost("login")]
        public ActionResult Login([FromBody] ChildLoginRequest request)
        {
            // validate request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // find child entity by username and password
            var child = _context.Child
                .Where(c => c.Username == request.Username && c.Password == request.Password)
                .FirstOrDefault();

            if (child == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // return child's details
            return Ok(new
            {
                ChildId = child.Id,
            
            });
        }

        public class UserLogin()
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }


        [HttpGet("{Id}/GetTasks")]
        public IActionResult GetTasks(int parentId)
        {
            var tasks = _context.Task.Where(t => t.ParentId == parentId).ToList();
            return Ok(tasks);
        }

        //[HttpPut("{childId}/tasks/{taskId}/complete")]
        //public IActionResult CompleteTask(int childId, int taskId)
        //{
        //    var child = _context.Child.Include(c => c.Tasks).FirstOrDefault(c => c.Id == childId);
        //    if (child == null)
        //    {
        //        return NotFound(new { message = "Child not found" });
        //    }

        //    var task = child.Tasks.FirstOrDefault(t => t.Id == taskId);
        //    if (task == null)
        //    {
        //        return NotFound(new { message = "Task not found" });
        //    }

        //    task.isCompleted = true;
        //    _context.SaveChanges();

        //    return Ok();
        //}

        [HttpPut("{childId}/tasks/{taskId}/complete")]
        public IActionResult CompleteTask(int childId, int taskId)
        {
            var child = _context.Child.Include(c => c.Tasks).FirstOrDefault(c => c.Id == childId);
            if (child == null)
            {
                return NotFound(new { message = "Child not found" });
            }

            var task = child.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return NotFound(new { message = "Task not found" });
            }

            var pointsToAdd = task.Points;
            child.Points += pointsToAdd;
            _context.SaveChanges();

            task.isCompleted = true;
            _context.SaveChanges();

            return Ok(new { Message = "Points added successfully and task completed." });
        }


        [HttpGet("{childId}/balance")]
        public ActionResult<GetBalanceResponse> GetBalance(int childId)
        {
            var child = _context.Child.FirstOrDefault(c => c.Id == childId);
            if (child == null)
            {
                return NotFound();
            }

            var balanceResponse = new GetBalanceResponse
            {
                Balance = child.Balance
            };

            return Ok(balanceResponse);
        }


        [HttpGet("GetPoints/{childId}")]
        public ActionResult<PointsResponse> GetPoints(int childId)
        {
            var child = _context.Child.FirstOrDefault(c => c.Id == childId);
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

        //[HttpPut("AddPoints/{childId}")]
        //public IActionResult AddPoints(int childId, AddPointsRequest request)
        //{
        //    var child = _context.Child.FirstOrDefault(c => c.Id == childId);
        //    if (child == null)
        //    {
        //        return NotFound();
        //    }

        //    child.Points += request.PointsToAdd;
        //    _context.SaveChanges();

        //    return Ok(new { Message = "Points added successfully." });
        //}
    }
}

    
