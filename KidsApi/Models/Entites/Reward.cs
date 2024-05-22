namespace KidsApi.Models.Entites
{
    public class Reward
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string RewardType { get; set; }
        public string Description { get; set; }
        public int RequiredPoints { get; set; }
        public List<Child> children { get; set; }
    }

    public class ClaimedRewards
    {
        public int Id { get; set; }
        public int RewardId { get; set; }
        public int ChildId { get; set; }
        public DateOnly claimDate { get; set; }
    }
}
