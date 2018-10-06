using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.Service.Controllers
{
    /// <summary>
    ///     Controller for Comments on Posts
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class CommentController : Controller
    {
        private ICommentRepo _cRepo;

        public CommentController(ICommentRepo cRepo)
        {
            _cRepo = cRepo;
        }

        /// <summary>
        ///     Gets a comment for a post
        /// </summary>
        /// <returns>The post with comments</returns>
        [HttpGet("{commentId}")]
        public IActionResult Get(int commentId)
        {
            var item = _cRepo.GetComment(commentId);
            if (item == null)
            {
                return NotFound();
            }
            return Json(item);
        }

        /// <summary>
        ///     Gets all comments for a post
        /// </summary>
        /// <returns>The post with all comments</returns>
        [HttpGet("{commentId}")]
        public IActionResult GetAllComments(int commentId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var item = _cRepo.GetComments(commentId, skip, take);
            if (item == null)
            {
                return NotFound();
            }
            return Json(item);
        }

        /// <summary>
        ///     Creates a comment for a post
        /// </summary>
        /// <returns>The comment</returns>
        [HttpPost]
        public IActionResult Create([FromBody] Comment comment)
        {
            if (comment == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            if (comment.Text == null /*&& comment.FilePath == null*/)
            {
                return BadRequest();
            }

            _cRepo.CreateComment(comment);
            return Created($"api/post/get/{comment.Id}", comment);
        }
    }
}
