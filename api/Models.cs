using Microsoft.EntityFrameworkCore;

namespace CampusEats.Api;

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public int Price { get; set; }
    public string Description { get; set; } = "";
    public string Emoji { get; set; } = "";
    public bool Featured { get; set; }
}

public sealed class CampusDbContext(DbContextOptions<CampusDbContext> options) : DbContext(options)
{
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
}

public static class DemoData
{
    public static readonly List<MenuItem> Items = new()
    {
        new() { Id = 1, Name = "Kottu Roti", Category = "Local Favourites", Price = 750, Description = "Chopped roti stir-fried with vegetables, egg and aromatic spices.", Emoji = "🍛", Featured = true },
        new() { Id = 2, Name = "Chicken Fried Rice", Category = "Rice & Bowls", Price = 850, Description = "Fluffy fragrant rice, tender chicken, garden vegetables and a little spice.", Emoji = "🍚", Featured = true },
        new() { Id = 3, Name = "Watalappan", Category = "Sweet Treats", Price = 350, Description = "Creamy Sri Lankan jaggery and coconut custard with warm spices.", Emoji = "🍮" },
        new() { Id = 4, Name = "Vegetable Noodles", Category = "Rice & Bowls", Price = 650, Description = "Wok-tossed noodles with crunchy seasonal vegetables.", Emoji = "🍜" },
        new() { Id = 5, Name = "Egg Hoppers", Category = "Local Favourites", Price = 300, Description = "Crispy rice-flour hoppers with a golden egg centre.", Emoji = "🥚" },
        new() { Id = 6, Name = "Iced Milo", Category = "Drinks", Price = 280, Description = "Chilled cocoa malt drink served over ice.", Emoji = "🥤" }
    };
}
