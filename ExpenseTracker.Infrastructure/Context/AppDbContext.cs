using ExprenseTracker.Domain;
using ExprenseTracker.Domain.Entities;
using ExprenseTracker.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Context;

public class AppDbContext: DbContext
{
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<ExpenseItem> ExpenseItems => Set<ExpenseItem>();
    public AppDbContext( DbContextOptions<AppDbContext> options ): base(options){}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(e =>
        {

            e.Property(ex => ex.Category).HasConversion<string>().IsRequired();
            
            e.HasKey(ex => ex.Id);
            e.ComplexProperty(ex => ex.Price, p =>
            {
                p.Property(pr => pr.Currency).HasConversion<string>().IsRequired();
            });
        });
        
        modelBuilder.Entity<ExpenseItem>(e =>
        {

            e.Property(ex => ex.Category).HasConversion<string>().IsRequired();
            
            e.HasKey(ex => ex.Id);
            e.ComplexProperty(ex => ex.Price, p =>
            {
                p.Property(pr => pr.Currency).HasConversion<string>().IsRequired();
            });
        });

        modelBuilder.Entity<Expense>()
            .HasMany<ExpenseItem>(e => e.ExpenseItems)
            .WithOne(e => e.Expense)
            .HasForeignKey(e => e.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);


    }
}