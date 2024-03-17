using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Services
{
    public interface IClientService
    {
        Task<(bool Success, string ErrorMessage)> CreateClientAsync(Client model, ClaimsPrincipal user);
        Task<(bool Success, string ErrorMessage)> DeleteClientAsync(int id, ClaimsPrincipal user);
        Task<(bool Success, string ErrorMessage)> EditClientAsync(int id, ClaimsPrincipal user);

    }
    public class ClientService : IClientService
    {
        private readonly ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;
        private readonly ClaimsPrincipal _user;
        public ClientService(ClientManagerDbContext dbContext, UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            _dbContext = dbContext;
            this._userManager = userManager;
            _user = user;

        }
        public async Task<(bool Success, string ErrorMessage)> CreateClientAsync(Client model, ClaimsPrincipal user)
        {
            try
            {
                    model.CreatedById = _userManager.GetUserName(user);
                    model.CreateTime = DateTime.Now;
                    await _dbContext.Clients.AddAsync(model);
                    await _dbContext.SaveChangesAsync();

                    return (true, null); 
                
            }
            catch (Exception ex)
            {

                return (false, "Došlo je do greške prilikom kreiranja klijenta." + ex.Message);
            }
        }



        public async Task<(bool Success, string ErrorMessage)> DeleteClientAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                var model = await _dbContext.Clients.FirstOrDefaultAsync(a => a.ID == id);

                var manufacturer = _dbContext.Offers
                       .Where(uv => uv.ClientId == model.ID && uv.IsDeleted == false)
                       .ToList();
                if (manufacturer.Any())
                {
                    throw new InvalidOperationException("Izbrišite ponudu ");
                }
                model.IsDeleted = true;
                model.DeletedById = _userManager.GetUserName(user);
                model.DeleteTime = DateTime.Now;
                await _dbContext.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom brisanja klijenta." + ex.Message);
            }
        }

        public async Task<(bool Success, string ErrorMessage)> EditClientAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                var client = await this._dbContext.Clients.FirstOrDefaultAsync(c => c.ID == id);
                client.UpdatedById = _userManager.GetUserName(user);

                client.IsActive = false;
                var newClient = new Client
                {
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Email=client.Email,
                    Address=client.Address,
                    PhoneNumber=client.PhoneNumber
                    CreateTime = DateTime.Now,
                    CreatedById = _userManager.GetUserName(user),
                    IsActive = true,
                };

                this._dbContext.Clients.Add(newClient);
            
                var client_a = _dbContext.Offers
                   .Where(uv => uv.ClientId == client.ID && uv.IsDeleted == false)
                   .ToList();
                if (client_a.Any())
                {
                    throw new InvalidOperationException("Izbrišite klijenta");
                }
                await this._dbContext.SaveChangesAsync();
                return (true, null);

            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom ažuriranja klijenta." + ex.Message);

            }
        }
    }
}
