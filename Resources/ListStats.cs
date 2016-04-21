namespace MailChimp.Resources {
    public class ListStats : MailChimpResource
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