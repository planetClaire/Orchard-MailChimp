using MailChimp.Resources;
using NUnit.Framework;

namespace MailChimpTests.Resources
{
    [TestFixture]
    public class MemberTests
    {
        [Test]
        public void HasChangedSinceOptin_IfTimestampsDifferByMoreThan1Second_ReturnsTrue() {
            var member = new Member {
                TimestampOpt = "2015-09-16 19:24:27",
                LastChanged = "2015-09-16 19:24:29"
            };

            Assert.True(member.HasChangedSinceOptin());
        }

        [Test]
        public void HasChangedSinceOptin_IfTimestampsDifferBy1Second_ReturnsFalse()
        {
            var member = new Member
            {
                TimestampOpt = "2015-09-16 19:24:28",
                LastChanged = "2015-09-16 19:24:29"
            };

            Assert.False(member.HasChangedSinceOptin());
        }

        [Test]
        public void HasChangedSinceOptin_IfTimestampsEqual_ReturnsFalse()
        {
            var member = new Member
            {
                TimestampOpt = "2015-09-16 19:24:28",
                LastChanged = "2015-09-16 19:24:28"
            };

            Assert.False(member.HasChangedSinceOptin());
        }
    }
}
