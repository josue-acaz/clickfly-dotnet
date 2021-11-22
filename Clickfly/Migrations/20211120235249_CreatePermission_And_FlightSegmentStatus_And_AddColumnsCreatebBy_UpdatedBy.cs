using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace clickfly.Migrations
{
    public partial class CreatePermission_And_FlightSegmentStatus_And_AddColumnsCreatebBy_UpdatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "timezones",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "timezones",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "tickets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "tickets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "states",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "states",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "passengers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "passengers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "manufacturers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "manufacturers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "flights",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "flights",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "flight_segments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "flight_segments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "files",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "files",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "customer_friends",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "customer_friends",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "customer_cards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "customer_cards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "customer_aerodromes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "customer_aerodromes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "customer_addresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "customer_addresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "cities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "cities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "bookings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "bookings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "booking_status",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "booking_status",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "booking_payments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "booking_payments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "aircrafts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "aircrafts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "aircraft_models",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "aircraft_models",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "aircraft_images",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "aircraft_images",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "air_taxis",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "air_taxis",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "air_taxi_bases",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "air_taxi_bases",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "aerodromes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "aerodromes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "account_verifications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "account_verifications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "access_tokens",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "access_tokens",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "flight_segment_status",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: true),
                    annotation = table.Column<string>(type: "text", nullable: true),
                    flight_segment_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_segment_status", x => x.id);
                    table.ForeignKey(
                        name: "FK_flight_segment_status_flight_segments_flight_segment_id",
                        column: x => x.flight_segment_id,
                        principalTable: "flight_segments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "permission_resources",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    table = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission_resources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permission_groups",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: true),
                    user_role_id = table.Column<string>(type: "text", nullable: true),
                    allowed = table.Column<bool>(type: "boolean", nullable: false),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission_groups", x => x.id);
                    table.ForeignKey(
                        name: "FK_permission_groups_user_roles_user_role_id",
                        column: x => x.user_role_id,
                        principalTable: "user_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_permission_groups_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    create = table.Column<bool>(type: "boolean", nullable: false),
                    read = table.Column<bool>(type: "boolean", nullable: false),
                    update = table.Column<bool>(type: "boolean", nullable: false),
                    delete = table.Column<bool>(type: "boolean", nullable: false),
                    permission_group_id = table.Column<string>(type: "text", nullable: true),
                    permission_resource_id = table.Column<string>(type: "text", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_permissions_permission_groups_permission_group_id",
                        column: x => x.permission_group_id,
                        principalTable: "permission_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_permissions_permission_resources_permission_resource_id",
                        column: x => x.permission_resource_id,
                        principalTable: "permission_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_flight_segment_status_flight_segment_id",
                table: "flight_segment_status",
                column: "flight_segment_id");

            migrationBuilder.CreateIndex(
                name: "IX_permission_groups_user_id",
                table: "permission_groups",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_permission_groups_user_role_id",
                table: "permission_groups",
                column: "user_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_permission_group_id",
                table: "permissions",
                column: "permission_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_permission_resource_id",
                table: "permissions",
                column: "permission_resource_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "flight_segment_status");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "permission_groups");

            migrationBuilder.DropTable(
                name: "permission_resources");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "users");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "timezones");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "timezones");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "states");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "states");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "passengers");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "passengers");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "manufacturers");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "manufacturers");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "flights");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "flights");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "flight_segments");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "flight_segments");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "files");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "files");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "customer_friends");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "customer_friends");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "customer_cards");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "customer_cards");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "customer_aerodromes");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "customer_aerodromes");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "customer_addresses");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "customer_addresses");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "cities");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "cities");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "booking_status");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "booking_status");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "booking_payments");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "booking_payments");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "aircrafts");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "aircrafts");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "aircraft_models");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "aircraft_models");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "aircraft_images");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "aircraft_images");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "air_taxis");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "air_taxis");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "air_taxi_bases");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "air_taxi_bases");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "aerodromes");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "aerodromes");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "account_verifications");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "account_verifications");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "access_tokens");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "access_tokens");
        }
    }
}
