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
        IEnumerable<UserPost> GetFollowPosts(string userId, int skip = 0, int take = 10);
        int DeletePost(Post post);
        int CreatePost(Post post);
        IEnumerable<UserPost> GetUsersPost(string userId, int skip = 0, int take = 10);
    }
}
