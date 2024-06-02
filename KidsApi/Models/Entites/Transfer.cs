namespace KidsApi.Models.Entites
{
    public class Transfer
    {
        public int Id { get; set; }
        public int ChildId { get; set; }
        public int ParentId { get; set; }
        public int amount { get; set; }
        public string TransferType { get; set; }
    }
    public class PointsTransferRequest
    {
        public int Id { get; set; }
        public int ChildId { get; set; }
        public string TransferToPointType { get; set; }
        public int deductedPoints { get; set; }

    }
}
