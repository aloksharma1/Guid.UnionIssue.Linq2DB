using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using UnionIssue.Linq2DB.Model;
using UnionIssue.Linq2DB.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();


app.MapGet("/customers", async (AppDbContext db) => await db.Customers.ToListAsync());

app.MapGet("/orders", async (AppDbContext db) => await db.Orders.ToListAsync());

app.MapGet("/products", async (AppDbContext db) => await db.Products.ToListAsync());

app.MapPost("/customers", async (AppDbContext db, Customer customer) =>
{
    db.Customers.Add(customer);
    await db.SaveChangesAsync();
    return Results.Created($"/customers/{customer.Id}", customer);
});

app.MapGet("/linq2db/customers", async (AppDbContext db) =>
{
    var customers = await db.Customers.ToLinqToDB().ToListAsync();
    return Results.Ok(customers);
});
app.MapGet("/union-results-1", async (AppDbContext db) =>
{
    

    // Query to combine Customers, Orders, and Products into CombinedResults
    var customerResults = db.Customers
        .Select(c => new CombinedResults
        {
            Id = "" + c.Id,
            Name = c.Name
        });

    var orderResults = db.Orders
        .Select(o => new CombinedResults
        {
            Id ="" + o.Id,
            Name = o.ProductName
        });

    var productResults = db.Products
        .Select(p => new CombinedResults
        {
            Id = "" + p.Id,
            Name = p.Name
        });

    // Union of all three results
    var combinedResults = customerResults
        .Union(orderResults)
        .Union(productResults);

    // Convert to list and return the results
    var result = await combinedResults.ToLinqToDB().ToListAsync();
    return Results.Ok(result);
});

app.MapGet("/", () => "Hello World!");

app.Run();
