namespace PriceComparisonMVCAdmin.Models.ViewModels.Seller
{
    public class PriceListViewModel
    {
        public IFormFile? PriceListFile { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

}
