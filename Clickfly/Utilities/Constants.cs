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
        public const string Users = "users";
        public const string Customers = "customers";
        public const string Aircrafts = "aircrafts";
        public const string AircraftImages = "aircraft_images";
        public const string Flights = "flights";
        public const string FlightSegments = "flight_segments";
        public const string AirTaxis = "air_taxis";
        public const string AirTaxiBases = "air_taxi_bases";
    };

    static class FieldNames {
        public const string Thumbnail = "thumbnail";
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
}
