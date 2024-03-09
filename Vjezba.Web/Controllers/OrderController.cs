
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
        public IActionResult Index()
        {

            var orders = _dbContext.Orders.Include(o => o.OrderStatus).ToList();
            return View(orders);
        }
        [HttpGet]
        public IActionResult Create()
        {
            this.FillDropdownValues();
            this.FillDropdownValuesStatus();
            Order order = new Order();
            order.OrderMaterials.Add(new OrderMaterial() { OrderId = 1 });
            return View(order);
        }
        public IActionResult Details(int Id)
        {
            Order order = _dbContext.Orders.Include(e => e.OrderMaterials).Where(a => a.ID == Id).FirstOrDefault();
            this.FillDropdownValues();
            return View(order);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = User;
            var model = _orderService.DeleteOrder(id, user);
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
        [HttpPost]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                var user = User;
                var result = _orderService.CreateOrder(order, user);
                if (result.Success)
                {
                    order.TotalPrice=_orderService.CalculateTotalPrice(order);
                    _orderService.UpdateMaterialStock(order);
                    _dbContext.SaveChanges();
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
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Order order = _dbContext.Orders.Include(e => e.OrderMaterials).Where(a => a.ID == Id).FirstOrDefault();
            this.FillDropdownValues();
            this.FillDropdownValuesStatus();
            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order order)
        {
            var result = _orderService.UpdateOrder(order);

            if (result.Success && ModelState.IsValid)
            {
                order.TotalPrice = _orderService.CalculateTotalPrice(order);
                if (order.OrderStatusId == 1)
                    _orderService.UpdateMaterialStock(order);
                else
                    _dbContext.SaveChanges();
                return RedirectToAction("index");
            }
            else
            {
                ModelState.AddModelError("", result.ErrorMessage);
            }

            return View(order);
        }


        private void FillDropdownValues()
        {
            var selectItems = new List<SelectListItem>();
            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);

            foreach (var category in this._dbContext.Materials)
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