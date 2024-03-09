using Microsoft.AspNetCore.Identity;
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
   
        public interface IMaterialService
        {
            (bool Success, string ErrorMessage) CreateMaterial(Material model, ClaimsPrincipal user);

            (bool Success, string ErrorMessage) DeleteMaterial(int id, ClaimsPrincipal user);
            (bool Success, string ErrorMessage) EditMaterial(int id, ClaimsPrincipal user);


        }
        public class MaterialService : IMaterialService
        {
            private readonly ClientManagerDbContext _dbContext;
            private UserManager<AppUser> _userManager;
            private readonly ClaimsPrincipal _user;
            public MaterialService(ClientManagerDbContext dbContext, UserManager<AppUser> userManager, ClaimsPrincipal user)
            {
                _dbContext = dbContext;
                this._userManager = userManager;
                _user = user;

            }

            public (bool Success, string ErrorMessage) CreateMaterial(Material model, ClaimsPrincipal user)
            {
            try
            {
                var existingMaterial = _dbContext.Materials
         .FirstOrDefault(x => x.MaterialName == model.MaterialName && x.IsDeleted == false);
                if (existingMaterial != null)
                {
                    throw new Exception("Naziv materijala već postoji");
                }
                else
                {

                    model.CreatedById = _userManager.GetUserName(user);
                    model.CreateTime = DateTime.Now;
                    _dbContext.Materials.Add(model);
                    _dbContext.SaveChanges();
                    return (true, null);
                }
            }
            catch (Exception ex)
            {

                return (false, "Došlo je do greške prilikom kreiranja usluge." + ex.Message);
            }
        }

            public (bool Success, string ErrorMessage) DeleteMaterial(int id, ClaimsPrincipal user)
            {
            try
            {
                var model = _dbContext.Materials.FirstOrDefault(a => a.ID == id);
                var materialOffer = _dbContext.MaterialOffers
            .Where(uv => uv.MaterialId == model.ID)
            .ToList();
                if (materialOffer.Any())
                {
                    throw new InvalidOperationException("Izbrišite materijal sa ponude");
                }
                var orderOffer = _dbContext.OrderMaterials
          .Where(uv => uv.MaterialId == model.ID)
          .ToList();
                if (orderOffer.Any())
                {
                    throw new InvalidOperationException("Izbrišite materijal iz narudžbe");
                }

                model.IsDeleted = true;
                model.DeletedById = _userManager.GetUserName(user);
                model.DeleteTime = DateTime.Now;
                _dbContext.SaveChanges();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Došlo je do greške prilikom brisanja materijala." + ex.Message);
            }
        }

            public (bool Success, string ErrorMessage) EditMaterial(int id, ClaimsPrincipal user)
            {
            try
            {
                var material = this._dbContext.Materials.FirstOrDefault(c => c.ID == id);
                material.UpdatedById = _userManager.GetUserName(user);

                material.IsActive = false;

                var newMaterial = new Material
                {
                    MaterialName = material.MaterialName,
                    InStockQuantity = material.InStockQuantity,
                    MaterialPrice = material.MaterialPrice,
                    MaterialDescription = material.MaterialDescription,

                    CreateTime = DateTime.Now,
                    CreatedById = _userManager.GetUserName(user),
                    IsActive = true,
                };

                this._dbContext.Materials.Add(newMaterial);
                var existingMaterial = _dbContext.Materials
           .FirstOrDefault(x => x.MaterialName == material.MaterialName && x.ID != material.ID && x.IsDeleted == false);

                if (existingMaterial != null)
                {
                    throw new Exception("Naziv materijala je već unesen.");
                }
                var materialOffer = _dbContext.MaterialOffers
           .Where(uv => uv.MaterialId == material.ID)
           .ToList();
                if (materialOffer.Any())
                {
                    throw new InvalidOperationException("Izbrišite materijal sa ponude");
                }
                var orderOffer = _dbContext.OrderMaterials
          .Where(uv => uv.MaterialId == material.ID)
          .ToList();
                if (orderOffer.Any())
                {
                    throw new InvalidOperationException("Izbrišite materijal iz narudžbe");
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
