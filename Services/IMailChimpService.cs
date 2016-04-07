using MailChimp.Resources;
using Orchard;

namespace MailChimp.Services
{
    public interface IMailChimpService : IDependency {
        Subscriber GetSubscriber(string listId, string emailAddress);
        //Subscriber AddOrUpdateSubscriber(Subscriber subscriber);
        //void GetListSubscribers(string idList);
    }
}