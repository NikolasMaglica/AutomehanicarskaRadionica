﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Services
{
    public interface IOrderService
    {
        Task<(bool Success, string ErrorMessage)> CreateOrderAsync(Order model, ClaimsPrincipal user);

        Task<(bool Success, string ErrorMessage)> DeleteOrderAsync(int id, ClaimsPrincipal user);
        decimal CalculateTotalPrice(Order order);
        void UpdateMaterialStock(Order order);
        Task<(bool Success, string ErrorMessage)> UpdateOrderAsync(Order order);


    }
    public class OrderService : IOrderService
    {
        private readonly ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;
        private readonly ClaimsPrincipal _user;
        public OrderService(ClientManagerDbContext dbContext, UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            _dbContext = dbContext;
            this._userManager = userManager;
            _user = user;

        }

        public async Task<(bool Success, string ErrorMessage)> DeleteOrderAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                var model = _dbContext.Orders.FirstOrDefault(a => a.ID == id);
                var ordermaterials = _dbContext.OrderMaterials
            .Where(uv => uv.OrderId == model.ID)
            .ToList();
                if (ordermaterials.Any())
                {
                    throw new InvalidOperationException(" Izbrišite stavke narudžbe.");
                }
                if (model == null)
                {
                    throw new InvalidOperationException("Narudžba nije pronađeno.");
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

        public async Task<(bool Success, string ErrorMessage)> CreateOrderAsync(Order model, ClaimsPrincipal user)
        {
            try
            {

                model.CreatedById = _userManager.GetUserName(user);
                model.CreateTime = DateTime.Now;
         
               await _dbContext.Orders.AddAsync(model);
                _dbContext.Entry(model).State = EntityState.Added;

               await _dbContext.SaveChangesAsync();

                return (true, null); 
            }
            catch (Exception ex)
            {

                return (false, "Došlo je do greške prilikom kreiranja narudžbe." + ex.Message);
            }
        }

        public decimal CalculateTotalPrice(Order order)
        {
            decimal total = 0;
            if (order.OrderMaterials == null)
            {
                Console.WriteLine("OrderMaterials je null");
            }
            else
            {
                foreach (var orderMaterial in order.OrderMaterials)
                {
                    // Provjeri jesu li OrderId i MaterialId različiti od null
                    if (orderMaterial.OrderId != null && orderMaterial.MaterialId != null)
                    {
                        // Pronađi odgovarajući materijal i pomnoži s količinom
                        Material material = _dbContext.Materials.Find(orderMaterial.MaterialId);
                        if (material != null)
                        {
                            total += orderMaterial.Quantity * material.MaterialPrice;
                        }
                    }

                }

            }
            return total;
        }
        public void UpdateMaterialStock(Order order)
        {
            if (order.OrderMaterials != null)
            {
                foreach (var orderMaterial in order.OrderMaterials)
                {
                    if (orderMaterial.OrderId != null && orderMaterial.MaterialId != null)
                    {
                        // Pronađi materijal iz narudžbe
                        Material material = _dbContext.Materials.Find(orderMaterial.MaterialId);

                        // Ažuriraj količinu materijala u skladištu
                        if (material != null)
                        {
                            material.InStockQuantity += orderMaterial.Quantity;
                        }
                    }
                }
            }
            _dbContext.SaveChanges();
        }
        public async Task<(bool Success, string ErrorMessage)> UpdateOrderAsync(Order order)
        {
            try
            {
                List<OrderMaterial> eduDetails = _dbContext.OrderMaterials.Where(d => d.OrderId == order.ID).ToList();
                _dbContext.OrderMaterials.RemoveRange(eduDetails);
               await _dbContext.SaveChangesAsync();
                _dbContext.Attach(order);
                _dbContext.Entry(order).State = EntityState.Modified;
                _dbContext.OrderMaterials.AddRange(order.OrderMaterials);
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


