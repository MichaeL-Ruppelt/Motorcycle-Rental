using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorCycleRentail.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "couriers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", fixedLength: true, maxLength: 36, nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    identifier = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(14)", unicode: false, maxLength: 14, nullable: false),
                    birthdate = table.Column<DateTime>(type: "DATE", nullable: false),
                    cnh_number = table.Column<string>(type: "character varying(11)", unicode: false, maxLength: 11, nullable: false),
                    cnh_type = table.Column<string>(type: "character varying(2)", unicode: false, maxLength: 2, nullable: false),
                    cnh_image_id = table.Column<string>(type: "text", unicode: false, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_couriers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "motorcycles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", fixedLength: true, maxLength: 36, nullable: false),
                    identifier = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    model = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: false),
                    license_plate = table.Column<string>(type: "character varying(7)", unicode: false, maxLength: 7, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motorcycles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rental_plan",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", fixedLength: true, maxLength: 36, nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    plan_days = table.Column<int>(type: "integer", nullable: false),
                    plan_value = table.Column<decimal>(type: "money", precision: 18, scale: 2, nullable: false),
                    fine_value = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_plan", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rentals",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", fixedLength: true, maxLength: 36, nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    plan_id = table.Column<int>(type: "integer", nullable: false),
                    courier_identifier = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    motorcycle_identifier = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    preview_end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    return_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentals", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_couriers_cnh_number",
                table: "couriers",
                column: "cnh_number");

            migrationBuilder.CreateIndex(
                name: "IX_couriers_is_deleted",
                table: "couriers",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_motorcycles_is_deleted",
                table: "motorcycles",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_rental_plan_is_deleted",
                table: "rental_plan",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_rental_plan_plan_days",
                table: "rental_plan",
                column: "plan_days");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_is_deleted",
                table: "rentals",
                column: "is_deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "couriers");

            migrationBuilder.DropTable(
                name: "motorcycles");

            migrationBuilder.DropTable(
                name: "rental_plan");

            migrationBuilder.DropTable(
                name: "rentals");
        }
    }
}
