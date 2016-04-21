using System;
using freakcode.Utils;

namespace MailChimp.Resources {
    public class MailChimpResource : IEquatable<MailChimpResource>{
        public override int GetHashCode()
        {
            return MemberwiseEqualityComparer<MailChimpResource>.Default.GetHashCode(this);
        }

        public override bool Equals(object obj)
        {
            var mailChimpResource = obj as MailChimpResource;
            if (mailChimpResource == null)
                return false;
            return Equals(mailChimpResource);
        }

        public virtual bool Equals(MailChimpResource other)
        {
            return MemberwiseEqualityComparer<MailChimpResource>.Default.Equals(this, other);
        }
    }
}