using LoanShark.API.Proxies;
using LoanShark.Domain;
using LoanShark.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LoanShark.MVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportServiceProxy _reportServiceProxy;
        private readonly ISocialUserServiceProxy _userServiceProxy;

        public ReportController(IReportServiceProxy reportServiceProxy, ISocialUserServiceProxy userServiceProxy)
        {
            _reportServiceProxy = reportServiceProxy;
            _userServiceProxy = userServiceProxy;
        }

        public IActionResult Index(int reportedUserId, int messageId)
        {
            var viewModel = new ReportViewModel
            {
                ReportedUserId = reportedUserId,
                MessageId = messageId
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(ReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                // Get current user ID from session
                var currentUserId = int.Parse(HttpContext.Session.GetString("id_user") ?? "0");

                // Based on the constructor parameters for Report
                var reason = model.SelectedCategory == "Other" ? model.OtherReason : model.SelectedCategory;

                // Create report using the constructor parameters in the correct order
                var report = new Report(
                    model.MessageId,        // messageID
                    model.ReportedUserId,   // reportedUserID
                    currentUserId.ToString(),// reporterID (as string)
                    reason,                  // reason
                    model.SelectedCategory   // category
                );

                await _reportServiceProxy.SendReport(report);

                TempData["SuccessMessage"] = "Report submitted successfully.";
                return RedirectToAction("Index", "Messages"); // Redirect to messages or another appropriate page
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while submitting your report.");
                return View("Index", model);
            }
        }

        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "Messages"); // Redirect to messages or another appropriate page
        }
    }
}