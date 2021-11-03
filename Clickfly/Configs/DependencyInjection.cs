using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using clickfly.Data;
using clickfly.Repositories;
using clickfly.Services;
using clickfly.Middlewares;
using clickfly.ViewModels;
using PagarmeCoreApi.Standard.Controllers;
using System;

namespace clickfly.Configs
{
    public static class DependencyInjection
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<IDataContext, DataContext>();
            services.AddScoped<IDBContext, DBContext>();
            services.AddScoped<IInformer, Informer>();
            services.AddScoped<IUtils, Utils>();
            services.AddScoped<IOrm, Orm>();

            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IStateRepository, StateRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ITimezoneService, TimezoneService>();
            services.AddScoped<ITimezoneRepository, TimezoneRepository>();

            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICityRepository, CityRepository>();

            services.AddScoped<IAerodromeService, AerodromeService>();
            services.AddScoped<IAerodromeRepository, AerodromeRepository>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddScoped<IAccountVerificationService, AccountVerificationService>();
            services.AddScoped<IAccountVerificationRepository, AccountVerificationRepository>();
            
            services.AddScoped<IFlightSegmentService, FlightSegmentService>();
            services.AddScoped<IFlightSegmentRepository, FlightSegmentRepository>();

            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<IAircraftService, AircraftService>();
            services.AddScoped<IAircraftRepository, AircraftRepository>();

            services.AddScoped<IAircraftImageService, AircraftImageService>();
            services.AddScoped<IAircraftImageRepository, AircraftImageRepository>();

            services.AddScoped<IAirTaxiService, AirTaxiService>();
            services.AddScoped<IAirTaxiRepository, AirTaxiRepository>();

            services.AddScoped<IAirTaxiBaseService, AirTaxiBaseService>();
            services.AddScoped<IAirTaxiBaseRepository, AirTaxiBaseRepository>();

            services.AddScoped<ICustomerAddressService, CustomerAddressService>();
            services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
            
            services.AddScoped<ICustomerFriendService, CustomerFriendService>();
            services.AddScoped<ICustomerFriendRepository, CustomerFriendRepository>();

            services.AddScoped<ICustomerCardService, CustomerCardService>();
            services.AddScoped<ICustomerCardRepository, CustomerCardRepository>();

            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<IAircraftModelService, AircraftModelService>();
            services.AddScoped<IAircraftModelRepository, AircraftModelRepository>();

            services.AddScoped<IFlightRepository, FlightRepository>();
            services.AddScoped<IFlightService, FlightService>();

            services.AddScoped<IBookingStatusRepository, BookingStatusRepository>();

            services.AddScoped<IBookingPaymentRepository, BookingPaymentRepository>();

            services.AddScoped<IPassengerRepository, PassengerRepository>();

            services.AddScoped<IFileRepository, FileRepository>();

            services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();

            services.AddScoped<ICoreService, CoreService>();

            services.AddScoped<IAppFlightService, AppFlightService>();
            services.AddScoped<IAppFlightRepository, AppFlightRepository>();
            
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<ICepService, CepService>();

            // Pagar.me
            services.AddScoped<ICustomersController, CustomersController>();
            services.AddScoped<IOrdersController, OrdersController>();

            return services;
        }

        public static IServiceCollection AddErrorHandler(this IServiceCollection services, Action<ErrorHandlerOptions> options = default)
        {
            options = options ?? (opts => {});
            services.Configure(options);
            return services;
        }

        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandler>();
        }
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app, Action<ErrorHandlerOptions> options = default)
        {
            ErrorHandlerOptions config = new ErrorHandlerOptions();
            options.Invoke(config);

            return app.UseMiddleware<ErrorHandler>(config);
        }
    }
}
