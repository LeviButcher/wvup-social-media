using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WVUPSM.Models.Entities;

namespace WVUPSM.DAL.Repos.Interfaces
{
    public interface IRoleRepo
    {
        IEnumerable<IdentityRole> GetRoles();

        Task<IEnumerable<User>> GetAdmins();

        Task<IEnumerable<User>> GetUsers();

        Task<bool> AddToRoleById(string userId, string roleId);

        Task<bool> AddToUsers(string userId);

        Task<bool> AddToAdmin(string userId);
    }
}
