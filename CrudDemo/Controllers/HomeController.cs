using CrudDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;

namespace CrudDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var countryModel = _context.Country.ToList();
            return View(countryModel);
        }

        public async Task<IActionResult> StateIndex()
        {
            List<State>states=new List<State>();
            var state = _context.States.ToList();
            foreach(var item in state)
            {
                State state1 = new State();
                state1.CountryId = item.CountryId;
                state1.Id = item.Id;
                state1.statename = await _context.Country.Where(x => x.Id.Equals(state1.CountryId)).Select(x => x.CountryName).FirstOrDefaultAsync();
                state1.Name = item.Name;
                states.Add(state1 );    

            }
            return View(states);
        }
        public IActionResult Create(int? id)
        {
            CountryModel countryModel = new CountryModel();
            if (id > 0)
            {
                var country = _context.Country.Where(x => x.Id == id).FirstOrDefault();
                countryModel.Id = country.Id;
                countryModel.CountryName = country.CountryName;

            }
            return View(countryModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(int Id, string Name)
        {
            CountryModel countryModel = new CountryModel();

            if (ModelState.IsValid)
            {
                if (Id > 0)
                {
                    var country = _context.Country.Where(x => x.Id == Id).FirstOrDefault();

                    country.Id = Id;
                    country.CountryName = Name;
                    _context.Country.Update(country);
                    await _context.SaveChangesAsync();


                }
                else
                {
                    countryModel.CountryName = Name;
                    _context.Country.Add(countryModel);
                    await _context.SaveChangesAsync();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(countryModel);
        }
        public IActionResult CreateState(int id,int CountryId)
        {
            State state = new State();
            if (id > 0)
            {
                var states = _context.States.Where(x => x.Id == id).FirstOrDefault();
                state.Id = states.Id;
                state.Name = states.Name;

                state.Countries = _context.Country.Select(x => new SelectListItem
                {
                    Text = x.CountryName,
                    Value = x.CountryName.ToLower().Replace(" ", "-").ToString(),
                    Selected = (CountryId == x.Id)
                }).OrderBy(x => x.Text).ToList();
            }
            else
            {
                state.Countries = _context.Country.Select(x => new SelectListItem
                {

                    Text = x.CountryName,
                    Value = x.CountryName.ToLower().Replace(" ", "-").ToString()
                }).OrderBy(x => x.Text).ToList();
            }

            return View(state);

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {


            var associatedStates = _context.States.Where(s => s.CountryId == id).ToList();

            if (associatedStates.Any())
            {
                return Content("parentexisted");
            }

            else
            {
                var country = await _context.Country.FindAsync(id);

                await _context.SaveChangesAsync();

                _context.Country.Remove(country);
                await _context.SaveChangesAsync();
                return Content("success");

            }




            return RedirectToAction(nameof(Index));

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteState(int id)
        {
            var States = _context.States.Where(s => s.Id == id).FirstOrDefault();
             _context.States.Remove(States);
              await _context.SaveChangesAsync();
            return RedirectToAction(nameof(StateIndex));

        }

        [HttpPost]
       
        public async Task<IActionResult> CreateState(int Id, string Name,string Country)
        {
            State state =new State();   
            if (ModelState.IsValid)
            {
                if (Id > 0)
                {
                    var states = _context.States.Where(x => x.Id == Id).FirstOrDefault();
                    states.Id= Id;
                    states.Name = Name;
                    state.CountryId = await _context.Country.Where(x => x.CountryName.Equals(Country)).Select(x => x.Id).FirstOrDefaultAsync();
                    states.CountryId = state.CountryId;
                    _context.States.Update(states);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    state.CountryId = await _context.Country.Where(x => x.CountryName.Equals(Country)).Select(x => x.Id).FirstOrDefaultAsync();
                    state.Name = Name;
                    _context.States.Add(state);

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("StateIndex");
            }

            return RedirectToAction("StateIndex");
        }

        public IActionResult CreateEmployee()
        {
            State state = new State();
            state.Countries = _context.Country.Select(x => new SelectListItem
            {

                Text = x.CountryName,
                Value = x.CountryName.ToLower().Replace(" ", "-").ToString()
            }).OrderBy(x => x.Text).ToList();
            return View(state);

        }



        [HttpPost]

        public async Task<IActionResult> CreateEmployee(string Name, string Country)
        {
            State state = new State();
            if (ModelState.IsValid)
            {
                state.CountryId = await _context.Country.Where(x => x.CountryName.Equals(Country)).Select(x => x.Id).FirstOrDefaultAsync();
                state.Name = Name;
                _context.States.Add(state);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}