using LoanShark.API.Proxies;
using LoanShark.EF.Repository.BankRepository;
using LoanShark.EF.Repository.SocialRepository;
using LoanShark.Service.BankService;
using LoanShark.Service.Service.BankService;
using LoanShark.Service.SocialService.Implementations;
using LoanShark.Service.SocialService.Interfaces;
using LoanShark.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using IUserService = LoanShark.Service.BankService.IUserService;
using UserService = LoanShark.Service.BankService.UserService;
using ISocialUserService = LoanShark.Service.SocialService.Interfaces.IUserService;
using SocialUserService = LoanShark.Service.SocialService.Implementations.UserService;

namespace LoanShark.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<LoanSharkDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("LoanShark")));
            builder.Services.AddScoped<ILoanSharkDbContext>(provider =>
                provider.GetRequiredService<LoanSharkDbContext>());
            builder.Services.AddAllServiceProxies();

            //Florin Transaction
            builder.Services.AddScoped<ITransactionsService, TransactionsService>();
            builder.Services.AddScoped<ITransactionsRepository, TransactionsRepositoryEF>();

            //Teo Bank Account
            builder.Services.AddScoped<IBankAccountService, BankAccountService>();
            builder.Services.AddScoped<IBankAccountRepository, BankAccountRepositoryEF>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepositoryEF>();
			
            //Loans
            builder.Services.AddScoped<ILoanService, LoanService>();
            builder.Services.AddScoped<ILoanRepository, LoanRepositoryEF>();

            //Maly Injections
            builder.Services.AddScoped<IMainPageService, MainPageService>();
            builder.Services.AddScoped<IMainPageRepository, MainPageRepositoryEF>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<ILoginRepository, LoginRepositoryEF>();

            builder.Services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();
            builder.Services.AddScoped<ITransactionHistoryRepository, TransactionHistoryRepositoryEF>();
            builder.Services.AddScoped<IRepository, RepositoryEF>();

            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IFeedService, FeedService>();

            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<ISocialUserService, SocialUserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
