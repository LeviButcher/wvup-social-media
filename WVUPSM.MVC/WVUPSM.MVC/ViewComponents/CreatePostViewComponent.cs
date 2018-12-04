using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.ViewComponents
{
    public class CreatePostViewComponent : ViewComponent
    {
        private IWebApiCalls _webApiCalls;
        public UserManager<User> _userManager { get; }

        public CreatePostViewComponent(IWebApiCalls webApiCalls, UserManager<User> userManager)
        {
            _webApiCalls = webApiCalls;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal User)
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.Groups = await _webApiCalls.GetGroupsForDropdown(userId);

            CreatePost model = new CreatePost()
            {
                UserName = User.Identity.Name,
                UserId = userId
            };

            return View("CreatePost",model);
        }
    }
}
