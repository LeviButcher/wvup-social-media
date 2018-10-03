using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.DAL.Repos.Interfaces
{
    public interface IGroupRepo
    {
        Group GetBaseGroup(int groupId);
        IEnumerable<UserProfile> GetGroupMembers(int groupId, int skip = 0, int take = 10);
        int GetMemberCount(int groupId);
        IEnumerable<GroupViewModel> GetUsersGroups(string userId, int skip = 0, int take = 10);
        int CreateGroup(Group group);
        int DeleteGroup(Group group);
        int UpdateGroup(Group group);
        IEnumerable<GroupViewModel> FindGroups(string term);
        GroupViewModel GetGroup(int id);
        IEnumerable<GroupViewModel> GetAllGroups();
        Task<bool> IsOwner(string userId, int groupId);
        UserProfile GetOwner(int id);
        Task<bool> IsMember(string userId, int groupId);
        Task<int> JoinGroup(string userId, int groupId);
        Task<int> LeaveGroup(string userId, int groupId);

    }
}
