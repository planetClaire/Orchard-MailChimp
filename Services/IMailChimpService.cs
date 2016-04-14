using System.Threading.Tasks;
using MailChimp.Resources;
using Orchard;

namespace MailChimp.Services
{
    public interface IMailChimpService : IDependency {
        Task<List> GetList(string listId);
        Task<Member> GetMember(string listId, string emailAddress);
        Task<ListMembers> GetMembers(string idList);
        void RefreshCache(string idList);
    }
}