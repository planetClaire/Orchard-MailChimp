using System.Threading.Tasks;
using MailChimp.Resources;
using Orchard;

namespace MailChimp.Services
{
    public interface IMailChimpService : IDependency {
        Task<Member> GetSubscriber(string listId, string emailAddress);
        Task<List> GetList(string listId);
        Task<ListMembers> GetMembers(string idList);
    }
}