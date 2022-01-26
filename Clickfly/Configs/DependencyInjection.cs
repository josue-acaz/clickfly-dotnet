using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using clickfly.Data;
using clickfly.Helpers;
using clickfly.Services;
using clickfly.Repositories;
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
            services.AddScoped<INotificator, Notificator>();
            services.AddScoped<IDapperWrapper, DapperWrapper>();
            services.AddScoped<IUtils, Utils>();

            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IStateRepository, StateRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();

            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();

            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionGroupRepository, PermissionGroupRepository>();
            
            services.AddScoped<IPermissionResourceService, PermissionResourceService>();
            services.AddScoped<IPermissionResourceRepository, PermissionResourceRepository>();

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

            services.AddScoped<IManufacturerService, ManufacturerService>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();

            services.AddScoped<IAircraftModelService, AircraftModelService>();
            services.AddScoped<IAircraftModelRepository, AircraftModelRepository>();

            services.AddScoped<IFlightRepository, FlightRepository>();
            services.AddScoped<IFlightService, FlightService>();

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ITicketService, TicketService>();

            services.AddScoped<IBookingStatusRepository, BookingStatusRepository>();

            services.AddScoped<IBookingPaymentRepository, BookingPaymentRepository>();

            services.AddScoped<IPassengerRepository, PassengerRepository>();

            services.AddScoped<IFileRepository, FileRepository>();

            services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();

            services.AddScoped<IDoubleCheckService, DoubleCheckService>();
            services.AddScoped<IDoubleCheckRepository, DoubleCheckRepository>();

            services.AddScoped<IFlightSegmentStatusRepository, FlightSegmentStatusRepository>();

            services.AddScoped<ICoreService, CoreService>();

            services.AddScoped<IAppFlightService, AppFlightService>();
            services.AddScoped<IAppFlightRepository, AppFlightRepository>();

            services.AddScoped<ISystemLogService, SystemLogService>();
            services.AddScoped<ISystemLogRepository, SystemLogRepository>();
            
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<ICepService, CepService>();

            // Pagar.me
            services.AddScoped<ChargesController, ChargesController>();
            services.AddScoped<ICustomersController, CustomersController>();
            services.AddScoped<IInvoicesController, InvoicesController>();
            services.AddScoped<IOrdersController, OrdersController>();
            services.AddScoped<IPlansController, PlansController>();
            services.AddScoped<IRecipientsController, RecipientsController>();
            services.AddScoped<ISellersController, SellersController>();
            services.AddScoped<ISubscriptionsController, SubscriptionsController>();
            services.AddScoped<ITokensController, TokensController>();
            services.AddScoped<ITransactionsController, TransactionsController>();
            services.AddScoped<ITransfersController, TransfersController>();

            return services;
        }
    }
}
