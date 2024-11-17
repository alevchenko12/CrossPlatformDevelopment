using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Transactions;
using LAB6.Models;

namespace LAB6.Models.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<RefProductType> RefProductTypes { get; set; }
        public DbSet<RefColour> RefColours { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductColour> ProductColours { get; set; }
        public DbSet<Retailer> Retailers { get; set; }
        public DbSet<RetailerProductPrice> RetailerProductPrices { get; set; }
        public DbSet<SpecialOffer> SpecialOffers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<CustomerOrderProduct> CustomerOrderProducts { get; set; }
        public DbSet<CustomerOrderSpecialOffer> CustomerOrderSpecialOffers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure primary keys
            modelBuilder.Entity<Brand>().HasKey(b => b.BrandId);
            modelBuilder.Entity<RefProductType>().HasKey(r => r.ProductTypeCode);
            modelBuilder.Entity<RefColour>().HasKey(r => r.ColourCode);
            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
            modelBuilder.Entity<ProductColour>().HasKey(pc => new { pc.ProductId, pc.ColourCode });
            modelBuilder.Entity<Retailer>().HasKey(r => r.RetailerId);
            modelBuilder.Entity<RetailerProductPrice>().HasKey(rp => new { rp.ProductId, rp.RetailerId });
            modelBuilder.Entity<SpecialOffer>().HasKey(so => so.SpecialOfferId);
            modelBuilder.Entity<Customer>().HasKey(c => c.CustomerId);
            modelBuilder.Entity<CustomerOrder>().HasKey(co => co.OrderId);
            modelBuilder.Entity<CustomerOrderProduct>().HasKey(cop => new { cop.OrderId, cop.ProductId, cop.RetailerId });
            modelBuilder.Entity<CustomerOrderSpecialOffer>().HasKey(cos => new { cos.OrderId, cos.SpecialOfferId });

            // Configure relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.RefProductType)
                .WithMany(rpt => rpt.Products)
                .HasForeignKey(p => p.ProductTypeCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductColour>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductColours)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductColour>()
                .HasOne(pc => pc.RefColour)
                .WithMany(rc => rc.ProductColours)
                .HasForeignKey(pc => pc.ColourCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RetailerProductPrice>()
                .HasOne(rp => rp.Product)
                .WithMany(p => p.RetailerProductPrices)
                .HasForeignKey(rp => rp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RetailerProductPrice>()
                .HasOne(rp => rp.Retailer)
                .WithMany(r => r.RetailerProductPrices)
                .HasForeignKey(rp => rp.RetailerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SpecialOffer>()
                .HasOne(so => so.Product)
                .WithMany(p => p.SpecialOffers)
                .HasForeignKey(so => so.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SpecialOffer>()
                .HasOne(so => so.Retailer)
                .WithMany(r => r.SpecialOffers)
                .HasForeignKey(so => so.RetailerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerOrder>()
                .HasOne(co => co.Customer)
                .WithMany(c => c.CustomerOrders)
                .HasForeignKey(co => co.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerOrderProduct>()
                .HasOne(cop => cop.CustomerOrder)
                .WithMany(co => co.CustomerOrderProducts)
                .HasForeignKey(cop => cop.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerOrderProduct>()
                .HasOne(cop => cop.Product)
                .WithMany(p => p.CustomerOrderProducts)
                .HasForeignKey(cop => cop.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerOrderProduct>()
                .HasOne(cop => cop.Retailer)
                .WithMany(r => r.CustomerOrderProducts)
                .HasForeignKey(cop => cop.RetailerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerOrderSpecialOffer>()
                .HasOne(cos => cos.CustomerOrder)
                .WithMany(co => co.CustomerOrderSpecialOffers)
                .HasForeignKey(cos => cos.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerOrderSpecialOffer>()
                .HasOne(cos => cos.SpecialOffer)
                .WithMany(so => so.CustomerOrderSpecialOffers)
                .HasForeignKey(cos => cos.SpecialOfferId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Product>().Property(p => p.ProductId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Retailer>().Property(r => r.RetailerId).ValueGeneratedOnAdd();
            modelBuilder.Entity<SpecialOffer>().Property(so => so.SpecialOfferId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Customer>().Property(c => c.CustomerId).ValueGeneratedOnAdd();
            modelBuilder.Entity<CustomerOrder>().Property(co => co.OrderId).ValueGeneratedOnAdd();
        }

        public void Seed()
        {
            if (Brands.Any()) return;

            // 1. Seed Reference Tables
            var refProductTypes = new List<RefProductType>
    {
        new RefProductType { ProductTypeCode = "ELEC", ParentProductCode = null, ProductTypeDescription = "Electronics" },
        new RefProductType { ProductTypeCode = "FOOD", ParentProductCode = null, ProductTypeDescription = "Food" },
        new RefProductType { ProductTypeCode = "TV", ParentProductCode = "ELEC", ProductTypeDescription = "Television" },
    };
            RefProductTypes.AddRange(refProductTypes);
            SaveChanges();

            var refColours = new List<RefColour>
    {
        new RefColour { ColourCode = "BLK", ColourDescription = "Black" },
        new RefColour { ColourCode = "WHT", ColourDescription = "White" },
        new RefColour { ColourCode = "RED", ColourDescription = "Red" }
    };
            RefColours.AddRange(refColours);
            SaveChanges();

            // 2. Seed Brands
            var brands = new List<Brand>
    {
        new Brand { BrandName = "Sony", BrandDescription = "Electronics Giant", OtherBrandDetails = "Known for TVs and Cameras" },
        new Brand { BrandName = "Samsung", BrandDescription = "Korean Tech Leader", OtherBrandDetails = "Leader in Smartphones and Appliances" }
    };
            Brands.AddRange(brands);
            SaveChanges();

            // 3. Seed Retailers
            var retailers = new List<Retailer>
    {
        new Retailer { RetailerName = "Best Buy", RetailerAddress = "123 Tech Street", RetailerWebSiteUrl = "www.bestbuy.com", OtherRetailerDetails = "Leading Tech Retailer" },
        new Retailer { RetailerName = "Amazon", RetailerAddress = "Online", RetailerWebSiteUrl = "www.amazon.com", OtherRetailerDetails = "E-commerce Giant" }
    };
            Retailers.AddRange(retailers);
            SaveChanges();

            // 4. Seed Products
            var products = new List<Product>
    {
        new Product { BrandId = brands[0].BrandId, ProductTypeCode = "TV", ProductName = "Sony Bravia", OtherProductDetails = "4K HDR TV" },
        new Product { BrandId = brands[1].BrandId, ProductTypeCode = "ELEC", ProductName = "Samsung Galaxy", OtherProductDetails = "Smartphone" }
    };
            Products.AddRange(products);
            SaveChanges();

            // 5. Seed Product Colours
            var productColours = new List<ProductColour>
    {
        new ProductColour { ProductId = products[0].ProductId, ColourCode = "BLK", Availability = true },
        new ProductColour { ProductId = products[1].ProductId, ColourCode = "RED", Availability = true }
    };
            ProductColours.AddRange(productColours);
            SaveChanges();

            // 6. Seed Retailer Product Prices
            var retailerProductPrices = new List<RetailerProductPrice>
    {
        new RetailerProductPrice { ProductId = products[0].ProductId, RetailerId = retailers[0].RetailerId, MinPrice = 500.00M, MaxPrice = 700.00M },
        new RetailerProductPrice { ProductId = products[1].ProductId, RetailerId = retailers[1].RetailerId, MinPrice = 1000.00M, MaxPrice = 1200.00M }
    };
            RetailerProductPrices.AddRange(retailerProductPrices);
            SaveChanges();

            // 7. Seed Special Offers
            var specialOffers = new List<SpecialOffer>
    {
        new SpecialOffer
        {
            ProductId = products[0].ProductId,
            RetailerId = retailers[0].RetailerId,
            StartDate = DateTime.Now.AddDays(-10),
            EndDate = DateTime.Now.AddDays(5),
            MinPrice = 450.00M,
            MaxPrice = 600.00M,
            TermsAndConditions = "Limited Time Offer",
            OtherDetails = "Discounted Price for Sony Bravia"
        }
    };
            SpecialOffers.AddRange(specialOffers);
            SaveChanges();

            // 8. Seed Customers
            var customers = new List<Customer>
    {
        new Customer
        {
            PaymentMethodCode = "CARD",
            CustomerCode = "CUST001",
            CustomerName = "John Doe",
            CustomerAddress = "456 Main Street",
            CustomerEmail = "john.doe@example.com",
            CustomerPhone = "123-456-7890",
            OtherCustomerDetails = "Frequent Buyer"
        }
    };
            Customers.AddRange(customers);
            SaveChanges();

            // 9. Seed Customer Orders
            var customerOrders = new List<CustomerOrder>
    {
        new CustomerOrder
        {
            CustomerId = customers[0].CustomerId,
            OrderStatusCode = "PLACED",
            OrderDate = DateTime.Now.AddDays(-2),
            OrderDetails = "Sony Bravia TV"
        }
    };
            CustomerOrders.AddRange(customerOrders);
            SaveChanges();

            // 10. Seed Customer Order Products
            var customerOrderProducts = new List<CustomerOrderProduct>
    {
        new CustomerOrderProduct
        {
            OrderId = customerOrders[0].OrderId,
            ProductId = products[0].ProductId,
            RetailerId = retailers[0].RetailerId,
            Quantity = 1,
            OtherDetails = "Delivery expected in 5 days"
        }
    };
            CustomerOrderProducts.AddRange(customerOrderProducts);
            SaveChanges();

            // 11. Seed Customer Orders Special Offers
            var customerOrdersSpecialOffers = new List<CustomerOrderSpecialOffer>
    {
        new CustomerOrderSpecialOffer
        {
            OrderId = customerOrders[0].OrderId,
            SpecialOfferId = specialOffers[0].SpecialOfferId,
            DateOrderPlaced = DateTime.Now.AddDays(-2),
            Quantity = 1,
            TotalValue = 450.00M,
            OrderDetails = "Discounted price applied"
        }
    };
            CustomerOrderSpecialOffers.AddRange(customerOrdersSpecialOffers);
            SaveChanges();
        }
    }
}
