using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace clickfly.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "access_tokens",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    token = table.Column<string>(type: "text", nullable: true),
                    resource_id = table.Column<string>(type: "text", nullable: true),
                    expires_in = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_tokens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "air_taxis",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    cnpj = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_air_taxis", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    emergency_phone_number = table.Column<string>(type: "text", nullable: true),
                    document = table.Column<string>(type: "text", nullable: true),
                    document_type = table.Column<string>(type: "text", nullable: true),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    password_reset_expires = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    password_reset_token = table.Column<string>(type: "text", nullable: true),
                    role = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    birthdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    verified = table.Column<bool>(type: "boolean", nullable: false),
                    customer_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    resource = table.Column<string>(type: "text", nullable: true),
                    resource_id = table.Column<string>(type: "text", nullable: true),
                    key = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    mimetype = table.Column<string>(type: "text", nullable: true),
                    field_name = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "manufacturers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_manufacturers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "states",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    prefix = table.Column<string>(type: "text", nullable: false),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_states", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "timezones",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    gmt = table.Column<int>(type: "integer", nullable: false),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timezones", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    air_taxi_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_air_taxis_air_taxi_id",
                        column: x => x.air_taxi_id,
                        principalTable: "air_taxis",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "account_verifications",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    expires = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    customer_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_verifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_account_verifications_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "customer_addresses",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    street = table.Column<string>(type: "text", nullable: true),
                    number = table.Column<string>(type: "text", nullable: true),
                    zipcode = table.Column<string>(type: "text", nullable: true),
                    neighborhood = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    address_id = table.Column<string>(type: "text", nullable: true),
                    complement = table.Column<string>(type: "text", nullable: true),
                    customer_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_customer_addresses_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "customer_cards",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    card_id = table.Column<string>(type: "text", nullable: true),
                    customer_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_cards", x => x.id);
                    table.ForeignKey(
                        name: "FK_customer_cards_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "customer_friends",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    birthdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    document = table.Column<string>(type: "text", nullable: true),
                    document_type = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    emergency_phone_number = table.Column<string>(type: "text", nullable: true),
                    customer_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_friends", x => x.id);
                    table.ForeignKey(
                        name: "FK_customer_friends_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "aircraft_models",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    carrier_size = table.Column<string>(type: "text", nullable: true),
                    number_of_engines = table.Column<string>(type: "text", nullable: true),
                    engine_type = table.Column<string>(type: "text", nullable: true),
                    carrier_dimensions = table.Column<string>(type: "text", nullable: true),
                    manufacturer_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aircraft_models", x => x.id);
                    table.ForeignKey(
                        name: "FK_aircraft_models_manufacturers_manufacturer_id",
                        column: x => x.manufacturer_id,
                        principalTable: "manufacturers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    prefix = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    state_id = table.Column<string>(type: "text", nullable: true),
                    timezone_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.id);
                    table.ForeignKey(
                        name: "FK_cities_states_state_id",
                        column: x => x.state_id,
                        principalTable: "states",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cities_timezones_timezone_id",
                        column: x => x.timezone_id,
                        principalTable: "timezones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "aircrafts",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    prefix = table.Column<string>(type: "text", nullable: true),
                    year = table.Column<int>(type: "integer", nullable: false),
                    crew = table.Column<int>(type: "integer", nullable: false),
                    passengers = table.Column<int>(type: "integer", nullable: false),
                    empty_weight = table.Column<float>(type: "real", nullable: false),
                    autonomy = table.Column<float>(type: "real", nullable: false),
                    maximum_takeoff_weight = table.Column<float>(type: "real", nullable: false),
                    maximum_speed = table.Column<float>(type: "real", nullable: false),
                    cruising_speed = table.Column<float>(type: "real", nullable: false),
                    range = table.Column<float>(type: "real", nullable: false),
                    fixed_price = table.Column<float>(type: "real", nullable: false),
                    fixed_price_radius = table.Column<float>(type: "real", nullable: false),
                    price_per_km = table.Column<float>(type: "real", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    pressurized = table.Column<bool>(type: "boolean", nullable: false),
                    aircraft_model_id = table.Column<string>(type: "text", nullable: true),
                    air_taxi_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aircrafts", x => x.id);
                    table.ForeignKey(
                        name: "FK_aircrafts_air_taxis_air_taxi_id",
                        column: x => x.air_taxi_id,
                        principalTable: "air_taxis",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aircrafts_aircraft_models_aircraft_model_id",
                        column: x => x.aircraft_model_id,
                        principalTable: "aircraft_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "aerodromes",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    oaci_code = table.Column<string>(type: "text", nullable: true),
                    ciad = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    latitude = table.Column<float>(type: "real", nullable: false),
                    longitude = table.Column<float>(type: "real", nullable: false),
                    altitude = table.Column<float>(type: "real", nullable: false),
                    length = table.Column<float>(type: "real", nullable: false),
                    width = table.Column<float>(type: "real", nullable: false),
                    operation = table.Column<string>(type: "text", nullable: true),
                    designation = table.Column<string>(type: "text", nullable: true),
                    resistance = table.Column<string>(type: "text", nullable: true),
                    surface = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    access = table.Column<string>(type: "text", nullable: true),
                    city_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aerodromes", x => x.id);
                    table.ForeignKey(
                        name: "FK_aerodromes_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "aircraft_images",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    view = table.Column<string>(type: "text", nullable: true),
                    aircraft_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aircraft_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_aircraft_images_aircrafts_aircraft_id",
                        column: x => x.aircraft_id,
                        principalTable: "aircrafts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "flights",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: true),
                    aircraft_id = table.Column<string>(type: "text", nullable: true),
                    air_taxi_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flights", x => x.id);
                    table.ForeignKey(
                        name: "FK_flights_air_taxis_air_taxi_id",
                        column: x => x.air_taxi_id,
                        principalTable: "air_taxis",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flights_aircrafts_aircraft_id",
                        column: x => x.aircraft_id,
                        principalTable: "aircrafts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "air_taxi_bases",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    latitude = table.Column<float>(type: "real", nullable: false),
                    longitude = table.Column<float>(type: "real", nullable: false),
                    air_taxi_id = table.Column<string>(type: "text", nullable: true),
                    aerodrome_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_air_taxi_bases", x => x.id);
                    table.ForeignKey(
                        name: "FK_air_taxi_bases_aerodromes_aerodrome_id",
                        column: x => x.aerodrome_id,
                        principalTable: "aerodromes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_air_taxi_bases_air_taxis_air_taxi_id",
                        column: x => x.air_taxi_id,
                        principalTable: "air_taxis",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "customer_aerodromes",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    customer_id = table.Column<string>(type: "text", nullable: true),
                    aerodrome_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_aerodromes", x => x.id);
                    table.ForeignKey(
                        name: "FK_customer_aerodromes_aerodromes_aerodrome_id",
                        column: x => x.aerodrome_id,
                        principalTable: "aerodromes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_customer_aerodromes_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "flight_segments",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    departure_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    arrival_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    price_per_seat = table.Column<float>(type: "real", nullable: false),
                    total_seats = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "text", nullable: true),
                    flight_id = table.Column<string>(type: "text", nullable: true),
                    aircraft_id = table.Column<string>(type: "text", nullable: true),
                    origin_aerodrome_id = table.Column<string>(type: "text", nullable: true),
                    destination_aerodrome_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_segments", x => x.id);
                    table.ForeignKey(
                        name: "FK_flight_segments_aerodromes_destination_aerodrome_id",
                        column: x => x.destination_aerodrome_id,
                        principalTable: "aerodromes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flight_segments_aerodromes_origin_aerodrome_id",
                        column: x => x.origin_aerodrome_id,
                        principalTable: "aerodromes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flight_segments_aircrafts_aircraft_id",
                        column: x => x.aircraft_id,
                        principalTable: "aircrafts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flight_segments_flights_flight_id",
                        column: x => x.flight_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    customer_id = table.Column<string>(type: "text", nullable: true),
                    flight_segment_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_bookings_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_flight_segments_flight_segment_id",
                        column: x => x.flight_segment_id,
                        principalTable: "flight_segments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "booking_payments",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    order_id = table.Column<string>(type: "text", nullable: true),
                    payment_method = table.Column<string>(type: "text", nullable: true),
                    booking_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_booking_payments_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "booking_status",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: true),
                    booking_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_status", x => x.id);
                    table.ForeignKey(
                        name: "FK_booking_status_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "passengers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    birthdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    document = table.Column<string>(type: "text", nullable: true),
                    document_type = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    emergency_phone_number = table.Column<string>(type: "text", nullable: true),
                    booking_id = table.Column<string>(type: "text", nullable: true),
                    flight_segment_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passengers", x => x.id);
                    table.ForeignKey(
                        name: "FK_passengers_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passengers_flight_segments_flight_segment_id",
                        column: x => x.flight_segment_id,
                        principalTable: "flight_segments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    qr_code = table.Column<string>(type: "text", nullable: true),
                    passenger_id = table.Column<string>(type: "text", nullable: true),
                    flight_segment_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.id);
                    table.ForeignKey(
                        name: "FK_tickets_flight_segments_flight_segment_id",
                        column: x => x.flight_segment_id,
                        principalTable: "flight_segments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tickets_passengers_passenger_id",
                        column: x => x.passenger_id,
                        principalTable: "passengers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_verifications_customer_id",
                table: "account_verifications",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_aerodromes_city_id",
                table: "aerodromes",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_air_taxi_bases_aerodrome_id",
                table: "air_taxi_bases",
                column: "aerodrome_id");

            migrationBuilder.CreateIndex(
                name: "IX_air_taxi_bases_air_taxi_id",
                table: "air_taxi_bases",
                column: "air_taxi_id");

            migrationBuilder.CreateIndex(
                name: "IX_aircraft_images_aircraft_id",
                table: "aircraft_images",
                column: "aircraft_id");

            migrationBuilder.CreateIndex(
                name: "IX_aircraft_models_manufacturer_id",
                table: "aircraft_models",
                column: "manufacturer_id");

            migrationBuilder.CreateIndex(
                name: "IX_aircrafts_air_taxi_id",
                table: "aircrafts",
                column: "air_taxi_id");

            migrationBuilder.CreateIndex(
                name: "IX_aircrafts_aircraft_model_id",
                table: "aircrafts",
                column: "aircraft_model_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_payments_booking_id",
                table: "booking_payments",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_status_booking_id",
                table: "booking_status",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_customer_id",
                table: "bookings",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_flight_segment_id",
                table: "bookings",
                column: "flight_segment_id");

            migrationBuilder.CreateIndex(
                name: "IX_cities_state_id",
                table: "cities",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_cities_timezone_id",
                table: "cities",
                column: "timezone_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_addresses_customer_id",
                table: "customer_addresses",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_aerodromes_aerodrome_id",
                table: "customer_aerodromes",
                column: "aerodrome_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_aerodromes_customer_id",
                table: "customer_aerodromes",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_cards_customer_id",
                table: "customer_cards",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_friends_customer_id",
                table: "customer_friends",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_segments_aircraft_id",
                table: "flight_segments",
                column: "aircraft_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_segments_destination_aerodrome_id",
                table: "flight_segments",
                column: "destination_aerodrome_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_segments_flight_id",
                table: "flight_segments",
                column: "flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_segments_origin_aerodrome_id",
                table: "flight_segments",
                column: "origin_aerodrome_id");

            migrationBuilder.CreateIndex(
                name: "IX_flights_air_taxi_id",
                table: "flights",
                column: "air_taxi_id");

            migrationBuilder.CreateIndex(
                name: "IX_flights_aircraft_id",
                table: "flights",
                column: "aircraft_id");

            migrationBuilder.CreateIndex(
                name: "IX_passengers_booking_id",
                table: "passengers",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_passengers_flight_segment_id",
                table: "passengers",
                column: "flight_segment_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_flight_segment_id",
                table: "tickets",
                column: "flight_segment_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_passenger_id",
                table: "tickets",
                column: "passenger_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_air_taxi_id",
                table: "users",
                column: "air_taxi_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_tokens");

            migrationBuilder.DropTable(
                name: "account_verifications");

            migrationBuilder.DropTable(
                name: "air_taxi_bases");

            migrationBuilder.DropTable(
                name: "aircraft_images");

            migrationBuilder.DropTable(
                name: "booking_payments");

            migrationBuilder.DropTable(
                name: "booking_status");

            migrationBuilder.DropTable(
                name: "customer_addresses");

            migrationBuilder.DropTable(
                name: "customer_aerodromes");

            migrationBuilder.DropTable(
                name: "customer_cards");

            migrationBuilder.DropTable(
                name: "customer_friends");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "passengers");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "flight_segments");

            migrationBuilder.DropTable(
                name: "aerodromes");

            migrationBuilder.DropTable(
                name: "flights");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "aircrafts");

            migrationBuilder.DropTable(
                name: "states");

            migrationBuilder.DropTable(
                name: "timezones");

            migrationBuilder.DropTable(
                name: "air_taxis");

            migrationBuilder.DropTable(
                name: "aircraft_models");

            migrationBuilder.DropTable(
                name: "manufacturers");
        }
    }
}
