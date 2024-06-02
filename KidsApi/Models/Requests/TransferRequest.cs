namespace KidsApi.Models.Requests
{
    public class TransferRequest
    {
        public int PointsToTransfer { get; set; }
        public string TrasferType { get; set; }
        public string ChildId { get; set; }
    }
}
