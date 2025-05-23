using Microsoft.EntityFrameworkCore;
using SAIS.Models;
using System.Linq;

namespace SAIS.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SAISContext context)
        {
            
            context.Database.Migrate();

            // Check if database is already seeded
            if (context.SexLookups.Any() ||
                context.MaritalStatusLookups.Any())
             
            {
                return; // DB has been seeded
            }

            // Seed lookup tables
            var sexes = new[]
            {
                new SexLookup { SexName = "Male", SexCode = "MALE" },
                new SexLookup { SexName = "Female", SexCode = "FEMALE" },
                new SexLookup { SexName = "Other", SexCode = "OTHER" }
            };
            context.SexLookups.AddRange(sexes);

            var maritalStatuses = new[]
            {
                new MaritalStatusLookup { StatusName = "Single", StatusCode = "SINGLE" },
                new MaritalStatusLookup { StatusName = "Married", StatusCode = "MARRIED" },
                new MaritalStatusLookup { StatusName = "Divorced", StatusCode = "DIVORCED" },
                new MaritalStatusLookup { StatusName = "Widowed", StatusCode = "WIDOWED" },
                new MaritalStatusLookup { StatusName = "Separated", StatusCode = "SEPARATED" }
            };
            context.MaritalStatusLookups.AddRange(maritalStatuses);

           

            // Save all lookups
            context.SaveChanges();

            // Seed programs
            var programs = new[]
            {
                new Program_ { ProgramCode = "OVC", ProgramName = "Orphans and vulnerable children",
                    Description = "Support for orphans and vulnerable children" },
                new Program_ { ProgramCode = "ELDERLY", ProgramName = "Poor elderly persons",
                    Description = "Support for poor elderly persons" },
                new Program_ { ProgramCode = "PWD", ProgramName = "Persons with disability",
                    Description = "Support for persons with disabilities" },
                new Program_ { ProgramCode = "EXTREME", ProgramName = "Persons in extreme poverty",
                    Description = "Support for persons in extreme poverty" },
                new Program_ { ProgramCode = "OTHER", ProgramName = "Any other",
                    Description = "Other social assistance programs" }
            };
            context.Programs.AddRange(programs);
            context.SaveChanges();

            // Seed sample geographic data
            var counties = new[]
            {
                new County { CountyCode = "001", CountyName = "Nairobi" },
                new County { CountyCode = "002", CountyName = "Mombasa" }
            };
            context.Counties.AddRange(counties);
            context.SaveChanges();

            var subCounties = new[]
            {
                new SubCounty { SubCountyCode = "001-001", SubCountyName = "Westlands", CountyID = 1 },
                new SubCounty { SubCountyCode = "001-002", SubCountyName = "Dagoretti", CountyID = 1 },
                new SubCounty { SubCountyCode = "002-001", SubCountyName = "Mvita", CountyID = 2 }
            };
            context.SubCounties.AddRange(subCounties);
            context.SaveChanges();

            var locations = new[]
            {
                new Location { LocationCode = "001-001-001", LocationName = "Parklands", SubCountyID = 1 },
                new Location { LocationCode = "001-001-002", LocationName = "Lavington", SubCountyID = 1 },
                new Location { LocationCode = "002-001-001", LocationName = "Old Town", SubCountyID = 3 }
            };
            context.Locations.AddRange(locations);
            context.SaveChanges();

            var subLocations = new[]
            {
                new SubLocation { SubLocationCode = "001-001-001-001", SubLocationName = "Parklands Central", LocationID = 1 },
                new SubLocation { SubLocationCode = "002-001-001-001", SubLocationName = "Old Town Central", LocationID = 3 }
            };
            context.SubLocations.AddRange(subLocations);
            context.SaveChanges();

            var villages = new[]
            {
                new Village { VillageCode = "001-001-001-001-001", VillageName = "Parklands Estate", SubLocationID = 1 },
                new Village { VillageCode = "002-001-001-001-001", VillageName = "Old Town Estate", SubLocationID = 2 }
            };
            context.Villages.AddRange(villages);
            context.SaveChanges();
        }
    }
}