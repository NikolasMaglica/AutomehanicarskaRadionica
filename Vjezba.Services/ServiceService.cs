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
        (bool Success, string ErrorMessage) CreateService(Service model, ClaimsPrincipal user);

        (bool Success, string ErrorMessage) DeleteService(int id, ClaimsPrincipal user);
        (bool Success, string ErrorMessage) EditService(int id, ClaimsPrincipal user);


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

        public (bool Success, string ErrorMessage) DeleteService(int id, ClaimsPrincipal user)
        {
            try
            {
                var model = _dbContext.Services.FirstOrDefault(a => a.ID == id);
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
                _dbContext.SaveChanges();

                return (true, null);  
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom brisanja narudžbe." + ex.Message);
            }
        }

        public (bool Success, string ErrorMessage) CreateService(Service model, ClaimsPrincipal user)
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
                    _dbContext.SaveChanges();
                    return (true, null);
                }
            }
            catch (Exception ex)
            {

                return (false, "Došlo je do greške prilikom kreiranja usluge." + ex.Message);
            }
        }

        public (bool Success, string ErrorMessage) EditService(int id, ClaimsPrincipal user)
        {
            try
            {
                var service = this._dbContext.Services.FirstOrDefault(c => c.ID == id);
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

                this._dbContext.Services.Add(newService);
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
                this._dbContext.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom ažuriranja narudžbe." + ex.Message);
            }
        }
    }
}
