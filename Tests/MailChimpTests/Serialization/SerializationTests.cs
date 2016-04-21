using System.Linq;
using MailChimp.Resources;
using MailChimp.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MailChimpTests.Serialization
{
    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void Serialization_IgnoresNullValues()
        {
            var member = new Member
            {
                EmailAddress = "me@email.com",
            };

            var result = JsonConvert.SerializeObject(member, new MailChimpSerializerSettings());

            Assert.False(result.Contains("email_client"));

        }

        [Test]
        public void Serialization_ConvertsToSnakeCaseByDefault()
        {
            var member = new Member
            {
                EmailAddress = "me@email.com"
            };

            var result = JsonConvert.SerializeObject(member, new MailChimpSerializerSettings());

            Assert.IsFalse((typeof(Member)).GetProperty("EmailAddress").GetCustomAttributes(typeof(JsonPropertyAttribute), false).Any());
            Assert.True(result.Contains("email_address"));

        }

        [Test]
        public void Serialization_ConvertsEnumsToCamelCase()
        {
            var member = new Member
            {
                EmailAddress = "me@email.com",
                Status = Status.Subscribed
            };

            var result = JsonConvert.SerializeObject(member, new MailChimpSerializerSettings());

            Assert.True(result.Contains("subscribed"));

        }

    }
}
