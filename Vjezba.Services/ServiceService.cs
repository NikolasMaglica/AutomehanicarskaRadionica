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
    public interface IServiceService
    {
        Task<(bool Success, string ErrorMessage)> CreateServiceAsync(Service model, ClaimsPrincipal user);

        Task<(bool Success, string ErrorMessage)> DeleteServiceAsync(int id, ClaimsPrincipal user);
        Task<(bool Success, string ErrorMessage)> EditServiceAsync(int id, ClaimsPrincipal user);


    }
    public class ServiceService : IServiceService
    {
        private readonly ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;
        private readonly ClaimsPrincipal _user;
        public ServiceService(ClientManagerDbContext dbContext, UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            _dbContext = dbContext;
            this._userManager = userManager;
            _user = user;

        }

        public async Task<(bool Success, string ErrorMessage)> DeleteServiceAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                var model = await _dbContext.Services.FirstOrDefaultAsync(a => a.ID == id);
                var serviceoffer = _dbContext.ServiceOffers
            .Where(uv => uv.ServiceId == model.ID)
            .ToList();
                if (serviceoffer.Any())
                {
                    throw new InvalidOperationException(" Tablica je povezana");
                }
              

                model.IsDeleted = true;
                model.DeletedById = _userManager.GetUserName(user);
                model.DeleteTime = DateTime.Now;
                 await  _dbContext.SaveChangesAsync();

                return (true, null);  
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom brisanja narudžbe." + ex.Message);
            }
        }

        public async Task<(bool Success, string ErrorMessage)> CreateServiceAsync(Service model, ClaimsPrincipal user)
        {
            try
            {
                var existingService = _dbContext.Services
         .FirstOrDefault(x => x.ServiceName == model.ServiceName && x.IsDeleted == false);
                if (existingService != null)
                {
                    throw new Exception("Naziv usluge je unesen");
                }
                else
                {

                    model.CreatedById = _userManager.GetUserName(user);
                    model.CreateTime = DateTime.Now;
                    _dbContext.Services.Add(model);
                   await _dbContext.SaveChangesAsync();
                    return (true, null);
                }
            }
            catch (Exception ex)
            {

                return (false, "Došlo je do greške prilikom kreiranja usluge." + ex.Message);
            }
        }

        public async Task<(bool Success, string ErrorMessage)> EditServiceAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                var service = await this._dbContext.Services.FirstOrDefaultAsync(c => c.ID == id);
                service.UpdatedById = _userManager.GetUserName(user);

                service.IsActive = false;
                
                var newService = new Service
                {
                    ServiceName = service.ServiceName,
                    ServicePrice = service.ServicePrice,
                    ServiceDescription=service.ServiceDescription,
                    CreateTime = DateTime.Now,
                    CreatedById = _userManager.GetUserName(user),
                    IsActive = true,
                };

                await this._dbContext.Services.AddAsync(newService);
                var existingService = _dbContext.Services
        .FirstOrDefault(x => x.ServiceName == service.ServiceName && x.IsDeleted == false);
                if (existingService != null)
                {
                    throw new Exception("Naziv proizvođača je već unesen");
                }
                var serevice_a = _dbContext.ServiceOffers
                   .Where(uv => uv.ServiceId == service.ID)
                   .ToList();
                if (serevice_a.Any())
                {
                    throw new InvalidOperationException("Izbrišite vozilo");
                }
               await this._dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom ažuriranja narudžbe." + ex.Message);
            }
        }
    }
}
