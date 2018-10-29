using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

namespace WVUPSM.DAL.Repos.Interfaces
{
    /// <summary>
    ///     Comment Repository Database API
    /// </summary>
    public interface ICommentRepo
    {
        /// <summary>
        ///     Gets a Comment based on which Id is passed in.
        /// </summary>
        /// <param name="commentId">The id of this Comment</param>
        /// <returns>CommentViewModel for Comment with id of commentId</returns>
        CommentViewModel GetComment(int commentId);
       
        /// <summary>
        ///     Creates a new Comment record
        /// </summary>
        /// <param name="comment">Comment object that MUST contain a userId and postId</param>
        /// <returns>integer value of number of records affected</returns>
        int CreateComment(Comment comment);

        /// <summary>
        ///    Gets all Comments
        /// </summary>        
        /// <returns>An IEnumerable of all comments</returns>
        IEnumerable<CommentViewModel> GetComments();

        /// <summary>
        ///    Gets all Comments for a Post, using skip and take
        /// </summary>        
        /// <returns>An IEnumerable of all Comments for a Post</returns>
        IEnumerable<CommentViewModel> GetComments(int postId, int skip, int take);
    }
}
