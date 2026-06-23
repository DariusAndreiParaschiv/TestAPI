using Microsoft.EntityFrameworkCore;

using (var context = new MyDBContext())
{
    var newItem1 = new Item()
    {
        Id = 1,
        Name = "Item 1"
    };

    context.Items.Add(newItem1);

    var newItem2 = new Item()
    {
        Id = 2,
        Name = "Item2"
    };

    context.Items.Add(newItem2);

    var newItem3 = new Item()
    {
        Id = 3,
        Name = "Item 3"
    };

    context.Items.Add(newItem3);
    context.SaveChanges();

    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();

    var item = app.MapGroup("/item");
    item.MapGet("/name/{name}", (string name) =>
    {
        return context.Items.FirstOrDefault(i => i.Name == name);
    });
    item.MapGet("/id/{id}", (int id) =>
    {
        return context.Items.FirstOrDefault(i => i.Id == id);
    });
    item.MapGet("/add", (int id, string name) =>
    {
        var newItem = new Item()
        {
            Id = id,
            Name = name
        };
        context.Items.Add(newItem);
        context.SaveChanges();
        return newItem;
    });
    item.MapGet("/remove", (int id) =>
    {
        var removed = context.Items.FirstOrDefault(item => item.Id == id);
        if (removed != null)
        {
            context.Items.Remove(removed);
            context.SaveChanges();
            return "Removed";
        }
        else
        {
            return "Not found";
        }
    });

    app.MapGet("/items", () => context.Items);

    app.Run();
}

class Item
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

class MyDBContext : DbContext
{
    public MyDBContext()
    {
    }
    public DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("ItemsDB");
    }
}