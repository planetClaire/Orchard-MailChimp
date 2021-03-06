﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MailChimp.Resources
{
    public class Member : MailChimpResource
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

        /// <summary>
        /// Compare timestamps to within 1 second
        /// Dirty way to tell if a member is new or not, since MC API doesn't distinguish between New and Edit when PUTing. Returns 200 for both rather than 201 for new resource
        /// </summary>
        /// <returns></returns>
        public bool HasChangedSinceOptin() {
            DateTime dateTimeLastChanged;
            DateTime dateTimeOpt;
            return DateTime.TryParse(LastChanged, out dateTimeLastChanged) && DateTime.TryParse(TimestampOpt, out dateTimeOpt)
                   && (dateTimeLastChanged - dateTimeOpt).Seconds > 1;
        }

        public override bool Equals(MailChimpResource other)
        {
            var member = other as Member;
            if (member == null) {
                return false;
            }
            var mergeFieldsEqual = MergeFields != null && member.MergeFields != null 
                && (MergeFields.Count == member.MergeFields.Count && !MergeFields.Except(member.MergeFields).Any());
            if (!mergeFieldsEqual) {
                return false;
            }
            return base.Equals(other);
        }
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