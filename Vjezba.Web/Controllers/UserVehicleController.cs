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
	public class UserVehicleController : Controller
	{
		private ClientManagerDbContext _dbContext;
		private UserManager<AppUser> _userManager;
        private IUserVehicleService _uservehicleService;

        public UserVehicleController(ClientManagerDbContext dbContext, IUserVehicleService userVehicleService, UserManager<AppUser> userManager)
		{
			this._dbContext = dbContext;
			this._userManager = userManager;
			this._uservehicleService = userVehicleService;
		}

        [Authorize]
        public async Task<IActionResult> Index()
		{
            var userv = await _dbContext.UserVehicles.Include(o => o.Vehicle).Include(o => o.AppUser).ToListAsync();
            return View(userv);
        }

        [Authorize]
        public IActionResult Create()
		{
			this.FillDropdownValues();
			this.FillDropdownValuesUser();
			return View();
		}

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = User;
            var model = await _uservehicleService.DeleteUserVehicleAsync(id, user);
            if (model.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", model.ErrorMessage);
            }

            var userv = await _dbContext.UserVehicles.Include(o => o.Vehicle).Include(o => o.AppUser).ToListAsync();
            return View("index",userv);
        }

        [Authorize]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await this._dbContext.UserVehicles.FirstOrDefaultAsync(c => c.ID == id);
            this.FillDropdownValues();
            this.FillDropdownValuesUser();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            if (ModelState.IsValid)
            {
                var vehicle = this._dbContext.UserVehicles.Single(c => c.ID == id);
                vehicle.UpdatedById = _userManager.GetUserName(base.User);
                vehicle.UpdateTime = DateTime.Now;
                var ok = await this.TryUpdateModelAsync(vehicle);
                var user = User;
                var result = await _uservehicleService.EditUserVehicleAsync(id, user);
                if (result.Success)
                {
                   await this._dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                }
            }

            this.FillDropdownValues();
            this.FillDropdownValuesUser();
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(UserVehicle model)
        {
            if (ModelState.IsValid)
            {
                var user = User;
                var result = await _uservehicleService.CreateUserVehicleAsync(model, user);
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
                    this.FillDropdownValuesUser();
                    return View();           
            
        }

		private void FillDropdownValuesUser()
		{
			var selectItems = new List<SelectListItem>();
			var listItem = new SelectListItem();
			listItem.Text = "- odaberite -";
			listItem.Value = "";
			selectItems.Add(listItem);

			foreach (var category in this._dbContext.Users)
			{
				var userInfo = $"{category.UserName}";
				listItem = new SelectListItem(userInfo, category.Id);
				selectItems.Add(listItem);
			}

			ViewBag.Users = selectItems;

		}
        private void FillDropdownValues()
        {
            var selectItems = new List<SelectListItem>();
            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);

            var vehicles = this._dbContext.Vehicles.Where(m => m.IsActive == true).ToList();

            foreach (var category in vehicles)
            {
                var manufacturerName = this._dbContext.Manufacturers
                    .Where(m => m.ID == category.ManufacturerID)
                    .Select(m => m.Name)
                    .FirstOrDefault();

                var vehiclesInfo = $"{manufacturerName} - {category.ModelName},{category.ModelYear}";
                if (category.IsDeleted == false && category.IsActive == true)
                {
                    listItem = new SelectListItem(vehiclesInfo, category.ID.ToString());
                    selectItems.Add(listItem);
                }
            }

            ViewBag.PossibleVehicles = selectItems;
        }

    }

}
