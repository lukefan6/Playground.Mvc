using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Playground.Mvc.Web.Models.Order
{
    public class CreateNewOrderViewModel
    {
        public IEnumerable<SelectListItem> CustomerList { get; set; }

        [Required]
        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }

        [Required]
        [Display(Name = "PO#")]
        public string PurchaseOrderId { get; set; }
    }
}
