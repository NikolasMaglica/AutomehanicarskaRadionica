using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vjezba.DAL;
using Vjezba.Model;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Vjezba.Web.Controllers
{
    public class ClientController : Controller
    {
        private ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;

        public ClientController(ClientManagerDbContext dbContext, UserManager<AppUser> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(this._dbContext.Clients.ToList());
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Client model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedById = _userManager.GetUserId(base.User);
                this._dbContext.Clients.Add(model);
                this._dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            var model = this._dbContext.Clients.FirstOrDefault(c => c.ID == id);
            return View(model);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            if (ModelState.IsValid)
            {
                var client = this._dbContext.Clients.Single(c => c.ID == id);
                var ok = await this.TryUpdateModelAsync(client);

                if (ok && this.ModelState.IsValid)
                {
                    client.UpdatedById = _userManager.GetUserId(base.User);
                    this._dbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
    }
}
