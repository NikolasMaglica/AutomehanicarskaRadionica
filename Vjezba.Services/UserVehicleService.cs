using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Services
{
    public interface IUserVehicleService
    {
        Task<(bool Success, string ErrorMessage)> CreateUserVehicleAsync(UserVehicle model, ClaimsPrincipal user);
        Task<(bool Success, string ErrorMessage)> DeleteUserVehicleAsync(int id, ClaimsPrincipal user);
        Task<(bool Success, string ErrorMessage)> EditUserVehicleAsync(int id, ClaimsPrincipal user);

    }
    public class UserVehicleService : IUserVehicleService
    {
        private readonly ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;
        private readonly ClaimsPrincipal _user;


        public UserVehicleService(ClientManagerDbContext dbContext, UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            _dbContext = dbContext;
            this._userManager = userManager;
            _user = user;

        }

        public async Task<(bool Success, string ErrorMessage)> CreateUserVehicleAsync(UserVehicle model, ClaimsPrincipal user)
        {
            try
            {
            
                    model.CreatedById = _userManager.GetUserName(user);
                    model.CreateTime = DateTime.Now;
                    _dbContext.UserVehicles.Add(model);
                   await _dbContext.SaveChangesAsync();
                    return (true, null);
                }
            
            catch (Exception ex)
            {

                return (false, "Došlo je do greške prilikom kreiranja vozila." + ex.Message);
            }
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteUserVehicleAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                var model = await _dbContext.UserVehicles.FirstOrDefaultAsync(a => a.ID == id);
                var offer = _dbContext.Offers
            .Where(uv => uv.UserVehicleId == model.ID)
            .ToList();
                if (offer.Any())
                {
                    throw new InvalidOperationException(" Izbrišite vozilo sa ponude");
                }


                model.IsDeleted = true;
                model.DeletedById = _userManager.GetUserName(user);
                model.DeleteTime = DateTime.Now;
                await _dbContext.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom brisanja vozila " + ex.Message);
            }
        }

        public async Task<(bool Success, string ErrorMessage)> EditUserVehicleAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                var users = await this._dbContext.UserVehicles.FirstOrDefaultAsync(c => c.ID == id);
                users.UpdatedById = _userManager.GetUserName(user);

                users.IsActive = false;

                var newUserV = new UserVehicle
                {
                    KilometersTraveled = users.KilometersTraveled,
                    Description = users.Description,
                    UserId = users.UserId,
                    VehicleID= users.VehicleID,
                    CreateTime = DateTime.Now,
                    CreatedById = _userManager.GetUserName(user),
                    IsActive = true,
                };

                this._dbContext.UserVehicles.Add(newUserV);
            
                var userv_a = _dbContext.Offers
                   .Where(uv => uv.UserVehicleId == users.ID)
                   .ToList();
                if (userv_a.Any())
                {
                    throw new InvalidOperationException("Izbrišite vozilo sa ponude");
                }
                await this._dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom ažuriranja vozila." + ex.Message);
            }
        }
    }
}
