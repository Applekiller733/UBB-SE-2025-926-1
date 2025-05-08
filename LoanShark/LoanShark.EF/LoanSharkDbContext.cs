using LoanShark.Domain;
using LoanShark.EF.EfModels;
using LoanShark.EF.EFModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

public class LoanSharkDbContext : DbContext, ILoanSharkDbContext
{

    public LoanSharkDbContext(DbContextOptions<LoanSharkDbContext> options)
        : base(options) { }

    public DbSet<BankAccountEF> BankAccount { get; set; }
    public DbSet<ChatEF> Chat { get; set; }
    public DbSet<CurrencyExchangeEF> CurrencyExchange { get; set; }
    public DbSet<LoanEF> Loan { get; set; }
    public DbSet<NotificationEF> Notification { get; set; }
    public DbSet<PostEF> Post { get; set; }
    public DbSet<ReportEF> Report { get; set; }
    public DbSet<TransactionEF> Transaction { get; set; }
    public DbSet<UserEF> User { get; set; }

    public DbSet<CurrencyEF> Currency { get; set; }

}

public interface ILoanSharkDbContext
{
    DbSet<BankAccountEF> BankAccount { get; set; }
    DbSet<ChatEF> Chat { get; set; }
    DbSet<CurrencyExchangeEF> CurrencyExchange { get; set; }
    DbSet<LoanEF> Loan { get; set; }
    DbSet<NotificationEF> Notification { get; set; }
    DbSet<PostEF> Post { get; set; }
    DbSet<ReportEF> Report { get; set; }
    DbSet<TransactionEF> Transaction { get; set; }
    DbSet<UserEF> User { get; set; }

    DbSet<CurrencyEF> Currency { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
