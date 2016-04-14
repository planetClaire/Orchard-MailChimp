using System.Collections.Generic;

namespace MailChimp.Resources
{
    public class Member
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string UniqueEmailId { get; set; }
        public EmailType EmailType { get; set; }
        public Status Status { get; set; }
        public Dictionary<string, string> MergeFields { get; set; }
        public MemberStats Stats { get; set; }
        public string IpSignup { get; set; }
        public string TimestampSignup { get; set; }
        public string IpOpt { get; set; }
        public string TimestampOpt { get; set; }
        public int MemberRating { get; set; }
        public string LastChanged { get; set; }
        public string Language { get; set; }
        public bool Vip { get; set; }
        public string EmailClient { get; set; }
        public Location Location { get; set; }
        public string ListId { get; set; }
        public List<Link> Links { get; set; }
    }

    public class MemberStats {
        public int AvgOpenRate { get; set; }
        public int AvgClickRate { get; set; }
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