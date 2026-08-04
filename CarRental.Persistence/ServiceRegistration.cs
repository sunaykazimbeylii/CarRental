using CarRental.Application.Interfaces.Repositories;
using CarRental.Application.Interfaces.Services;
using CarRental.Application.Services;
using CarRental.Domain.Entities;
using CarRental.Persistence.Contexts;
using CarRental.Persistence.Implementations.Repositories;
using CarRental.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("default")));
            services.AddIdentity<AppUser, IdentityRole<long>>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;
                opt.User.RequireUniqueEmail = true;
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            }
            ).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();


            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();


            return services;

        }


    }
}
