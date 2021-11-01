using Microsoft.EntityFrameworkCore.Migrations;

namespace clickfly.Migrations
{
    public partial class add_flight_time_to_flight_segments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "flight_time",
                table: "flight_segments",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "customers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "flight_time",
                table: "flight_segments");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "customers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
