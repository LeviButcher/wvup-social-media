using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;

namespace WVUPSM.DAL.Repos
{
    public class FileRepo : IFileRepo
    {
        private readonly SMContext _db;

        /// <summary>
        ///     File Table in database
        /// </summary>
        public DbSet<File> Table;

        /// <summary>
        ///     Database context
        /// </summary>
        public SMContext Context => _db;

        public FileRepo()
        {
            _db = new SMContext();
            Table = _db.Set<File>();
        }

        /// <summary>
        ///     Overloaded Constructor
        /// </summary>
        public FileRepo(DbContextOptions<SMContext> options)
        {
            _db = new SMContext(options);
            Table = _db.Set<File>();
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
        ///     Creates File
        /// </summary>
        /// <param name="file">File to be created</param>
        /// <returns>Number of affected records</returns>
        public int CreateFile(File file)
        {
            Table.Add(file);

            return this.SaveChanges();
        }

        /// <summary>
        ///     Deletes File
        /// </summary>
        /// <param name="file">File to be deleted</param>
        /// <returns>Number of affected records</returns>
        public int DeleteFile(File file)
        {
            Table.Remove(file);

            return this.SaveChanges();
        }

        /// <summary>
        ///     Returns File with matching fileId
        /// </summary>
        /// <param name="fileId">Id of File to be returned</param>
        /// <returns>File with corresponding fileId </returns>
        public File GetFile(int fileId)
        {
            return Table.Where(x => x.Id == fileId).First();
        }

        /// <summary>
        ///     Used to check if a file exists, and return that file's id
        /// </summary>
        /// <param name="fileName"> fileName to check in DB</param>
        /// <param name="content"> file Content to checn in DB</param>
        /// <param name="contentType">file ContentType to check in DB</param>
        /// <returns> Id of file with matching properties, if found. else, -1</returns>
        public int GetFileByProps(string fileName, byte[] content, string contentType)
        {
            IEnumerable<File> files = Table.Where(x => x.FileName == fileName && x.Content == content && x.ContentType == contentType);

            if(!files.Any())
            {
                return -1;
            }
            else
            {
                return files.First().Id;
            }
        }
    }
}
