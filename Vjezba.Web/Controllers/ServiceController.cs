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

        [AllowAnonymous]
        public ActionResult Index()
        {
            var services = _dbContext.Services.Include(o => o.ServiceOffers).ToList();
            return View(services);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = User;
            var model = _serviceService.DeleteService(id, user);
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Service model)
        {
            if (ModelState.IsValid)
            {
                var user = User;
                var result = _serviceService.CreateService(model, user);

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
            var model = this._dbContext.Services.FirstOrDefault(c => c.ID == id);
            return View(model);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            if (ModelState.IsValid)
            {
                var service = this._dbContext.Services.Single(c => c.ID == id);
                var ok = await this.TryUpdateModelAsync(service);
                var user = User;
                var result = _serviceService.EditService(id, user);
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
