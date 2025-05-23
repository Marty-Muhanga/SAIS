using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAIS.Data;
using SAIS.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SAIS.Controllers
{
    public class LookupController : Controller
    {
        private readonly SAISContext _context;

        public LookupController(SAISContext context)
        {
            _context = context;
        }

        // GET: Lookup
        public async Task<IActionResult> Index()
        {
            var lookups = new LookupViewModel
            {
                SexLookup = await _context.SexLookups.ToListAsync(),
                MaritalStatusLookup = await _context.MaritalStatusLookups.ToListAsync(),
                ApplicationStatusLookup = await _context.ApplicationStatusLookups.ToListAsync(),
                Programs = await _context.Programs.ToListAsync()
            };
            return View(lookups);
        }

        // POST: Lookup/AddSex
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSex([Bind("SexName,SexCode")] SexLookup sexLookup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sexLookup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await GetLookupViewModel());
        }

        // POST: Lookup/AddMaritalStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMaritalStatus([Bind("StatusName,StatusCode")] MaritalStatusLookup maritalStatusLookup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maritalStatusLookup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await GetLookupViewModel());
        }

        // POST: Lookup/AddApplicationStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddApplicationStatus([Bind("StatusName,StatusCode")] ApplicationStatusLookup applicationStatusLookup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationStatusLookup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await GetLookupViewModel());
        }

        // POST: Lookup/AddProgram
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProgram([Bind("ProgramCode,ProgramName,Description")] Models.Program_ program)
        {
            if (ModelState.IsValid)
            {
                _context.Add(program);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await GetLookupViewModel());
        }

        private async Task<LookupViewModel> GetLookupViewModel()
        {
            return new LookupViewModel
            {
                SexLookup = await _context.SexLookups.ToListAsync(),
                MaritalStatusLookup = await _context.MaritalStatusLookups.ToListAsync(),
                ApplicationStatusLookup = await _context.ApplicationStatusLookups.ToListAsync(),
                Programs = await _context.Programs.ToListAsync()
            };
        }
    }

    public class LookupViewModel
    {
        public System.Collections.Generic.List<SexLookup> SexLookup { get; set; }
        public System.Collections.Generic.List<MaritalStatusLookup> MaritalStatusLookup { get; set; }
        public System.Collections.Generic.List<ApplicationStatusLookup> ApplicationStatusLookup { get; set; }
        public System.Collections.Generic.List<Models.Program_> Programs { get; set; }
    }
}