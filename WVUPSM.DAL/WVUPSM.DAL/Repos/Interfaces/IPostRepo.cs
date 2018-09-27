using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.DAL.Repos.Interfaces
{
    public interface IPostRepo
    {
        UserPost GetPost(int id);
        Post GetBasePost(int id);
        IEnumerable<UserPost> GetFollowingPosts(string userId, int skip = 0, int take = 10);
        int DeletePost(Post post);
        int CreatePost(Post post);
        IEnumerable<UserPost> GetUsersPost(string userId, int skip = 0, int take = 10);
        IEnumerable<UserPost> GetGroupPost(int groupId, int skip = 0, int take = 10);
    }
}
