using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text.log;
using iTextSharp.xmp.impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Cms;
using SAIS.Data;
using SAIS.Models;
using SAIS.Services;
using SAIS.ViewModels;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace SAIS.Controllers
{
    public class ApplicantsController : Controller
    {
        private readonly SAISContext _context;
        private readonly IDropdownService _dropdownService;
        private readonly ILogger _logger;

        private readonly ISmsService _smsService;

        public ApplicantsController(SAISContext context, IDropdownService dropdownService, ISmsService smsService)
        {
            _context = context;
            _dropdownService = dropdownService;
            _smsService = smsService;
        }

        // GET: Applicants
        public async Task<IActionResult> Index(string searchTerm, int? statusId, DateTime? fromDate, DateTime? toDate,
    int? countyId, int? subCountyId, int? locationId, int? subLocationId, int? villageId, int page = 1)
        {
            try
            {
                var viewModel = new SearchViewModel
                {
                    CountyID = countyId,
                    SubCountyID = subCountyId,
                    LocationID = locationId,
                    SubLocationID = subLocationId,
                    VillageID = villageId
                };

                // Populate dropdowns
                await _dropdownService.PopulateDropdowns(viewModel);

                // Build query
                var query = _context.Applicants.AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(a => a.FirstName.Contains(searchTerm) ||
                                           a.LastName.Contains(searchTerm) ||
                                           a.IDNumber.Contains(searchTerm));
                }

                if (statusId.HasValue)
                {
                    query = query.Where(a => a.StatusID == statusId.Value);
                }

                if (fromDate.HasValue)
                {
                    query = query.Where(a => a.ApplicationDate >= fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    query = query.Where(a => a.ApplicationDate <= toDate.Value);
                }

                if (villageId.HasValue)
                {
                    query = query.Where(a => a.VillageID == villageId.Value);
                }
                else if (subLocationId.HasValue)
                {
                    query = query.Where(a => a.Village.SubLocationID == subLocationId.Value);
                }
                else if (locationId.HasValue)
                {
                    query = query.Where(a => a.Village.SubLocation.LocationID == locationId.Value);
                }
                else if (subCountyId.HasValue)
                {
                    query = query.Where(a => a.Village.SubLocation.Location.SubCountyID == subCountyId.Value);
                }
                else if (countyId.HasValue)
                {
                    query = query.Where(a => a.Village.SubLocation.Location.SubCounty.CountyID == countyId.Value);
                }

                // Pagination
                int pageSize = 10;
                var totalRecords = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                viewModel.Applicants = await query
             .Include(a => a.Sex)
             .Include(a => a.MaritalStatus)
             .Include(a => a.Status)
             .Include(a => a.Village)
                 .ThenInclude(v => v.SubLocation)
                     .ThenInclude(sl => sl.Location)
                         .ThenInclude(l => l.SubCounty)
                             .ThenInclude(sc => sc.County)
             .Include(a => a.ApplicantPrograms)
                 .ThenInclude(ap => ap.Program)
             .OrderBy(a => a.LastName) 
             .ThenBy(a => a.FirstName)  
             .Skip((page - 1) * pageSize)
             .Take(pageSize)
             .ToListAsync();

                // Set pagination data
                viewModel.CurrentPage = page;
                viewModel.PageSize = totalPages;
                viewModel.TotalCount = totalRecords;


                // Preserve filter values
                viewModel.SearchTerm = searchTerm;
                viewModel.StatusID = statusId;
                viewModel.FromDate = fromDate;
                viewModel.ToDate = toDate;

                await SetCascadingDropdownViewData(countyId, subCountyId, locationId, subLocationId);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                var errorViewModel = new SearchViewModel { Applicants = new List<Applicant>() };
                await _dropdownService.PopulateDropdowns(errorViewModel);
                return View(errorViewModel);
            }
        }

        // GET: Applicants/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var applicant = await _context.Applicants
                .Include(a => a.Sex)
                .Include(a => a.MaritalStatus)
                .Include(a => a.Village)
                    .ThenInclude(v => v.SubLocation)
                    .ThenInclude(sl => sl.Location)
                    .ThenInclude(l => l.SubCounty)
                    .ThenInclude(sc => sc.County)
                .Include(a => a.Status)
                .Include(a => a.ApplicantPrograms)
                    .ThenInclude(ap => ap.Program)
                .FirstOrDefaultAsync(m => m.ApplicantID == id);

            if (applicant == null) return NotFound();

            return View(applicant);
        }

        // GET: Applicants/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new ApplicantViewModel
            {
                ApplicationDate = DateTime.Now
            };

            // Populate all dropdowns for ApplicantViewModel
            await _dropdownService.PopulateDropdowns(viewModel);
            // Also populate ViewData for cascading dropdowns
            //await _dropdownService.PopulateViewDataDropdowns(ViewData);

            return View(viewModel);
        }

        // POST: Applicants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] ApplicantViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var applicant = viewModel.ToApplicant();
                    applicant.CreatedBy = User.Identity?.Name ?? "System";
                    applicant.CreatedDate = DateTime.Now;

                    // Generate a reference number
                    var referenceNumber = GenerateReferenceNumber();

                    _context.Add(applicant);
                    await _context.SaveChangesAsync();

                    // Handle selected programs
                    if (viewModel.SelectedProgramIds != null && viewModel.SelectedProgramIds.Any())
                    {
                        foreach (var programId in viewModel.SelectedProgramIds)
                        {
                            _context.ApplicantPrograms.Add(new ApplicantProgram
                            {
                                ApplicantID = applicant.ApplicantID,
                                ProgramID = programId,
                                StatusID = viewModel.StatusID
                            });
                        }
                        await _context.SaveChangesAsync();
                    }

                    // Send SMS notification
                    if (!string.IsNullOrEmpty(applicant.Telephone))
                    {
                        var fullName = $"{applicant.FirstName} {applicant.LastName}";
                        await _smsService.SendApplicationSubmittedSms(
                            applicant.Telephone,
                            fullName,
                            referenceNumber);
                    }

                    // Return JSON response for AJAX
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                        Request.ContentType?.Contains("application/json") == true)
                    {
                        return Json(new { success = true, redirectUrl = Url.Action(nameof(Index)) });
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Return validation errors as JSON
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                        Request.ContentType?.Contains("application/json") == true)
                    {
                        var errors = ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                            .ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                            );

                        return Json(new { success = false, errors = errors });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger?.LogError(ex, "Error creating applicant");

                // Return error response for AJAX
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                    Request.ContentType?.Contains("application/json") == true)
                {
                    return Json(new { success = false, message = "An error occurred while saving the applicant. Please try again." });
                }

                ModelState.AddModelError("", "An error occurred while saving the applicant. Please try again.");
            }

            // Repopulate dropdowns on validation error (for non-AJAX requests)
            await _dropdownService.PopulateDropdowns(viewModel);
            return View(viewModel);
        }

        // GET: Applicants/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var applicant = await _context.Applicants
                .Include(a => a.ApplicantPrograms)
                .FirstOrDefaultAsync(a => a.ApplicantID == id);

            if (applicant == null) return NotFound();

            var viewModel = ApplicantViewModel.FromApplicant(applicant);

            // Populate all dropdowns
            await _dropdownService.PopulateDropdowns(viewModel);
            //await _dropdownService.PopulateViewDataDropdowns(ViewData);

            return View(viewModel);
        }

        // POST: Applicants/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ApplicantViewModel viewModel)
        {
            if (id != viewModel.ApplicantID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing applicant to compare changes
                    var existingApplicant = await _context.Applicants
                        .Include(a => a.Status)
                        .Include(a => a.ApplicantPrograms)
                            .ThenInclude(ap => ap.Program)
                        .FirstOrDefaultAsync(a => a.ApplicantID == id);

                    if (existingApplicant == null) return NotFound();

                    // Check if status changed to "Approved"
                    var statusChangedToApproved = viewModel.StatusID != existingApplicant.StatusID &&
                                                _context.ApplicationStatusLookups
                                                    .Any(s => s.StatusID == viewModel.StatusID &&
                                                           s.StatusName.Contains("Approved"));

                    // Check if any important details changed
                    var detailsChanged = viewModel.FirstName != existingApplicant.FirstName ||
                                        viewModel.LastName != existingApplicant.LastName ||
                                        viewModel.Telephone != existingApplicant.Telephone ||
                                        viewModel.IDNumber != existingApplicant.IDNumber;

                    // Update the existing entity instead of creating a new one
                    viewModel.UpdateApplicant(existingApplicant);
                    existingApplicant.ModifiedBy = User.Identity.Name;
                    existingApplicant.ModifiedDate = DateTime.Now;

                    // Update applicant programs
                    var existingPrograms = await _context.ApplicantPrograms
                        .Where(ap => ap.ApplicantID == id)
                        .ToListAsync();

                    // Remove programs no longer selected
                    var programsToRemove = existingPrograms
                        .Where(ep => !viewModel.SelectedProgramIds.Contains(ep.ProgramID))
                        .ToList();

                    _context.ApplicantPrograms.RemoveRange(programsToRemove);

                    // Add new programs
                    var existingProgramIds = existingPrograms.Select(ep => ep.ProgramID).ToList();
                    var programsToAdd = viewModel.SelectedProgramIds
                        .Where(pid => !existingProgramIds.Contains(pid))
                        .Select(pid => new ApplicantProgram
                        {
                            ApplicantID = id,
                            ProgramID = pid,
                            StatusID = viewModel.StatusID
                        });

                    _context.ApplicantPrograms.AddRange(programsToAdd);

                    // No need to call Update since we're modifying the tracked entity
                    await _context.SaveChangesAsync();

                    // Send appropriate SMS notifications
                    if (!string.IsNullOrEmpty(viewModel.Telephone))
                    {
                        var fullName = $"{viewModel.FirstName} {viewModel.LastName}";
                        var referenceNumber = GenerateReferenceNumber();
                        var statusName = await _context.ApplicationStatusLookups
                            .Where(s => s.StatusID == viewModel.StatusID)
                            .Select(s => s.StatusName)
                            .FirstOrDefaultAsync();

                        if (statusChangedToApproved)
                        {
                            var programNames = string.Join(", ",
                                await _context.ApplicantPrograms
                                    .Where(ap => ap.ApplicantID == id)
                                    .Select(ap => ap.Program.ProgramName)
                                    .ToListAsync());

                            await _smsService.SendApplicationApprovedSms(
                                viewModel.Telephone,
                                fullName,
                                programNames,
                                referenceNumber);
                        }
                        else if (detailsChanged)
                        {
                            await _smsService.SendApplicationUpdatedSms(
                                viewModel.Telephone,
                                fullName,
                                referenceNumber);
                        }
                        else if (viewModel.StatusID != existingApplicant.StatusID)
                        {
                            await _smsService.SendStatusChangedSms(
                                viewModel.Telephone,
                                fullName,
                                statusName,
                                referenceNumber);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ApplicantExists(viewModel.ApplicantID))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            // Repopulate dropdowns on validation error
            await _dropdownService.PopulateDropdowns(viewModel);
            return View(viewModel);
        }


        private async Task<bool> ApplicantExists(int id)
        {
            return await _context.Applicants.AnyAsync(e => e.ApplicantID == id);
        }

        private async Task SetCascadingDropdownViewData(int? countyId, int? subCountyId, int? locationId, int? subLocationId)
        {
            if (subCountyId.HasValue)
            {
                var subCounty = await _context.SubCounties.FindAsync(subCountyId.Value);
                ViewData["SubCountyName"] = subCounty?.SubCountyName;
            }

            if (locationId.HasValue)
            {
                var location = await _context.Locations.FindAsync(locationId.Value);
                ViewData["LocationName"] = location?.LocationName;
            }

            if (subLocationId.HasValue)
            {
                var subLocation = await _context.SubLocations.FindAsync(subLocationId.Value);
                ViewData["SubLocationName"] = subLocation?.SubLocationName;
            }
        }


        public async Task<JsonResult> GetLocationsBySubCounty(int subCountyId)
        {
            var locations = await _context.Locations
                .Where(l => l.SubCountyID == subCountyId)
                .OrderBy(l => l.LocationName)
                .ToListAsync();
            return Json(locations);
        }

        public async Task<JsonResult> GetSubLocationsByLocation(int locationId)
        {
            var subLocations = await _context.SubLocations
                .Where(sl => sl.LocationID == locationId)
                .OrderBy(sl => sl.SubLocationName)
                .ToListAsync();
            return Json(subLocations);
        }

        public async Task<JsonResult> GetVillagesBySubLocation(int subLocationId)
        {
            var villages = await _context.Villages
                .Where(v => v.SubLocationID == subLocationId)
                .OrderBy(v => v.VillageName)
                .ToListAsync();
            return Json(villages);
        }

        // For SMS
        private string GenerateReferenceNumber()
        {
            return $"SA{DateTime.Now:yyMMddHHmmss}";
        }

    }


}