using Microsoft.EntityFrameworkCore;
using RentACarProject.Domain.Common;
using RentACarProject.Domain.Entities;
using System.Linq.Expressions;

namespace RentACarProject.Persistence.Context
{
    public class RentACarDbContext : DbContext
    {
        public Guid? CurrentUserId { get; set; }

        public RentACarDbContext(DbContextOptions<RentACarDbContext> options)
            : base(options)
        {
        }

        // 🟢 DbSet'ler
        public DbSet<User> Users => Set<User>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Model> Models => Set<Model>();
        public DbSet<Log> Logs => Set<Log>();
        public DbSet<Car> Cars => Set<Car>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Global Soft Delete Filter
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(RentACarDbContext).GetMethod(nameof(GetIsDeletedFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                        .MakeGenericMethod(entityType.ClrType);
                    var filter = method.Invoke(null, Array.Empty<object>());
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter((LambdaExpression)filter!);
                }
            }

            // User
            modelBuilder.Entity<User>().Property(u => u.UserName).HasMaxLength(30);
            modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasMaxLength(255);
            modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(100);

            // Customer
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Customer>().Property(c => c.FirstName).HasMaxLength(30);
            modelBuilder.Entity<Customer>().Property(c => c.LastName).HasMaxLength(30);
            modelBuilder.Entity<Customer>().Property(c => c.Email).HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.Phone).HasMaxLength(15);

            // Brand
            modelBuilder.Entity<Brand>().Property(b => b.Name).HasMaxLength(50);

            // Model
            modelBuilder.Entity<Model>().Property(m => m.Name).HasMaxLength(50);
            modelBuilder.Entity<Brand>()
                .HasMany(b => b.Models)
                .WithOne(m => m.Brand)
                .HasForeignKey(m => m.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Model>()
                .HasMany(m => m.Cars)
                .WithOne(c => c.Model)
                .HasForeignKey(c => c.ModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Car
            modelBuilder.Entity<Car>().Property(c => c.Plate).HasMaxLength(450);
            modelBuilder.Entity<Car>().Property(c => c.DailyPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Car>().Property(c => c.Description).HasMaxLength(500);
            modelBuilder.Entity<Car>().HasIndex(c => c.Plate).IsUnique();

            modelBuilder.Entity<Car>()
                .Property(c => c.FuelType)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Car>()
                .Property(c => c.TransmissionType)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Car>()
                .Property(c => c.VehicleClass)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Car>()
                .Property(c => c.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Reservation
            modelBuilder.Entity<Reservation>().Property(r => r.TotalPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Payment
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Payment>().Property(p => p.PaymentType).HasMaxLength(20);
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Reservation)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Location – Reservation
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.PickupLocation)
                .WithMany(l => l.PickupReservations)
                .HasForeignKey(r => r.PickupLocationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.DropoffLocation)
                .WithMany(l => l.DropoffReservations)
                .HasForeignKey(r => r.DropoffLocationId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }

        private static LambdaExpression GetIsDeletedFilter<TEntity>() where TEntity : BaseEntity
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
            return filter;
        }

        public override int SaveChanges()
        {
            UpdateBaseEntityFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateBaseEntityFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateBaseEntityFields()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.CreatedByUserId = CurrentUserId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedDate = DateTime.UtcNow;
                    entry.Entity.ModifiedByUserId = CurrentUserId;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedDate = DateTime.UtcNow;
                    entry.Entity.ModifiedByUserId = CurrentUserId;
                }
            }
        }
    }
}
