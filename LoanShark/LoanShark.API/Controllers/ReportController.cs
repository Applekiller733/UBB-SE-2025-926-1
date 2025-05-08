using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LoanShark.Service.SocialService.Implementations;
using LoanShark.Domain;
using LoanShark.API.Models;
using LoanShark.EF.Repository.SocialRepository;
using LoanShark.Service.SocialService.Interfaces;
using System.Collections.Generic;

namespace LoanShark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController()
        {
            // !! guys look here !!
            IRepository repository = new Repository();
            INotificationService notificationService = new NotificationService(repository);
            IUserService userService = new UserService(repository, notificationService);
            _reportService = new ReportService(repository, userService);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportViewModel>> GetReportById(int id)
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now, matching UserController
            var report = _reportService.GetReportById(id);
            if (report == null)
                return NotFound();

            var dto = new ReportViewModel
            {
                MessageID = report.MessageID,
                ReporterUserID = report.ReporterUserID,
                Reason = report.Reason,
                Description = report.Description,
                Status = report.Status,
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> AddReport([FromBody] ReportViewModel reportDto)
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now
            if (reportDto == null)
                return BadRequest("Report data is required.");

            var report = new Report(
                reportDto.MessageID,
                reportDto.ReporterUserID,
                reportDto.Status,
                reportDto.Reason,
                reportDto.Description
            );

            _reportService.AddReport(report);
            return CreatedAtAction(nameof(GetReportById), new { id = report.MessageID }, reportDto);
        }

        [HttpGet("exists/{messageId}/{reporterUserId}")]
        public async Task<ActionResult<bool>> CheckIfReportExists(int messageId, int reporterUserId)
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now
            var exists = _reportService.CheckIfReportExists(messageId, reporterUserId);
            return Ok(exists);
        }

        [HttpPost("increase-report-count/{reportedId}")]
        public async Task<ActionResult> IncreaseReportCount(int reportedId)
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now
            _reportService.IncreaseReportCount(reportedId);
            return NoContent();
        }

        [HttpPost("log-reports")]
        public async Task<ActionResult> LogReportedMessages([FromBody] List<ReportViewModel> reportDtos)
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now
            if (reportDtos == null || reportDtos.Count == 0)
                return BadRequest("Report data is required.");

            var reports = reportDtos.Select(dto => new Report(
                dto.MessageID,
                dto.ReporterUserID,
                dto.Status,
                dto.Reason,
                dto.Description
            )).ToList();

            _reportService.LogReportedMessages(reports);
            return NoContent();
        }

        [HttpPost("send")]
        public async Task<ActionResult> SendReport([FromBody] ReportViewModel reportDto)
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now
            if (reportDto == null)
                return BadRequest("Report data is required.");

            var report = new Report(
                reportDto.MessageID,
                reportDto.ReporterUserID,
                reportDto.Status,
                reportDto.Reason,
                reportDto.Description
            );

            _reportService.SendReport(report);
            return NoContent();
        }
    }
}