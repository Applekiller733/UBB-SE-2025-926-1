using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoanShark.API.Models;
using LoanShark.Domain;

namespace LoanShark.API.Proxies
{
    public interface IFeedServiceProxy
    {
        Task<List<Post>?> GetFeedContent();
    }

    public class FeedServiceProxy : IFeedServiceProxy
    {
        private readonly HttpClient _httpClient;

        public FeedServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Post>?> GetFeedContent()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7097/api/Feed/content");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json))
                {
                    var emptyFeed= new List<Post>();
                    return emptyFeed;
                }
                var dto = JsonSerializer.Deserialize<List<FeedViewModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return dto?.Select(post => new Post(post.PostID, post.Title, post.Category, post.Content, post.Timestamp)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Exceptie Feed: " + ex.Message.ToString());
            }
        }
    }
}