
using SAIS.Data.Repository;
using SAIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIS.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly IApplicantRepository _repository;

        public ApplicantService(IApplicantRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Applicant>> GetApplicantsAsync(int page = 1, int pageSize = 2)
        {
            var allApplicants = await _repository.GetApplicantsAsync();
            return allApplicants.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<Applicant> GetApplicantByIdAsync(int id)
        {
            return await _repository.GetApplicantByIdAsync(id);
        }

        public async Task AddApplicantAsync(Applicant applicant)
        {
            await _repository.AddApplicantAsync(applicant);
        }

        public async Task UpdateApplicantAsync(Applicant applicant)
        {
            await _repository.UpdateApplicantAsync(applicant);
        }

        public async Task DeleteApplicantAsync(int id)
        {
            await _repository.DeleteApplicantAsync(id);
        }

        public async Task<bool> ApplicantExistsAsync(int id)
        {
            return await _repository.ApplicantExistsAsync(id);
        }

        public async Task<IEnumerable<Applicant>> SearchApplicantsAsync(
            string searchTerm, int? statusId, DateTime? fromDate, DateTime? toDate,
            int? countyId, int? subCountyId, int? locationId, int? subLocationId, int? villageId,
            int page = 1, int pageSize = 2)
        {
            var results = await _repository.SearchApplicantsAsync(
                searchTerm, statusId, fromDate, toDate,
                countyId, subCountyId, locationId, subLocationId, villageId);

            return results.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<int> CountApplicantsAsync()
        {
            return await _repository.CountApplicantsAsync();
        }

        public async Task<int> CountSearchResultsAsync(
            string searchTerm, int? statusId, DateTime? fromDate, DateTime? toDate,
            int? countyId, int? subCountyId, int? locationId, int? subLocationId, int? villageId)
        {
            var results = await _repository.SearchApplicantsAsync(
                searchTerm, statusId, fromDate, toDate,
                countyId, subCountyId, locationId, subLocationId, villageId);

            return results.Count();
        }
    }
}