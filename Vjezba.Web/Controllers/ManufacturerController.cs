using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using Vjezba.Services;

namespace Vjezba.Web.Controllers
{
        public class ManufacturerController : Controller
        {
            private ClientManagerDbContext _dbContext;
            private UserManager<AppUser> _userManager;
            private IManufacturerService _manufacturerService;
            public ManufacturerController(ClientManagerDbContext dbContext, IManufacturerService manufacturerService, UserManager<AppUser> userManager)
            {
                this._dbContext = dbContext;
                this._userManager = userManager;
                this._manufacturerService = manufacturerService;
            }

            [AllowAnonymous]
            public ActionResult Index()
            {
			var manufacturers = _dbContext.Manufacturers.Include(o => o.Vehicles).ToList();
			return View(manufacturers);
		}

            [HttpPost]
            public IActionResult Delete(int id)
            {
                var user = User;
                var model = _manufacturerService.DeleteManufacturer(id, user);
                if (model.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", model.ErrorMessage);
                }
                var manufacturer = _dbContext.Manufacturers.ToList();
                return View("Index", manufacturer);
            }
            public IActionResult Create()
            {
                return View();
            }
            [HttpPost]
            public IActionResult Create(Manufacturer model)
            {
                if (ModelState.IsValid)
                {
                var user = User;
                var result = _manufacturerService.CreateManufacturer(model, user);

                    if (result.Success)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", result.ErrorMessage);
                    }
                }

                return View();
            }


            [ActionName(nameof(Edit))]
            public IActionResult Edit(int id)
            {
                var model = this._dbContext.Manufacturers.FirstOrDefault(c => c.ID == id);
                return View(model);
            }

            [HttpPost]
            [ActionName(nameof(Edit))]
            public async Task<IActionResult> EditPost(int id)
            {
                if (ModelState.IsValid)
                {
                    var manufacturer = this._dbContext.Manufacturers.Single(c => c.ID == id);
                     manufacturer.UpdatedById = _userManager.GetUserName(base.User);
                     manufacturer.UpdateTime = DateTime.Now;
                    var ok = await this.TryUpdateModelAsync(manufacturer);
                    var user = User;
                    var result = _manufacturerService.EditManufacturer(id, user);
                    if (result.Success && ok)
                    {
                        this._dbContext.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", result.ErrorMessage);
                    }
                }

                return View();
            
        }
    }
}
