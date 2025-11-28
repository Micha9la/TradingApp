using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using TradingApp.BlazorUI.Components;
using TradingApp.BlazorUI.Components.Account;
using TradingApp.BlazorUI.Data;
using TradingApp.BlazorUI.Services;

namespace TradingApp.BlazorUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Read credentials path from environment variable (injected by Docker)
            var credentialPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            if (string.IsNullOrEmpty(credentialPath) || !File.Exists(credentialPath))
            {
                Console.WriteLine("Warning: Google credentials file not found or GOOGLE_APPLICATION_CREDENTIALS not set.");
            }

            // Register custom GoogleSheetsService
            builder.Services.AddScoped(provider =>
            {
                // you can move your real spreadsheet ID into configuration later
                string spreadsheetId = "your-spreadsheet-id";
                return new GoogleSheetsService(spreadsheetId, credentialPath);
            });

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            //builder.Services.AddSingleton<ITradeService, InMemoryTradeService>();
        //   builder.Services.AddScoped<ITradeService, EFTradeService>(); //.NET uses EFTradeService whenever someone asks for ITradeService
            builder.Services.AddScoped<CsvTradeImporter>();
            builder.Services.AddScoped<ITradeService,GSTradeService>();
            //   builder.Services.AddScoped< EFTradeService>();
            builder.Services.AddMudServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}
