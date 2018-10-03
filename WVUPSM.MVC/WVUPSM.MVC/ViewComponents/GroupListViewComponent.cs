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

