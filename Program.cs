var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
List<Item> items = new List<Item>()
{
    new Item()
    { 
        Id = 1,
        Name = "Item 1"
    },
    new Item
    {
        Id = 2,
        Name = "Item 2"
    },
    new Item
    {
        Id = 3,
        Name = "Item 3"
    }
};
var item = app.MapGroup("/item");
item.MapGet("/name/{name}", (string name) =>
{
    return items.FirstOrDefault(i => i.Name == name);
});
item.MapGet("/id/{id}", (int id) =>
{
    return items.FirstOrDefault(i => i.Id == id);
});
item.MapGet("/add", (int id, string name) =>
{
    var newItem = new Item()
    {
        Id = id,
        Name = name
    };
    items.Add(newItem);
    return newItem;
});
item.MapGet("/remove", (int id) =>
{
    var removed = items.FirstOrDefault(item => item.Id == id);
    if (removed != null)
    {
        items.Remove(removed);
        return "Removed";
    }
    else
    {
        return "Not found";
    }   
});

app.MapGet("/items", () => items);

app.Run();

class Item
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

