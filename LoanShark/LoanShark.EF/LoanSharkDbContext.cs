using LoanShark.Domain;
//using LoanShark.EF.EfModels;
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
    //public DbSet<FriendshipEF> Friendship { get; set; }
    public DbSet<ChatUserEF> ChatUser { get; set; }

    //goes in action at runtime -> creates a composite PK on Friends table
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // composite PK for chat user
        modelBuilder.Entity<ChatUserEF>()
                .HasKey(cu => new { cu.ChatId, cu.UserId });

        //modelBuilder.Entity<FriendshipEF>()
        //    .HasKey(friendship => new { friendship.UserId, friendship.FriendId });

        ////disable cascade
        //modelBuilder.Entity<FriendshipEF>()
        //    .HasOne(f => f.User)
        //    .WithMany()
        //    .HasForeignKey(f => f.UserId)
        //    .OnDelete(DeleteBehavior.Restrict);

        //modelBuilder.Entity<FriendshipEF>()
        //    .HasOne(f => f.Friend)
        //    .WithMany()
        //    .HasForeignKey(f => f.FriendId)
        //    .OnDelete(DeleteBehavior.Restrict);
    }

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
    //DbSet<FriendshipEF> Friendship { get; set; }
    DbSet<ChatUserEF> ChatUser { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
