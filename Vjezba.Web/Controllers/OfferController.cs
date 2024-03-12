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
        public IActionResult Index()
        {

            var offers = _dbContext.Offers.Include(o => o.OfferStatuses).Include(m => m.Clients).Include(a => a.AppUser).Include(w => w.UserVehicles).ToList();
            return View(offers);
        }
        [HttpGet]
        public IActionResult Create()
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
        [HttpPost]
        public IActionResult Create(Offer offer)
        {
            if (ModelState.IsValid)
            {
                var user = User;
                var result = _offerService.CreateOffer(offer, user); 
                if (result.Success)
                {
                    offer.TotalPrice = _offerService.CalculateTotalPrice(offer);
                    if (offer.OfferStatusId == 1)
                        _offerService.UpdateMaterialStock(offer);
                    _dbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                }
            }
            return View();
        }

        public IActionResult Details(int Id)
        {
            Offer offer = _dbContext.Offers.Include(e => e.MaterialOffers).Include(d=>d.ServiceOffers).Where(a => a.ID == Id).FirstOrDefault();
            this.FillDropdownValuesMaterial();
            this.FillDropdownValuesVehicle();
            this.FillDropdownValuesClient();
            this.FillDropdownValuesServices();
            this.FillDropdownValuesUser();
            this.FillDropdownValuesStatus();
            return View(offer);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = User;
            var model = _offerService.DeleteOffer(id, user);
            if (model.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", model.ErrorMessage);
            }
            var offers = _dbContext.Offers.Include(o => o.OfferStatuses).Include(m => m.Clients).Include(a => a.AppUser).Include(w => w.UserVehicles).ToList();
            return View("Index", offers);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Offer offer = _dbContext.Offers.Include(e => e.MaterialOffers).Where(a => a.ID == Id).FirstOrDefault();
            this.FillDropdownValuesMaterial();
            this.FillDropdownValuesVehicle();
            this.FillDropdownValuesClient();
            this.FillDropdownValuesServices();
            this.FillDropdownValuesUser();
            this.FillDropdownValuesStatus();
            return View(offer);
        }

        [HttpPost]
        public IActionResult Edit(Offer offer)
        {
            var result = _offerService.UpdateOffer(offer);

            if (result.Success && ModelState.IsValid)
            {
                offer.TotalPrice = _offerService.CalculateTotalPrice(offer);
                if (offer.OfferStatusId == 1)
                    _offerService.UpdateMaterialStock(offer);
                else
                    _dbContext.SaveChanges();
                return RedirectToAction("index");
            }
            else
            {
                ModelState.AddModelError("", result.ErrorMessage);
            }

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

            foreach (var category in this._dbContext.Clients)
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

            foreach (var category in this._dbContext.Materials)
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

            foreach (var category in this._dbContext.Services)
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

            var vehicles = this._dbContext.UserVehicles.Where(m => m.IsActive == true).ToList();

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
