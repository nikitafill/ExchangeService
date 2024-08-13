using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rate",
                columns: table => new
                {
                    Cur_ID = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Cur_Abbreviation = table.Column<string>(type: "text", nullable: false),
                    Cur_Scale = table.Column<int>(type: "integer", nullable: false),
                    Cur_Name = table.Column<string>(type: "text", nullable: false),
                    Cur_OfficialRate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rate", x => new { x.Date, x.Cur_ID });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rate");
        }
    }
}
