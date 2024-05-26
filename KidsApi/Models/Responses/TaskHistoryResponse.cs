namespace KidsApi.Models.Responses
{
    public class TaskHistoryResponse
    {
        public int TaskId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public DateOnly CompletionDate { get; set; }
    }
}
