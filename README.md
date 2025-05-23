# Social Assistance Information System (SAIS)

A comprehensive web application designed to manage applications for social assistance programs, built with .NET 9 and SQL Server.

## Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Database Initialization](#database-initialization)
- [Running the Application](#running-the-application)
- [Using the Application](#using-the-application)
- [Database Structure](#database-structure)
- [Contributing](#contributing)

## Overview

The Social Assistance Information System (SAIS) streamlines the management of social assistance program applications. The system supports multiple programs including OVC (Orphans and Vulnerable Children), PWD (Persons with Disabilities), and Elderly assistance programs.

## Prerequisites

Before setting up SAIS, ensure you have the following installed:

- **.NET 9 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Microsoft SQL Server 2022** - [Download here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- **IIS** (for production deployment)
- **Git** (for cloning the repository)

## Installation

### 1. Clone the Repository

```bash
git clone https://github.com/Marty-Muhanga/SAIS.git
cd SAIS
```

### 2. Restore Dependencies

```bash
dotnet restore
```

## Database Setup

SAIS offers two approaches for database setup:

1. **Automatic Setup (Recommended)**: The application automatically initializes the database with required data
2. **Manual Setup**: You can manually create and seed the database using SQL scripts

### Option 1: Automatic Setup (Recommended)

The simplest approach is to let the application handle database initialization automatically:

1. Create an empty database named `SAIS`
2. Update your connection string in `appsettings.json`
3. Run the application - it will automatically create tables and seed initial data

### Option 2: Manual Database Setup

If you prefer manual control over database creation and seeding, follow these detailed steps:

### 1. Create Database

Execute the following SQL script to create the main database:

```sql
-- Database creation
CREATE DATABASE SAIS;
GO

USE SAIS;
GO
```

### 2. Create Tables

The database follows Third Normal Form (3NF) for optimal normalization. Execute the following scripts to create the required tables:

#### Lookup Tables

```sql
-- Sex lookup table
CREATE TABLE SexLookup (
    SexID INT PRIMARY KEY IDENTITY(1,1),
    SexName NVARCHAR(50) NOT NULL,
    SexCode NVARCHAR(10) NOT NULL UNIQUE
);

-- Marital status lookup table
CREATE TABLE MaritalStatusLookup (
    MaritalStatusID INT PRIMARY KEY IDENTITY(1,1),
    StatusName NVARCHAR(50) NOT NULL,
    StatusCode NVARCHAR(10) NOT NULL UNIQUE
);

-- Application status lookup table
CREATE TABLE ApplicationStatusLookup (
    StatusID INT PRIMARY KEY IDENTITY(1,1),
    StatusName NVARCHAR(50) NOT NULL,
    StatusCode NVARCHAR(10) NOT NULL UNIQUE
);
```

#### Geographic Hierarchy Tables

```sql
-- Counties table
CREATE TABLE Counties (
    CountyID INT PRIMARY KEY IDENTITY(1,1),
    CountyCode NVARCHAR(20) NOT NULL UNIQUE,
    CountyName NVARCHAR(100) NOT NULL
);

-- Sub-counties table
CREATE TABLE SubCounties (
    SubCountyID INT PRIMARY KEY IDENTITY(1,1),
    SubCountyCode NVARCHAR(20) NOT NULL UNIQUE,
    SubCountyName NVARCHAR(100) NOT NULL,
    CountyID INT NOT NULL,
    FOREIGN KEY (CountyID) REFERENCES Counties(CountyID)
);

-- Locations table
CREATE TABLE Locations (
    LocationID INT PRIMARY KEY IDENTITY(1,1),
    LocationCode NVARCHAR(20) NOT NULL UNIQUE,
    LocationName NVARCHAR(100) NOT NULL,
    SubCountyID INT NOT NULL,
    FOREIGN KEY (SubCountyID) REFERENCES SubCounties(SubCountyID)
);

-- Sub-locations table
CREATE TABLE SubLocations (
    SubLocationID INT PRIMARY KEY IDENTITY(1,1),
    SubLocationCode NVARCHAR(20) NOT NULL UNIQUE,
    SubLocationName NVARCHAR(100) NOT NULL,
    LocationID INT NOT NULL,
    FOREIGN KEY (LocationID) REFERENCES Locations(LocationID)
);

-- Villages table
CREATE TABLE Villages (
    VillageID INT PRIMARY KEY IDENTITY(1,1),
    VillageCode NVARCHAR(20) NOT NULL UNIQUE,
    VillageName NVARCHAR(100) NOT NULL,
    SubLocationID INT NOT NULL,
    FOREIGN KEY (SubLocationID) REFERENCES SubLocations(SubLocationID)
);
```

#### Programs and Applicants Tables

```sql
-- Programs table
CREATE TABLE Programs (
    ProgramID INT PRIMARY KEY IDENTITY(1,1),
    ProgramCode NVARCHAR(20) NOT NULL UNIQUE,
    ProgramName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500)
);

-- Applicants table
CREATE TABLE Applicants (
    ApplicantID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    MiddleName NVARCHAR(100),
    LastName NVARCHAR(100) NOT NULL,
    SexID INT NOT NULL,
    Age INT NOT NULL,
    MaritalStatusID INT NOT NULL,
    IDNumber NVARCHAR(20) NOT NULL,
    PostalAddress NVARCHAR(200),
    PhysicalAddress NVARCHAR(200) NOT NULL,
    Telephone NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100),
    VillageID INT NOT NULL,
    ApplicationDate DATETIME NOT NULL DEFAULT GETDATE(),
    StatusID INT NOT NULL DEFAULT 1,
    SignatureImage VARBINARY(MAX),
    CreatedBy NVARCHAR(100) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedBy NVARCHAR(100),
    ModifiedDate DATETIME,
    FOREIGN KEY (SexID) REFERENCES SexLookup(SexID),
    FOREIGN KEY (MaritalStatusID) REFERENCES MaritalStatusLookup(MaritalStatusID),
    FOREIGN KEY (VillageID) REFERENCES Villages(VillageID),
    FOREIGN KEY (StatusID) REFERENCES ApplicationStatusLookup(StatusID)
);

-- Junction table for applicant-program relationships
CREATE TABLE ApplicantPrograms (
    ApplicantProgramID INT PRIMARY KEY IDENTITY(1,1),
    ApplicantID INT NOT NULL,
    ProgramID INT NOT NULL,
    StatusID INT NOT NULL,
    ApprovalDate DATETIME,
    ApprovedBy NVARCHAR(100),
    FOREIGN KEY (ApplicantID) REFERENCES Applicants(ApplicantID),
    FOREIGN KEY (ProgramID) REFERENCES Programs(ProgramID),
    FOREIGN KEY (StatusID) REFERENCES ApplicationStatusLookup(StatusID)
);
```

### 3. Seed Initial Data

Insert the required lookup and sample data:

```sql
-- Insert lookup data
INSERT INTO SexLookup (SexName, SexCode) VALUES 
('Male', 'MALE'),
('Female', 'FEMALE'),
('Other', 'OTHER');

INSERT INTO MaritalStatusLookup (StatusName, StatusCode) VALUES 
('Single', 'SINGLE'),
('Married', 'MARRIED'),
('Divorced', 'DIVORCED'),
('Widowed', 'WIDOWED'),
('Separated', 'SEPARATED');

INSERT INTO ApplicationStatusLookup (StatusName, StatusCode) VALUES 
('Draft', 'DRAFT'),
('Submitted', 'SUBMITTED'),
('Approved', 'APPROVED'),
('Rejected', 'REJECTED');

-- Insert program data
INSERT INTO Programs (ProgramCode, ProgramName, Description) VALUES 
('OVC', 'Orphans and Vulnerable Children', 'Support program for orphans and vulnerable children'),
('PWD', 'Persons with Disabilities', 'Assistance program for persons with disabilities'),
('ELDERLY', 'Elderly Support', 'Support program for elderly citizens');

-- Sample geographic data (replace with actual data from Bureau of Statistics)
INSERT INTO Counties (CountyCode, CountyName) VALUES ('001', 'Nairobi');
INSERT INTO SubCounties (SubCountyCode, SubCountyName, CountyID) VALUES ('001-001', 'Westlands', 1);
INSERT INTO Locations (LocationCode, LocationName, SubCountyID) VALUES ('001-001-001', 'Parklands', 1);
INSERT INTO SubLocations (SubLocationCode, SubLocationName, LocationID) VALUES ('001-001-001-001', 'Parklands Central', 1);
INSERT INTO Villages (VillageCode, VillageName, SubLocationID) VALUES ('001-001-001-001-001', 'Parklands Estate', 1);
```

### 4. Insert Sample Applicants (Optional)

```sql
-- Sample applicants with proper foreign key references
DECLARE @MaleSexID INT = (SELECT SexID FROM SexLookup WHERE SexCode = 'MALE');
DECLARE @FemaleSexID INT = (SELECT SexID FROM SexLookup WHERE SexCode = 'FEMALE');
DECLARE @MarriedStatusID INT = (SELECT MaritalStatusID FROM MaritalStatusLookup WHERE StatusCode = 'MARRIED');
DECLARE @SingleStatusID INT = (SELECT MaritalStatusID FROM MaritalStatusLookup WHERE StatusCode = 'SINGLE');
DECLARE @SubmittedStatusID INT = (SELECT StatusID FROM ApplicationStatusLookup WHERE StatusCode = 'SUBMITTED');
DECLARE @ApprovedStatusID INT = (SELECT StatusID FROM ApplicationStatusLookup WHERE StatusCode = 'APPROVED');
DECLARE @ParklandsVillageID INT = (SELECT VillageID FROM Villages WHERE VillageCode = '001-001-001-001-001');

INSERT INTO Applicants (
    FirstName, MiddleName, LastName, 
    SexID, Age, MaritalStatusID, 
    IDNumber, PostalAddress, PhysicalAddress, 
    Telephone, Email, VillageID, 
    ApplicationDate, StatusID, CreatedBy
) VALUES (
    'John', 'Kamau', 'Mwangi',
    @MaleSexID, 35, @MarriedStatusID,
    '12345678', 'P.O. Box 123', '123 Parklands Road',
    '0712345678', 'john.mwangi@example.com', @ParklandsVillageID,
    '2025-01-15', @SubmittedStatusID, 'system'
),
(
    'Mary', 'Wanjiku', 'Njoroge',
    @FemaleSexID, 28, @SingleStatusID,
    '87654321', 'P.O. Box 456', '456 Lavington Lane',
    '0723456789', 'mary.njoroge@example.com', @ParklandsVillageID,
    '2025-02-20', @ApprovedStatusID, 'system'
);
```

## Configuration

### 1. Update Connection String

Update the connection string in `appsettings.json` to point to your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "SAISContext": "Server=YOUR_SERVER_NAME;Database=SAIS;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### 2. Entity Framework Setup

Run the following Entity Framework Core commands:

```bash
# Add initial migration
dotnet ef migrations add InitialCreate

# Update database schema
dotnet ef database update
```

### 3. Optional: Production Seeding Configuration

To enable automatic database seeding in production environments, add this to your `appsettings.Production.json`:

```json
{
  "SeedDatabaseInProduction": true
}
```

**Note**: The application includes automatic database initialization, so manual SQL seeding may not be necessary. See the [Database Initialization](#database-initialization) section for details.

## Database Initialization

SAIS includes an automatic database initialization feature that will seed your database with essential lookup data and sample geographic data if the database is empty.

### Automatic Seeding Behavior

The application automatically handles database initialization through the `DbInitializer` class:

- **First Run**: If your database is empty (no lookup data exists), the system will automatically seed it with:
  - Sex lookup data (Male, Female, Other)
  - Marital status lookup data (Single, Married, Divorced, Widowed, Separated)
  - Application status lookup data (Draft, Submitted, Approved, Rejected)
  - Sample programs (OVC, PWD, Elderly, Extreme Poverty, Other)
  - Sample geographic hierarchy (Counties, Sub-Counties, Locations, Sub-Locations, Villages)

- **Existing Data**: If lookup data already exists in your database, the seeding process will skip automatically - no duplicate data will be created.

### Environment-Specific Behavior

- **Development**: Database seeding runs automatically on application startup
- **Production**: Seeding is disabled by default for safety. To enable in production, add the following to your `appsettings.Production.json`:

```json
{
  "SeedDatabaseInProduction": true
}
```

### Manual Database Setup (Alternative)

If you prefer to manually set up your database instead of using automatic seeding, you can use the SQL scripts provided in the [Database Setup](#database-setup) section above. The manual approach gives you more control over the initial data.

## Running the Application

### Development Environment

```bash
# Run the application
dotnet run

# Or run with hot reload
dotnet watch run
```

The application will automatically:
1. Apply any pending Entity Framework migrations
2. Seed the database with initial data (if empty)
3. Start the web server

### Production Deployment

For production deployment on IIS, publish the application:

```bash
dotnet publish -c Release -o ./publish
```

Then deploy the published files to your IIS server.

**Production Notes:**
- Database seeding is disabled by default in production
- Enable seeding in production only if you're deploying to a fresh database
- The application includes retry policies for database connections to handle transient failures

## Using the Application

Once your SAIS application is running, follow these steps to set up and use the system effectively:

### 1. Configure System Parameters

**Navigate to the Lookups Page** to configure essential system parameters:

- **Sex Types**: Male, Female, Other (pre-configured)
- **Marital Status**: Single, Married, Divorced, Widowed, Separated (pre-configured)
- **Application Status**: Draft, Submitted, Approved, Rejected (pre-configured)
- **Custom Parameters**: Add any additional lookup values specific to your organization's needs

The lookups page allows you to:
- Add new lookup values
- Edit existing parameters
- Maintain system-wide consistency

### 2. Set Up Geographic Hierarchy

**Use the Locations Page** to create your geographic structure following Kenya's administrative hierarchy:

**Step-by-Step Location Setup:**

1. **Counties**: Start by creating or verifying county records
   - Navigate to Counties section
   - Add counties with proper codes (e.g., "001" for Nairobi)

2. **Sub-Counties**: Create sub-counties under each county
   - Link each sub-county to its parent county
   - Use hierarchical codes (e.g., "001-001" for Westlands under Nairobi)

3. **Locations**: Add locations within each sub-county
   - Continue the coding pattern (e.g., "001-001-001" for Parklands)

4. **Sub-Locations**: Create sub-locations under locations
   - Maintain coding consistency (e.g., "001-001-001-001")

5. **Villages**: Add villages as the most granular level
   - Complete the hierarchy (e.g., "001-001-001-001-001")

**Geographic Hierarchy Example:**
```
County: Nairobi (001)
├── Sub-County: Westlands (001-001)
    ├── Location: Parklands (001-001-001)
        ├── Sub-Location: Parklands Central (001-001-001-001)
            └── Village: Parklands Estate (001-001-001-001-001)
```

### 3. Create and Manage Applications

**Application Creation Process:**

#### Step 1: Create an Applicant
Navigate to the Applicants section and provide:
- **Personal Information**: 
  - First Name, Middle Name, Last Name
  - Age, Sex (from lookup)
  - ID Number, Marital Status (from lookup)
- **Contact Details**:
  - Physical Address, Postal Address
  - Telephone, Email
- **Geographic Location**:
  - Select Village (which automatically determines Sub-Location, Location, Sub-County, and County)
- **Additional Information**:
  - Application Date (auto-generated)
  - Digital Signature (optional)

#### Step 2: Attach Programs
After creating an applicant, attach them to relevant social assistance programs:
- **Available Programs**:
  - OVC (Orphans and Vulnerable Children)
  - PWD (Persons with Disabilities) 
  - Elderly Support
  - Extreme Poverty Support
  - Other Programs

#### Step 3: Application Processing
- **Status Tracking**: Monitor application progress through statuses:
  - Draft → Submitted → Approved/Rejected
- **Approval Workflow**: 
  - Review applicant details
  - Verify eligibility criteria
  - Approve or reject with comments
  - Record approval date and approving officer

### 4. Application Management Features

**Search and Filter:**
- Filter by application status
- Search by applicant name or ID number
- Filter by program type
- Geographic filtering (by county, sub-county, etc.)

**Reporting:**
- Generate reports by program
- Geographic distribution reports
- Application status summaries
- Approval/rejection statistics

**Data Management:**
- Bulk import capabilities (if configured)
- Export functionality
- Audit trail tracking
- Data validation and verification

### 5. Best Practices for Usage

1. **Setup Sequence**: Always configure lookups and geographic data before creating applications
2. **Data Consistency**: Use standardized codes for geographic hierarchy
3. **Regular Maintenance**: Periodically review and update lookup values
4. **Geographic Accuracy**: Ensure village-level data is accurate for proper beneficiary tracking
5. **Program Management**: Regularly review program eligibility and update criteria as needed

### 6. User Workflow Summary

```
System Setup:
Lookups → Geographic Hierarchy → Programs

Application Process:
Create Applicant → Select Location → Attach Programs → Process Application

Management:
Track Status → Generate Reports → Manage Data
```

This workflow ensures proper data organization and efficient processing of social assistance applications.

## Database Structure

The system implements a normalized database structure with the following key components:

- **Lookup Tables**: Sex, Marital Status, and Application Status
- **Geographic Hierarchy**: Counties → Sub-Counties → Locations → Sub-Locations → Villages
- **Core Tables**: Programs, Applicants, and ApplicantPrograms (junction table)

### Key Features

- Third Normal Form (3NF) normalization
- Proper foreign key relationships
- Audit trail with Created/Modified tracking
- Support for digital signatures
- Flexible program assignment through junction table

## Important Notes

- **Foreign Key Dependencies**: Always insert lookup and geographic data before inserting applicant records
- **Data Validation**: Verify lookup table data exists before referencing in other tables
- **Geographic Data**: Replace sample geographic data with actual data from your Bureau of Statistics
- **Security**: Update default connection strings and credentials before production deployment

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Support

For support and questions, please open an issue on the GitHub repository or contact the development team.

---

**Version**: 1.0.0  
**Last Updated**: January 2025
