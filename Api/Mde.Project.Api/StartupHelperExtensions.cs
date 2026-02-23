using Mde.Project.Api.Core.Data;
using Mde.Project.Api.Core.Services.Interfaces;
using Mde.Project.Api.Core.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Mde.Project.Api.Core.Entities.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Mde.Project.Api.Services.Interfaces;
using Mde.Project.Api.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Mde.Project.Api.Hubs;
using Mde.Project.Api.Core.Services.Files;
using QuestPDF.Drawing;
using System.Reflection;
using QuestPDF.Infrastructure;

namespace Mde.Project.Api
{
    public static class StartupHelperExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            // Configure required Accept header for json and XML support.
            builder.Services.AddControllers(configure =>
            {
                configure.ReturnHttpNotAcceptable = true;

            })
            .AddNewtonsoftJson(setupAction => 
            {
                setupAction.SerializerSettings.ContractResolver = 
                    new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters();

            // Database connection
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("InventoryDb")
                )
            );

            // Identity configuration
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Memory cache
            builder.Services.AddMemoryCache();

            // Signal R configuration
            builder.Services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            // Add cors
            builder.Services.AddCors();

            // Swagger configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.Events.OnRedirectToAuthorizationEndpoint = context =>
                {
                    context.Response.Redirect(context.RedirectUri + "&prompt=consent");
                    return Task.CompletedTask;
                };
            })
            .AddJwtBearer
            (options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                RequireExpirationTime = true,
                ValidAudience = builder.Configuration.GetValue<string>("JWTConfiguration:Audience"),
                ValidIssuer = builder.Configuration.GetValue<string>("JWTConfiguration:Issuer"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetValue<string>("JWTConfiguration:SigningKey")))
            });

            // Services
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IWarehouseService, WarehouseService>();
            builder.Services.AddScoped<IReportsService, ReportsService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IStatisticsService, StatisticsService>();
            builder.Services.AddScoped<IFilesService, FilesService>();


            // QuestPDf configuration
            QuestPDF.Settings.License = LicenseType.Community;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Mde.Project.Api.Resources.Fonts.LibreBarcodeEAN13Text-Regular.ttf";
            using Stream fontStream = assembly.GetManifestResourceStream(resourceName);

            FontManager.RegisterFontWithCustomName("Barcode", fontStream);

            return builder.Build();
        }

        public static void ConfigurePipeline(this WebApplication app)
        {

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appbuilder =>
                {
                    appbuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Use cors
            app.UseCors(options =>
                options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<WarehouseHub>("/warehouseHub");

            app.MapControllers();
        }
    }
}
