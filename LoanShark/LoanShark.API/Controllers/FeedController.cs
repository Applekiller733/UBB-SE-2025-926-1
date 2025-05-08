using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LoanShark.Service.SocialService.Implementations;
using LoanShark.EF.Repository.SocialRepository;
using LoanShark.Domain;
using LoanShark.API.Models;
using LoanShark.Service.SocialService.Interfaces;

namespace LoanShark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedController : ControllerBase
    {
        private readonly FeedService _feedService;

        public FeedController(ILoanSharkDbContext dbContext)
        {
            // Probabil o sa fie nevoie de ceva modificare dupa ce termina George cu repo
            // ANTO/ZELU 
            // In rest cred ca merge 
            IRepository repository = new RepositoryEF(dbContext);
            INotificationService notification = new NotificationService(repository);
            IUserService userService = new UserService(repository, notification);
            _feedService = new FeedService(repository, userService);
        }

        [HttpGet("content")]
        public ActionResult<List<FeedViewModel>> GetFeedContent()
        {
            List<Post> posts = _feedService.GetFeedContent();
            if (posts == null || posts.Count == 0)
            {
                return NotFound();
            }

            List<FeedViewModel> dto = posts.Select(post => new FeedViewModel
            {
                PostID = post.PostID,
                Title = post.Title,
                Category = post.Category,
                Content = post.Content,
                Timestamp = post.Timestamp,
            }).ToList();

            return Ok(dto);
        }
    }
}