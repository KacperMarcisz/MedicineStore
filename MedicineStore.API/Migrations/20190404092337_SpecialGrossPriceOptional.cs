using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicineStore.API.Migrations
{
    public partial class SpecialGrossPriceOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SpecialGrossPrice",
                table: "Medicines",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SpecialGrossPrice",
                table: "Medicines",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
