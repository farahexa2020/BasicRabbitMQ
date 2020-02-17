using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroRabbit.Transfer.Data.Migrations
{
    public partial class UpdateTranferLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountBalance",
                table: "TransferLog");

            migrationBuilder.AddColumn<decimal>(
                name: "TransferAmount",
                table: "TransferLog",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferAmount",
                table: "TransferLog");

            migrationBuilder.AddColumn<decimal>(
                name: "AccountBalance",
                table: "TransferLog",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
