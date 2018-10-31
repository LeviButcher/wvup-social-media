using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVUPSM.MVC.Models
{
    public class RssFeedItemViewModel
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string PubDate { get; set; }

        public string ThumbnailUrl { get; set; }
    }
}
