using SAIS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAIS.ViewModels
{
    public class ApplicantViewModel
    {
        public int ApplicantID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Middle Name")]
        [StringLength(100)]
        public string MiddleName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Sex")]
        public int SexID { get; set; }

        [Required]
        [Range(0, 120)]
        public int Age { get; set; }

        [Required]
        [Display(Name = "Marital Status")]
        public int MaritalStatusID { get; set; }

        [Required]
        [Display(Name = "ID/Passport Number")]
        [StringLength(20)]
        public string IDNumber { get; set; } = string.Empty;

        [Display(Name = "Postal Address")]
        [StringLength(200)]
        public string PostalAddress { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Physical Address")]
        [StringLength(200)]
        public string PhysicalAddress { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Telephone")]
        [StringLength(20)]
        [Phone]
        public string Telephone { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Village is required")]
        [Display(Name = "Village")]
        public int VillageID { get; set; }

        [Required]
        [Display(Name = "Application Date")]
        [DataType(DataType.Date)]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Status")]
        public int StatusID { get; set; } = 1; // Default to Draft

        // For geographic hierarchy display
        public string VillageName { get; set; } = string.Empty;
        public string SubLocationName { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string SubCountyName { get; set; } = string.Empty;
        public string CountyName { get; set; } = string.Empty;

        // For geographic hierarchy selection
        public int? CountyID { get; set; }
        public int? SubCountyID { get; set; }
        public int? LocationID { get; set; }
        public int? SubLocationID { get; set; }


        // For program selection
        public List<int> SelectedProgramIds { get; set; } = new List<int>();

        // Navigation properties for dropdowns
        public IEnumerable<SexLookup> SexOptions { get; set; } = new List<SexLookup>();
        public IEnumerable<MaritalStatusLookup> MaritalStatusOptions { get; set; } = new List<MaritalStatusLookup>();
        public IEnumerable<ApplicationStatusLookup> StatusOptions { get; set; } = new List<ApplicationStatusLookup>();
        public IEnumerable<Program_> ProgramOptions { get; set; } = new List<Program_>();
        public IEnumerable<County> CountyOptions { get; set; } = new List<County>();
        public IEnumerable<SubCounty> SubCountyOptions { get; set; } = new List<SubCounty>();
        public IEnumerable<Location> LocationOptions { get; set; } = new List<Location>();
        public IEnumerable<SubLocation> SubLocationOptions { get; set; } = new List<SubLocation>();
        public IEnumerable<Village> VillageOptions { get; set; } = new List<Village>();

        // Navigation property for Village (needed for Edit view)
        public Village? Village { get; set; }

        // Navigation property for ApplicantPrograms
        public ICollection<ApplicantProgram> ApplicantPrograms { get; set; } = new List<ApplicantProgram>();

        // Helper method to convert to domain model
        public Applicant ToApplicant()
        {
            return new Applicant
            {
                ApplicantID = this.ApplicantID,
                FirstName = this.FirstName,
                MiddleName = this.MiddleName,
                LastName = this.LastName,
                SexID = this.SexID,
                Age = this.Age,
                MaritalStatusID = this.MaritalStatusID,
                IDNumber = this.IDNumber,
                PostalAddress = this.PostalAddress,
                PhysicalAddress = this.PhysicalAddress,
                Telephone = this.Telephone,
                Email = this.Email,
                VillageID = this.VillageID,
                ApplicationDate = this.ApplicationDate,
                StatusID = this.StatusID,
                ApplicantPrograms = this.SelectedProgramIds?.Select(pid => new ApplicantProgram
                {
                    ProgramID = pid,
                    StatusID = this.StatusID,
                   
                }).ToList() ?? new List<ApplicantProgram>()
            };
        }


        // Updated Applicant
        public void UpdateApplicant(Applicant applicant)
        {
            applicant.FirstName = this.FirstName;
            applicant.MiddleName = this.MiddleName;
            applicant.LastName = this.LastName;
            applicant.SexID = this.SexID;
            applicant.Age = this.Age;
            applicant.MaritalStatusID = this.MaritalStatusID;
            applicant.IDNumber = this.IDNumber;
            applicant.PostalAddress = this.PostalAddress;
            applicant.PhysicalAddress = this.PhysicalAddress;
            applicant.Telephone = this.Telephone;
            applicant.Email = this.Email;
            applicant.VillageID = this.VillageID;
            applicant.ApplicationDate = this.ApplicationDate;
            applicant.StatusID = this.StatusID;
            
        }

        // Helper method to create from domain model
        public static ApplicantViewModel FromApplicant(Applicant applicant)
        {
            if (applicant == null) return new ApplicantViewModel();

            return new ApplicantViewModel
            {
                ApplicantID = applicant.ApplicantID,
                FirstName = applicant.FirstName ?? string.Empty,
                MiddleName = applicant.MiddleName ?? string.Empty,
                LastName = applicant.LastName ?? string.Empty,
                SexID = applicant.SexID,
                Age = applicant.Age,
                MaritalStatusID = applicant.MaritalStatusID,
                IDNumber = applicant.IDNumber ?? string.Empty,
                PostalAddress = applicant.PostalAddress ?? string.Empty,
                PhysicalAddress = applicant.PhysicalAddress ?? string.Empty,
                Telephone = applicant.Telephone ?? string.Empty,
                Email = applicant.Email ?? string.Empty,
                VillageID = applicant.VillageID,
                ApplicationDate = applicant.ApplicationDate,
                StatusID = applicant.StatusID,
                SelectedProgramIds = applicant.ApplicantPrograms?.Select(ap => ap.ProgramID).ToList() ?? new List<int>(),
                CountyID = applicant.Village?.SubLocation?.Location?.SubCounty?.CountyID,
                SubCountyID = applicant.Village?.SubLocation?.Location?.SubCountyID,
                LocationID = applicant.Village?.SubLocation?.LocationID,
                SubLocationID = applicant.Village?.SubLocationID,
                Village = applicant.Village,
                VillageName = applicant.Village?.VillageName ?? string.Empty,
                SubLocationName = applicant.Village?.SubLocation?.SubLocationName ?? string.Empty,
                LocationName = applicant.Village?.SubLocation?.Location?.LocationName ?? string.Empty,
                SubCountyName = applicant.Village?.SubLocation?.Location?.SubCounty?.SubCountyName ?? string.Empty,
                CountyName = applicant.Village?.SubLocation?.Location?.SubCounty?.County?.CountyName ?? string.Empty,
                ApplicantPrograms = applicant.ApplicantPrograms ?? new List<ApplicantProgram>()
            };
        }
    }
}