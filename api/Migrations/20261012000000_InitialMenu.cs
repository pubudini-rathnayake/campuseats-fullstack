using CampusEats.Api;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable
namespace CampusEats.Api.Migrations;

[DbContext(typeof(CampusDbContext))]
[Migration("20261012000000_InitialMenu")]
public partial class InitialMenu : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "MenuItems",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false).Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false),
                Category = table.Column<string>(type: "text", nullable: false),
                Price = table.Column<int>(type: "integer", nullable: false),
                Description = table.Column<string>(type: "text", nullable: false),
                Emoji = table.Column<string>(type: "text", nullable: false),
                Featured = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_MenuItems", x => x.Id); });
    }
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(name: "MenuItems");
}
