using Azure.Core;

namespace KidsApi.Models.Responses
{
    public class ChildAccountResponce
    {

        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public int Points { get; set; }
        public int BaitiAccountNumber { get; set; }
        public int SavingsAccountNumber { get; set; }
        public List<Task> Tasks { get; set; }

    }
}
