using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAIS.Models
{
    public class ApplicantProgram
    {
        [Key]
        public int ApplicantProgramID { get; set; }

        [ForeignKey("Applicant")]
        public int ApplicantID { get; set; }
        public Applicant? Applicant { get; set; }

        [ForeignKey("Program")]
        public int ProgramID { get; set; }
        public Program_? Program { get; set; }

        [ForeignKey("Status")]
        public int StatusID { get; set; }
        public ApplicationStatusLookup? Status { get; set; }

        public DateTime? ApprovalDate { get; set; }


        [StringLength(100)]
        public string? ApprovedBy { get; set; }
    }
}
