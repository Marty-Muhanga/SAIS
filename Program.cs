using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.EntityFrameworkCore;
using SAIS.Data;
using SAIS.Data.Repository;
using SAIS.Models;
using SAIS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure DbContext with retry policy for production
builder.Services.AddDbContext<SAISContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SAISContext"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
});

// Register services with interfaces
builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
builder.Services.AddScoped<IApplicantService, ApplicantService>();
builder.Services.AddScoped<IDropdownService, DropdownService>();
builder.Services.AddTransient<ISmsService, SmsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // Development-only middleware
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


// Map controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Applicants}/{action=Index}/{id?}");

// Database seeding with additional checks
await SeedDatabaseAsync(app);

app.Run();

async Task SeedDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<SAISContext>();

        // Apply pending migrations first
        if (context.Database.GetPendingMigrations().Any())
        {
            logger.LogInformation("Applying pending migrations...");
            await context.Database.MigrateAsync();
        }

        // Only seed if we're not in production or if explicitly configured
        if (!app.Environment.IsProduction() ||
            builder.Configuration.GetValue<bool>("SeedDatabaseInProduction"))
        {
            logger.LogInformation("Seeding database...");
            DbInitializer.Initialize(context);
            logger.LogInformation("Database seeding completed successfully.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
        if (app.Environment.IsProduction())
        {
            throw; // Rethrow in production to fail fast
        }
    }
}