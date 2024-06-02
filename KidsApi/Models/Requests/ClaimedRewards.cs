namespace KidsApi.Models.Requests
{
    public class ClaimedRewards
    {
        public int Id { get; set; }
        public int RewardId { get; set; }
        public int ChildId { get; set; }
        public DateOnly claimDate { get; set; }
    }
}
