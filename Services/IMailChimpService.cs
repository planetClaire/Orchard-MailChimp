using System.Threading.Tasks;
using MailChimp.Resources;
using Orchard;

namespace MailChimp.Services
{
    public interface IMailChimpService : IDependency {
        Task<Member> GetMember(string listId, string emailAddress);
        Task<List> GetList(string listId, string[] fields = null);
        Task<ListMembers> GetMembersInfo(string idList, string[] fields = null);
        Task<ListMembers> GetAllMembers(string idList);
        Task<Member> AddMember(Member member);
        void RefreshCache(string idList);
    }
}