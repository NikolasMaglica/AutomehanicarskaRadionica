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
    public class OfferController : Controller
    {
        private ClientManagerDbContext _dbContext;
        private UserManager<AppUser> _userManager;
        private IOfferService _offerService;
        public OfferController(ClientManagerDbContext dbContext, UserManager<AppUser> userManager, IOfferService offerService)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            _offerService = offerService;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {

            var offers = _dbContext.Offers.Include(o => o.OfferStatuses).Include(m => m.Clients).Include(a => a.AppUser).Include(w => w.UserVehicles).ToList();
            return View(offers);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            this.FillDropdownValuesStatus();
            this.FillDropdownValuesClient();
            this.FillDropdownValuesUser();
            this.FillDropdownValuesServices();
            this.FillDropdownValuesMaterial();
            this.FillDropdownValuesVehicle();
            Offer offer = new Offer();
            offer.MaterialOffers.Add(new MaterialOffer() { OfferId = 1 });
            offer.ServiceOffers.Add(new ServiceOffer() { OfferId = 1 });

            return View(offer);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Offer offer)
        {
            if (ModelState.IsValid)
            {
                var user = User;
                var result = await _offerService.CreateOfferAsync(offer, user); 
                if (result.Success)
                {
                    offer.TotalPrice = _offerService.CalculateTotalPrice(offer);
                    if (offer.OfferStatusId == 1)
                       await _offerService.UpdateMaterialStockAsync(offer);
                   await _dbContext.SaveChangesAsync();
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
        public async Task<IActionResult> Details(int Id)
        {
            Offer offer = await _dbContext.Offers.Include(e => e.MaterialOffers).Include(d=>d.ServiceOffers).Where(a => a.ID == Id).FirstOrDefaultAsync();
            this.FillDropdownValuesMaterial();
            this.FillDropdownValuesVehicle();
            this.FillDropdownValuesClient();
            this.FillDropdownValuesServices();
            this.FillDropdownValuesUser();
            this.FillDropdownValuesStatus();
            return View(offer);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = User;
            var model = await _offerService.DeleteOfferAsync(id, user);
            if (model.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", model.ErrorMessage);
            }
            var offers = await _dbContext.Offers.Include(o => o.OfferStatuses).Include(m => m.Clients).Include(a => a.AppUser).Include(w => w.UserVehicles).ToListAsync();
            return View("Index", offers);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            Offer offer =await _dbContext.Offers.Include(e => e.MaterialOffers).Include(a=>a.ServiceOffers).Where(a => a.ID == Id).FirstOrDefaultAsync();
            this.FillDropdownValuesMaterial();
            this.FillDropdownValuesVehicle();
            this.FillDropdownValuesClient();
            this.FillDropdownValuesServices();
            this.FillDropdownValuesUser();
            this.FillDropdownValuesStatus();
            return View(offer);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Offer offer)
        {
            var result = await _offerService.UpdateOfferAsync(offer);


            if (result.Success && ModelState.IsValid)
            {
                offer.TotalPrice = _offerService.CalculateTotalPrice(offer);
                if (offer.OfferStatusId == 1)
                    await _offerService.UpdateMaterialStockAsync(offer);
                else
                  await  _dbContext.SaveChangesAsync();
                return RedirectToAction("index");
            }
            else
            {
                ModelState.AddModelError("", result.ErrorMessage);
            }
            this.FillDropdownValuesMaterial();
            this.FillDropdownValuesVehicle();
            this.FillDropdownValuesClient();
            this.FillDropdownValuesServices();
            this.FillDropdownValuesUser();
            this.FillDropdownValuesStatus();
            return View(offer);
        }


        private void FillDropdownValuesStatus()
        {
            var selectItems = new List<SelectListItem>();
            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);

            foreach (var category in this._dbContext.OfferStatuses)
            {
                listItem = new SelectListItem(category.OfferStatusName, category.ID.ToString());
                selectItems.Add(listItem);
            }

            ViewBag.PossibleStatus = selectItems;

        }
        private void FillDropdownValuesClient()
        {
            var selectItems = new List<SelectListItem>();
            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);

            var clients = this._dbContext.Clients.Where(m => m.IsActive == true && !m.IsDeleted).ToList();

            foreach (var category in clients)
            {
                var clientInfo = $"{category.FirstName}  {category.LastName}";
                listItem = new SelectListItem(clientInfo, category.ID.ToString());
                selectItems.Add(listItem);
            }

            ViewBag.PossibleClients = selectItems;

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

            ViewBag.User = selectItems;

        }
        private void FillDropdownValuesMaterial()
        {
            var selectItems = new List<SelectListItem>();
            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);
            var material = this._dbContext.Materials.Where(m => m.IsActive == true && !m.IsDeleted).ToList();


            foreach (var category in material)
            {
                listItem = new SelectListItem(category.MaterialName, category.ID.ToString());
                selectItems.Add(listItem);
            }

            ViewBag.PossibleMaterials = selectItems;

        }
        private void FillDropdownValuesServices()
        {
            var selectItems = new List<SelectListItem>();
            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);
            var services = this._dbContext.Services.Where(m => m.IsActive == true && !m.IsDeleted).ToList();


            foreach (var category in services)
            {
                listItem = new SelectListItem(category.ServiceName, category.ID.ToString());
                selectItems.Add(listItem);
            }

            ViewBag.PossibleServices = selectItems;

        }
        private void FillDropdownValuesVehicle()
        {
            var selectItems = new List<SelectListItem>();
            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);

            var vehicles = this._dbContext.UserVehicles.Where(m => m.IsActive == true && !m.IsDeleted).ToList();

            foreach (var category in vehicles)
            {
                var serviceName = this._dbContext.Vehicles
                    .Where(m => m.ID == category.VehicleID)
                    .Select(m => m.ModelName)
                    .FirstOrDefault();

                var vehiclesInfo = $"{serviceName} - {category.KilometersTraveled}";
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
