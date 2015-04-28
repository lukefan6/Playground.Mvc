using System.ComponentModel.DataAnnotations;
using Playground.Mvc.DataModel;

namespace Playground.Mvc.Web.Models
{
    public class SmsViewModel
    {
        public SmsViewModel() { }

        public SmsViewModel(Sms sms)
        {
            this.Id = sms.Id;
            this.Message = sms.Message
                .Replace("\r\n", "</br>")
                .Replace("\r", "</br>")
                .Replace("\n", "</br>");
        }

        public int Id { get; set; }

        [Required]
        [StringLength(140)]
        public string Message { get; set; }
    }
}