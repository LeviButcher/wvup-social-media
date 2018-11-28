using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.DAL.Repos.Interfaces
{
    /// <summary>
    ///     Interface for Notifications Repo
    /// </summary>
    public interface INotificationRepo
    {
        /// <summary>
        ///     Gets the Users Unread Notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        IEnumerable<NotificationViewModel> GetUsersUnreadNotifications(string userId, int skip, int take);

        /// <summary>
        ///     Gets details on Paging for UnreadNotifications lookup
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns>PageViewModel</returns>
        PagingViewModel GetUnreadPageDetails(string userId, int pageSize, int pageIndex);

        /// <summary>
        ///     Creates a new Notifications
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        int CreateNotification(Notification notification);

        /// <summary>
        ///     Marks a Notification as being read
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int MarkAsRead(int id);

        /// <summary>
        ///     Gets a users read notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        IEnumerable<NotificationViewModel> GetUsersReadNotifications(string userId, int skip, int take);

        /// <summary>
        ///     Gets details on Paging for UnreadNotifications lookup
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns>PageViewModel</returns>
        PagingViewModel GetReadPageDetails(string userId, int pageSize, int pageIndex);

        /// <summary>
        ///  Gets a list of today's notifications for this user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<NotificationViewModel> GetTodaysNotifcations(string userId);

        /// <summary>
        /// Gets the base notification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Notification GetNotification(int id);

        /// <summary>
        ///     Gets the number of unread Notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetUnReadNotificationCount(string userId);
    }
}
