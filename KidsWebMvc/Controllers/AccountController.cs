using KidsApi.Models.Requests;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using KidsWebMvc.API;

namespace KidsWebMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly KidsApiClient _api;
        public AccountController(KidsApiClient api)
        {
            _api = api;
        }
    [HttpGet]
    public IActionResult Signup()
    {
      var signUpRequest = new SignUpRequest();
      return View(signUpRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Signup(SignUpRequest req)
    {
      var created = await _api.Register(req);
      if (created)
      {
        return Redirect("/Account/Login");
      }
      ModelState.AddModelError("Username", "Something happened, Could not create user");
      return View(req);
    }

    //[HttpGet]
    //public IActionResult Register()
    //{
    //  var signUpRequest = new SignUpRequest();
    //  return View(signUpRequest);
    //}

    //[HttpPost]
    //    public async Task<IActionResult> Register(SignUpRequest req)
    //    {
    //        var created = await _api.Register(req);
    //        if (created)
    //        { 
    //            return Redirect("/Account/Login");
    //        }
    //        ModelState.AddModelError("Username", "Something happened, Could not create user");
    //        return View(req);
    //    }
    [HttpGet]
    public IActionResult Login()
    {
      var model = new UserLoginRequest();
      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
      if (!ModelState.IsValid)
      {
        // If model state is not valid, return the view with errors
        return View(request);
      }

      var jwtToken = await _api.Login(request.Username, request.Password);
      if (!string.IsNullOrEmpty(jwtToken))
      {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

        var claimsIdentity = new ClaimsIdentity(jwtSecurityToken.Claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        { IsPersistent = false, AllowRefresh = true };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        HttpContext.Session.SetString("Token", jwtToken);
        HttpContext.Response.Cookies.Append("Token", jwtToken);

        return RedirectToAction("Index", "ParentDashBoard"); // Redirect to the desired page after successful login
      }

      ModelState.AddModelError(string.Empty, "Invalid Username and/or Password");
      return View(request);
    }
    public IActionResult Logout()
        {
            HttpContext.SignOutAsync(); 
            return Redirect("/Home/Index");
        }
        public IActionResult Denied()
        {
            return View();
        }
    }
}


