using System.Collections.Generic;

namespace MailChimp.Resources
{
    public class List
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Contact Contact { get; set; }
        public string PermissionReminder { get; set; }
        public bool UseArchiveBar { get; set; }
        public CampaignDefaults CampaignDefaults { get; set; }
        public string NotifyOnSubscribe { get; set; }
        public string NotifyOnUnsubscribe { get; set; }
        public string DateCreated { get; set; }
        public int ListRating { get; set; }
        public bool EmailTypeOption { get; set; }
        public string SubscribeUrlShort { get; set; }
        public string SubscribeUrlLong { get; set; }
        public string BeamerAddress { get; set; }
        public string Visibility { get; set; }
        public List<object> Modules { get; set; }
        public ListStats Stats { get; set; }
        public List<Link> Links { get; set; }
    }

    public class CampaignDefaults
    {
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Language { get; set; }
    }

    public class Contact
    {
        public string Company { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
    }

    public class ListStats
    {
        public int MemberCount { get; set; }
        public int UnsubscribeCount { get; set; }
        public int CleanedCount { get; set; }
        public int MemberCountSinceSend { get; set; }
        public int UnsubscribeCountSinceSend { get; set; }
        public int CleanedCountSinceSend { get; set; }
        public int CampaignCount { get; set; }
        public string CampaignLastSent { get; set; }
        public int MergeFieldCount { get; set; }
        public int AvgSubRate { get; set; }
        public int AvgUnsubRate { get; set; }
        public int TargetSubRate { get; set; }
        public int OpenRate { get; set; }
        public int ClickRate { get; set; }
        public string LastSubDate { get; set; }
        public string LastUnsubDate { get; set; }
    }
}