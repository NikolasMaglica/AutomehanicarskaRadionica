using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using Vjezba.Services;

namespace Vjezba.Web.Controllers
{
    public class MaterialController : Controller
    {
        private ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;
        private IMaterialService _materialService;
        public MaterialController(ClientManagerDbContext dbContext, IMaterialService materialService, UserManager<AppUser> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._materialService = materialService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var materials = _dbContext.Materials.Include(o => o.MaterialOffers).Include(m => m.OrderMaterials).ToList();
            return View(materials);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = User;
            var model = _materialService.DeleteMaterial(id, user);
            if (model.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", model.ErrorMessage);
            }
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
        public IActionResult Create(Material model)
        {
            if (ModelState.IsValid)
            {
                var user = User;
                var result = _materialService.CreateMaterial(model, user);

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
            var model = this._dbContext.Materials.FirstOrDefault(c => c.ID == id);
            return View(model);
        }
        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            if (ModelState.IsValid)
            {
                var service = this._dbContext.Materials.Single(c => c.ID == id);
                var ok = await this.TryUpdateModelAsync(service);
                var user = User;
                var result = _materialService.EditMaterial(id, user);
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
