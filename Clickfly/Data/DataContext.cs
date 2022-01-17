using Microsoft.EntityFrameworkCore;
using clickfly.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using clickfly.Mappings;

namespace clickfly.Data 
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext() {}

        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<State> States { get; set; }
        public DbSet<Timezone> Timezones { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Aerodrome> Aerodromes { get; set; }
        public DbSet<AirTaxi> AirTaxis { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<AirTaxiBase> AirTaxiBases { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<AircraftModel> AircraftModels { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<AircraftImage> AircraftImages { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightSegment> FlightSegments { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerCard> CustomerCards { get; set; }
        public DbSet<CustomerFriend> CustomerFriends { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<CustomerAerodrome> CustomerAerodromes { get; set; }
        public DbSet<AccountVerification> AccountVerifications { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingPayment> BookingPayments { get; set; }
        public DbSet<BookingStatus> BookingStatus { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<FlightSegmentStatus> FlightSegmentStatus { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<PermissionResource> PermissionResources { get; set; }
        public DbSet<UserRolePermission> UserRolePermissions { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; }
        public DbSet<Boarding> Boardings { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignScore> CampaignScores { get; set; }
        public DbSet<CampaignStatus> CampaignStatus { get; set; }
        public DbSet<ContactRequest> ContactRequests { get; set; }
        public DbSet<CustomerAtShoppingCart> CustomerAtShoppingCarts { get; set; }
        public DbSet<CustomerContact> CustomerContacts { get; set; }
        public DbSet<CustomerSearch> CustomerSearches { get; set; }
        public DbSet<DoubleCheck> DoubleChecks { get; set; }
        public DbSet<PushNotification> PushNotifications { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseNpgsql("Server=127.0.0.1;Port=5432;User Id=postgres;Password=97531482;Database=clickfly_dotnet;");
            }
        }
        */

        protected override void OnModelCreating(ModelBuilder builder) {
            foreach (var property in builder.Model.GetEntityTypes()
                           .SelectMany(e => e.GetProperties()
                                .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");
            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            base.OnModelCreating(builder);
        }
    }
}