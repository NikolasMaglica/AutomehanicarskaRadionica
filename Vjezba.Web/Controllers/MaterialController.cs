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

        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var materials = _dbContext.Materials.Include(o => o.MaterialOffers).Include(m => m.OrderMaterials).ToList();
            return View(materials);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = User;
            var model =await _materialService.DeleteMaterialAsync(id, user);
            if (model.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", model.ErrorMessage);
            }
            var materials = _dbContext.Materials.Include(o => o.MaterialOffers).Include(m => m.OrderMaterials).ToList();
            return View("index",materials);
        }

        [Authorize]
        public async Task<IActionResult> Create()
		{
			return View();
		}

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Material model)
        {
            if (ModelState.IsValid)
            {
                var user = User;
                var result =await _materialService.CreateMaterialAsync(model, user);

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

        [Authorize]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await this._dbContext.Materials.FirstOrDefaultAsync(c => c.ID == id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            if (ModelState.IsValid)
            {
                var service =await this._dbContext.Materials.SingleAsync(c => c.ID == id);
                var ok = await this.TryUpdateModelAsync(service);
                var user = User;
                var result = await _materialService.EditMaterialAsync(id, user);
                if (result.Success && ok)
                {
                    await this._dbContext.SaveChangesAsync();
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
