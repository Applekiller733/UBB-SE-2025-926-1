using LoanShark.API.Proxies;
using LoanShark.Service.BankService;
using LoanShark.Service.Service.BankService;
using LoanShark.Web.Extensions;

namespace LoanShark.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSession();// for login

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAllServiceProxies();
            builder.Services.AddHttpClient<ILoginService, LoginServiceProxy>();
            builder.Services.AddHttpClient<IMainPageService, MainPageServiceProxy>();
            builder.Services.AddHttpClient<ITransactionsService, TransactionsServiceProxy>();


            //Transactions Florin
            builder.Services.AddHttpClient<ITransactionsService, TransactionsServiceProxy>();
            builder.Services.AddHttpClient<ITransactionHistoryService, TransactionHistoryProxy>();

            // Posts(Feed) George
            builder.Services.AddHttpClient<IFeedServiceProxy, FeedServiceProxy>();
            builder.Services.AddHttpClient<INotificationServiceProxy, NotificationServiceProxy>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession(); // for login

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
