using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Playground.Mvc.Core;
using Playground.Mvc.Core.Base;

namespace Playground.Mvc.Web.Controllers
{
    public class ImageController : BaseController<FileManager>
    {
        // GET: Image
        public ActionResult Index(int skip = 0, int take = 10)
        {
            return View(Manager.FetchFileId(skip, take));
        }

        public ViewResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase image)
        {
            if (image == null)
            {
                ModelState.AddModelError("", "No file uploaded.");
                return View();
            }

            if (!image.IsImage())
            {
                ModelState.AddModelError("", "Uploaded file is not of supported image type.");
                return View();
            }

            try
            {
                using (var binaryReader = new BinaryReader(image.InputStream))
                {
                    if (!Manager.Add(binaryReader.ReadBytes(image.ContentLength), image.ContentType))
                    {
                        ModelState.AddModelError("", "Upload file failed");
                        return View();
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Get(int? id)
        {
            if (!id.HasValue) { return HttpNotFound(); }

            try
            {
                var result = Manager.GetFileById(id.Value);
                return (result == null)
                    ? HttpNotFound() as ActionResult
                    : File(result.Content, result.ContentType);
            }
            catch
            {
                return HttpNotFound();
            }
        }
    }
}