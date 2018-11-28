using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models.Services;

namespace WVUPSM.Models.ViewModels
{
    /// <summary>
    /// ViewModel for notifications
    /// </summary>
    public class NotificationViewModel
    {
        /// <summary>
        /// Id for this notification
        /// </summary>
        public int NotificationId { get; set; }

        /// <summary>
        /// Date Notification Created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///     String telling how old the Notification is
        /// </summary>
        public string TimeSinceCreation
        {
            get
            {
                return Time.TimeSince(DateCreated);
            }
        }

        /// <summary>
        ///     Text detailing the notification
        /// </summary>
        public string NotificationText { get; set; }

        /// <summary>
        ///     Link to navigate in MVC to the place where notification is describing
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        ///     True if this notification has been read, false otherwise
        ///     Past tense by the way
        /// </summary>
        public bool Read { get; set; }
    }
}
