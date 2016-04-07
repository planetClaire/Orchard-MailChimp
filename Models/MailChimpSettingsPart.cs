using Orchard.ContentManagement;

namespace MailChimp.Models
{
    public class MailChimpSettingsPart : ContentPart
    {
        public string MailChimpApiKey
        {
            get { return this.Retrieve(x => x.MailChimpApiKey); }
            set { this.Store(x => x.MailChimpApiKey, value); }
        }
    }
}