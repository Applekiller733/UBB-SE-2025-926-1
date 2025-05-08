using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LoanShark.API.Models;
using LoanShark.Domain;

namespace LoanShark.API.Proxies
{
    public interface IReportServiceProxy
    {
        Task<Report?> GetReportByIdAsync(int id);
        Task AddReportAsync(Report report);
    }

    public class ReportServiceProxy : IReportServiceProxy
    {
        private readonly HttpClient _httpClient;

        public ReportServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Report?> GetReportByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/Report/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<ReportViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new Report(
                dto.MessageID,
                dto.ReporterUserID,
                dto.Status,
                dto.Reason,
                dto.Description
            );
        }

        public async Task AddReportAsync(Report report)
        {
            var dto = new ReportViewModel
            {
                MessageID = report.MessageID,
                ReporterUserID = report.ReporterUserID,
                Reason = report.Reason,
                Description = report.Description,
                Status = report.Status
            };

            var content = new StringContent(
                JsonSerializer.Serialize(dto),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("https://localhost:7097/api/Report", content);
            response.EnsureSuccessStatusCode();
        }
    }
}