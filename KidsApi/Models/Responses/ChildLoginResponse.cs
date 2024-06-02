using KidsApi.Models.Entites;

namespace KidsApi.Models.Responses
{
    public class ChildLoginResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public int ParentId { get; set; }
        public int Points { get; set; }
        public int BaitiAccountNumber { get; set; }
        public int SavingsAccountNumber { get; set; }//savings
        public decimal Balance { get; set; }

        public int ChildId { get; set; }
        public ICollection<ParentChildRelationship> ParentChildRelationships { get; set; }

        public ICollection<MyTask> Tasks { get; set; } = new List<MyTask>();

    }
}
