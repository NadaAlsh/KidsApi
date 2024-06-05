using System.ComponentModel.DataAnnotations;

namespace KidsApi.Models.Responses
{
    public class UserLoginResponce
    {
        public string Token { get; set; }
        public int ParentId { get; set; }
        public string Username { get; set; }

    }
}
