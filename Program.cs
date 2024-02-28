using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstatePipeline.Model;
using RealEstatePipeline.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

using System;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RealEstatePipeline
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add this line to load user secrets in development environment
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

            // Retrieve the password from environment variables
            var dbPassword = builder.Configuration["Database:Password"];
            var dbServer = builder.Configuration["Database:Server"];
            var emailApiKey = builder.Configuration["Email:ServiceApiKey"];
            

            // Ensure that you have the values before continuing
            if (string.IsNullOrEmpty(dbPassword) || string.IsNullOrEmpty(dbServer))
            {
                // Handle the case where necessary configuration is missing
                throw new InvalidOperationException("Database configuration is missing.");
            }



            // Build the connection string
            var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var connectionString = $"Server={dbServer};{defaultConnectionString};Password={dbPassword};";
           




            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

            // Register the EmailSender with the API key
            builder.Services.AddSingleton<IEmailSender>(provider =>
                new EmailSender(emailApiKey));

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(30); // Example: User will be logged in for 30 days
                options.SlidingExpiration = true; // Resets the cookie expiration time if the user is active
            });
            builder.Services.AddScoped<ClientService>(); // Add this line

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("In development mode");  
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Add this line
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
