using KidsApi.Models.Entites;

namespace KidsApi.Models.Requests
{
    public class SignUpRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
    }
}
