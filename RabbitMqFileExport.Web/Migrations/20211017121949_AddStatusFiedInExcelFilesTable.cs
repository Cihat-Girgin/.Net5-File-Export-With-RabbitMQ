using Microsoft.EntityFrameworkCore.Migrations;

namespace RabbitMqFileExport.Web.Migrations
{
    public partial class AddStatusFiedInExcelFilesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "ExcelFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "ExcelFiles");
        }
    }
}
