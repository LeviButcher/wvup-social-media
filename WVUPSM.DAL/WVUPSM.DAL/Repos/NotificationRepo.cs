using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using System.Linq;

namespace WVUPSM.DAL.Repos
{
    /// <summary>
    ///     Notifications lookup methods
    /// </summary>
    public class NotificationRepo : INotificationRepo
    {
        private readonly SMContext _db;

        /// <summary>
        ///     Notification Table in database
        /// </summary>
        public DbSet<Notification> Table;

        /// <summary>
        ///     Database context
        /// </summary>
        public SMContext Context => _db;

        /// <summary>
        ///     Repo Constructor
        /// </summary>
        public NotificationRepo()
        {
            _db = new SMContext();
            Table = _db.Set<Notification>();
        }

        /// <summary>
        ///     Overloaded Constructor
        /// </summary>
        public NotificationRepo(DbContextOptions<SMContext> options)
        {
            _db = new SMContext(options);
            Table = _db.Set<Notification>();
        }

        private bool _disposed = false;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                //Free any other managed objects here
            }
            _db.Dispose();
            _disposed = true;
        }

        /// <summary>
        ///     Saves Changes within Database
        /// </summary>
        public int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //A concurrency error occurred
                Console.WriteLine(ex);
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                //_dbResiliency retry limit exceeded
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        ///     Creates a new Notifications
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public int CreateNotification(Notification notification)
        {
            Table.Add(notification);
            return this.SaveChanges();
        }

        public NotificationViewModel GetRecord(Notification n)
        {
            var model = new NotificationViewModel
            {
                DateCreated = n.DateCreated,
                NotificationId = n.Id,
                Read = n.Read,
            };

            //Logic for making the Text and Link
            switch (n.Type)
            {
                case NotificationType.Comment:
                    model.NotificationText = String.Format("{0} has commented on your post", n.Comment.User.UserName);
                    model.Link = String.Format("Post/Index/{0}", n.Comment.PostId);
                    break;
                case NotificationType.Follow:
                    model.NotificationText = String.Format("{0} has started following you", n.InteractingUser.UserName);
                    model.Link = String.Format("User/Index/{0}", n.InteractingUserId);
                    break;
                case NotificationType.Message:
                    model.NotificationText = String.Format("{0} has sent you a message", n.InteractingUser.UserName);
                    model.Link = String.Format("Message/{0}", n.InteractingUserId);
                    break;
                default:
                    model.NotificationText = String.Format("Something wrong has happened, {0}", n.Id);
                    model.Link = String.Format("/BadNotification", n.InteractingUserId);
                    break;
            }
            return model;
        }

        /// <summary>
        ///  Gets a list of today's notifications for this user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<NotificationViewModel> GetTodaysNotifcations(string userId)
            => Table.Include(x => x.User).Include(x => x.InteractingUser).Include(x => x.Comment).ThenInclude(x => x.User)
            .Where(x => x.UserId == userId && x.DateCreated.Day == DateTime.Now.Day)
            .OrderByDescending(x => x.DateCreated)
            .Select(x => GetRecord(x));


        /// <summary>
        ///     Gets a users read notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<NotificationViewModel> GetUsersReadNotifications(string userId, int skip, int take)
            => Table.Include(x => x.User).Include(x => x.InteractingUser).Include(x => x.Comment).ThenInclude(x => x.User)
            .Where(x => x.UserId == userId && x.Read == true)
            .OrderByDescending(x => x.DateCreated)
            .Skip(skip).Take(take)
            .Select(x => GetRecord(x));

        /// <summary>
        ///     Gets the Users Unread Notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<NotificationViewModel> GetUsersUnreadNotifications(string userId, int skip, int take)
            => Table.Include(x => x.User).Include(x => x.InteractingUser).Include(x => x.Comment).ThenInclude(x => x.User)
                .Where(x => x.UserId == userId && x.Read == false)
                .OrderByDescending(x => x.DateCreated)
                .Skip(skip).Take(take)
                .Select(x => GetRecord(x));

        /// <summary>
        /// Gets the base notification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Notification GetNotification(int id)
        => Table.Include(x => x.User)
                .First(x => x.Id == id);

        /// <summary>
        ///     Marks a Notification as being read
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int MarkAsRead(int id)
        {
            var notify = GetNotification(id);
            if(notify != null)
            {
                notify.Read = true;
            }
            return this.SaveChanges();
        }

        /// <summary>
        ///     Gets the number of unread Notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUnReadNotificationCount(string userId)
            => Table.Count(x => x.UserId == userId && x.Read == false);
    }
}
