using System.Threading.Tasks;
using MailChimp.Resources;
using Orchard;

namespace MailChimp.Services
{
    public interface IMailChimpService : IDependency {
        Task<Subscriber> GetSubscriber(string listId, string emailAddress);
        Subscriber AddOrUpdateSubscriber(Subscriber subscriber);
    }
}