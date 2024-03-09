using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Services
{
    public interface IManufacturerService
    {
        (bool Success, string ErrorMessage) CreateManufacturer(Manufacturer model,ClaimsPrincipal user);
        (bool Success, string ErrorMessage) DeleteManufacturer(int id, ClaimsPrincipal user);
        (bool Success, string ErrorMessage) EditManufacturer(int id, ClaimsPrincipal user);

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
		public (bool Success, string ErrorMessage) CreateManufacturer(Manufacturer model,ClaimsPrincipal user)
        {
            try
            {
                /*var existingManufacturer = _dbContext.Manufacturers.SingleOrDefault(x=> x.Name == model.Name); 

                if (existingManufacturer != null && !existingManufacturer.IsDeleted)
                {
                    throw new Exception("Naziv proizvođača je unesen");
                }*/
                var existingManufacturer = _dbContext.Manufacturers
            .FirstOrDefault(x => x.Name == model.Name && x.IsDeleted == false);
                if (existingManufacturer != null)
                {
                    throw new Exception("Naziv proizvođača je unesen");
                }
                else
                {
                    model.CreatedById = _userManager.GetUserName(user);
                    model.CreateTime = DateTime.Now;
                    _dbContext.Manufacturers.Add(model);
                    _dbContext.SaveChanges();

                    return (true, null); // Uspješno stvoreno vozilo
                }
            }
            catch (Exception ex)
            {

                return (false, "Došlo je do greške prilikom kreiranja proizvođača." + ex.Message);
            }
        }

     

        public (bool Success, string ErrorMessage) DeleteManufacturer(int id, ClaimsPrincipal user)
        {
            try
            {
                var model = _dbContext.Manufacturers.FirstOrDefault(a => a.ID == id);

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
                _dbContext.SaveChanges();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom brisanja proizvođača." + ex.Message);
            }
        }

        public (bool Success, string ErrorMessage) EditManufacturer(int id, ClaimsPrincipal user)
        {
            try
            {
                var manufacturer = this._dbContext.Manufacturers.FirstOrDefault(c => c.ID == id);
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
                var existingManufacturer = _dbContext.Manufacturers
        .FirstOrDefault(x => x.Name == manufacturer.Name && x.IsDeleted == false);
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
                this._dbContext.SaveChanges();
                return (true, null);

            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom ažuriranja proizvođača." + ex.Message);

            }
        }
    }
}
