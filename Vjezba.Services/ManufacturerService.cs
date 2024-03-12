using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Services
{
    public interface IManufacturerService
    {
        Task<(bool Success, string ErrorMessage)> CreateManufacturerAsync(Manufacturer model, ClaimsPrincipal user);
        Task<(bool Success, string ErrorMessage)> DeleteManufacturerAsync(int id, ClaimsPrincipal user);
        Task<(bool Success, string ErrorMessage)> EditManufacturerAsync(int id, ClaimsPrincipal user);

    }
    public class ManufacturerService : IManufacturerService
    {
        private readonly ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;
        private readonly ClaimsPrincipal _user;
		public ManufacturerService(ClientManagerDbContext dbContext, UserManager<AppUser> userManager, ClaimsPrincipal user)
		{
			_dbContext = dbContext;
			this._userManager = userManager;
			_user = user;

		}
        public async Task<(bool Success, string ErrorMessage)> CreateManufacturerAsync(Manufacturer model, ClaimsPrincipal user)
        {
            try
            {
            
                var existingManufacturer = await _dbContext.Manufacturers
            .FirstOrDefaultAsync(x => x.Name == model.Name && x.IsDeleted == false);
                if (existingManufacturer != null)
                {
                    throw new Exception("Naziv proizvođača je unesen");
                }
                else
                {
                    model.CreatedById = _userManager.GetUserName(user);
                    model.CreateTime = DateTime.Now;
                    await _dbContext.Manufacturers.AddAsync(model);
                    await _dbContext.SaveChangesAsync();

                    return (true, null); // Uspješno stvoreno vozilo
                }
            }
            catch (Exception ex)
            {

                return (false, "Došlo je do greške prilikom kreiranja proizvođača." + ex.Message);
            }
        }



        public async Task<(bool Success, string ErrorMessage)> DeleteManufacturerAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                var model = await _dbContext.Manufacturers.FirstOrDefaultAsync(a => a.ID == id);

                var manufacturer = _dbContext.Vehicles
                       .Where(uv => uv.ManufacturerID == model.ID && uv.IsDeleted == false)
                       .ToList();
                if (manufacturer.Any())
                {
                    throw new InvalidOperationException("Izbrišite vozilo");
                }
                model.IsDeleted = true;
                model.DeletedById = _userManager.GetUserName(user);
                model.DeleteTime = DateTime.Now;
               await _dbContext.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom brisanja proizvođača." + ex.Message);
            }
        }

        public async Task<(bool Success, string ErrorMessage)> EditManufacturerAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                var manufacturer = await this._dbContext.Manufacturers.FirstOrDefaultAsync(c => c.ID == id);
                manufacturer.UpdatedById = _userManager.GetUserName(user);

                manufacturer.IsActive = false;
                var newManufacturer = new Manufacturer
                {
                    Name = manufacturer.Name,
                
                    CreateTime = DateTime.Now,
                    CreatedById = _userManager.GetUserName(user),
                    IsActive = true,
                };

                this._dbContext.Manufacturers.Add(newManufacturer);
                var existingManufacturer = await _dbContext.Manufacturers
        .FirstOrDefaultAsync(x => x.Name == manufacturer.Name && x.IsDeleted == false);
                if (existingManufacturer != null)
                {
                    throw new Exception("Naziv proizvođača je već unesen");
                }
                var manufacturer_a = _dbContext.Vehicles
                   .Where(uv => uv.ManufacturerID == manufacturer.ID && uv.IsDeleted == false)
                   .ToList();
                if (manufacturer_a.Any())
                {
                    throw new InvalidOperationException("Izbrišite vozilo");
                }
               await this._dbContext.SaveChangesAsync();
                return (true, null);

            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom ažuriranja proizvođača." + ex.Message);

            }
        }
    }
}
