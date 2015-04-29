using System;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Playground.Mvc.Core.DataAccess;

namespace Playground.Mvc.Core.Base
{
    public abstract class BaseManager : IDisposable
    {
        protected ApplicationDbContext Database { get; private set; }

        public static T Create<T>(IdentityFactoryOptions<T> options, IOwinContext context) where T : BaseManager, new()
        {
            return new T() { Database = context.Get<ApplicationDbContext>() };
        }

        #region IDisposable 成員

        void IDisposable.Dispose()
        {
            if (Database != null)
            {
                Database.Dispose();
            }
        }

        #endregion
    }
}
