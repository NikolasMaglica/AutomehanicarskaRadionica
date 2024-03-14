using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Services
{
    public interface IOfferService
    {
        Task<(bool Success, string ErrorMessage)> CreateOfferAsync(Offer model, ClaimsPrincipal user);

        Task<(bool Success, string ErrorMessage)> DeleteOfferAsync(int id, ClaimsPrincipal user);
        decimal CalculateTotalPrice(Offer order);
        Task<(bool Success, string ErrorMessage)> UpdateMaterialStockAsync(Offer offer);
        Task<(bool Success, string ErrorMessage)> UpdateOfferAsync(Offer offer);


    }
    public class OfferService : IOfferService
    {
        private readonly ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;
        private readonly ClaimsPrincipal _user;
        public OfferService(ClientManagerDbContext dbContext, UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            _dbContext = dbContext;
            this._userManager = userManager;
            _user = user;

        }
        public decimal CalculateTotalPrice(Offer offer)
        {
            decimal total = 0;

            if (offer.MaterialOffers == null)
            {
                Console.WriteLine("Null je");
            }
            else
            {
                foreach (var offerMaterial in offer.MaterialOffers)
                {
                    if (offerMaterial.OfferId != null && offerMaterial.MaterialId != null)
                    {
                        Material material = _dbContext.Materials.Find(offerMaterial.MaterialId);
                        if (material != null)
                        {
                            total += offerMaterial.Quantity * material.MaterialPrice;
                        }
                    }
                
            }
            }
           if (offer.ServiceOffers == null)
            {
                Console.WriteLine("Null je");
            }
           else
            {
                foreach (var serviceOffer in offer.ServiceOffers)
                {
                    if (serviceOffer.OfferId != null && serviceOffer.ServiceId != null)
                    {
                        Service service = _dbContext.Services.Find(serviceOffer.ServiceId);
                        if (service != null)
                        {
                            total += serviceOffer.Quantity * service.ServicePrice;
                        }
                    }
                }
            }

            return total;
        }

        public async Task<(bool Success, string ErrorMessage)> CreateOfferAsync(Offer model, ClaimsPrincipal user)
        {
            try
            {
                model.CreatedById = _userManager.GetUserName(user);
                model.CreateTime = DateTime.Now;
                await _dbContext.Offers.AddAsync(model);
                _dbContext.Entry(model).State = EntityState.Added;

                await _dbContext.SaveChangesAsync();
                return (true, null); 
            }
            catch (Exception ex)
            {

                return (false, "Došlo je do greške prilikom kreiranja ponude." + ex.Message);
            }
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteOfferAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                var model =await _dbContext.Offers.FirstOrDefaultAsync(a => a.ID == id);
                var offermaterials = _dbContext.MaterialOffers
            .Where(uv => uv.OfferId == model.ID)
            .ToList();
                var servicematerials = _dbContext.ServiceOffers
          .Where(u => u.OfferId == model.ID)
          .ToList();

                if (offermaterials.Any() || servicematerials.Any())
                {
                    throw new InvalidOperationException(" Izbrišite stavke ponude.");
                }
             

                if (model == null)
                {
                    throw new InvalidOperationException("Ponuda nije pronađena.");
                }

                model.IsDeleted = true;
                model.DeletedById = _userManager.GetUserName(user);
                model.DeleteTime = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return (true, null); 
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom brisanja ponude." + ex.Message);
            }
        }


        public async Task<(bool Success, string ErrorMessage)> UpdateMaterialStockAsync(Offer offer)
        {
            try
            {
                if (offer.MaterialOffers != null)
                {
                    foreach (var offerMaterial in offer.MaterialOffers)
                    {
                        if (offerMaterial.OfferId != null && offerMaterial.MaterialId != null)
                        {
                            Material material = _dbContext.Materials.Find(offerMaterial.MaterialId);
                         
                            if (material != null)
                            {

                                material.InStockQuantity -= offerMaterial.Quantity;
                            
                            }
                        }
                    }
                }
              
               await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom kreiranja ponude." + ex.Message);

            }
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateOfferAsync(Offer offer)
        {
            try
            {
                List<MaterialOffer> offDetails = await _dbContext.MaterialOffers.Where(d => d.OfferId == offer.ID).ToListAsync();
                List<ServiceOffer> serDetails = await _dbContext.ServiceOffers.Where(d => d.OfferId == offer.ID).ToListAsync();

                _dbContext.MaterialOffers.RemoveRange(offDetails);
                _dbContext.ServiceOffers.RemoveRange(serDetails);

                await _dbContext.SaveChangesAsync();

                _dbContext.Attach(offer);
                _dbContext.Entry(offer).State = EntityState.Modified;
                _dbContext.MaterialOffers.AddRange(offer.MaterialOffers);
                _dbContext.ServiceOffers.AddRange(offer.ServiceOffers);

                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom ažuriranja narudžbe." + ex.Message);
            }
        }


    }

}
