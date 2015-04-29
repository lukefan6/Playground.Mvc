using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace Playground.Mvc.Core.Base
{
    public class BaseController<T> : Controller where T : BaseManager
    {
        private T _manager;
        public T Manager
        {
            get { return _manager ?? HttpContext.GetOwinContext().Get<T>(); }
            set { _manager = value; }
        }
    }
}
