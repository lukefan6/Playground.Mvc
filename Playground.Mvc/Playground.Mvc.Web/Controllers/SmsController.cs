using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Playground.Mvc.Core;
using Playground.Mvc.Web.Models;

namespace Playground.Mvc.Web.Controllers
{
    public class SmsController : Controller
    {
        private SmsManager _smsManager;

        public SmsManager SmsManager
        {
            get { return _smsManager ?? HttpContext.GetOwinContext().Get<SmsManager>(); }
            set { _smsManager = value; }
        }

        // GET: Sms
        public ViewResult Index()
        {
            return View(SmsManager.GetAll().Select(x => new SmsViewModel(x)));
        }

        public ViewResult Create()
        {
            return View(new SmsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SmsViewModel model)
        {
            if (!ModelState.IsValid) { return View(model); }

            try
            {
                if (!await SmsManager.AddAsync(model.Message))
                {
                    ModelState.AddModelError("", "Add message failed");
                    return View(model);
                }
            }
            catch (Exception e)
            {
                //TODO LOG
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateSelected(int[] selectedIdList)
        {
            await SmsManager.UpdateSelected(selectedIdList ?? new int[] { });
            return RedirectToAction("Index");
        }
    }
}