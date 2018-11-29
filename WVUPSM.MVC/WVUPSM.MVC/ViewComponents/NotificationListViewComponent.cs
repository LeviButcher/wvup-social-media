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
    public class NotificationListViewComponent : ViewComponent
    {
        public IWebApiCalls _api { get; }

        public NotificationListViewComponent(IWebApiCalls api)
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
        public async Task<IViewComponentResult> InvokeAsync(string userId, bool read, bool today = false, int skip = 0, int take = 10)
        {
            IList<NotificationViewModel> notifications = null;

            if(today)
            {
                notifications = await _api.GetTodaysNotifications(userId);
            }
            else if(read)
            {
                notifications = await _api.GetReadNotifications(userId, skip, take);
            }
            else
            {
                //Display unread notifications
                notifications = await _api.GetUnreadNotifications(userId, skip, take);
            }

            return View("NotificationViewList", notifications);
        }
    }
}
