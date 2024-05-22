using KidsApi.Models.Entites;
using KidsApi.Models.Requests;
using KidsApi.Models.Responses;
using KidsApi.Services;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("Login")]
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
            //newUser.Username = request.Username;
            //newUser.Password = request.Password;
           
            context.Parent.Add(newUser);
            context.SaveChanges();

            return Ok(new { Message = "User Created" });

        }

        [HttpGet("Details/{id}")]
        [ProducesResponseType(typeof(ChildAccountResponce), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ChildAccountResponce> Details([FromRoute] int id)
        {

            var child = context.Children.Find(id);
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
            context.Children.Add(newChild);
            context.SaveChanges();

            return CreatedAtAction(nameof(Details), new { Id = newChild.Id }, newChild);
        }

        [HttpGet("GetChildren/{parentId}")]
        public IActionResult GetChildren(int parentId)
        {
            var children = context.Children
                .Where(c => c.ParentId == parentId)
                .ToList();

            if (children == null || children.Count == 0)
            {
                return NotFound();
            }

            return Ok(children);
        }

        [HttpPost("AddReward")]
        public IActionResult Add(AddRewardRequest req)
        {
            var newReward = new Reward()
            {
                Id = req.Id,
                RequiredPoints = req.RequiredPoints,
                Description = req.Description,
                RewardType = req.RewardType,
                children = req.children
            };

            context.Rewards.Add(newReward);
            context.SaveChanges();

            return CreatedAtAction(nameof(Details), new { Id = newReward.Id }, newReward);
        }

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


