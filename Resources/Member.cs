using System;
using System.Collections.Generic;

namespace MailChimp.Resources
{
    public class Member
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string UniqueEmailId { get; set; }
        public EmailType EmailType { get; set; }
        public Status? Status { get; set; }
        public Status StatusIfNew { get; set; }
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

        public bool HasChangedSinceOptin() {
            // dirty way to tell if a member is new or not, compare timestamps to within 1 second
            // since MC API doesn't distinguish between New and Edit when PUTing. Returns 200 for both rather than 201 for new resource
            DateTime dateTimeLastChanged;
            DateTime dateTimeOpt;
            return DateTime.TryParse(LastChanged, out dateTimeLastChanged) && DateTime.TryParse(TimestampOpt, out dateTimeOpt)
                   && (dateTimeLastChanged - dateTimeOpt).Seconds > 1;
        }
    }

    public class MemberStats {
        public int AvgOpenRate { get; set; }
        public int AvgClickRate { get; set; }
    }

    public class Location
    {
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public int Gmtoff { get; set; }
        public int Dstoff { get; set; }
        public string CountryCode { get; set; }
        public string Timezone { get; set; }
    }

    public enum EmailType
    {
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