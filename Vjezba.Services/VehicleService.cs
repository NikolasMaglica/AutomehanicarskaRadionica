using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Services
{
    public interface IVehicleService
    {
        (bool Success, string ErrorMessage) CreateVehicle(Vehicle model, string createdById);
        (List<Vehicle> Vehicles, bool Success, string ErrorMessage) GetFilteredVehicles(VehicleFilterModel filter);
        (bool Success, string ErrorMessage) DeleteVehicle(int id, ClaimsPrincipal user);
        (bool Success, string ErrorMessage) EditVehicle(int id, ClaimsPrincipal user);

    }
    public class VehicleService : IVehicleService
    {
        private readonly ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;
        private readonly ClaimsPrincipal _user;


        public VehicleService(ClientManagerDbContext dbContext, UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            _dbContext = dbContext;
            this._userManager = userManager;
            _user = user;

        }

        public (bool Success, string ErrorMessage) CreateVehicle(Vehicle model, string createdById)
        {
            try
            {
                if (string.IsNullOrEmpty(model.ModelName) || model.ModelYear != 111111)
                {
                    throw new InvalidOperationException("Nedostaju potrebni podaci za stvaranje vozila.");
                }

                model.CreatedById = createdById;
                model.CreateTime = DateTime.Now;
                _dbContext.Vehicles.Add(model);
                _dbContext.Entry(model).State = EntityState.Added;
                _dbContext.SaveChanges();

                return (true, null); // Uspješno stvoreno vozilo
            }
            catch (Exception ex)
            {

                return (false, "Došlo je do greške prilikom kreiranja vozila." + ex.Message);
            }
        }

        public (List<Vehicle> Vehicles, bool Success, string ErrorMessage) GetFilteredVehicles(VehicleFilterModel filter)
        {
            try
            {
                var clientQuery = _dbContext.Vehicles.Include(p => p.Manufacturer).AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.ModelName))
                    clientQuery = clientQuery.Where(p => p.ModelName.ToLower().Contains(filter.ModelName.ToLower()));

                if (filter.ModelYear > 0)
                    clientQuery = clientQuery.Where(p => p.ModelYear == filter.ModelYear);

                if (filter.ManufacturerID > 0)
                    clientQuery = clientQuery.Where(p => p.ManufacturerID == filter.ManufacturerID);

                var vehicles = clientQuery.ToList();

                return (vehicles, true, null);
            }
            catch (Exception ex)
            {
                return (null, false, "Došlo je do greške prilikom dohvaćanja filtriranih vozila." + ex.Message);
            }
        }
        public (bool Success, string ErrorMessage) DeleteVehicle(int id, ClaimsPrincipal user)
        {
            try
            {
                var model = _dbContext.Vehicles.FirstOrDefault(a => a.ID == id);
                var userVehicles = _dbContext.UserVehicles
            .Where(uv => uv.VehicleID == model.ID)
            .ToList();
                if (userVehicles.Any())
                {
                    throw new InvalidOperationException("Tablica je povezana.");
                }
                if (model == null)
                {
                    return (false, "Vozilo nije pronađeno.");
                }

                model.IsDeleted = true;
                model.DeletedById = _userManager.GetUserName(user);
                model.DeleteTime = DateTime.Now;
                _dbContext.SaveChanges();

                return (true, null);  // Uspješno izbrisano vozilo
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom brisanja vozila." + ex.Message);
            }
        }

        public (bool Success, string ErrorMessage) EditVehicle(int id, ClaimsPrincipal user)
        {
            try
            {
                var vehicle = this._dbContext.Vehicles.Single(c => c.ID == id);
                vehicle.UpdatedById = _userManager.GetUserName(user);

                var userVehicles = _dbContext.UserVehicles
                .Where(uv => uv.VehicleID == vehicle.ID)
                .ToList();

                if (userVehicles.Any())
                {
                    throw new InvalidOperationException("Tablica je povezana.");
                }

                vehicle.IsActive = false;
                var newVehicle = new Vehicle
                {
                    ModelName = vehicle.ModelName,
                    ModelYear = vehicle.ModelYear,
                    ManufacturerID = vehicle.ManufacturerID,
                    CreateTime = DateTime.Now,
                    CreatedById = _userManager.GetUserName(user),
                    IsActive = true,
                };

                this._dbContext.Vehicles.Add(newVehicle);
                this._dbContext.SaveChanges();
                return (true, null);

            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom ažuriranja vozila." + ex.Message);

            }

        }
    }

}