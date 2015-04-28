using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Playground.Mvc.DataModel;

namespace Playground.Mvc.Core.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public IDbSet<Sms> Sms { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
