using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using MailChimp.Services;
using NUnit.Framework;
using Orchard.Tests.Modules;
using Autofac;
using MailChimp.Handlers;
using MailChimp.Models;
using MailChimp.Resources;
using MailChimp.Serialization;
using Moq;
using Newtonsoft.Json;
using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.Records;
using Orchard.Core.Settings.Handlers;
using Orchard.Core.Settings.Metadata;
using Orchard.Core.Settings.Services;
using Orchard.Data;
using Orchard.Settings;
using Orchard.Tests.Stubs;
using Orchard.Tests.Utility;
using RichardSzalay.MockHttp;

namespace MailChimpTests.Services
{
    [TestFixture]
    public class MailChimpServiceTests : DatabaseEnabledTestsBase
    {
        private const string BaseUrl = "http://localhost";
        private const string ApiVersion = "3.0";

        private Mock<WorkContext> _workContext;
        private MockHttpMessageHandler _mockHttpMessageHandler;
        private IMailChimpService _mailChimpService;

        public override void Register(ContainerBuilder builder)
        {
            builder.RegisterAutoMocking(MockBehavior.Loose);
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));

            builder.RegisterType<DefaultContentManager>().As<IContentManager>();
            builder.RegisterType<StubCacheManager>().As<ICacheManager>();
            builder.RegisterType<Signals>().As<ISignals>();
            builder.RegisterType<SiteService>().As<ISiteService>();
            builder.RegisterType<ContentDefinitionManager>().As<IContentDefinitionManager>();
            builder.RegisterType<SiteSettingsPartHandler>().As<IContentHandler>();
            builder.RegisterType<DefaultContentQuery>().As<IContentQuery>().InstancePerDependency();

            _workContext = new Mock<WorkContext>();
            _workContext.Setup(w => w.GetState<ISite>(It.Is<string>(s => s == "CurrentSite"))).Returns(() => _container.Resolve<ISiteService>().GetSiteSettings());
            var workContextAccessor = new Mock<IWorkContextAccessor>();
            workContextAccessor.Setup(w => w.GetContext()).Returns(_workContext.Object);
            builder.RegisterInstance(workContextAccessor.Object).As<IWorkContextAccessor>();

            builder.RegisterType<MailChimpService>().As<IMailChimpService>();
            builder.RegisterType<MailChimpSettingsPartHandler>().As<IContentHandler>();
        }

        public override void Init()
        {
            base.Init();

            _workContext.Object.CurrentSite.As<MailChimpSettingsPart>().MailChimpApiKey = "apikey";
            _mockHttpMessageHandler = new MockHttpMessageHandler();
            var fakeHttpClient = new HttpClient(_mockHttpMessageHandler) {BaseAddress = new Uri(BaseUrl)};
            _mailChimpService = _container.Resolve<IMailChimpService>(new NamedParameter("httpClient", fakeHttpClient));

        }

        protected override IEnumerable<Type> DatabaseTypes
        {
            get
            {
                return new[]
                    {
                        typeof (ContentTypeRecord),
                        typeof (ContentItemRecord),
                        typeof (ContentItemVersionRecord)
                    };
            }
        }

        [Test]
        public async void GetMember_ConstructsCorrectEndpoint() {
            const string listId = "123";
            _mockHttpMessageHandler.Expect(string.Format("{0}/{1}/lists/{2}/members/*", BaseUrl, ApiVersion, listId))
                .Respond(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new Member(), new MailChimpSerializerSettings()))
                });

            await _mailChimpService.GetMember(listId, "me@email.com");

            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();

            ClearSession();
        }

        [Test]
        public async void GetMember_HashesEmailCorrectly() {
            const string listId = "123";
            const string email = "me@email.com";
            const string emailMD5 = "8f9dc04e6abdcc9fea53e81945c7294b";
            _mockHttpMessageHandler.Expect(string.Format("{0}/{1}/lists/{2}/members/{3}", BaseUrl, ApiVersion, listId, emailMD5))
                .Respond(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new Member(), new MailChimpSerializerSettings()))
                });

            await _mailChimpService.GetMember(listId, email);

            _mockHttpMessageHandler.VerifyNoOutstandingExpectation();

            ClearSession();
        }

        [Test]
        public async void GetMember_ReturnsPopulatedMember() {
            const string listId = "123";
            const string email = "me@email.com";
            const string emailMD5 = "8f9dc04e6abdcc9fea53e81945c7294b";
            var location = new Location {
                CountryCode = "AU",
                Dstoff = 1,
                Gmtoff = 2,
                Latitude = 123,
                Longitude = 456,
                Timezone = "8"
            };
            var member = new Member {
                EmailAddress = email,
                EmailClient = "client",
                EmailType = EmailType.Html,
                Id = "1",
                IpOpt = "192.168.100.101",
                IpSignup = "192.168.100.102",
                Status = Status.Subscribed,
                Stats = new MemberStats {
                    AvgClickRate = 10,
                    AvgOpenRate = 11
                },
                LastChanged = "2016-08-15",
                MergeFields = new Dictionary<string, string> {
                    {"FNAME", "first"},
                    {"LNAME", "last"}
                },
                Vip = true,
                ListId = listId,
                MemberRating = 2,
                Language = "en",
                Location = location,
                TimestampOpt = "2016-08-02",
                TimestampSignup = "2016-08-01",
                UniqueEmailId = "4"
            };
            _mockHttpMessageHandler.Expect(string.Format("{0}/{1}/lists/{2}/members/{3}", BaseUrl, ApiVersion, listId, emailMD5))
                .Respond(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(member, 
                    new MailChimpSerializerSettings()))
                });

            var returnedMember = await _mailChimpService.GetMember(listId, email);

            Assert.AreEqual(member, returnedMember, "Expected member object not returned");
            ClearSession();
        }

    }
}
