using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionIssue.Linq2DB.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Products
            var product1 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Laptop",
                Price = 1200.00m
            };
            var product2 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Smartphone",
                Price = 800.00m
            };

            modelBuilder.Entity<Product>().HasData(product1, product2);

            // Seed Customers
            var customer1 = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "John Doe"
            };
            var customer2 = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Jane Doe"
            };

            modelBuilder.Entity<Customer>().HasData(customer1, customer2);

            // Seed Orders
            var order1 = new Order
            {
                Id = Guid.NewGuid(),
                ProductName = product1.Name,
                Quantity = 2,
                CustomerId = customer1.Id
            };
            var order2 = new Order
            {
                Id = Guid.NewGuid(),
                ProductName = product2.Name,
                Quantity = 1,
                CustomerId = customer2.Id
            };

            modelBuilder.Entity<Order>().HasData(order1, order2);
        }
    }
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }

}
