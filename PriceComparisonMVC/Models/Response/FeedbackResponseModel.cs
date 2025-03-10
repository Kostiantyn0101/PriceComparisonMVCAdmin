namespace PriceComparisonMVCAdmin.Models.Response
{
    public class FeedbackResponseModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string FeedbackText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }
    }

}
