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

        [HttpGet]
        public ActionResult AddProducts()
        {
            var model = Session[typeof(AddProductsViewModel).FullName] as AddProductsViewModel;

            return View(model ?? new AddProductsViewModel());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult EditFinishingProductPartial(string productInfoId, string parentPrefix)
        {
            return PartialView("_EditFinishingProduct", new FinishingProduct
            {
                ProductInfoId = productInfoId,
                DictionaryRepresentationPrefix = parentPrefix
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult EditProductArtInfoPartial(int? serviceId, string parentPrefix)
        {
            return PartialView("_EditProductArtInfo", new ProductArtInfo
            {
                ServiceId = serviceId,
                DictionaryRepresentationPrefix = parentPrefix
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProducts(AddProductsViewModel model)
        {
            if (!ValidationHelper.ValidateDictionaryItems(ModelState, model, x => x.FinishingProduct))
            {
                return View(model);
            }

            if (!model.FinishingProduct.Values.All(x =>
            {
                return ValidationHelper.ValidateDictionaryItems(ModelState, x, y => y.ProductArtInfo);
            }))
            {
                return View(model);
            }

            Session[model.GetType().FullName] = model;

            return RedirectToAction("AddProducts");
        }
    }
}
