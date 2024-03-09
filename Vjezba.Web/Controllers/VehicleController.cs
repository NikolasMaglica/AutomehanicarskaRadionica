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

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

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
        /*	public ActionResult Delete(int? id)
			{
				if (id == null)
				{
					 Console.WriteLine("Nema id");
				}
				var launchEntry = _dbContext.Vehicles.Find(id);
				if (launchEntry == null)
				{
					Console.WriteLine("Nema id-a nije pronađen");
				}
				return PartialView("_DeleteModal", launchEntry);
			}
			// POST: Launch/Delete/5
			[HttpPost, ActionName("Delete")]
			[ValidateAntiForgeryToken]
			public ActionResult DeleteConfirmed(int id)
			{
				var launchEntry = _dbContext.Vehicles.Find(id);
				launchEntry.IsDeleted = true;
				_dbContext.SaveChanges();
				return RedirectToAction("Index");
			}
		*/
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = User;
            var model = _vehicleService.DeleteVehicle(id,user);
            if (model.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", model.ErrorMessage);
            }
            var vehicles = _dbContext.Vehicles.Where(v => v.IsActive && v.IsDeleted).ToList();
            return View("Index", vehicles);
        }
        public IActionResult Create()
        {
            this.FillDropdownValues();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Vehicle model)
        {
            if (ModelState.IsValid)
            {
                string createdById = _userManager.GetUserName(base.User);
                var result = _vehicleService.CreateVehicle(model, createdById);

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


        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            var model = this._dbContext.Vehicles.FirstOrDefault(c => c.ID == id);
            this.FillDropdownValues();
            return View(model);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            if (ModelState.IsValid)
            {
            var vehicle = this._dbContext.Vehicles.Single(c => c.ID == id);
            vehicle.UpdatedById = _userManager.GetUserName(base.User);
            vehicle.UpdateTime = DateTime.Now;
            var ok = await this.TryUpdateModelAsync(vehicle);
            var user = User;
            var result = _vehicleService.EditVehicle(id, user);
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
