using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesMine.Catalog.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbCatalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    Image = table.Column<string>(type: "varchar(250)", nullable: false),
                    QtyInStock = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCatalog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbCatalog");
        }
    }
}
