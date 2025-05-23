using SAIS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAIS.Models
{
    public class Applicant
    {
        [Key]
        public int ApplicantID { get; set; }

        [Required]
        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }

        [ForeignKey("Sex")]
        public int SexID { get; set; }
        public SexLookup? Sex { get; set; }

        public int Age { get; set; }

        [ForeignKey("MaritalStatus")]
        public int MaritalStatusID { get; set; }
        public MaritalStatusLookup? MaritalStatus { get; set; }

        [Required]
        [StringLength(20)]
        public string? IDNumber { get; set; }

        [StringLength(200)]
        public string? PostalAddress { get; set; }

        [Required]
        [StringLength(200)]
        public string? PhysicalAddress { get; set; }

        [Required]
        [StringLength(20)]
        public string? Telephone { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [ForeignKey("Village")]
        public int VillageID { get; set; }
        public Village? Village { get; set; }

        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [ForeignKey("Status")]
        public int StatusID { get; set; } = 1; // Default to Draft
        public ApplicationStatusLookup? Status { get; set; }

        public byte[]? SignatureImage { get; set; }

        [Required]
        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public ICollection<ApplicantProgram>? ApplicantPrograms { get; set; }

     
    }
}