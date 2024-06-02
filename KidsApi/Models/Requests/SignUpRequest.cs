using KidsApi.Models.Entites;
using System.ComponentModel.DataAnnotations;

namespace KidsApi.Models.Requests
{
    public class SignUpRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
    }
}
