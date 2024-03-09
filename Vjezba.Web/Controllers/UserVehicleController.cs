using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Web.Controllers
{
	public class UserVehicleController : Controller
	{
		private ClientManagerDbContext _dbContext;
		private UserManager<AppUser> _userManager;

		public UserVehicleController(ClientManagerDbContext dbContext, UserManager<AppUser> userManager)
		{
			this._dbContext = dbContext;
			this._userManager = userManager;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Create()
		{
			this.FillDropdownValues();
			this.FillDropdownValuesUser();
			return View();
		}

		private void FillDropdownValues()
		{
			var selectItems = new List<SelectListItem>();
			var listItem = new SelectListItem();
			listItem.Text = "- odaberite -";
			listItem.Value = "";
			selectItems.Add(listItem);

			// Materijaliziraj rezultate upita korištenjem ToList()
			var vehicles = this._dbContext.Vehicles.Where(m=>m.IsActive==true).ToList();

			foreach (var category in vehicles)
			{
				// Ovdje možete izravno koristiti varijablu 'category' umjesto ponovnog upita
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

			// Postavite ViewBag izvan petlje
			ViewBag.PossibleVehicles = selectItems;		
		}
      
        [HttpPost]
		public IActionResult Create(UserVehicle model)
		{
			if (ModelState.IsValid)
			{
				model.CreatedById = _userManager.GetUserName(base.User);
				model.CreateTime = DateTime.Now;
				this._dbContext.UserVehicles.Add(model);
				this._dbContext.SaveChanges();

				return RedirectToAction(nameof(Index));
			}
			else
			{
				this.FillDropdownValues();
				this.FillDropdownValuesUser();
				return View();
			}
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

	}

}
