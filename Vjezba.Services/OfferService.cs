using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Services
{
    public interface IOfferService
    {
        (bool Success, string ErrorMessage) CreateOffer(Offer model, ClaimsPrincipal user);

        (bool Success, string ErrorMessage) DeleteOffer(int id, ClaimsPrincipal user);
        decimal CalculateTotalPrice(Offer order);
        (bool Success, string ErrorMessage) UpdateMaterialStock(Offer offer);
        (bool Success, string ErrorMessage) UpdateOffer(Order order);

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

            // Materijali
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

        public (bool Success, string ErrorMessage) CreateOffer(Offer model, ClaimsPrincipal user)
        {
            try
            {

                _dbContext.Offers.Add(model);

                _dbContext.SaveChanges();
                return (true, null); // Uspješno stvoreno vozilo
            }
            catch (Exception ex)
            {

                return (false, "Došlo je do greške prilikom kreiranja ponude." + ex.Message);
            }
        }

        public (bool Success, string ErrorMessage) DeleteOffer(int id, ClaimsPrincipal user)
        {
            try
            {
                var model = _dbContext.Offers.FirstOrDefault(a => a.ID == id);
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
                    throw new InvalidOperationException("Narudžba nije pronađeno.");
                }

                model.IsDeleted = true;
                model.DeletedById = _userManager.GetUserName(user);
                model.DeleteTime = DateTime.Now;
                _dbContext.SaveChanges();
                return (true, null);  // Uspješno izbrisano vozilo
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom brisanja ponude." + ex.Message);
            }
        }
 

        public (bool Success, string ErrorMessage) UpdateMaterialStock(Offer offer)
        {
            try
            {
                if (offer.MaterialOffers != null)
                {
                    foreach (var offerMaterial in offer.MaterialOffers)
                    {
                        if (offerMaterial.OfferId != null && offerMaterial.MaterialId != null)
                        {
                            // Pronađi materijal iz narudžbe
                            Material material = _dbContext.Materials.Find(offerMaterial.MaterialId);

                            // Ažuriraj količinu materijala u skladištu
                            if (material != null)
                            {

                                material.InStockQuantity -= offerMaterial.Quantity;
                            
                            }
                        }
                    }
                }
                _dbContext.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom kreiranja ponude." + ex.Message);

            }
        }

            public (bool Success, string ErrorMessage) UpdateOffer(Order order)
        {
            throw new NotImplementedException();
        }
    }

}
