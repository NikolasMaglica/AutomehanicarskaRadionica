using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Vjezba.Services;


namespace Vjezba.Web.Controllers
{
    public class VehicleController : Controller
    {
        private ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;
        private IVehicleService _vehicleService;
        public VehicleController(ClientManagerDbContext dbContext, IVehicleService vehicleService, UserManager<AppUser> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._vehicleService = vehicleService;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult IndexAjax(VehicleFilterModel filter)
        {
            var (model, success, errorMessage) = _vehicleService.GetFilteredVehicles(filter);

            if (success)
            {
                return PartialView("_IndexTable", model);
            }
            else
            {
                ModelState.AddModelError("", errorMessage);
                return PartialView("_IndexTable");
            }
        }

        [Authorize]
        public IActionResult Details(int? id = null)
        {
            var client = this._dbContext.Vehicles
                .Include(p => p.Manufacturer)
                .Where(p => p.ID == id)
                .FirstOrDefault();

            return View(client);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = User;
            var model = await _vehicleService.DeleteVehicleAsync(id,user);
            if (model.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", model.ErrorMessage);
            }
            var vehicles = await _dbContext.Vehicles.Where(v => v.IsActive && v.IsDeleted).ToListAsync();
            return View("Index", vehicles);
        }

        [Authorize]
        public IActionResult Create()
        {
            this.FillDropdownValues();
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Vehicle model)
        {
            if (ModelState.IsValid)
            {
                string createdById = _userManager.GetUserName(base.User);
                var result = await _vehicleService.CreateVehicleAsync(model, createdById);

                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                }
            }

            this.FillDropdownValues();
            return View();
        }

        [Authorize]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await this._dbContext.Vehicles.FirstOrDefaultAsync(c => c.ID == id);
            this.FillDropdownValues();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            if (ModelState.IsValid)
            {
            var vehicle = await this._dbContext.Vehicles.SingleAsync(c => c.ID == id);
            vehicle.UpdatedById = _userManager.GetUserName(base.User);
            vehicle.UpdateTime = DateTime.Now;
            var ok = await this.TryUpdateModelAsync(vehicle);
            var user = User;
            var result = await _vehicleService.EditVehicleAsync(id, user);
            if (result.Success)
            {
                    this._dbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
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
            var vehicles = this._dbContext.Manufacturers.Where(m => m.IsActive == true && !m.IsDeleted).ToList();

            foreach (var category in vehicles)
            {
                var userInfo = $"{category.Name}";
                listItem = new SelectListItem(userInfo, category.ID.ToString());
                selectItems.Add(listItem);
            }

            ViewBag.PossibleVehicles = selectItems;

        }
    }
}
