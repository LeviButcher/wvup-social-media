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
