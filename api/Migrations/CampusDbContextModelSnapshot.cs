using CampusEats.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable
namespace CampusEats.Api.Migrations;

[DbContext(typeof(CampusDbContext))]
public partial class CampusDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "8.0.16")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);
        modelBuilder.Entity("CampusEats.Api.MenuItem", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("integer")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
            b.Property<string>("Category").IsRequired().HasColumnType("text");
            b.Property<string>("Description").IsRequired().HasColumnType("text");
            b.Property<string>("Emoji").IsRequired().HasColumnType("text");
            b.Property<bool>("Featured").HasColumnType("boolean");
            b.Property<string>("Name").IsRequired().HasColumnType("text");
            b.Property<int>("Price").HasColumnType("integer");
            b.HasKey("Id");
            b.ToTable("MenuItems");
        });
    }
}
