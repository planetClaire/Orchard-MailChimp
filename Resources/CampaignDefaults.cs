namespace MailChimp.Resources {
    public class CampaignDefaults : MailChimpResource
    {
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Language { get; set; }
    }
}