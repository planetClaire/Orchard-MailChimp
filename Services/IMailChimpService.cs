﻿using System.Collections.Generic;
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
        Task<Member> AddOrUpdateMember(Member member);
        Task<Member> UnsubscribeMember(string email, string listId);
        Task<bool> DeleteMember(string idList, string emailAddress);
        Task<Batch> CreateBatch(List<Member> batch);
        Task<Batch> CreateBatch(List<Member> listMembersToPut, List<Member> listMembersToDelete);
        Task<BatchCollection> GetBatches();
        Task<BatchCollection> GetAllBatches();
        void RefreshCache(string idList);
    }
}