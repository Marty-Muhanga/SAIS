using SAIS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAIS.Services
{
    public interface IApplicantService
    {
        Task<IEnumerable<Applicant>> GetApplicantsAsync(int page = 1, int pageSize = 2);
        Task<Applicant> GetApplicantByIdAsync(int id);
        Task AddApplicantAsync(Applicant applicant);
        Task UpdateApplicantAsync(Applicant applicant);
        Task DeleteApplicantAsync(int id);
        Task<bool> ApplicantExistsAsync(int id);
        Task<IEnumerable<Applicant>> SearchApplicantsAsync(
            string searchTerm, int? statusId, DateTime? fromDate, DateTime? toDate,
            int? countyId, int? subCountyId, int? locationId, int? subLocationId, int? villageId,
            int page = 1, int pageSize = 2);
        Task<int> CountApplicantsAsync();
        Task<int> CountSearchResultsAsync(
            string searchTerm, int? statusId, DateTime? fromDate, DateTime? toDate,
            int? countyId, int? subCountyId, int? locationId, int? subLocationId, int? villageId);
    }
}