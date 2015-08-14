using System.Linq;
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

            return RedirectToAction("EditShippingInfo");
        }

        [HttpGet]
        public ActionResult EditShippingInfo()
        {
            var model = Session[typeof(ShippingInfoViewModel).FullName] as ShippingInfoViewModel;

            return View(model ?? new ShippingInfoViewModel());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult EditShippingInfoPartial()
        {
            return PartialView("_EditShippingInfo", new ShippingInfo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditShippingInfo(ShippingInfoViewModel model)
        {
            if (!ValidationHelper.ValidateDictionaryItems(ModelState, model, x => x.ShippingInfo))
            {
                return View(model);
            }

            Session[model.GetType().FullName] = model;

            return RedirectToAction("EditShippingInfo");
        }
    }
}
