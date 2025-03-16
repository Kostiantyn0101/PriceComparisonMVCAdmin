namespace PriceComparisonMVCAdmin.Models.Response
{
    public class PopularCategoryResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<PopularProductResponseModel> Products { get; set; }
    }
}
