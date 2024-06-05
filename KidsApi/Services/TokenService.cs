using KidsApi.Models.Entites;
using KidsApi.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KidsApi.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly KidsContext context;

        public TokenService(IConfiguration configuration, KidsContext context)
        {
            _configuration = configuration;
            this.context = context;
        }


        internal object GenerateToken(object username, object password)
        {
            throw new NotImplementedException();
        }

     //   public (bool IsValid, string Token) ChildGenerateToken(string username, string password)
     //   {
     //       if (username != "admin" || password != "admin")
     //       {
     //           return (false, "");
     //       }

     //       var userAccount = context.Child.SingleOrDefault(r => r.Username == username);

     //       if (userAccount == null)
     //       {
     //           return (false, "");
     //       }


     //       var validPassword = BCrypt.Net.BCrypt.EnhancedVerify(password, userAccount.Password);
     //       if (!validPassword)
     //       {
     //           return (false, "");

     //       }
     //       var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
     //       var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

     //       // From here
     //       var claims = new[]
     //       {
     //           new Claim(TokenClaimsConstant.Username, username),
     //   new Claim(TokenClaimsConstant.UserId, userAccount.Id.ToString()),
     //           // new Claim(ClaimTypes.Role, userAccount.IsAdmin ? "Admin" : "User")
            
     //};
     //       var token = new JwtSecurityToken(
     //           issuer: _configuration["Jwt:Issuer"],
     //           audience: _configuration["Jwt:Audience"],
     //           claims: claims,
     //           expires: DateTime.Now.AddMinutes(30), // Expire
     //           signingCredentials: credentials);

     //       var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
     //       return (true, generatedToken);
     //   }

        public ChildLoginResponse ChildGenerateToken(string username, string password)
        {
            // Find child entity by username and password
            var child = context.Child
                .Where(c => c.Username == username && c.Password == password)
                .FirstOrDefault();

            if (child == null)
            {
                return new ChildLoginResponse
                {
                   
                };
            }

            // Generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, child.ChildId.ToString()),
            new Claim(ClaimTypes.Role, "Child")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new ChildLoginResponse
            {
               
                Token = tokenString
            };
        }

        public (bool IsValid, string Token) GenerateToken(string username, string password)
        {
            var userAccount = context.Parent.SingleOrDefault(r => r.Username == username);

            if (userAccount == null)
            {
                return (false, "");
            }

            var validPassword = BCrypt.Net.BCrypt.EnhancedVerify(password, userAccount.Password);
            if (!validPassword)
            {
                return (false, "");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    // From here
                    var claims = new[]

            {

                new Claim(TokenClaimsConstant.Username, username),
                new Claim(TokenClaimsConstant.UserId, userAccount.ParentId.ToString()),
                new Claim(ClaimTypes.Role, userAccount.IsAdmin ? "Admin" : "User")

        };
            // To Here
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Expire
                signingCredentials: credentials);
            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return (true, generatedToken);
        }
    }
}







