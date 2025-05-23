using Microsoft.EntityFrameworkCore;
using SAIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIS.Data.Repository
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly SAISContext _context;

        public ApplicantRepository(SAISContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Applicant>> GetApplicantsAsync()
        {
            return await _context.Applicants
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
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();
        }

        public async Task<Applicant> GetApplicantByIdAsync(int id)
        {
            return await _context.Applicants
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
                .FirstOrDefaultAsync(a => a.ApplicantID == id);
        }

        public async Task AddApplicantAsync(Applicant applicant)
        {
            _context.Applicants.Add(applicant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateApplicantAsync(Applicant applicant)
        {
            _context.Entry(applicant).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteApplicantAsync(int id)
        {
            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant != null)
            {
                _context.Applicants.Remove(applicant);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ApplicantExistsAsync(int id)
        {
            return await _context.Applicants.AnyAsync(a => a.ApplicantID == id);
        }

        public async Task<IEnumerable<Applicant>> SearchApplicantsAsync(
            string searchTerm, int? statusId, DateTime? fromDate, DateTime? toDate,
            int? countyId, int? subCountyId, int? locationId, int? subLocationId, int? villageId)
        {
            var query = _context.Applicants
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
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a =>
                    a.FirstName.Contains(searchTerm) ||
                    a.MiddleName.Contains(searchTerm) ||
                    a.LastName.Contains(searchTerm) ||
                    a.IDNumber.Contains(searchTerm) ||
                    a.Telephone.Contains(searchTerm));
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

            return await query.OrderByDescending(a => a.ApplicationDate).ToListAsync();
        }

        public async Task<int> CountApplicantsAsync()
        {
            return await _context.Applicants.CountAsync();
        }
    }
}