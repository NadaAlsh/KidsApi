using Azure.Core;

namespace KidsApi.Models.Entites
{
    public class Child
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public int ParentId { get; set; }
        public bool isCompleted { get; set; }
        public int Points { get; set; }
        public int BaitiAccountNumber { get; set; }
        public int SavingsAccountNumber { get; set; }

        public ICollection<Task> Tasks { get; set; } = new List<Task>();
  
      //  public List<Request> Requests { get; set; }



    }
}
