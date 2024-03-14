using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using Vjezba.Services;

namespace Vjezba.Web.Controllers
{
    public class ServiceController : Controller
    {
        private ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;
        private IServiceService _serviceService;
        public ServiceController(ClientManagerDbContext dbContext, IServiceService serviceService, UserManager<AppUser> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._serviceService = serviceService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var services = _dbContext.Services.Include(o => o.ServiceOffers).ToList();
            return View(services);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = User;
            var model =await _serviceService.DeleteServiceAsync(id, user);
            if (model.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", model.ErrorMessage);
            }
            var service = _dbContext.Services.Include(o => o.ServiceOffers).ToList();

            return View("Index", service);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Service model)
        {
            if (ModelState.IsValid)
            {
                var user = User;
                var result = await _serviceService.CreateServiceAsync(model, user);

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
            var model = await this._dbContext.Services.FirstOrDefaultAsync(c => c.ID == id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            if (ModelState.IsValid)
            {
                var service = this._dbContext.Services.Single(c => c.ID == id);
                var ok = await this.TryUpdateModelAsync(service);
                var user = User;
                var result =await  _serviceService.EditServiceAsync(id, user);
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
