using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.ViewComponents
{
    public class PostFeedViewComponent : ViewComponent
    {
        private IWebApiCalls _webApiCalls;

        public PostFeedViewComponent(IWebApiCalls webApiCalls)
        {
            _webApiCalls = webApiCalls;
        }

        /// <summary>
        ///     Returns a PostFeed existing of the Posts from a user's Following List or a user's own posts.
        ///     if myPosts is true the posts will be the user's, else user's Following
        ///     or if term is provided it'll override every other option
        /// </summary>
        /// <param name="userId">user's Id</param>
        /// <param name="myPosts">Boolean determining which list of posts to return </param>
        /// <returns>Returns a list of UserPosts</returns>
        public async Task<IViewComponentResult> InvokeAsync(string userId, bool myPosts = false)
        {
            IList<UserPost> posts = null;
            if (myPosts)
            {
                ViewData["post-call"] = "Post/GetPosts";
                posts = await _webApiCalls.GetMyPostAsync(userId);
            }
            else
            {
                ViewData["post-call"] = "Home/GetFollowingPost";
                posts = await _webApiCalls.GetFollowingPostAsync(userId);
            }
            return View(posts);
        }
    }
}
