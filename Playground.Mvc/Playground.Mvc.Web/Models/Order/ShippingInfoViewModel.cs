using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Playground.Mvc.Web.Models.Order
{
    public class ShippingInfoViewModel
    {
        public IDictionary<string, ShippingInfo> ShippingInfo { get; set; }

        public ShippingInfoViewModel()
        {
            ShippingInfo = new Dictionary<string, ShippingInfo>();
        }
    }

    public class ShippingInfo : OrderViewModelBase
    {
        [Required]
        [Display(Name = "Shipping Type")]
        public ShipType? ShippingType { get; set; }

        [Required]
        [Display(Name = "Shipping Method")]
        public ShipMethod? ShippingMethod { get; set; }

        [Display(Name = "Other Method")]
        public string OtherMethod { get; set; }

        [Required]
        [Display(Name = "Ship Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ShipDate { get; set; }
    }

    public enum ShipType
    {
        None = 0,
        Truck = 1,
        SmallPackage = 2
    }

    public enum ShipMethod
    {
        Gound = 0,
        Other = 1
    }
}
