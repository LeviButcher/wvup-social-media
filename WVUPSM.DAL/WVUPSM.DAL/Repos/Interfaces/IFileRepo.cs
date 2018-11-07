using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models.Entities;

namespace WVUPSM.DAL.Repos.Interfaces
{
    /// <summary>
    ///    File Repository Database API
    /// </summary>
    public interface IFileRepo
    {
        /// <summary>
        ///     Gets a File based on which Id is passed in.
        /// </summary>
        /// <param name="fileId">The id of this File</param>
        /// <returns>File id of fileId</returns>
        File GetFile(int fileId);

        /// <summary>
        ///     Creates a new File record
        /// </summary>
        /// <param name="file">File object to be created</param>
        /// <returns>Integer value of number of records affected</returns>
        int CreateFile(File file);

        /// <summary>
        ///    Deletes an existing File record
        /// </summary> 
        /// <param name="file">File object to be deleted</param>
        /// <returns>Integer value of number of records affected</returns>
        int DeleteFile(File file);

        /// <summary>
        ///     Used to check if a file exists, and return that file's id
        /// </summary>
        /// <param name="fileName"> fileName to check in DB</param>
        /// <param name="content"> file Content to checn in DB</param>
        /// <param name="contentType">file ContentType to check in DB</param>
        /// <returns> Id of file with matching properties, if found. else, -1</returns>
        int GetFileByProps(string fileName, byte[] content, string contentType);
    }
}
