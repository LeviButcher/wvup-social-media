using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.ViewComponents
{
    public class GroupListViewComponent : ViewComponent
    {
        private IWebApiCalls _webApiCalls;
        public UserManager<User> UserManager { get; }

        public GroupListViewComponent(IWebApiCalls webApiCalls, UserManager<User> userManager)
        {
            _webApiCalls = webApiCalls;
            UserManager = userManager;
        }

        /// <summary>
        ///     Returns a list of GroupViewModels either based on a search term or the groups a user is a member of
        ///     if term is a non-empty string, get list of groups that match search term, else get list of groups
        ///     where the user is a member
        /// </summary>
        /// <param name="term">Search term</param>
        /// <returns>Returns a list of GroupViewModels</returns>
        public async Task<IViewComponentResult> InvokeAsync(string term = "")
        {
            IEnumerable<GroupViewModel> groups;
            User user = await UserManager.GetUserAsync(HttpContext.User);

                        
            if (term != "")
            {
                groups = await _webApiCalls.SearchGroupAsync(term);
            }
            else
            {  
                groups = await _webApiCalls.GetUsersGroupsAsync(user.Id);
            }

            return View(groups);
        }
    }
}

