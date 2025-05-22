using LoanShark.API.Proxies;
using LoanShark.Domain;
using Microsoft.AspNetCore.Mvc;

namespace LoanShark.MVC.Controllers
{
    public class FeedController : Controller
    {
        private readonly IFeedServiceProxy feedService;

        public FeedController(IFeedServiceProxy feedService)
        {
            this.feedService = feedService;
        }

        public async Task<IActionResult> Index()
        {
            var feedItems = await LoadFeedItems();

            return View(feedItems);
        }

        public async Task<List<Post>> LoadFeedItems()
        {
            return await this.feedService.GetFeedContent();
        }
    }
}
