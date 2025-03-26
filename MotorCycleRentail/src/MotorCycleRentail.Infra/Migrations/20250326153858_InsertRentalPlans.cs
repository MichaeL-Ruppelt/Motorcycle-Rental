using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorCycleRentail.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InsertRentalPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Seed(migrationBuilder);
        }

        private static void Seed(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "rental_plan",
                columns: new[] { "id", "created_at", "update_at", "plan_days", "plan_value", "fine_value", "is_deleted" },
                values: new object[,]
                {
                    { Guid.NewGuid().ToString(), DateTime.Now, DateTime.Now, 7, 30.00, 0.2, false },
                    { Guid.NewGuid().ToString(), DateTime.Now, DateTime.Now, 15, 28.00, 0.4, false },
                    { Guid.NewGuid().ToString(), DateTime.Now, DateTime.Now, 30, 22.00, 0.4, false },
                    { Guid.NewGuid().ToString(), DateTime.Now, DateTime.Now, 45, 20.00, 0.4, false },
                    { Guid.NewGuid().ToString(), DateTime.Now, DateTime.Now, 50, 28.00, 0.4, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "rental_plan",
                keyColumn: "plan_days",
                keyValues: new object[] { 7, 15, 30, 45, 50 }
                );
        }
    }
}
