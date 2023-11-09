using Humanizer.Bytes;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Models;
using System.Text;

namespace ShowroomManagement.Data
{
    public class ShowroomContext : DbContext
    {
        public DbSet<Products> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Device> Devices { get; set; }

        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesTarget> SalesTargets { get; set; }

        public DbSet<TestDrive> TestDrives { get; set; }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<Tasks> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Products>().ToTable("Products");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<PurchaseInvoice>().ToTable("PurchaseInvoices");
            modelBuilder.Entity<SalesInvoice>().ToTable("SalesInvoices");
            modelBuilder.Entity<SalesTarget>().ToTable("SalesTargets");
            modelBuilder.Entity<TestDrive>().ToTable("TestDrive");
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<ProductImages>().ToTable("Product_Images");
            modelBuilder.Entity<Tasks>().ToTable("Tasks");
        }

        public ShowroomContext(DbContextOptions<ShowroomContext> options)
            : base(options)
        {
            // TODO: Config the Context.
        }

        public DbSet<ShowroomManagement.Models.Customer> Customer { get; set; }

        public DbSet<ShowroomManagement.Models.Source> Source { get; set; }

        [DbFunction("login_check")]
        public static bool CheckLogin(string username,
            string password)
            => throw new Exception();
        
    }
}
