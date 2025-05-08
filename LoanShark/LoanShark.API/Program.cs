using LoanShark.API.Proxies;
using LoanShark.EF.Repository.BankRepository;
using LoanShark.Service.BankService;
using LoanShark.Web.Extensions;
using Microsoft.EntityFrameworkCore;

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
