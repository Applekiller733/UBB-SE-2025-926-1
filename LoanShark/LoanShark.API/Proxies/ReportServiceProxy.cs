using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LoanShark.API.Models;
using LoanShark.Domain;
using LoanShark.EF.EFModels;
using System.Collections.Generic;
using LoanShark.Service.SocialService.Interfaces;

namespace LoanShark.API.Proxies
{
    public interface IReportServiceProxy
    {
        Task<Report?> GetReportById(int id);
        Task AddReport(Report report);
        Task<bool> CheckIfReportExists(int messageId, int reporterUserId);
        Task IncreaseReportCount(int reportedId);
        Task LogReportedMessages(List<Report> reports);
        Task SendReport(Report report);
    }

    public class ReportServiceProxy : IReportServiceProxy
    {
        private readonly HttpClient _httpClient;

        public ReportServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Report?> GetReportById(int id)
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

        public async Task AddReport(Report report)
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

        public async Task<bool> CheckIfReportExists(int messageId, int reporterUserId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/Report/exists/{messageId}/{reporterUserId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<bool>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task IncreaseReportCount(int reportedId)
        {
            var response = await _httpClient.PostAsync($"https://localhost:7097/api/Report/increase-report-count/{reportedId}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task LogReportedMessages(List<Report> reports)
        {
            var dtos = reports.Select(report => new ReportViewModel
            {
                MessageID = report.MessageID,
                ReporterUserID = report.ReporterUserID,
                Reason = report.Reason,
                Description = report.Description,
                Status = report.Status
            }).ToList();

            var content = new StringContent(
                JsonSerializer.Serialize(dtos),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("https://localhost:7097/api/Report/log-reports", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendReport(Report report)
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

            var response = await _httpClient.PostAsync("https://localhost:7097/api/Report/send", content);
            response.EnsureSuccessStatusCode();
        }
    }
}