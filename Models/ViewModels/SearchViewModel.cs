using SAIS.Models;
using System;
using System.Collections.Generic;

namespace SAIS.ViewModels
{
    public class SearchViewModel
    {
        public string? SearchTerm { get; set; }
        public int? StatusID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        // Geographic filters
        public int? CountyID { get; set; }
        public int? SubCountyID { get; set; }
        public int? LocationID { get; set; }
        public int? SubLocationID { get; set; }
        public int? VillageID { get; set; }

        // Pagination
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 2;
        public int TotalCount { get; set; }

        public IEnumerable<Applicant>? Applicants { get; set; }

        // For dropdowns
        public IEnumerable<County>? Counties { get; set; }
        public IEnumerable<SubCounty>? SubCounties { get; set; }
        public IEnumerable<Location>? Locations { get; set; }
        public IEnumerable<SubLocation>? SubLocations { get; set; }
        public IEnumerable<Village>? Villages { get; set; }
        public IEnumerable<ApplicationStatusLookup>? Statuses { get; set; }

        // Calculate total pages
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        // Helper method to check if any search criteria is set
        public bool HasSearchCriteria =>
            !string.IsNullOrEmpty(SearchTerm) ||
            StatusID.HasValue ||
            FromDate.HasValue ||
            ToDate.HasValue ||
            CountyID.HasValue ||
            SubCountyID.HasValue ||
            LocationID.HasValue ||
            SubLocationID.HasValue ||
            VillageID.HasValue;
    }
}