using SAIS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAIS.Data.Repository
{
    public interface IApplicantRepository
    {
        Task<IEnumerable<Applicant>> GetApplicantsAsync();
        Task<Applicant> GetApplicantByIdAsync(int id);
        Task AddApplicantAsync(Applicant applicant);
        Task UpdateApplicantAsync(Applicant applicant);
        Task DeleteApplicantAsync(int id);
        Task<bool> ApplicantExistsAsync(int id);
        Task<IEnumerable<Applicant>> SearchApplicantsAsync(string searchTerm, int? statusId, DateTime? fromDate, DateTime? toDate, int? countyId, int? subCountyId, int? locationId, int? subLocationId, int? villageId);
        Task<int> CountApplicantsAsync();
    }
}