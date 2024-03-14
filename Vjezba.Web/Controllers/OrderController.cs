
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Vjezba.DAL;
using Vjezba.Model;
using Vjezba.Services;

namespace Vjezba.Web.Controllers
{
    public class OrderController : Controller
    {
        private ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;
        private IOrderService _orderService;

        public OrderController(ClientManagerDbContext dbContext, UserManager<AppUser> userManager, IOrderService orderService)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._orderService = orderService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            var orders = _dbContext.Orders.Include(o => o.OrderStatus).ToList();
            return View(orders);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            this.FillDropdownValues();
            this.FillDropdownValuesStatus();
            Order order = new Order();
            order.OrderMaterials.Add(new OrderMaterial() { ID = 1 });
            return View(order);
        }

        [Authorize]
        public async Task<IActionResult> Details(int Id)
        {
            Order order = _dbContext.Orders.Include(e => e.OrderMaterials).Where(a => a.ID == Id).FirstOrDefault();
            this.FillDropdownValues();
            return View(order);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = User;
            var model =await _orderService.DeleteOrderAsync(id, user);
            if (model.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", model.ErrorMessage);
            }
            var orders = _dbContext.Orders.Include(o => o.OrderStatus).ToList();
            return View("Index", orders);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                var user = User;
                var result = await _orderService.CreateOrderAsync(order, user);
                if (result.Success)
                {
                    order.TotalPrice=_orderService.CalculateTotalPrice(order);
                    if (order.OrderStatusId == 1)
                        _orderService.UpdateMaterialStock(order);
                   await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                }
            }
            this.FillDropdownValues();
            this.FillDropdownValuesStatus();
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            Order order =await _dbContext.Orders.Include(e => e.OrderMaterials).Where(a => a.ID == Id).FirstOrDefaultAsync();
            this.FillDropdownValues();
            this.FillDropdownValuesStatus();
            return View(order);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                var result = await _orderService.UpdateOrderAsync(order);
                if (result.Success)
                {
                    order.TotalPrice = _orderService.CalculateTotalPrice(order);
                    if (order.OrderStatusId == 1)
                        _orderService.UpdateMaterialStock(order);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("index");
                }
                else
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                }
            }
            this.FillDropdownValues();
            return View();
        }


        private void FillDropdownValues()
        {
            var selectItems = new List<SelectListItem>();
            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);

            var materials = this._dbContext.Materials.Where(m => m.IsActive == true && !m.IsDeleted).ToList();

            foreach (var category in materials)
            {
                listItem = new SelectListItem(category.MaterialName, category.ID.ToString());
                selectItems.Add(listItem);
            }

            ViewBag.PossibleMaterials = selectItems;

        }
        private void FillDropdownValuesStatus()
        {
            var selectItems = new List<SelectListItem>();
            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);

            foreach (var category in this._dbContext.OrderStatuses)
            {
                listItem = new SelectListItem(category.Name, category.ID.ToString());
                selectItems.Add(listItem);
            }

            ViewBag.PossibleStatuses = selectItems;

        }

    }
}