using System.Linq;
using System.Net;
using System.Web.Mvc;
using Playground.Mvc.Core;
using Playground.Mvc.Core.Base;
using Playground.Mvc.Web.Models.Order;

namespace Playground.Mvc.Web.Controllers
{
    public class OrderController : BaseController<OrderManager>
    {
        [HttpGet]
        public ActionResult CreateNewOrder()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewOrder(CreateNewOrderViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            if (!Manager.CustomerList.Any(x => x == model.CustomerId))
            {
                ModelState.AddModelError("CustomerId", "Customer does not exist.");
                return View(model);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
