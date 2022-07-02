using System;

namespace clickfly
{
    static class UserTypes {
        public const string User = "user";
        public const string Customer = "customer";
    }

    static class UserRoles {
        public const string EMPLOYEE = "employee";
        public const string MANAGER = "manager";
        public const string ADMINISTRATOR = "administrator";
        public const string GENERAL_ADMINISTRATOR = "general_administrator";
    };

    static class UserIdTypes {
        public const string UserId = "user_id";
        public const string CustomerId = "customer_id";
    };

    static class PaymentMethods {
        public const string CreditCard = "credit_card";
        public const string Pix = "pix";
    };

    static class AircraftTypes {
        public const string Aircraft = "aircraft";
        public const string Helicopter = "helicopter";
        public const string Seaplane = "seaplane";
        public const string Jet = "jet";
    };

    static class AircraftImageViews {
        public const string Interior = "interior";
        public const string Exterior = "exterior";
    };

    static class AircraftThumbnailTypes {
        public const string Thumbnail = "thumbnail";
        public const string SeatingMap = "seating_map";
    };

    static class BookingStatusTypes {
        public const string Pending = "pending";
        public const string Approved = "approved";
        public const string Canceled = "canceled";
        public const string Expired = "expired";
        public const string NotApproved = "not_approved";
    };

    static class CustomerTypes {
        public const string Individual = "individual";
        public const string Company = "company";
    };

    static class DocumentTypes {
        public const string CPF = "cpf";
        public const string CNPJ = "cnpj";
    };

    static class Resources {
        public const string AccessTokens = "access_tokens";
        public const string AccountVerifications = "account_verifications";
        public const string Aerodromes = "aerodromes";
        public const string Aircrafts = "aircrafts";
        public const string AircraftImages = "aircraft_images";
        public const string AircraftModels = "aircraft_models";
        public const string AirTaxis = "air_taxis";
        public const string AirTaxiBases = "air_taxi_bases";
        public const string Boardings = "boardings";
        public const string Bookings = "bookings";
        public const string BookingPayments = "booking_payments";
        public const string BookingStatus = "booking_status";
        public const string Campaigns = "campaigns";
        public const string CampaignScores = "campaign_scores";
        public const string CampaignStatus = "campaign_status";
        public const string Cities = "cities";
        public const string ContactRequests = "contact_requests";
        public const string Customers = "customers";
        public const string CustomerAddresses = "customer_addresses";
        public const string CustomerAerodromes = "customer_aerodromes";
        public const string CustomerAtShoppingCart = "customer_at_shopping_carts";
        public const string CustomerCards = "customer_cards";
        public const string CustomerContacts = "customer_contacts";
        public const string CustomerFriends = "customer_friends";
        public const string CustomerSearches = "customer_searches";
        public const string DoubleChecks = "double_checks";
        public const string Files = "files";
        public const string Flights = "flights";
        public const string FlightSegments = "flight_segments";
        public const string FlightSegmentStatus = "flight_segment_status";
        public const string Manufacturers = "manufacturers";
        public const string Newsletters = "newsletters";
        public const string NewsletterSubscribers = "newsletter_subscribers";
        public const string Passengers = "passengers";
        public const string Permissions = "permissions";
        public const string PermissionGroups = "permission_groups";
        public const string PermissionResources = "permission_resources";
        public const string PushNotifications = "push_notifications";
        public const string Scores = "scores";
        public const string States = "states";
        public const string Subscribers = "subscribers";
        public const string SystemConfigs = "system_configs";
        public const string SystemLogs = "system_logs";
        public const string Tickets = "tickets";
        public const string Timezones = "timezones";
        public const string Users = "users";
        public const string UserRoles = "user_roles";
        public const string UserRolePermissions = "user_role_permissions";
    };

    static class FieldNames {
        public const string Thumbnail = "thumbnail";
        public const string AircraftImage = "aircraft_image";
    };

    static class FlightTypes {
        public const string Charter = "charter";
        public const string Shared = "shared";
    };

    static class FlightSegmentTypes {
        public const string Trip = "trip";
        public const string Transfer = "transfer";
    };

    static class FlightSegmentStatusTypes {
        public const string Active = "active";
        public const string Pending = "pending";
        public const string Inactive = "inactive";
        public const string Running = "running";
        public const string Finished = "finished";
        public const string Canceled = "canceled";
        public const string Expired = "expired";
    };

    static class Actions {
        public const string Create = "create";
        public const string Update = "update";
        public const string Read = "read";
        public const string Delete = "delete";
    };

    static class OrderStatus {
        public const string Pending = "pending";
        public const string Paid = "paid";
        public const string Canceled = "canceled";
        public const string Failed = "failed";
    };

    static class ChargeStatus {
        public const string AuthorizedPendingCapture = "authorized_pending_capture";
        public const string NotAuthorized = "not_authorized";
        public const string Captured = "captured";
        public const string PartialCapture = "partial_capture";
        public const string WaitingCapture = "waiting_capture";
        public const string Refunded = "refunded";
        public const string Voided = "voided";
        public const string PartialRefunded = "partial_refunded";
        public const string PartialVoid = "partial_void";
        public const string ErrorOnVoiding = "error_on_voiding";
        public const string ErrorOnRefunding = "error_on_refunding";
        public const string WaitingCancellation = "waiting_cancellation";
        public const string WithError = "with_error";
        public const string Failed = "failed";
    };
}
