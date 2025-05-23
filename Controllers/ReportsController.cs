using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAIS.Data;
using SAIS.Models;

namespace SAIS.Controllers
{
    public class ReportsController : Controller
    {
        private readonly SAISContext _context;

        public ReportsController(SAISContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ExportToExcel()
        {
            var applicants = await _context.Applicants
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

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Applicants");
                var currentRow = 1;

                // Header
                worksheet.Cell(currentRow, 1).Value = "Application Date";
                worksheet.Cell(currentRow, 2).Value = "Full Name";
                worksheet.Cell(currentRow, 3).Value = "Sex";
                worksheet.Cell(currentRow, 4).Value = "Age";
                worksheet.Cell(currentRow, 5).Value = "Marital Status";
                worksheet.Cell(currentRow, 6).Value = "ID/Passport Number";
                worksheet.Cell(currentRow, 7).Value = "County";
                worksheet.Cell(currentRow, 8).Value = "Sub-County";
                worksheet.Cell(currentRow, 9).Value = "Location";
                worksheet.Cell(currentRow, 10).Value = "Sub-Location";
                worksheet.Cell(currentRow, 11).Value = "Village";
                worksheet.Cell(currentRow, 12).Value = "Postal Address";
                worksheet.Cell(currentRow, 13).Value = "Physical Address";
                worksheet.Cell(currentRow, 14).Value = "Telephone";
                worksheet.Cell(currentRow, 15).Value = "Programs";
                worksheet.Cell(currentRow, 16).Value = "Status";

                // Body
                foreach (var applicant in applicants)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = applicant.ApplicationDate.ToShortDateString();
                    worksheet.Cell(currentRow, 2).Value = $"{applicant.FirstName} {applicant.MiddleName} {applicant.LastName}";
                    worksheet.Cell(currentRow, 3).Value = applicant.Sex?.SexName;
                    worksheet.Cell(currentRow, 4).Value = applicant.Age;
                    worksheet.Cell(currentRow, 5).Value = applicant.MaritalStatus?.StatusName;
                    worksheet.Cell(currentRow, 6).Value = applicant.IDNumber;
                    worksheet.Cell(currentRow, 7).Value = applicant.Village?.SubLocation?.Location?.SubCounty?.County?.CountyName;
                    worksheet.Cell(currentRow, 8).Value = applicant.Village?.SubLocation?.Location?.SubCounty?.SubCountyName;
                    worksheet.Cell(currentRow, 9).Value = applicant.Village?.SubLocation?.Location?.LocationName;
                    worksheet.Cell(currentRow, 10).Value = applicant.Village?.SubLocation?.SubLocationName;
                    worksheet.Cell(currentRow, 11).Value = applicant.Village?.VillageName;
                    worksheet.Cell(currentRow, 12).Value = applicant.PostalAddress;
                    worksheet.Cell(currentRow, 13).Value = applicant.PhysicalAddress;
                    worksheet.Cell(currentRow, 14).Value = applicant.Telephone;
                    worksheet.Cell(currentRow, 15).Value = string.Join(", ", applicant.ApplicantPrograms.Select(ap => ap.Program.ProgramName));
                    worksheet.Cell(currentRow, 16).Value = applicant.Status?.StatusName;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Applicants.xlsx");
                }
            }
        }

        public async Task<IActionResult> ExportToPDF()
        {
            var applicants = await _context.Applicants
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

            using (var memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Add title
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
                Paragraph title = new Paragraph("Social Assistance Fund - Applicants Report", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(new Paragraph("\n"));

                // Add table
                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2f, 3f, 2f, 2f, 3f, 2f });

                // Header
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
                PdfPCell headerCell = new PdfPCell(new Phrase("Application Date", headerFont));
                headerCell.BackgroundColor = new BaseColor(79, 129, 189);
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(headerCell);

                headerCell = new PdfPCell(new Phrase("Full Name", headerFont));
                headerCell.BackgroundColor = new BaseColor(79, 129, 189);
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(headerCell);

                headerCell = new PdfPCell(new Phrase("ID Number", headerFont));
                headerCell.BackgroundColor = new BaseColor(79, 129, 189);
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(headerCell);

                headerCell = new PdfPCell(new Phrase("Location", headerFont));
                headerCell.BackgroundColor = new BaseColor(79, 129, 189);
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(headerCell);

                headerCell = new PdfPCell(new Phrase("Programs", headerFont));
                headerCell.BackgroundColor = new BaseColor(79, 129, 189);
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(headerCell);

                headerCell = new PdfPCell(new Phrase("Status", headerFont));
                headerCell.BackgroundColor = new BaseColor(79, 129, 189);
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(headerCell);

                // Body
                Font bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK);
                foreach (var applicant in applicants)
                {
                    table.AddCell(new Phrase(applicant.ApplicationDate.ToShortDateString(), bodyFont));
                    table.AddCell(new Phrase($"{applicant.FirstName} {applicant.MiddleName} {applicant.LastName}", bodyFont));
                    table.AddCell(new Phrase(applicant.IDNumber, bodyFont));
                    table.AddCell(new Phrase($"{applicant.Village?.VillageName}, {applicant.Village?.SubLocation?.SubLocationName}, {applicant.Village?.SubLocation?.Location?.LocationName}", bodyFont));
                    table.AddCell(new Phrase(string.Join(", ", applicant.ApplicantPrograms.Select(ap => ap.Program.ProgramName)), bodyFont));
                    table.AddCell(new Phrase(applicant.Status?.StatusName, bodyFont));
                }

                document.Add(table);
                document.Close();

                return File(memoryStream.ToArray(), "application/pdf", "Applicants.pdf");
            }
        }
    }
}