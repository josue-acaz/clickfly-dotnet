using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using clickfly.Models;

namespace clickfly.Data
{
    public interface IDataContext
    {
        DatabaseFacade Database { get; }
        DbSet<State> States { get; set; }
        DbSet<Timezone> Timezones { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<Aerodrome> Aerodromes { get; set; }
        DbSet<AirTaxi> AirTaxis { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<AirTaxiBase> AirTaxiBases { get; set; }
        DbSet<Manufacturer> Manufacturers { get; set; }
        DbSet<AircraftModel> AircraftModels { get; set; }
        DbSet<Aircraft> Aircrafts { get; set; }
        DbSet<AircraftImage> AircraftImages { get; set; }
        DbSet<Flight> Flights { get; set; }
        DbSet<FlightSegment> FlightSegments { get; set; }
        DbSet<File> Files { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<CustomerCard> CustomerCards { get; set; }
        DbSet<CustomerFriend> CustomerFriends { get; set; }
        DbSet<CustomerAddress> CustomerAddresses { get; set; }
        DbSet<CustomerAerodrome> CustomerAerodromes { get; set; }
        DbSet<AccountVerification> AccountVerifications { get; set; }
        DbSet<Booking> Bookings { get; set; }
        DbSet<BookingPayment> BookingPayments { get; set; }
        DbSet<BookingStatus> BookingStatus { get; set; }
        DbSet<Passenger> Passengers { get; set; }
        DbSet<Ticket> Tickets { get; set; }
        DbSet<AccessToken> AccessTokens { get; set; }
        DbSet<FlightSegmentStatus> FlightSegmentStatus { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<PermissionGroup> PermissionGroups { get; set; }
        DbSet<PermissionResource> PermissionResources { get; set; }
        DbSet<UserRolePermission> UserRolePermissions { get; set; }
        DbSet<SystemLog> SystemLogs { get; set; }
        DbSet<Newsletter> Newsletters { get; set; }
        DbSet<Subscriber> Subscribers { get; set; }
        DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; }        
        DbSet<Boarding> Boardings { get; set; }
        DbSet<Campaign> Campaigns { get; set; }
        DbSet<CampaignScore> CampaignScores { get; set; }
        DbSet<CampaignStatus> CampaignStatus { get; set; }
        DbSet<ContactRequest> ContactRequests { get; set; }
        DbSet<CustomerAtShoppingCart> CustomerAtShoppingCarts { get; set; }
        DbSet<CustomerContact> CustomerContacts { get; set; }
        DbSet<CustomerSearch> CustomerSearches { get; set; }
        DbSet<DoubleCheck> DoubleChecks { get; set; }
        DbSet<PushNotification> PushNotifications { get; set; }
        DbSet<Score> Scores { get; set; }
        DbSet<SystemConfig> SystemConfigs { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
