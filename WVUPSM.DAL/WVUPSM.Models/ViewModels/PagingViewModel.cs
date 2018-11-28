using System;
using System.Collections.Generic;
using System.Text;

namespace WVUPSM.Models.ViewModels
{
    /// <summary>
    ///     Represents information needed to do Paging for lookups
    /// </summary>
    public class PagingViewModel
    {
        /// <summary>
        ///     Is there a Page after this one
        /// </summary>
        public bool HasNext { get
            {
                return PageIndex < TotalPages;
            }
        }

        /// <summary>
        ///     Is there a Page before this one
        /// </summary>
        public bool HasPrev { get
            {
                return PageIndex > 1;
            }
        }

        /// <summary>
        ///     Total Amount of Pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        ///     Current Page Index
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     Amount of records displayed on each page
        /// </summary>
        public int PageSize { get; set; }
    }
}
