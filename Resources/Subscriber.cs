using System;

namespace MailChimp.Resources
{
    public class Subscriber
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string UniqueEmailId { get; set; }
        public EmailType EmailType { get; set; }
        public Status Status { get; set; }
        // todo merge_fields, interests, stats, ip_signup, timestamp_signup, ip_opt, timestamp_opt, member_rating, language, vip, email_client, location, last_note, links
        public DateTime LastChanged { get; set; }
        public string ListId { get; set; }
    }

    public enum EmailType {
        Html,
        Text
    }

    public enum Status {
        Subscribed,
        Unsubscribed,
        Cleaned,
        Pending
    }
}