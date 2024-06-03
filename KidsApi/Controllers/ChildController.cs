using KidsApi.Models.Entites;
using KidsApi.Models.Requests;
using KidsApi.Models.Responses;
using KidsApi.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KidsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {
        private readonly TokenService _service;
        private readonly KidsContext _context;
        private readonly IConfiguration _configuration;

        public ChildController(TokenService service, KidsContext context, IConfiguration configuration)
        {
            _service = service;
            _context = context;
            _configuration = configuration;
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

        //[HttpPost("login")]
        //public ActionResult Login([FromBody] ChildLoginRequest request)
        //{
        //    // validate request
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // find child entity by username and password
        //    var child = _context.Child
        //        .Where(c => c.Username == request.Username && c.Password == request.Password)
        //        .FirstOrDefault();

        //    if (child == null)
        //    {
        //        return Unauthorized("Invalid username or password");
        //    }

        //    // return child's details
        //    return Ok(new
        //    {
        //        ChildId = child.Id,

        //    });
        ////}
        //[HttpPost("login")]
        //public ActionResult Login([FromBody] ChildLoginRequest request)
        //{
        //    // Validate request
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Find child entity by username and password
        //    var child = _context.Child
        //        .Where(c => c.Username == request.Username && c.Password == request.Password)
        //        .FirstOrDefault();

        //    if (child == null)
        //    {
        //        return Unauthorized("Invalid username or password");
        //    }

        //    // Generate token for the child
        //    var tokenService = new TokenService(_configuration , _context);
        //    var tokenResponse = tokenService.ChildGenerateToken(child.Username, child.Password);

        //    if (!tokenResponse.IsValid)
        //    {
        //        return BadRequest("Failed to generate token");
        //    }

        //    // Return child's details along with their tasks
        //    return Ok(new ChildLoginResponse
        //    {
        //        Token = tokenResponse.Token
        //    });
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
            var response = _service.ChildGenerateToken(request.Username, request.Password);
            if (child == null)
            {
                return Unauthorized("Invalid username or password");
            }
           
            // return child's details along with their tasks
            return Ok(new ChildLoginResponse
            {

                Token = response.Token,
                Username = child.Username,
                BaitiAccountNumber = child.BaitiAccountNumber,
                SavingsAccountNumber = child.SavingsAccountNumber,
                Balance = child.Balance,
                Points  = child.Points,
                ParentId = child.ParentId,
                ChildId = child.Id,
                
            });
        }

        public class UserLogin()
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }


        //[HttpGet("{Id}/GetTasks")]
        //public IActionResult GetTasks(int parentId)
        //{
        //    var tasks = _context.Task.Where(t => t.ParentId == parentId).ToList();
        //    return Ok(tasks);
        //}
        [HttpGet("{childId}/GetTasks")]
        public IActionResult GetTasks(int childId)
        {
            // Find child entity by ID
            var child = _context.Child.FirstOrDefault(c => c.Id == childId);

            if (child == null)
            {
                return NotFound("Child not found");
            }

            // Find tasks associated with the child
            var tasks = _context.Task.Where(t => t.ParentId == child.ParentId && t.Id == childId).ToList();

            return Ok(tasks);
        }

        [HttpGet("{Id}/GetRewards")]
        public IActionResult GetRewards(int parentId)
        {
            var rewards = _context.Rewards.Where(t => t.Id == parentId).ToList();
            return Ok(rewards);
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

        [HttpPost("{childId}/transfer-to-baitiaccount")]
        public IActionResult TransferToBaitiAccount(int childId, TransferToBaitiAccountRequest request)
        {
            var child = _context.Child.FirstOrDefault(c => c.Id == childId);

            if (child == null)
            {
                return NotFound();
            }

            if (child.Balance < request.Amount)
            {
                return BadRequest("Insufficient balance");
            }

            child.Balance -= request.Amount;
            child.BaitiAccountNumber += request.Amount;
            _context.SaveChanges();

            var transfer = new Transfer
            {
                amount = request.Amount,
                TransferType = "BaitiAccount",
                ParentId = child.ParentId,
                ChildId = childId
            };

            _context.Transfers.Add(transfer);
            _context.SaveChanges();

            return Ok(new { Message = "Transfer successful." });
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

        [HttpGet("{parentId}/transfers")]
        public IActionResult GetTransfers(int parentId)
        {
            var parent = _context.Parent
                .Include(p => p.ParentChildRelationships)
                .ThenInclude(pcr => pcr.Child)
                .FirstOrDefault(p => p.ParentId == parentId);

            if (parent == null)
            {
                return NotFound();
            }

            var transfers = _context.Transfers
                .Where(t => t.ParentId == parentId)
                .Include(t => t.ChildId)
                .Include(t => t.ParentId)
                .ToList();

            return Ok(transfers);
        }
        //[HttpGet("{parentId}/transfers/{childId}")]
        //public IActionResult GetTransfers(int parentId, int childId)
        //{
        //    var parent = _context.Parent
        //       .Include(p => p.ParentChildRelationships)
        //       .ThenInclude(pcr => pcr.Child)
        //       .FirstOrDefault(p => p.ParentId == parentId);

        //    if (parent == null)
        //    {
        //        return NotFound();
        //    }

        //    var child = parent.ParentChildRelationships
        //       .Select(pcr => pcr.Child)
        //       .FirstOrDefault(c => c.Id == childId);

        //    if (child == null)
        //    {
        //        return NotFound();
        //    }

        //    var transfers = _context.Transfers
        //       .Where(t => t.ChildId == childId && t.ParentId == parentId)
        //       .ToList();

        //    return Ok(transfers);
        //}


        [HttpPost("{parentId}/transfer/{childId}")]
        public IActionResult TransferPointsToMoney(int parentId, int childId, TransferRequest request)
        {
            var parent = _context.Parent
                .Include(p => p.ParentChildRelationships)
                .ThenInclude(pcr => pcr.Child)
                .FirstOrDefault(p => p.ParentId == parentId);

            if (parent == null)
            {
                return NotFound();
            }

            var child = parent.ParentChildRelationships
                .Select(pcr => pcr.Child)
                .FirstOrDefault(c => c.Id == childId);

            if (child == null)
            {
                return NotFound();
            }

            if (child.Points < request.PointsToTransfer)
            {
                return BadRequest("Insufficient points");
            }

            child.Points -= request.PointsToTransfer;
            parent.Balance += request.PointsToTransfer;
            _context.SaveChanges();

            Transfer transfer = new Transfer()
            {
                 amount = request.PointsToTransfer,
                    TransferType = request.Type,
                    ParentId = parentId,
                    ChildId = childId,

             };
            _context.Transfers.Add(transfer);
            _context.SaveChanges();

            return Ok(new { Message = "Transfer successful." });
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


        [HttpGet("{childId}/claimedrewards")]
        public ActionResult<List<ClaimedRewardResponce>> GetClaimedRewards(int childId)
        {
            var claimedRewards = _context.ClaimedRewards
                .Where(cr => cr.ChildId == childId)
                .Select(cr => new ClaimedRewardResponce
                {
                    RewardId = cr.RewardId,
                    ClaimDate = cr.claimDate,
                    // Add any other properties you want to include in the response
                })
                .ToList();

            return Ok(claimedRewards);
        }


        [HttpPost("{childId}/claimThisReward")]
        public IActionResult AddClaimedReward(int childId, ClaimedRewards request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ClaimedReward = new ClaimedRewards
            {
                ChildId = childId,
                RewardId = request.RewardId,
                claimDate = request.claimDate,
                // Add any other properties you want to include in the claimed reward
            };

            _context.ClaimedRewards.Add(ClaimedReward);
            _context.SaveChanges();


            return CreatedAtAction(nameof(AddClaimedReward), new { rewardId = ClaimedReward.Id }, ClaimedReward);
        }
    }
}

    
