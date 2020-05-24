using System.ComponentModel.DataAnnotations;

namespace Wp.Core.Domain.Sections
{
    public class ContactFormSection : Section
    {
        [Required]
        public string EmailTo { get; set; }
        public string EmailCc { get; set; }
        public string EmailBcc { get; set; }
        public string Subject { get; set; }
        public string IntroText { get; set; }
        public string ThankYouText { get; set; }

        public bool NameEnabled { get; set; }
        public bool ExtendedEnabled { get; set; }//displays address and telephone and mobile      

    }
}
