using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAIS.Models;

using System.Linq;
using System.Threading.Tasks;
using SAIS.Data;

namespace SAIS.Controllers
{
    public class LocationsController : Controller
    {
        private readonly SAISContext _context;

        public LocationsController(SAISContext context)
        {
            _context = context;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {
            // First load counties with subcounties
            var counties = await _context.Counties
                .Include(c => c.SubCounties)
                .OrderBy(c => c.CountyName)
                .ToListAsync();

            // Then explicitly load locations for each subcounty
            foreach (var county in counties)
            {
                foreach (var subCounty in county.SubCounties)
                {
                    await _context.Entry(subCounty)
                        .Collection(sc => sc.Locations)
                        .Query()
                        .Include(l => l.SubLocations)
                            .ThenInclude(sl => sl.Villages)
                        .LoadAsync();
                }
            }

            return View(counties);
        }

        // POST: Locations/AddCounty
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCounty([Bind("CountyCode,CountyName")] County county)
        {
            if (ModelState.IsValid)
            {
                _context.Add(county);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await GetLocationData());
        }

        // POST: Locations/AddSubCounty
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubCounty([Bind("SubCountyCode,SubCountyName,CountyID")] SubCounty subCounty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subCounty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await GetLocationData());
        }

        // POST: Locations/AddLocation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLocation([Bind("LocationCode,LocationName,SubCountyID")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await GetLocationData());
        }

        // POST: Locations/AddSubLocation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubLocation([Bind("SubLocationCode,SubLocationName,LocationID")] SubLocation subLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await GetLocationData());
        }

        // POST: Locations/AddVillage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVillage([Bind("VillageCode,VillageName,SubLocationID")] Village village)
        {
            if (ModelState.IsValid)
            {
                _context.Add(village);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await GetLocationData());
        }

        private async Task<List<County>> GetLocationData()
        {
            return await _context.Counties
        .Include(c => c.SubCounties)
            .ThenInclude(sc => sc.Locations)
                .ThenInclude(l => l.SubLocations)
                    .ThenInclude(sl => sl.Villages)
        .OrderBy(c => c.CountyName)
        .ToListAsync();
        }

     
        [HttpGet]
        public async Task<JsonResult> GetSubCountiesByCounty(int countyId)
        {
            var subCounties = await _context.SubCounties
                .Where(sc => sc.CountyID == countyId)
                .OrderBy(sc => sc.SubCountyName)
                .Select(sc => new {
                    subCountyID = sc.SubCountyID,
                    subCountyName = sc.SubCountyName
                })
                .ToListAsync();
            return Json(subCounties);
        }

        [HttpGet]
        public async Task<JsonResult> GetLocationsBySubCounty(int subCountyId)
        {
            var locations = await _context.Locations
                .Where(l => l.SubCountyID == subCountyId)
                .OrderBy(l => l.LocationName)
                .Select(l => new {
                    locationID = l.LocationID,
                    locationName = l.LocationName
                })
                .ToListAsync();
            return Json(locations);
        }

        [HttpGet]
        public async Task<JsonResult> GetSubLocationsByLocation(int locationId)
        {
            var subLocations = await _context.SubLocations
                .Where(sl => sl.LocationID == locationId)
                .OrderBy(sl => sl.SubLocationName)
                .Select(sl => new {
                    subLocationID = sl.SubLocationID,
                    subLocationName = sl.SubLocationName
                })
                .ToListAsync();
            return Json(subLocations);
        }

        [HttpGet]
        public async Task<JsonResult> GetVillagesBySubLocation(int subLocationId)
        {
            var villages = await _context.Villages
                .Where(v => v.SubLocationID == subLocationId)
                .OrderBy(v => v.VillageName)
                .Select(v => new {
                    villageID = v.VillageID,
                    villageName = v.VillageName
                })
                .ToListAsync();
            return Json(villages);
        }
    }
}