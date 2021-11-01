using System;

namespace clickfly
{
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
        public const string Customers = "customers";
        public const string Aircrafts = "aircrafts";
        public const string AircraftImages = "aircraft_images";
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
}
