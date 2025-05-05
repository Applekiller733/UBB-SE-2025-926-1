using LoanShark.Domain;
using LoanShark.EF.EFModels;
using Microsoft.EntityFrameworkCore;

public class LoanSharkDbContext : DbContext
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

}
