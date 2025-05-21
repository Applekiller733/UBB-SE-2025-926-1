using LoanShark.API.Proxies;
using LoanShark.MVC.Models;
using LoanShark.Service.SocialService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LoanShark.MVC.Controllers
{
    public class GenerateTransferController : Controller
    {
        private readonly IChatServiceProxy _chatService;

        public GenerateTransferController(IChatServiceProxy chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public IActionResult Index(int chatId = 123)
        {
            var model = new GenerateTransferViewModel
            {
                ChatId = chatId,
                TransferTypeIndex = -1,
                CurrencyIndex = -1,
                AmountText = "",
                Description = ""
            };
            model.ValidateForm();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(GenerateTransferViewModel model)
        {
            // Validate and check funds on form update
            model.ValidateForm();
            if (model.TransferTypeIndex >= 0 && model.CurrencyIndex >= 0 && float.TryParse(model.AmountText, out float amount) && amount > 0)
            {
                if (model.SelectedTransferType == "Transfer Money")
                {
                    int currentUserId = await _chatService.GetCurrentUserID();
                    int participantCount = await _chatService.GetNumberOfParticipants(model.ChatId);
                    float totalAmount = amount * (participantCount - 1);
                    model.HasSufficientFunds = await _chatService.EnoughFunds(totalAmount, model.Currency, currentUserId);
                }
            }
            model.ValidateForm();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessTransfer(GenerateTransferViewModel model)
        {
            if (!ModelState.IsValid || !model.IsFormValid)
            {
                model.ValidateForm();
                return View("Index", model);
            }

            try
            {
                float amount = float.Parse(model.AmountText);
                switch (model.SelectedTransferType)
                {
                    case "Transfer Money":
                        await _chatService.SendMoneyViaChat(amount, model.Currency, model.Description, model.ChatId);
                        break;
                    case "Request Money":
                        await _chatService.RequestMoneyViaChat(amount, model.Currency, model.ChatId, model.Description);
                        break;
                    case "Split Bill":
                        int numOfParticipants = await _chatService.GetNumberOfParticipants(model.ChatId);
                        float splitAmount = amount / numOfParticipants;
                        await _chatService.RequestMoneyViaChat(splitAmount, model.Currency, model.ChatId, model.Description);
                        break;
                }

                TempData["AlertMessage"] = "Transfer processed successfully!";
                return RedirectToAction("Index", "MainPage");
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = $"Error processing transfer: {ex.Message}";
                return View("Index", model);
            }
        }
    }
}