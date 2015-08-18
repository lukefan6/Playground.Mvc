using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Playground.Mvc.Web.Models.Order
{
    public class AddProductsViewModel
    {
        public IDictionary<string, FinishingProduct> FinishingProduct { get; set; }

        public AddProductsViewModel()
        {
            FinishingProduct = new Dictionary<string, FinishingProduct>();
        }
    }

    public class FinishingProduct : OrderViewModelBase
    {
        [Required]
        [Display(Name = "Product Id")]
        public string ProductInfoId { get; set; }

        public IDictionary<string, ProductArtInfo> ProductArtInfo { get; set; }

        public FinishingProduct()
        {
            ProductArtInfo = new Dictionary<string, ProductArtInfo>();
        }
    }

    public class ProductArtInfo : OrderViewModelBase
    {
        [Required]
        public int? ServiceId { get; set; }

        [Required]
        [Display(Name = "Art Description")]
        public string ArtDescription { get; set; }
    }
}