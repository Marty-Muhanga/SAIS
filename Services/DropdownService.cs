using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SAIS.Data;
using SAIS.Models;
using SAIS.ViewModels;

namespace SAIS.Services
{
    public class DropdownService : IDropdownService
    {
        private readonly SAISContext _context;
        private readonly IMemoryCache _cache;

        public DropdownService(SAISContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task PopulateDropdowns(object viewModel)
        {
            try
            {
                switch (viewModel)
                {
                    case ApplicantViewModel applicantVm:
                        applicantVm.SexOptions = await GetCachedLookup<SexLookup>("SexOptions");
                        applicantVm.MaritalStatusOptions = await GetCachedLookup<MaritalStatusLookup>("MaritalStatusOptions");
                        applicantVm.StatusOptions = await GetCachedLookup<ApplicationStatusLookup>("StatusOptions");
                        applicantVm.ProgramOptions = await GetCachedLookup<Program_>("ProgramOptions");
                        applicantVm.CountyOptions = await GetCachedLookup<County>("CountyOptions");

                        // For edit view, load the geographic hierarchy
                        if (applicantVm.ApplicantID > 0)
                        {
                            var applicant = await _context.Applicants
                                .Include(a => a.Village)
                                    .ThenInclude(v => v.SubLocation)
                                        .ThenInclude(sl => sl.Location)
                                            .ThenInclude(l => l.SubCounty)
                                                .ThenInclude(sc => sc.County)
                                .FirstOrDefaultAsync(a => a.ApplicantID == applicantVm.ApplicantID);

                            if (applicant?.Village != null)
                            {
                                applicantVm.CountyID = applicant.Village.SubLocation.Location.SubCounty.CountyID;
                                applicantVm.SubCountyID = applicant.Village.SubLocation.Location.SubCountyID;
                                applicantVm.LocationID = applicant.Village.SubLocation.LocationID;
                                applicantVm.SubLocationID = applicant.Village.SubLocationID;
                            }
                        }
                        break;

                    case SearchViewModel searchVm:
                        searchVm.Counties = await GetCachedLookup<County>("CountyOptions");
                        searchVm.Statuses = await GetCachedLookup<ApplicationStatusLookup>("StatusOptions");

                        // Only load these if we have the parent IDs
                        if (searchVm.CountyID.HasValue)
                        {
                            searchVm.SubCounties = await _context.SubCounties
                                .Where(sc => sc.CountyID == searchVm.CountyID.Value)
                                .OrderBy(sc => sc.SubCountyName)
                                .ToListAsync();
                        }
                        if (searchVm.SubCountyID.HasValue)
                        {
                            searchVm.Locations = await _context.Locations
                                .Where(l => l.SubCountyID == searchVm.SubCountyID.Value)
                                .OrderBy(l => l.LocationName)
                                .ToListAsync();
                        }
                        if (searchVm.LocationID.HasValue)
                        {
                            searchVm.SubLocations = await _context.SubLocations
                                .Where(sl => sl.LocationID == searchVm.LocationID.Value)
                                .OrderBy(sl => sl.SubLocationName)
                                .ToListAsync();
                        }
                        if (searchVm.SubLocationID.HasValue)
                        {
                            searchVm.Villages = await _context.Villages
                                .Where(v => v.SubLocationID == searchVm.SubLocationID.Value)
                                .OrderBy(v => v.VillageName)
                                .ToListAsync();
                        }
                        break;
                }
            }
            catch
            {
                // Initialize empty collections if population fails
                switch (viewModel)
                {
                    case ApplicantViewModel applicantVm:
                        applicantVm.SexOptions ??= new List<SexLookup>();
                        applicantVm.MaritalStatusOptions ??= new List<MaritalStatusLookup>();
                        applicantVm.StatusOptions ??= new List<ApplicationStatusLookup>();
                        applicantVm.ProgramOptions ??= new List<Program_>();
                        applicantVm.CountyOptions ??= new List<County>();
                        break;

                    case SearchViewModel searchVm:
                        searchVm.Counties ??= new List<County>();
                        searchVm.Statuses ??= new List<ApplicationStatusLookup>();
                        searchVm.SubCounties ??= new List<SubCounty>();
                        searchVm.Locations ??= new List<Location>();
                        searchVm.SubLocations ??= new List<SubLocation>();
                        searchVm.Villages ??= new List<Village>();
                        break;
                }
            }
        }

        private async Task<List<T>> GetCachedLookup<T>(string cacheKey) where T : class
        {
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
                return await _context.Set<T>().ToListAsync();
            });
        }
    }
}