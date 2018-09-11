using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.DAL.Repos.Interfaces
{
    public interface IFollowRepo
    {
        IEnumerable<UserProfile> GetFollowers(string userId, int skip = 0, int take = 10);
        IEnumerable<UserProfile> GetFollowing(string userId, int skip = 0, int take = 10);
        int CreateFollower(Follow follow);
        int DeleteFollower(Follow follow);
        int GetFollowingCount(string userId);
        int GetFollowerCount(string userId);
        Task<bool> IsFollowingAsync(string userId, string followId);
    }
}
