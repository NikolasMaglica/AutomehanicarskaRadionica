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

            var offers = _dbContext.Offers.Include(o => o.OfferStatuses).Include(m => m.Clients).Include(a => a.AppUser).ToList();
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
            var orders = _dbContext.Offers.Include(o => o.OfferStatuses).Include(m => m.Clients).Include(a => a.AppUser).ToList();
            return View("Index", orders);
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
    }
}
