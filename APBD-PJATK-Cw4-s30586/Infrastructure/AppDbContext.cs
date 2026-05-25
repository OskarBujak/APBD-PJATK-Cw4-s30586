using System;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<PC> PCs { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<PCComponent> PCComponents { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PC>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Weight).HasColumnType("float(5)").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.Warranty).IsRequired();
            entity.Property(e => e.Stock).IsRequired();
        });

        modelBuilder.Entity<ComponentManufacturer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).HasMaxLength(30).IsRequired();
            entity.Property(e => e.FullName).HasMaxLength(300).IsRequired();
            entity.Property(e => e.FoundationDate).HasColumnType("date").IsRequired();
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasColumnType("char(10)").IsRequired();
            entity.Property(e => e.Name).HasMaxLength(300).IsRequired();
            entity.Property(e => e.Description).HasColumnType("nvarchar(max)").IsRequired();
            
            entity.HasOne(e => e.ComponentManufacturer)
                  .WithMany(m => m.Components)
                  .HasForeignKey(e => e.ComponentManufacturersId);
                  
            entity.HasOne(e => e.ComponentType)
                  .WithMany(t => t.Components)
                  .HasForeignKey(e => e.ComponentTypesId);
        });

        modelBuilder.Entity<PCComponent>(entity =>
        {
            entity.HasKey(e => new { e.PCId, e.ComponentCode });
            
            entity.Property(e => e.ComponentCode).HasColumnType("char(10)").IsRequired();

            entity.HasOne(e => e.PC)
                  .WithMany(p => p.PCComponents)
                  .HasForeignKey(e => e.PCId);
                  
            entity.HasOne(e => e.Component)
                  .WithMany(c => c.PCComponents)
                  .HasForeignKey(e => e.ComponentCode);
        });

        
        //tu wygenerowałem ai :)
        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer { Id = 1, Abbreviation = "AMD", FullName = "Advanced Micro Devices", FoundationDate = new DateOnly(1969, 5, 1) },
            new ComponentManufacturer { Id = 2, Abbreviation = "NV", FullName = "NVIDIA Corporation", FoundationDate = new DateOnly(1993, 4, 5) },
            new ComponentManufacturer { Id = 3, Abbreviation = "COR", FullName = "Corsair Gaming Inc.", FoundationDate = new DateOnly(1994, 1, 1) }
        );

        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor" },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory" }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component { Code = "CPU0000001", Name = "Ryzen 7 7800X3D", Description = "8-core gaming processor", ComponentManufacturersId = 1, ComponentTypesId = 1 },
            new Component { Code = "GPU0000001", Name = "RTX 4080 Super", Description = "High-end gaming graphics card", ComponentManufacturersId = 2, ComponentTypesId = 2 },
            new Component { Code = "RAM0000001", Name = "Corsair Vengeance DDR5 16GB", Description = "DDR5 RAM module 16GB", ComponentManufacturersId = 3, ComponentTypesId = 3 }
        );

        modelBuilder.Entity<PC>().HasData(
            new PC { Id = 1, Name = "Gaming Beast X", Weight = 12.5f, Warranty = 36, CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0), Stock = 5 },
            new PC { Id = 2, Name = "Office Mini Pro", Weight = 4.2f, Warranty = 24, CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0), Stock = 12 },
            new PC { Id = 3, Name = "Budget Build", Weight = 7.0f, Warranty = 24, CreatedAt = new DateTime(2026, 5, 10, 10, 0, 0), Stock = 8 }
        );

        modelBuilder.Entity<PCComponent>().HasData(
            new PCComponent { PCId = 1, ComponentCode = "CPU0000001", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "GPU0000001", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "RAM0000001", Amount = 2 },
            new PCComponent { PCId = 2, ComponentCode = "CPU0000001", Amount = 1 },
            new PCComponent { PCId = 3, ComponentCode = "RAM0000001", Amount = 1 }
        );
    }
}