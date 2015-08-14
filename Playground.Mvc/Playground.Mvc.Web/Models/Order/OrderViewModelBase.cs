using System;

namespace Playground.Mvc.Web.Models.Order
{
    public abstract class OrderViewModelBase
    {
        private Guid? viewModelId;

        public string ViewModelId
        {
            get
            {
                if (!viewModelId.HasValue) { viewModelId = Guid.NewGuid(); }
                return viewModelId.Value.ToString("N");
            }
            set { viewModelId = Guid.Parse(value); }
        }
    }
}
