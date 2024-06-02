namespace KidsApi.Models.Entites
{
    public class Reward
    {
        public int Id { get; set; }
        public string RewardType { get; set; }
        public string Description { get; set; }
        public int RequiredPoints { get; set; }
        public int ChildId { get; set; }
        public List<Child> Children { get; set; }
    }

 
}
