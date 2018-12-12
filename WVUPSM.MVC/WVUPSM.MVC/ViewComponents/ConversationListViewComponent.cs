using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.Models.ViewModels;
using WVUPSM.MVC.WebServiceAccess;
using WVUPSM.MVC.WebServiceAccess.Base;

namespace WVUPSM.MVC.ViewComponents
{
    public class ConversationListViewComponent : ViewComponent
    {
        public IWebApiCalls _api { get; }

        public ConversationListViewComponent(IWebApiCalls api)
        {
            _api = api;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="today"></param>
        /// <param name="read"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(string userId, int skip = 0, int take = 10)
        {
            IList<InboxMessageViewModel> messages = null;

            messages = await _api.GetInboxAsync(userId, skip, take);
           

            return View("InboxViewList", messages);
        }

    }
}
