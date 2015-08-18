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

        private string dictionaryRepresentationPrefix;

        public string DictionaryRepresentationPrefix
        {
            get { return dictionaryRepresentationPrefix ?? GetType().Name; }
            set { dictionaryRepresentationPrefix = value; }
        }
    }
}
