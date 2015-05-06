using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Playground.Mvc.Core.DataAccess;
using Playground.Mvc.DataModel;

namespace Playground.Mvc.Core
{
    public class SmsManager : IDisposable
    {
        public SmsManager(ApplicationDbContext database)
        {
            this.Database = database;
        }

        public ApplicationDbContext Database { get; private set; }

        public static SmsManager Create(IdentityFactoryOptions<SmsManager> options, IOwinContext context)
        {
            return new SmsManager(context.Get<ApplicationDbContext>());
        }

        public async virtual Task<bool> AddAsync(string message)
        {
            if (string.IsNullOrEmpty(message)) { return false; }

            bool isSuccess = false;

            try
            {
                Database.Sms.Add(new Sms { Message = message });
                isSuccess = (await Database.SaveChangesAsync() == 1);
            }
            catch (Exception e)
            {
                //TODO LOG
                throw e;
            }

            return isSuccess;
        }

        public virtual IEnumerable<Sms> GetAll()
        {
            return Database.Sms;
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

        public virtual async Task UpdateSelected(int[] selectedIdList)
        {
            foreach (var sms in GetAll())
            {
                sms.IsSelected = selectedIdList.Any(selectedId => selectedId == sms.Id);
                Database.Entry<Sms>(sms).State = EntityState.Modified;
            }

            await Database.SaveChangesAsync();
        }
    }
}
