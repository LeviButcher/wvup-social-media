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
    public class TagListViewComponent : ViewComponent
    {
        private IWebApiCalls _webApiCalls;

        public TagListViewComponent(IWebApiCalls webApiCalls)
        {
            _webApiCalls = webApiCalls;
        }

        /// <summary>
        ///     Returns a list of TagViewModels based on a search term
        /// </summary>
        /// <param name="term">Search term</param>
        /// <returns>Returns a list of Tags</returns>
        public async Task<IViewComponentResult> InvokeAsync( string term)
        {
            IEnumerable<Tag> tags = new List<Tag>(); ;
            if(term != null)
            {
                if (term.Trim().Length > 0)
                {
                    tags = await _webApiCalls.SearchTags(term);
                }
            }

            return View("TagList", tags);
        }
    }
}

