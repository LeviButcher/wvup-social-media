using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.ViewComponents
{
    public class UserListViewComponent : ViewComponent
    {
        private IWebApiCalls _webApiCalls;

        public UserListViewComponent(IWebApiCalls webApiCalls)
        {
            _webApiCalls = webApiCalls;
        }
        
        /// <summary>
        ///     Returns a UserList existing of the user's Followers or user's Following or userList matching a search term.
        ///     if Following is true list will be user's Following, else followers 
        ///     or if term is provided it'll override every other option
        /// </summary>
        /// <param name="userId">user's Id</param>
        /// <param name="term">Search term</param>
        /// <param name="Following">List or Following else Followers</param>
        /// <returns>Returns a view contain Followers or Following</returns>
        public async Task<IViewComponentResult> InvokeAsync(string userId, string term = "", bool Following = false)
        {
            IList<UserProfile> users;
            if(term != "")
            {
                users = await _webApiCalls.SearchUserAsync(term);
            }
            else if(Following)
            {
                users = await _webApiCalls.GetFollowingAsync(userId);
            }
            else
            {
                users = await _webApiCalls.GetFollowersAsync(userId);
            }
            return View(users);
        }
    }
}
